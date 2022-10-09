namespace Heus.Core;

public abstract class ApiResult
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public abstract  object? GetData();

    public static ApiResult<object> Error(Exception ex)
    {
        return new ApiResult<object>() { Message = ex.Message, Code = 500 };
    }
    public static ApiResult<T> Ok<T>(T? data)
    {
        return new ApiResult<T>() { Data = data};
    }
}

public class ApiResult<T> : ApiResult
{

    public T? Data { get; set; }

    public override object? GetData()
    {
        return Data;
    }

    public ApiResult()
    {
     
    }

}