namespace Heus.Core.Common;

public  class ApiResult
{
    public int Code { get; }
    public string? Message { get;  }

    public ApiResult(int code, string? message)
    {
        Code = code;
        Message = message;
    }
    public static ApiResult Error(Exception ex)
    {
        return new ApiResult (500,ex.Message) ;
    }
    public static ApiResult<T> Ok<T>(T? data)
    {
        return new ApiResult<T>( data);
    }
}

public class ApiResult<T> : ApiResult
{
    public ApiResult(T? data) : base(0, null)
    {
        Data = data;
    }
    public T? Data { get;  }
   
}