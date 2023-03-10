

namespace Heus.Core;

public class BusinessException : Exception
{
    public  int Code { get; }
    public BusinessException(string message,int code, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
    }
    public BusinessException(string message):this(message,500){}
   

}

