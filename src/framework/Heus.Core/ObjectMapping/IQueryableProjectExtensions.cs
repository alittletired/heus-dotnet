using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Core.ObjectMapping
{
    internal  static class IQueryableProjectExtensions
    {
        public static TDestination MapTo<TDestination>(this IQueryable query) { 
            return query.ProjectTo<TDestination>(); }
    }
}
