using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Models.Enumerates;

namespace Candy.Service.API.Services.Interfaces;

public interface ITaskService
{
    Task<Mod_Result> CreateTaskByAccountId(Dbe_Task task);
    Task<Mod_Result> QueryTasksByAccountId(Dbe_Account account);
    Task<Mod_Result> UpdateTaskStatusById(Dbe_Task task);
    Task<Mod_Result> DeleteTaskById(Dbe_Task task);
    Task<Mod_Result> UpdateTaskContentById(Dbe_Task task);
}