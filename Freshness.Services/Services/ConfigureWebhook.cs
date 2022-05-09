using Freshness.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Freshness.Services.Services
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly BotConfiguration _callBotConfig;
        private readonly BotConfiguration _orderBotConfig;

        public ConfigureWebhook(IConfiguration configuration)
        {
            _callBotConfig = configuration.GetSection("TelegramBotCallConfiguration").Get<BotConfiguration>();
            _orderBotConfig = configuration.GetSection("TelegramBotOrderConfiguration").Get<BotConfiguration>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var callBotClient = new TelegramBotClient(_callBotConfig.Token);
            var orderBotClient = new TelegramBotClient(_orderBotConfig.Token);

            var callWebhookAddress = @$"{_callBotConfig.HostAddress}/api/{_callBotConfig.Token}";
            var orderWebhookAddress = @$"{_orderBotConfig.HostAddress}/api/{_orderBotConfig.Token}";

            var allowedUpdates = new List<UpdateType>() {
                UpdateType.Message,
                UpdateType.CallbackQuery
            };

            await callBotClient.SetWebhookAsync(
                url: callWebhookAddress,
                allowedUpdates: allowedUpdates,
                cancellationToken: cancellationToken);

            await orderBotClient.SetWebhookAsync(
                url: orderWebhookAddress,
                allowedUpdates: allowedUpdates,
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var callBotClient = new TelegramBotClient(_callBotConfig.Token);
            var orderBotClient = new TelegramBotClient(_orderBotConfig.Token);

            await callBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
            await orderBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
