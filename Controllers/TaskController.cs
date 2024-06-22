using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Models.Enumerates;
using Candy.Service.API.Models.Requests;
using Candy.Service.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Candy.Service.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController
{
    private readonly ITaskService _svcTask;

    public TaskController(ITaskService svcTask)
    {
        _svcTask = svcTask;
    }
    
    [HttpPost("CreateTaskByAccountId")]
    public async Task<Mod_Result> CreateTaskByAccountId(Req_Basic<Dbe_Task> task)
    {
        if (task.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }
        return await _svcTask.CreateTaskByAccountId(task.Data);
    }

    [HttpPost("QueryTasksByAccountId")]
    public async Task<Mod_Result> QueryTasksByAccountId(Req_Basic<Dbe_Account> account)
    {
        if (account.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }

        return await _svcTask.QueryTasksByAccountId(account.Data);
    }

    [HttpPost("CompletedTaskById")]
    public async Task<Mod_Result> CompletedTaskById(Req_Basic<Req_CompletedTask> request)
    {
        if (request.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }

        var task = new Dbe_Task
        {
            Id = request.Data.Id,
            TaskStatus = Enum_TaskStatus.Completed
        };
        
        return await _svcTask.UpdateTaskStatusById(task);
    }

    [HttpPost("DeletedTaskById")]
    public async Task<Mod_Result> DeletedTaskById(Req_Basic<Req_DeletedTask> request)
    {
        if (request.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }
        var task = new Dbe_Task
        {
            Id = request.Data.Id
        };
        return await _svcTask.DeleteTaskById(task);
    }
    
    [HttpPost("UpdateTaskContentById")]
    public async Task<Mod_Result> UpdateTaskContentById(Req_Basic<Dbe_Task> task)
    {
        if (task.Data is null)
        {
            return new Mod_Result().Failed("The request parameter data cannot be empty.");
        }
        return await _svcTask.UpdateTaskContentById(task.Data);
    }
}