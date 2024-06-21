using Telegram.Bot;
using Telegram.Bot.Types;

namespace Candy.Service.API.Services.Interfaces;

public interface ITelegramBotService
{
    Task OnBotReceiveAsync(ITelegramBotClient client, Update update, CancellationToken token);
    Task OnBotExceptionAsync(ITelegramBotClient client, Exception exception, CancellationToken token);
}