
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Heus.Ddd.Query;

internal class QueryFilterItem: IQueryFilter
{
  
    public string PropertyName { get; set; }
   
    public string OperatorType { get; set; }
    public object Value { get; set; }
    public string? EntityAliasName { get; set; }
    public QueryFilterItem(string propertyName,string opType, object value,string? alias=null)
    {
        PropertyName=propertyName; ;
        OperatorType = opType;
        Value = value;
        EntityAliasName = alias;
    }
}
