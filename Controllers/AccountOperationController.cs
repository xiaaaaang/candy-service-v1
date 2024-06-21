using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Models.Requests;
using Candy.Service.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Candy.Service.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountOperationController : ControllerBase
{
    private readonly IAccountOperationService _svcAccountOperation;

    public AccountOperationController(IAccountOperationService svcAccountOperation)
    {
        _svcAccountOperation = svcAccountOperation;
    }

    [HttpPost("QueryAccountByUserId")]
    public async Task<Mod_Result> QueryAccountByUserId(Req_Basic<Dbe_Account> user)
    {
        if (user.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }
        return await _svcAccountOperation.QueryAccountByUserIdAsync(user.Data.UserId);
    }
}