using System.Configuration;
using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Candy.Service.API;

public class TelegramBotCommandMap
{
    private Dictionary<string, Func<Message, Task<Mod_Result>>> _map = new();

    //注入服务
    private readonly IAccountOperationService _accountOperationService;
    private readonly IConfiguration _configuration;

    public TelegramBotCommandMap(IAccountOperationService accountOperationService, IConfiguration configuration)
    {
        _accountOperationService = accountOperationService;
        _configuration = configuration;

        //添加命令映射
        _map.Add("/start", async x =>
        {
            if (x.From == null) return new Mod_Result().Failed("No user information was provided.");

            if (x.Chat.Type != ChatType.Private) return new Mod_Result().Failed("");
            
            var startResult = await _accountOperationService.RegisterAccountAsync(x.From);
            if (!startResult.Status)
            {
                return new Mod_Result().Failed("Some minor errors were encountered, please try again later!");
            }

            var button = new InlineKeyboardButton("Open Candy")
            {
                WebApp = new WebAppInfo()
                {
                    Url = _configuration["Candy"] 
                          ?? throw new ConfigurationErrorsException("Parameter Candy is not configured.")
                }
            };

            var markup = new InlineKeyboardMarkup(button);

            return new Mod_Result().Success(markup, "Click the button below to start viewing your tasks and plans!");
        });
    }

    public async Task<Mod_Result> ExecuteCommandAsync(string command, Message message)
    {
        return _map.ContainsKey(command)
            ? await _map[command](message)
            : new Mod_Result().Failed("This command does not exist.");
    }
}