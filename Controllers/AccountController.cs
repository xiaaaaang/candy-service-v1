using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Models.Requests;
using Candy.Service.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Candy.Service.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _svcAccount;

    public AccountController(IAccountService svcAccount)
    {
        _svcAccount = svcAccount;
    }

    [HttpPost("QueryAccountByUserId")]
    public async Task<Mod_Result> QueryAccountByUserId(Req_Basic<Dbe_Account> user)
    {
        if (user.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }
        return await _svcAccount.QueryAccountByUserIdAsync(user.Data.UserId);
    }
}