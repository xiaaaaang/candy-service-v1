using SqlSugar;

namespace Candy.Service.API.Models.DatabaseEntities;

public class Dbe_Basic
{
    [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
    public long Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
}