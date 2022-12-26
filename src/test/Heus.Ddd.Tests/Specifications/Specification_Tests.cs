
using System.Linq.Expressions;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
using Heus.Ddd.Specifications;

namespace Heus.Ddd.Tests.Specifications;

internal class Eq1Specification1:Specification<int>
{
    public override Expression<Func<int, bool>> ToExpression()
    {
        return i => i == 1;
    }
}
internal class Eq2Specification1:Specification<int>
{
    public override Expression<Func<int, bool>> ToExpression()
    {
        return i => i == 2;
    }
}
internal class Not0Specification1:Specification<int>
{
    public override Expression<Func<int, bool>> ToExpression()
    {
        return i => i !=0;
    }
}


public class Specification_Tests
{
    [Fact]
    public void AndNotSpecification_Test()
    {
        var eq1 = new Eq1Specification1();
        var eq2 = new Eq2Specification1();
        var not0 = new Not0Specification1();
        eq1.AndNot(eq2).IsSatisfiedBy(1).ShouldBeTrue();
        eq1.AndNot(eq1).IsSatisfiedBy(1).ShouldBeFalse();

       
        
        eq1.And(eq2).IsSatisfiedBy(1).ShouldBeFalse();
        eq1.And(not0).IsSatisfiedBy(1).ShouldBeTrue();
        
        eq1.Or(eq2).IsSatisfiedBy(1).ShouldBeTrue();
        eq1.Or(eq2).IsSatisfiedBy(2).ShouldBeTrue();
        eq1.Or(eq2).IsSatisfiedBy(3).ShouldBeFalse();

        eq1.Not().IsSatisfiedBy(1).ShouldBeFalse();
        

        var anySpec = new AnySpecification<int>();
        anySpec.IsSatisfiedBy(1).ShouldBeTrue();
        var noneSpec = new NoneSpecification<int>();
        noneSpec.IsSatisfiedBy(1).ShouldBeFalse();

        Expression<Func<int, bool>> expr = ( i) => i == 1;
        var es = new ExpressionSpecification<int>(expr);
        es.IsSatisfiedBy(1).ShouldBeTrue();

        eq1.ToExpression().ShouldNotBeNull();

    }
}
