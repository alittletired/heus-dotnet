using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Core.Tests.Extensions
{
    public class ObjectExtensions_Tests
    {
        [Fact]
        public void As_Test()
        {
            var obj = (object)new ObjectExtensions_Tests();
            obj.As<ObjectExtensions_Tests>().ShouldNotBe(null);

            obj = null;
            obj.As<ObjectExtensions_Tests>().ShouldBe(null);
        }
        [Fact]
        public void ConvertTo_Tests()
        {
            "42".ConvertTo<int>().ShouldBeOfType<int>().ShouldBe(42);

            "28173829281734".ConvertTo<long>().ShouldBeOfType<long>().ShouldBe(28173829281734);

            "2.0".ConvertTo<double>().ShouldBe(2.0);
            "0.2".ConvertTo<double>().ShouldBe(0.2);
            (2.0).ConvertTo<int>().ShouldBe(2);

            "false".ConvertTo<bool>().ShouldBeOfType<bool>().ShouldBe(false);
            "True".ConvertTo<bool>().ShouldBeOfType<bool>().ShouldBe(true);
            "False".ConvertTo<bool>().ShouldBeOfType<bool>().ShouldBe(false);
            "TrUE".ConvertTo<bool>().ShouldBeOfType<bool>().ShouldBe(true);

            Assert.Throws<FormatException>(() => "test".ConvertTo<bool>());
            Assert.Throws<FormatException>(() => "test".ConvertTo<int>());

            "2260AFEC-BBFD-42D4-A91A-DCB11E09B17F".ConvertTo<Guid>().ShouldBeOfType<Guid>().ShouldBe(new Guid("2260afec-bbfd-42d4-a91a-dcb11e09b17f"));
        }
        [Fact]
        public void IsIn_Test()
        {
            5.IsIn(1, 3, 5, 7).ShouldBe(true);
            6.IsIn(1, 3, 5, 7).ShouldBe(false);

            int? number = null;
            number.IsIn(2, 3, 5).ShouldBe(false);

            var str = "a";
            str.IsIn("a", "b", "c").ShouldBe(true);

            str = null;
            str.IsIn("a", "b", "c").ShouldBe(false);
        }

    }
}
