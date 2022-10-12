

namespace Heus.Ddd.Qeury;

[AttributeUsage(AttributeTargets.Property)]
public class QueryFilterAttribute : Attribute, IQueryFilter
{
    public QueryFilterAttribute() { }
    public QueryFilterAttribute(string operatorType)
    {
        OperatorType = operatorType;
    }
    public string? PropertyName { get; set; }
    public string? EntityAliasName { get; set; }
    public string OperatorType { get; set; } = OperatorTypes.Equal;
}
