using Candy.Service.API.Models.Enumerates;
using SqlSugar;

namespace Candy.Service.API.Models.DatabaseEntities;

[SugarTable("Tasks")]
public class Dbe_Task : Dbe_Basic
{
    public long AccountId { get; set; }
    public Enum_TaskStatus TaskStatus { get; set; } = Enum_TaskStatus.Pending;
    [SugarColumn(Length = 500)] public string Name { get; set; }
    [SugarColumn(Length = 5000, IsNullable = true)] public string? Description { get; set; }
    public Enum_TaskType TaskType { get; set; }
    [SugarColumn(Length = 500)] public string TypeValue { get; set; }
    [SugarColumn(IsIgnore = true)]
    public string Today { get; set; }
}