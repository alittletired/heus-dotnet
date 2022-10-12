

namespace Heus.Ddd.Qeury;

internal interface IQueryFilter
{
   
    public string? EntityAliasName { get; set; }
    public string OperatorType { get; set; } 
}
