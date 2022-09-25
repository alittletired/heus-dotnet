using System.Runtime.Serialization;

namespace Heus.Core;

public class BusinessException : Exception
{
    public BusinessException(string message, Exception? innerException = null)
        : base(message, innerException)
    {

    }


}

