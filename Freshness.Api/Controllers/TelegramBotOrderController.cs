using Freshness.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Freshness.Controllers
{
    [Route("api/{telegram-bot-token-here}")]
    [ApiController]
    [EnableCors("default")]
    public class TelegramBotOrderController : ControllerBase
    {
        private readonly ITelegramBotOrderService _telegramOrderService;

        public TelegramBotOrderController(ITelegramBotOrderService telegramOrderService)
        {
            _telegramOrderService = telegramOrderService;
        }

        /// <summary>
        /// Api for TelegramBotOrder
        /// </summary>
        /// <param name="update">Update</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _telegramOrderService.TelegramUpdate(update);

            return Ok();
        }
    }
}
