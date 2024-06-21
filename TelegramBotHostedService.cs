using Candy.Service.API.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Candy.Service.API;

public class TelegramBotHostedService : IHostedService
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly ITelegramBotService _telegramBotService;
    
    public TelegramBotHostedService(ITelegramBotClient telegramBot, ITelegramBotService telegramBotService)
    {
        _telegramBot = telegramBot;
        _telegramBotService = telegramBotService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var options = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message
            }
        };

        _telegramBot.StartReceiving(
            _telegramBotService.OnBotReceiveAsync,
            _telegramBotService.OnBotExceptionAsync,
            options,
            cancellationToken: cancellationToken
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _telegramBot.CloseAsync(cancellationToken);
        return Task.CompletedTask;
    }
}