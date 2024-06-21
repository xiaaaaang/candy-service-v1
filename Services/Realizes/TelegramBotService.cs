using Candy.Service.API.Commons;
using Candy.Service.API.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Candy.Service.API.Services.Realizes;

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotCommandMap _commandMap;

    public TelegramBotService(TelegramBotCommandMap commandMap)
    {
        _commandMap = commandMap;
    }

    public async Task OnBotReceiveAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Message == null || string.IsNullOrEmpty(update.Message.Text)) return;
        
        //检测消息是否为命令
        var isCommand = update.Message.Text.IsTelegramBotCommand(out var command);
        if (!isCommand) return;

        //返回命令执行的结果
        var result = await _commandMap.ExecuteCommandAsync(command, update.Message);
        if (!string.IsNullOrEmpty(result.Message))
        {
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                result.Message,
                cancellationToken: token,
                replyMarkup: result.Status ? result.Value as InlineKeyboardMarkup : null
            );
        }
    }

    public Task OnBotExceptionAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        client.DeleteWebhookAsync(true, cancellationToken: token);

        var error = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        
        Console.WriteLine(error);
        
        return Task.CompletedTask;
    }
}