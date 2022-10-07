namespace Heus.Core;

public abstract class ApiResult
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public abstract  object? GetData();
}

public class ApiResult<T> : ApiResult
{

    public T? Data { get; set; }

    public override object? GetData()
    {
        return Data;
    }

    public ApiResult(T data)
    {
        Data = data;
    }

}