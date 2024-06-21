using SqlSugar;

namespace Candy.Service.API.Models.DatabaseEntities;

public class Dbe_Basic
{
    [SugarColumn(IsPrimaryKey = true)]
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
}