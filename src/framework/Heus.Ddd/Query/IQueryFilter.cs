

namespace Heus.Ddd.Query;

internal interface IQueryFilter
{
   
    public string? EntityAliasName { get; set; }
    public string OperatorType { get; set; } 
}
