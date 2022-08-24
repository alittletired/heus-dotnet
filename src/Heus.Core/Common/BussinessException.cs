using System.Runtime.Serialization;

namespace Heus.Core;

public class BussinessException : Exception
{
    public BussinessException(string message)
        : base(message)
    {

    }

    public BussinessException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public BussinessException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}

