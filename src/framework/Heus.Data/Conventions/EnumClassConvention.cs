// using Heus.Core.Common;
// using Heus.Data.EfCore.ValueConverters;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore.Metadata.Conventions;
//
// namespace Heus.Data.Conventions;
//
// public class EnumClassConvention : IEntityTypeAddedConvention
// {
//
//     public void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder,
//         IConventionContext<IConventionEntityTypeBuilder> context)
//     {
//         var props = entityTypeBuilder.Metadata.ClrType.GetProperties();
//         foreach (var property in props)
//         {
//
//             if (IsDerived(property.PropertyType, typeof(EnumClass<>)))
//             {
//                 var converterType = typeof(EnumClassValueConverter<>).MakeGenericType(property.PropertyType);
//                
//                 entityTypeBuilder.Property(property.PropertyType,property.Name)!.HasConversion(converterType);
//                 // property.Builder.HasConversion(converterType);
//             }
//
//
//         }
//     }
//
//     private static bool IsDerived(Type objectType, Type mainType)
//     {
//         Type? currentType = objectType.BaseType;
//
//         if (currentType == null)
//         {
//             return false;
//         }
//
//         while (currentType != typeof(object))
//         {
//             if (currentType?.IsGenericType==true && currentType.GetGenericTypeDefinition() == mainType)
//                 return true;
//
//             currentType = currentType?.BaseType;
//         }
//
//         return false;
//     }
// }