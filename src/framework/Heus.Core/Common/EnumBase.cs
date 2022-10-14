using System.Runtime.CompilerServices;
using Heus.Core.Utils;
namespace Heus.Core;

public abstract class EnumBase<TEnum> : IEquatable<EnumBase<TEnum>>
    , IComparable<EnumBase<TEnum>> where TEnum : EnumBase<TEnum>
{
    protected EnumBase(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public int Value { get; }

    private static readonly Lazy<TEnum[]> EnumOptions =
        new(() => TypeHelper.GetFields<TEnum>(typeof(TEnum)).OrderBy(t => t.Name)
            .ToArray(), LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<Dictionary<string, TEnum>> FromNames =
        new(() => EnumOptions.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private static readonly Lazy<Dictionary<int, TEnum>> FromValues =
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

    public override string ToString() => Name;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Value.GetHashCode();

    public bool Equals(EnumBase<TEnum>? other)
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
    public int CompareTo(EnumBase<TEnum>? other)
    {
        return Value.CompareTo(other?.Value);
    }

    public override bool Equals(object? obj) => obj is EnumBase<TEnum> other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(EnumBase<TEnum> left, EnumBase<TEnum> right)
    {


        // Equals handles null on right side
        return left.Equals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(EnumBase<TEnum> left, EnumBase<TEnum> right) =>
        !(left == right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(EnumBase<TEnum> left, EnumBase<TEnum> right) =>
        left.CompareTo(right) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(EnumBase<TEnum> left, EnumBase<TEnum> right) =>
        left.CompareTo(right) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(EnumBase<TEnum> left, EnumBase<TEnum> right) =>
        left.CompareTo(right) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(EnumBase<TEnum> left, EnumBase<TEnum> right) =>
        left.CompareTo(right) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(EnumBase<TEnum> enumBase) => enumBase.Value;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumBase<TEnum>(int value) =>
        FromValue(value);

    #endregion



}