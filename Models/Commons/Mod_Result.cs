namespace Candy.Service.API.Models.Commons;

public class Mod_Result
{
    private int _code = 200;
    private bool _status = false;
    private string _message = string.Empty;
    private object _value = null!;
    
    public int Code => _code;
    public bool Status => _status;
    public string Message => _message;
    public object Value => _value;

    public Mod_Result Success(object value, string message = "")
    {
        this._status = true;
        this._message = message;
        this._value = value;
        return this;
    }

    public Mod_Result Failed(string message)
    {
        this._status = false;
        this._message = message;
        this._value = new object();
        return this;
    }
}