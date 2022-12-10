using System.Runtime.CompilerServices;
using Heus.Core.Utils;

namespace Heus.Core.Common;

public  interface IEnumClass{
     string Name { get; }
     int Value { get; }
    string Title { get; }
}


public abstract class EnumClass<TEnum> : IEnumClass, IEquatable<EnumClass<TEnum>>
    , IComparable<EnumClass<TEnum>> where TEnum : EnumClass<TEnum>
{
    protected EnumClass(string name, int value,string title)
    {
        Name = name;
        Value = value;
        Title = title;
    }

    public string Name { get; }
    public int Value { get; }
    public string Title { get; }

    public static TEnum[] GetEnumOptions() {
        return EnumOptions.Value;
        }
    private readonly static Lazy<TEnum[]> EnumOptions =
        new(() => {
            var options = TypeUtils.GetFields<TEnum>(typeof(TEnum));
            return options.OrderBy(t => t.Name)            .ToArray();
            },  LazyThreadSafetyMode.ExecutionAndPublication);

    private readonly static Lazy<Dictionary<string, TEnum>> FromNames =
        new(() => EnumOptions.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private readonly static Lazy<Dictionary<int, TEnum>> FromValues =
        new(() => EnumOptions.Value.ToDictionary(e => e.Value));


    public static TEnum FromName(string name)
    {
        return FromNames.Value[name];
    }

    public static bool TryFromName(string name, out TEnum? result)
    {
        return FromNames.Value.TryGetValue(name, out result);

    }

    public static TEnum FromValue(int value)
    {
        return FromValues.Value[value];
    }

    public static bool TryFromValue(int value, out TEnum? result)
    {
        return FromValues.Value.TryGetValue(value, out result);

    }

    #region overrides

    public override string ToString()
    {
        return Name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public bool Equals(EnumClass<TEnum>? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        // it's not same instance so 
        // check if it's not null and is same value
        if (other is null)
            return false;

        return Value.Equals(other.Value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(EnumClass<TEnum>? other)
    {
        return Value.CompareTo(other?.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is EnumClass<TEnum> other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(EnumClass<TEnum> left, EnumClass<TEnum> right)
    {
        // Equals handles null on right side
        return left.Equals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(EnumClass<TEnum> left, EnumClass<TEnum> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(EnumClass<TEnum> left, EnumClass<TEnum> right)
    {
        return left.CompareTo(right) < 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(EnumClass<TEnum> left, EnumClass<TEnum> right)
    {
        return left.CompareTo(right) <= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(EnumClass<TEnum> left, EnumClass<TEnum> right)
    {
        return left.CompareTo(right) > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(EnumClass<TEnum> left, EnumClass<TEnum> right)
    {
        return left.CompareTo(right) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(EnumClass<TEnum> enumBase)
    {
        return enumBase.Value;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumClass<TEnum>(int value)
    {
        return FromValue(value);
    }

    #endregion
}