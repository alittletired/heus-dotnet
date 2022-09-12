using System.Runtime.Serialization;

namespace Heus.Core;

public class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {

    }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public BusinessException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}

