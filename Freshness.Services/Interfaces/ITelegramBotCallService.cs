using Freshness.Domain.Entities;
using Freshness.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Freshness.Services.Interfaces
{
    public interface ITelegramBotCallService
    {
        /// <summary>
        /// Notifies all signed in users about new call
        /// </summary>
        /// <param name="call">Call model</param>
        /// <returns>Task</returns>
        Task BulkSent(CallResponseModel call);

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
        Task<List<TelegramBotCallUser>> GetTelegramBotCallAllUsers();

        /// <summary>
        /// Telegram web hook sends request to this method
        /// </summary>
        /// <param name="update">Update</param>
        /// <returns>Task</returns>
        Task TelegramUpdate(Update update);
    }
}
