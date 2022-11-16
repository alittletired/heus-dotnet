using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace Heus.Data.EfCore.ValueConverters;

public class EnumClassValueConverter<TEnum> : ValueConverter<TEnum, int>
    where TEnum : EnumClass<TEnum>
{

    public EnumClassValueConverter() : base(v => v.Value,
        v => EnumClass<TEnum>.FromValue(v))
    {
    }
}

