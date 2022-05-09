using Freshness.Common.Constants;
using Freshness.Common.Extensions;
using Freshness.Common.ResponseMessages;
using Freshness.Common.Settings;
using Freshness.DAL.Interfaces;
using Freshness.Domain.Entities;
using Freshness.Models.Models;
using Freshness.Models.ResponseModels;
using Freshness.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Freshness.Services.Services
{
    public class TelegramBotOrderService : ITelegramBotOrderService
    {
        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;

        private readonly BotConfiguration _orderBotConfig;
        private readonly TelegramBotClient _botClient;
        private readonly IUnitOfWork _unitOfWork;

        public TelegramBotOrderService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _orderBotConfig = configuration.GetSection("TelegramBotOrderConfiguration").Get<BotConfiguration>();
            _botClient = new TelegramBotClient(_orderBotConfig.Token);
            _cancellationToken = _cancelTokenSource.Token;
            _unitOfWork = unitOfWork;
        }

        public async Task BulkSent(OrderResponseModel order)
        {
            var telegramBotOrderExistingUsers = await GetTelegramBotOrderSignedIdUsers();

            var inlineQueryModel = new TelegramBotInlineQueryModel
            {
                DoneEntityId = order.Id
            };

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "В архів", callbackData: JsonSerializer.Serialize(inlineQueryModel)),
                },
            });

            telegramBotOrderExistingUsers.ForEach(async user =>
            {
                await SendMessage(user.ChatId, order.ToString(), inlineKeyboard);
            });
        }

        public async Task<Message> SendMessage(long chatId, string message, InlineKeyboardMarkup inlineKeyboar = null)
        {
            return await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: message,
                replyMarkup: inlineKeyboar,
                cancellationToken: _cancellationToken);
        }

        public async Task<List<TelegramBotOrderUser>> GetTelegramBotOrderAllUsers()
        {
            var telegramBotUsers = await _unitOfWork.Repository<TelegramBotOrderUser>().GetAsync(item => true);

            return telegramBotUsers;
        }

        public async Task TelegramUpdate(Update update)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await BotOnCallbackQueryReceived(update);

                return;
            }

            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
            {
                await SendMessage(update.Message.Chat.Id, TelegramBotResponseMessages.UnknownCommand);

                return;
            }

            var chatId = update.Message.Chat.Id;
            var message = update.Message.Text;
            
            var user = await GetUserAsync(chatId);

            //Firstly we register all users who send request to the bot
            if (user == null)
            {
                user = await RegisterUser(update.Message.Chat);
            }

            //If user is signedIn we do not respond to his commands, except for deletion
            if (user.AuthorizationStage == AuthorizationStage.SignedIn && message != TelegramBotCommand.Delete)
            {
                user.LastUpdate = DateTime.UtcNow;

                await UpdateUserAsync(user);

                await SendMessage(chatId, TelegramBotResponseMessages.AlreadyAuthorized);

                return;
            }

            //After we added chatId to database we check the message.
            //If user want to delete from the bot or to cancel some action we set authorizationStage to unauthorized
            if (user.AuthorizationStage == AuthorizationStage.SignedIn && message == TelegramBotCommand.Delete)
            {
                await ResetUserAsync(chatId);

                await SendMessage(chatId, TelegramBotResponseMessages.SuccessfullySignedOut);

                return;
            }

            //If user is unauthorized and want to cancel some action we set authorizationStage to unauthorized
            if (message == TelegramBotCommand.Cancel)
            {
                await ResetUserAsync(chatId);

                await SendMessage(chatId, TelegramBotResponseMessages.InputPhone);

                return;
            }

            //Checks password if the user is an unauthorized, but already have verified phone
            if (user.AuthorizationStage == AuthorizationStage.PhoneConfirmed)
            {
                var password = message.Trim();

                var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Phone == user.Phone &&
                    item.Password == password.GetCustomHash());

                if (worker == null)
                {
                    await SendMessage(chatId, TelegramBotResponseMessages.WrongPassword);

                    return;
                }

                user.AuthorizationStage = AuthorizationStage.SignedIn;
                user.SignedInDate = DateTime.Now;
                user.Role = worker.Role;
                user.LastUpdate = DateTime.UtcNow;

                await UpdateUserAsync(user);

                await SendMessage(chatId, TelegramBotResponseMessages.SuccessfullySignedIn);

                return;
            }

            //If user is unuthorized we need to check if user exists with phone number
            if (user.AuthorizationStage == AuthorizationStage.Unauthorized)
            {
                var phone = message.Trim();

                if (!IsPhoneValid(chatId, phone))
                {
                    await SendMessage(chatId, TelegramBotResponseMessages.InputPhone);

                    return;
                }

                // Only one authorization available using one credentials.
                var authorizedUser = await _unitOfWork.Repository<TelegramBotOrderUser>().FindAsync(x => x.Phone == phone &&
                    x.AuthorizationStage == AuthorizationStage.SignedIn);

                if (authorizedUser != null)
                {
                    await SendMessage(chatId, TelegramBotResponseMessages.AlreadyAuthorizedFromDifferentDevice);

                    return;
                }

                var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Phone == phone);

                if (worker == null)
                {
                    await SendMessage(chatId, TelegramBotResponseMessages.UserDoesNotExist);

                    return;
                }

                user.AuthorizationStage = AuthorizationStage.PhoneConfirmed;
                user.Phone = phone;
                user.LastUpdate = DateTime.UtcNow;

                await UpdateUserAsync(user);

                await SendMessage(chatId, TelegramBotResponseMessages.InputPassword);

                return;
            }

            user.LastUpdate = DateTime.UtcNow;

            await UpdateUserAsync(user);

            await SendMessage(chatId, TelegramBotResponseMessages.UnknownCommand);
        }

        private async Task BotOnCallbackQueryReceived(Update update)
        {
            var inlineQueryModel = JsonSerializer.Deserialize<TelegramBotInlineQueryModel>(update.CallbackQuery.Data);

            if (inlineQueryModel == null)
            {
                await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: $"Виникла помилка!");

                return;
            }

            var order = await _unitOfWork.Repository<Order>().FindAsync(x => x.Id == inlineQueryModel.DoneEntityId);

            if (order == null || order.IsDone == true)
            {
                await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: $"Замовлення успішно додано до архіву!");

                await _botClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);

                return;
            }

            order.IsDone = true;
            order.IsDoneDate = DateTime.UtcNow;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.SaveChangesAsync();

            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: $"Замовлення успішно додано до архіву!");

            await _botClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
        }


        private async Task<TelegramBotOrderUser> RegisterUser(Chat chat)
        {
            var user = new TelegramBotOrderUser
            {
                ChatId = chat.Id,
                Username = chat.Username,
                FirstName = chat.FirstName,
                AuthorizationStage = AuthorizationStage.Unauthorized,
                LastUpdate = DateTime.UtcNow,
            };

            var createdUser = await CreateUserAsync(user);

            return createdUser;
        }

        private async Task<TelegramBotOrderUser> GetUserAsync(long chatId)
        {
            var user = await _unitOfWork.Repository<TelegramBotOrderUser>().FindAsync(item => item.ChatId == chatId);

            return user;
        }

        private async Task<TelegramBotOrderUser> CreateUserAsync(TelegramBotOrderUser user)
        {
            var createdUser = await _unitOfWork.Repository<TelegramBotOrderUser>().InsertAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return createdUser;
        }

        private async Task<TelegramBotOrderUser> UpdateUserAsync(TelegramBotOrderUser user)
        {
            var updatedUser = _unitOfWork.Repository<TelegramBotOrderUser>().Update(user);

            await _unitOfWork.SaveChangesAsync();

            return updatedUser;
        }

        private async Task ResetUserAsync(long chatId)
        {
            var user = await GetUserAsync(chatId);

            if (user != null)
            {
                user.AuthorizationStage = AuthorizationStage.Unauthorized;
                user.LastUpdate = DateTime.UtcNow;

                await UpdateUserAsync(user);
            }
        }

        private async Task<List<TelegramBotOrderUser>> GetTelegramBotOrderSignedIdUsers()
        {
            var telegramBotOrderSignedInUsers = await _unitOfWork.Repository<TelegramBotOrderUser>().GetAsync(user => user.AuthorizationStage == AuthorizationStage.SignedIn);

            return telegramBotOrderSignedInUsers;
        }

        private bool IsPhoneValid(long chatId, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }

            var isNumber = Int32.TryParse(message, out var phone);

            if (!isNumber)
            {
                return false;
            }

            if (message.StartsWith("+38") || message.StartsWith("38") || message.Length != 10)
            {
                return false;
            }

            return true;
        }
    }
}
