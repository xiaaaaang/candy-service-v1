using Candy.Service.API.Commons;
using Candy.Service.API.Models.Commons;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Models.Enumerates;
using Candy.Service.API.Services.Interfaces;
using Newtonsoft.Json;
using SqlSugar;

namespace Candy.Service.API.Services.Realizes;

public class TaskService : ITaskService
{
    private readonly ISqlSugarClient _scope;

    public TaskService(ISqlSugarClient scope)
    {
        _scope = scope;
    }

    public async Task<Mod_Result> CreateTaskByAccountId(Dbe_Task task)
    {
        var isCreated = await _scope.Insertable(task).ExecuteCommandAsync();
        return isCreated > 0
            ? new Mod_Result().Success(true, "Creation is successful.")
            : new Mod_Result().Failed("Creation is Failed.");
    }

    public async Task<Mod_Result> UpdateTaskContentById(Dbe_Task task)
    {
        var isUpdate = await _scope
            .Updateable(task)
            .UpdateColumns(x => new
            {
                x.Name,
                x.Description,
                x.TaskType,
                x.TypeValue
            })
            .Where(x => x.Id == task.Id)
            .ExecuteCommandAsync();

        return isUpdate > 0
            ? new Mod_Result().Success(true)
            : new Mod_Result().Failed("Update Failed.");
    }

    public async Task<Mod_Result> UpdateTaskStatusById(Dbe_Task task)
    {
        var isUpdate = await _scope
            .Updateable(task)
            .UpdateColumns(x => x.TaskStatus)
            .Where(x => x.Id == task.Id)
            .ExecuteCommandAsync();

        return isUpdate > 0 
            ? new Mod_Result().Success(true) 
            : new Mod_Result().Failed("Update Failed.");
    }

    public async Task<Mod_Result> DeleteTaskById(Dbe_Task task)
    {
        var isDelete = await _scope
            .Deleteable(task)
            .Where(x => x.Id == task.Id)
            .ExecuteCommandAsync();
        
        return isDelete > 0 
            ? new Mod_Result().Success(true) 
            : new Mod_Result().Failed("Delete Failed."); 
    }

    public async Task<Mod_Result> QueryTasksByAccountId(Dbe_Account account)
    {
        var result = new List<Dbe_Task>();

        var tasks = await _scope
            .Queryable<Dbe_Task>()
            .Where(x => x.AccountId == account.Id && x.TaskStatus == Enum_TaskStatus.Pending)
            .ToListAsync();

        var normal = new { Key = -1, StartTime = "", CloseTime = "" };
        var circular = new
        {
            Key = -1,
            StartTime = "",
            DayOfWeek = -1,
            DayOfMonth = -1,
            DayOfYear = -1
        };

        tasks.ForEach(x =>
        {
            if (x.TaskType == Enum_TaskType.Circular)
            {
                var circularModel = JsonConvert.DeserializeAnonymousType(x.TypeValue, circular);
                Dictionary<int, Func<DateTime>> condition = new()
                {
                    {
                        0, () =>
                        {
                            var isParse = DateTime.TryParse(circularModel.StartTime, out var circularValue);
                            return isParse ? circularValue : DateTime.Now;
                        }
                    },
                    {
                        1, () => DateTimeHelper.GetSpecificDayInThisWeek(circularModel.DayOfWeek) 
                    },
                    { 2, () => DateTimeHelper.GetDayOfCurrentMonth(circularModel.DayOfMonth)},
                    { 3, () => DateTimeHelper.GetDateFromDayOfYear(circularModel.DayOfYear)}
                };
                var time = condition[circularModel.Key].Invoke();
                x.Today = time.ToString("yyyy.MM.dd");
                result.Add(x);
            }
            else
            {
                var normalModel = JsonConvert.DeserializeAnonymousType(x.TypeValue, normal) ?? new
                {
                    Key = 0,
                    StartTime = DateTime.Now.ToString("yyyy.MM.dd"),
                    CloseTime = ""
                };
                var isParse = DateTime.TryParse(normalModel.StartTime, out var value);
                if (isParse)
                {
                    x.Today = value.ToString("yyyy.MM.dd");
                    result.Add(x);
                }
            }
        });

        return new Mod_Result().Success(result);
    }
}