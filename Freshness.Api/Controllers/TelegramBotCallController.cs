using Freshness.Models.ResponseModels;
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
    public class TelegramBotCallController : ControllerBase
    {
        private readonly ITelegramBotCallService _telegramCallService;

        public TelegramBotCallController(ITelegramBotCallService telegramCallService)
        {
            _telegramCallService = telegramCallService;
        }

        /// <summary>
        /// Api for TelegramBotCall
        /// </summary>
        /// <param name="update">Update</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _telegramCallService.TelegramUpdate(update);

            return Ok();
        }
    }
}
