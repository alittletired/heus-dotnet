using Heus.Core.Common;
using Heus.Data.EfCore.ValueConverters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Heus.Data.Conventions;

public class EnumClassConvention: IEntityTypeAddedConvention 
{
 
    public void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
    {
        var props = entityTypeBuilder.Metadata.GetDeclaredProperties();
        foreach (var property in props)
        {

            if (property.ClrType .IsGenericType && property.ClrType.GetGenericTypeDefinition() == typeof(EnumClass<>))
            {
                var converterType = typeof(EnumClassValueConverter<>).MakeGenericType(property.ClrType);
                property.Builder.HasConversion(converterType);
            }


        }
    }
}