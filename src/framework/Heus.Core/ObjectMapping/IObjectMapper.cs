using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Core.ObjectMapping
{
    public interface IObjectMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
       
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
