

namespace Heus.Core;

public class BusinessException : Exception
{
    public  int Code { get; }
    public BusinessException(string message,int code=500, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
    }
  
   

}

