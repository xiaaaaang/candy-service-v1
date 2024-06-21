namespace Candy.Service.API.Models.Requests;

public class Req_Basic<T> where T : class, new()
{
    public long Timestamp { get; set; }
    public T? Data { get; set; }
}