using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Services.Interfaces;
using SqlSugar;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Candy.Service.API.Services.Realizes;

public class AccountService : IAccountService
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly ISqlSugarClient _scope;
    private readonly IConfiguration _configuration;

    public AccountService(ITelegramBotClient telegramBot, ISqlSugarClient scope, IConfiguration configuration)
    {
        _telegramBot = telegramBot;
        _scope = scope;
        _configuration = configuration;
    }
    
    public async Task<Mod_Result> RegisterAccountAsync(User user)
    {
        var isAnyAccount = await _scope
            .Queryable<Dbe_Account>()
            .Where(x => x.UserId == user.Id)
            .AnyAsync();
        
        if (isAnyAccount)
        {
            return new Mod_Result().Success(true,"You already have an account :)");
        }

        var entity = new Dbe_Account()
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            LanguageCode = user.LanguageCode
        };

        var result = await _telegramBot.GetUserProfilePhotosAsync(user.Id);
        if (result.Photos.Any())
        {
            var file = await _telegramBot.GetFileAsync(result.Photos[0][0].FileId);
            entity.PhotoUrl = $"{_configuration["Telegram:Files"]}{_configuration["Telegram:Token"]}/{file.FilePath}";
        }
        
        var isRegistered = await _scope
            .Insertable(entity)
            .ExecuteCommandAsync();

        return isRegistered > 0
            ? new Mod_Result().Success(true, "Registration is successful.")
            : new Mod_Result().Failed("Registration is Failed.");
    }
    
    public async Task<Mod_Result> QueryAccountByUserIdAsync(long userId)
    {
        var list = await _scope
            .Queryable<Dbe_Account>()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return list.Any() 
            ? new Mod_Result().Success(list.First()) 
            : new Mod_Result().Failed("This user does not have a registered account.");
    }
}