using Heus.Core.ObjectMapping;
using Heus.TestBase;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Tests.ObjectMapping;

public class Object1
{
    public int A { get; set; }
    public int B { get; set; }
}
[ObjectMapping(typeof(Object1), MapType.MapTo)]
public class Object2
{
    public int B { get; set; }
}
[ObjectMapping(typeof(Object1), MapType.MapFrom)]
public class Object3
{
    public int B { get; set; }
}
[ObjectMapping(typeof(Object1), MapType.TwoWay)]
public class Object4
{
    public int B { get; set; }
}
[TestClass]
public class ObjectMappingTests : IntegratedTestBase<CoreModuleInitializer>
{
    private IObjectMapper ObjectMapper => ServiceProvider.GetRequiredService<IObjectMapper>();



    [TestMethod]
    public void MapToTest()
    {
        var object2 = new Object2() { B = 3 };
        ObjectMapper.Map<Object1>(object2).B.ShouldBe(object2.B);

    }
}