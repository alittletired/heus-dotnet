using Heus.Core;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Heus.Data.EfCore.ValueConverters;

public class EnumValueConverter<TEnum> : ValueConverter<TEnum, int>
    where TEnum : EnumClass<TEnum>
{

    public EnumValueConverter() : base(v => v.Value,
        v => EnumClass<TEnum>.FromValue(v))
    {
    }
}

