using Candy.Service.API.Models.Commons;
using Telegram.Bot.Types;

namespace Candy.Service.API.Services.Interfaces;

public interface IAccountOperationService
{
    Task<Mod_Result> RegisterAccountAsync(User user);
    Task<Mod_Result> QueryAccountByUserIdAsync(long userId);
}