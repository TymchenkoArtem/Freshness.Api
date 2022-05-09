using Freshness.Domain.Entities;
using Freshness.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Freshness.Services.Interfaces
{
    public interface ITelegramBotOrderService
    {
        /// <summary>
        /// Notifies all signed in users about new order
        /// </summary>
        /// <param name="order">Order model</param>
        /// <returns>Task</returns>
        Task BulkSent(OrderResponseModel order);

        /// <summary>
        /// Sends message to the telegram to user with chatId
        /// </summary>
        /// <param name="chatId">ChatId</param>
        /// <param name="message">Message</param>
        /// <returns>Returned message from telegram</returns>
        Task<Message> SendMessage(long chatId, string message, InlineKeyboardMarkup inlineKeyboar = null);

        /// <summary>
        /// Retrieves all telegram users
        /// </summary>
        /// <returns>Telegram users</returns>
        Task<List<TelegramBotOrderUser>> GetTelegramBotOrderAllUsers();

        /// <summary>
        /// Telegram web hook sends request to this method
        /// </summary>
        /// <param name="update">Update</param>
        /// <returns>Task</returns>
        Task TelegramUpdate(Update update);
    }
}
