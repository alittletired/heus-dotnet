namespace Heus.Ddd.Data;

//https://github.com/mongodb/mongo-csharp-driver/blob/master/src/MongoDB.Bson/ObjectModel/ObjectId.cs
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
/// <summary>
/// 实体主键，长度24
/// </summary>
[Serializable]
public struct EntityId : IComparable<EntityId>, IEquatable<EntityId>
{
    // private static fields
    private static readonly EntityId _emptyInstance = default;
    private static readonly long _random = CalculateRandomValue();
    private static int _staticIncrement = (new Random()).Next();

    // private fields
    private readonly int _a;
    private readonly int _b;
    private readonly int _c;

    // constructors
    /// <summary>
    /// Initializes a new instance of the EntityId class.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    public EntityId(byte[] bytes)
    {
        if (bytes.Length != 12)
        {
            throw new ArgumentException("Byte array must be 12 bytes long", nameof(bytes));
        }

        FromByteArray(bytes, 0, out _a, out _b, out _c);
    }

    /// <summary>
    /// Initializes a new instance of the EntityId class.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <param name="index">The index into the byte array where the EntityId starts.</param>
    internal EntityId(byte[] bytes, int index)
    {
        FromByteArray(bytes, index, out _a, out _b, out _c);
    }


    /// <summary>
    /// Initializes a new instance of the EntityId class.
    /// </summary>
    /// <param name="value">The value.</param>
    public EntityId(string value)
    {
        var bytes = DataUtils.ParseHexString(value);
        FromByteArray(bytes, 0, out _a, out _b, out _c);
    }

    private EntityId(int a, int b, int c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    // public static properties
    /// <summary>
    /// Gets an instance of EntityId where the value is empty.
    /// </summary>
    public static EntityId Empty
    {
        get { return _emptyInstance; }
    }

    // public properties
    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    public int GetTimestamp()
    {
        return _a;
    }


    /// <summary>
    /// Gets the creation time (derived from the timestamp).
    /// </summary>
    public DateTime GetCreationTime()
    {
      return DataUtils.UnixEpoch.AddSeconds((uint)GetTimestamp()); 
    }

    // public operators
    /// <summary>
    /// Compares two EntityIds.
    /// </summary>
    /// <param name="lhs">The first EntityId.</param>
    /// <param name="rhs">The other EntityId</param>
    /// <returns>True if the first EntityId is less than the second EntityId.</returns>
    public static bool operator <(EntityId lhs, EntityId rhs)
    {
        return lhs.CompareTo(rhs) < 0;
    }

    /// <summary>
    /// Compares two EntityIds.
    /// </summary>
    /// <param name="lhs">The first EntityId.</param>
    /// <param name="rhs">The other EntityId</param>
    /// <returns>True if the first EntityId is less than or equal to the second EntityId.</returns>
    public static bool operator <=(EntityId lhs, EntityId rhs)
    {
        return lhs.CompareTo(rhs) <= 0;
    }

    /// <summary>
    /// Compares two EntityIds.
    /// </summary>
    /// <param name="lhs">The first EntityId.</param>
    /// <param name="rhs">The other EntityId.</param>
    /// <returns>True if the two EntityIds are equal.</returns>
    public static bool operator ==(EntityId lhs, EntityId rhs)
    {
        return lhs.Equals(rhs);
    }

    /// <summary>
    /// Compares two EntityIds.
    /// </summary>
    /// <param name="lhs">The first EntityId.</param>
    /// <param name="rhs">The other EntityId.</param>
    /// <returns>True if the two EntityIds are not equal.</returns>
    public static bool operator !=(EntityId lhs, EntityId rhs)
    {
        return !(lhs == rhs);
    }

    /// <summary>
    /// Compares two EntityIds.
    /// </summary>
    /// <param name="lhs">The first EntityId.</param>
    /// <param name="rhs">The other EntityId</param>
    /// <returns>True if the first EntityId is greather than or equal to the second EntityId.</returns>
    public static bool operator >=(EntityId lhs, EntityId rhs)
    {
        return lhs.CompareTo(rhs) >= 0;
    }

    /// <summary>
    /// Compares two EntityIds.
    /// </summary>
    /// <param name="lhs">The first EntityId.</param>
    /// <param name="rhs">The other EntityId</param>
    /// <returns>True if the first EntityId is greather than the second EntityId.</returns>
    public static bool operator >(EntityId lhs, EntityId rhs)
    {
        return lhs.CompareTo(rhs) > 0;
    }

    // public static methods
    /// <summary>
    /// Generates a new EntityId with a unique value.
    /// </summary>
    /// <returns>An EntityId.</returns>
    public static EntityId NewId()
    {
        return NewId(GetTimestampFromDateTime(DateTime.UtcNow));
    }

    /// <summary>
    /// Generates a new EntityId with a unique value (with the timestamp component based on a given DateTime).
    /// </summary>
    /// <param name="timestamp">The timestamp component (expressed as a DateTime).</param>
    /// <returns>An EntityId.</returns>
    public static EntityId NewId(DateTime timestamp)
    {
        return NewId(GetTimestampFromDateTime(timestamp));
    }

    /// <summary>
    /// Generates a new EntityId with a unique value (with the given timestamp).
    /// </summary>
    /// <param name="timestamp">The timestamp component.</param>
    /// <returns>An EntityId.</returns>
    public static EntityId NewId(int timestamp)
    {
        int increment = Interlocked.Increment(ref _staticIncrement) & 0x00ffffff; // only use low order 3 bytes
        return Create(timestamp, _random, increment);
    }



    /// <summary>
    /// Parses a string and creates a new EntityId.
    /// </summary>
    /// <param name="s">The string value.</param>
    /// <returns>A EntityId.</returns>
    public static EntityId Parse(string s)
    {

        if (TryParse(s, out EntityId EntityId))
        {
            return EntityId;
        }
        else
        {
            var message = string.Format("'{0}' is not a valid 24 digit hex string.", s);
            throw new FormatException(message);
        }
    }

    /// <summary>
    /// Tries to parse a string and create a new EntityId.
    /// </summary>
    /// <param name="s">The string value.</param>
    /// <param name="EntityId">The new EntityId.</param>
    /// <returns>True if the string was parsed successfully.</returns>
    public static bool TryParse(string s, out EntityId EntityId)
    {
        // don't throw ArgumentNullException if s is null
        if (s != null && s.Length == 24)
        {

            if (DataUtils.TryParseHexString(s, out var bytes))
            {
                EntityId = new EntityId(bytes!);
                return true;
            }
        }

        EntityId = default;
        return false;
    }



    // private static methods
    private static long CalculateRandomValue()
    {
        var seed = (int)DateTime.UtcNow.Ticks ^ GetMachineHash() ^ GetPid();
        var random = new Random(seed);
        var high = random.Next();
        var low = random.Next();
        var combined = (long)((ulong)(uint)high << 32 | (ulong)(uint)low);
        return combined & 0xffffffffff; // low order 5 bytes
    }

    private static EntityId Create(int timestamp, long random, int increment)
    {
        if (random < 0 || random > 0xffffffffff)
        {
            throw new ArgumentOutOfRangeException(nameof(random), "The random value must be between 0 and 1099511627775 (it must fit in 5 bytes).");
        }
        if (increment < 0 || increment > 0xffffff)
        {
            throw new ArgumentOutOfRangeException(nameof(increment), "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
        }

        var a = timestamp;
        var b = (int)(random >> 8); // first 4 bytes of random
        var c = (int)(random << 24) | increment; // 5th byte of random and 3 byte increment
        return new EntityId(a, b, c);
    }

    /// <summary>
    /// Gets the current process id.  This method exists because of how CAS operates on the call stack, checking
    /// for permissions before executing the method.  Hence, if we inlined this call, the calling method would not execute
    /// before throwing an exception requiring the try/catch at an even higher level that we don't necessarily control.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int GetCurrentProcessId()
    {
        return Environment.ProcessId;
    }

    private static int GetMachineHash()
    {
        // use instead of Dns.HostName so it will work offline
        var machineName = GetMachineName();
        return 0x00ffffff & machineName.GetHashCode(); // use first 3 bytes of hash
    }

    private static string GetMachineName()
    {
        return Environment.MachineName;
    }

    private static short GetPid()
    {
        try
        {
            return (short)GetCurrentProcessId(); // use low order two bytes only
        }
        catch (SecurityException)
        {
            return 0;
        }
    }

    private static int GetTimestampFromDateTime(DateTime timestamp)
    {
        var secondsSinceEpoch = (long)Math.Floor((DataUtils.ToUniversalTime(timestamp) - DataUtils.UnixEpoch).TotalSeconds);
        if (secondsSinceEpoch < uint.MinValue || secondsSinceEpoch > uint.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp));
        }
        return (int)(uint)secondsSinceEpoch;
    }

    private static void FromByteArray(byte[] bytes, int offset, out int a, out int b, out int c)
    {
        a = (bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3];
        b = (bytes[offset + 4] << 24) | (bytes[offset + 5] << 16) | (bytes[offset + 6] << 8) | bytes[offset + 7];
        c = (bytes[offset + 8] << 24) | (bytes[offset + 9] << 16) | (bytes[offset + 10] << 8) | bytes[offset + 11];
    }

    // public methods
    /// <summary>
    /// Compares this EntityId to another EntityId.
    /// </summary>
    /// <param name="other">The other EntityId.</param>
    /// <returns>A 32-bit signed integer that indicates whether this EntityId is less than, equal to, or greather than the other.</returns>
    public int CompareTo(EntityId other)
    {
        int result = ((uint)_a).CompareTo((uint)other._a);
        if (result != 0) { return result; }
        result = ((uint)_b).CompareTo((uint)other._b);
        if (result != 0) { return result; }
        return ((uint)_c).CompareTo((uint)other._c);
    }

    /// <summary>
    /// Compares this EntityId to another EntityId.
    /// </summary>
    /// <param name="rhs">The other EntityId.</param>
    /// <returns>True if the two EntityIds are equal.</returns>
    public bool Equals(EntityId rhs)
    {
        return
            _a == rhs._a &&
            _b == rhs._b &&
            _c == rhs._c;
    }

    /// <summary>
    /// Compares this EntityId to another object.
    /// </summary>
    /// <param name="obj">The other object.</param>
    /// <returns>True if the other object is an EntityId and equal to this one.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is EntityId id)
        {
            return Equals(id);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the hash code.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        int hash = 17;
        hash = 37 * hash + _a.GetHashCode();
        hash = 37 * hash + _b.GetHashCode();
        hash = 37 * hash + _c.GetHashCode();
        return hash;
    }

    /// <summary>
    /// Converts the EntityId to a byte array.
    /// </summary>
    /// <returns>A byte array.</returns>
    public byte[] ToByteArray()
    {
        var bytes = new byte[12];
        ToByteArray(bytes, 0);
        return bytes;
    }

    /// <summary>
    /// Converts the EntityId to a byte array.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="offset">The offset.</param>
    public void ToByteArray(byte[] destination, int offset)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }
        if (offset + 12 > destination.Length)
        {
            throw new ArgumentException("Not enough room in destination buffer.", nameof(offset));
        }

        destination[offset + 0] = (byte)(_a >> 24);
        destination[offset + 1] = (byte)(_a >> 16);
        destination[offset + 2] = (byte)(_a >> 8);
        destination[offset + 3] = (byte)(_a);
        destination[offset + 4] = (byte)(_b >> 24);
        destination[offset + 5] = (byte)(_b >> 16);
        destination[offset + 6] = (byte)(_b >> 8);
        destination[offset + 7] = (byte)(_b);
        destination[offset + 8] = (byte)(_c >> 24);
        destination[offset + 9] = (byte)(_c >> 16);
        destination[offset + 10] = (byte)(_c >> 8);
        destination[offset + 11] = (byte)(_c);
    }

    /// <summary>
    /// Returns a string representation of the value.
    /// </summary>
    /// <returns>A string representation of the value.</returns>
    public override string ToString()
    {
        var c = new char[24];
        c[0] = DataUtils.ToHexChar((_a >> 28) & 0x0f);
        c[1] = DataUtils.ToHexChar((_a >> 24) & 0x0f);
        c[2] = DataUtils.ToHexChar((_a >> 20) & 0x0f);
        c[3] = DataUtils.ToHexChar((_a >> 16) & 0x0f);
        c[4] = DataUtils.ToHexChar((_a >> 12) & 0x0f);
        c[5] = DataUtils.ToHexChar((_a >> 8) & 0x0f);
        c[6] = DataUtils.ToHexChar((_a >> 4) & 0x0f);
        c[7] = DataUtils.ToHexChar(_a & 0x0f);
        c[8] = DataUtils.ToHexChar((_b >> 28) & 0x0f);
        c[9] = DataUtils.ToHexChar((_b >> 24) & 0x0f);
        c[10] = DataUtils.ToHexChar((_b >> 20) & 0x0f);
        c[11] = DataUtils.ToHexChar((_b >> 16) & 0x0f);
        c[12] = DataUtils.ToHexChar((_b >> 12) & 0x0f);
        c[13] = DataUtils.ToHexChar((_b >> 8) & 0x0f);
        c[14] = DataUtils.ToHexChar((_b >> 4) & 0x0f);
        c[15] = DataUtils.ToHexChar(_b & 0x0f);
        c[16] = DataUtils.ToHexChar((_c >> 28) & 0x0f);
        c[17] = DataUtils.ToHexChar((_c >> 24) & 0x0f);
        c[18] = DataUtils.ToHexChar((_c >> 20) & 0x0f);
        c[19] = DataUtils.ToHexChar((_c >> 16) & 0x0f);
        c[20] = DataUtils.ToHexChar((_c >> 12) & 0x0f);
        c[21] = DataUtils.ToHexChar((_c >> 8) & 0x0f);
        c[22] = DataUtils.ToHexChar((_c >> 4) & 0x0f);
        c[23] = DataUtils.ToHexChar(_c & 0x0f);
        return new string(c);
    }
    
}