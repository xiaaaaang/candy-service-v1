using SqlSugar;

namespace Candy.Service.API.Models.DatabaseEntities;

[SugarTable("Accounts")]
public class Dbe_Account : Dbe_Basic
{
    public long UserId { get; set; }
    [SugarColumn(IsNullable = true)] public string? Username { get; set; }
    [SugarColumn(IsNullable = true)] public string? FirstName { get; set; }
    [SugarColumn(IsNullable = true)] public string? LastName { get; set; }

    [SugarColumn(Length = 1000, IsNullable = true)]
    public string? PhotoUrl { get; set; }
    [SugarColumn(IsNullable = true)] public string? LanguageCode { get; set; }
}