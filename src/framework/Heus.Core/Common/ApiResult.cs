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
    public static ApiResult FromException(Exception ex)
    {
        var code = 500;
        if (ex is BusinessException businessException)
        {
            code = businessException.Code;
        }
        return new ApiResult (code,ex.Message) ;
    }
    public static ApiResult<T> Ok<T>(T data)
    {
        return new ApiResult<T>( data);
    }
}

public class ApiResult<T> : ApiResult
{
    public ApiResult(T data,int code=0, string? message=null) : base(code, message)
    {
        Data = data;
    }
    public T? Data { get; }

}