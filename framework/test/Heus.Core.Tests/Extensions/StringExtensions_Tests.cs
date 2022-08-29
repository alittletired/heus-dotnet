using Heus.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Core.Tests.Extensions
{
    public class StringExtensions_Tests
    {
        private enum MyEnum
        {
            MyValue1,
            MyValue2
        }
        [Fact]
        public void EnsureEndsWith_Test()
        {
            //Expected use-cases
            "Test".EnsureEndsWith('!').ShouldBe("Test!");
            "Test!".EnsureEndsWith('!').ShouldBe("Test!");
            @"C:\test\folderName".EnsureEndsWith('\\').ShouldBe(@"C:\test\folderName\");
            @"C:\test\folderName\".EnsureEndsWith('\\').ShouldBe(@"C:\test\folderName\");
            "Sarı".EnsureEndsWith('ı').ShouldBe("Sarı");

            //Case differences
            "TurkeY".EnsureEndsWith('y').ShouldBe("TurkeYy");
        }

        [Fact]
        public void EnsureEndsWith_CultureSpecific_Test()
        {
            using (CultureHelper.Use("tr-TR"))
            {
                "Kırmızı".EnsureEndsWith('I', StringComparison.CurrentCultureIgnoreCase).ShouldBe("Kırmızı");
            }
        }
        [Theory]
        [InlineData("Test", '~', "~Test")]
        [InlineData("~Test", '~', "~Test")]
        [InlineData("Turkey", 't', "tTurkey")]
        public void EnsureStartsWith_Test(string origin,char startChar,string dest)
        {
            //Expected use-cases
            origin.EnsureStartsWith(startChar).ShouldBe(dest);
          
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("helloWorld", "HelloWorld")]
        [InlineData("istanbul", "Istanbul")]
        public void ToPascalCase_Test(string origin, string dest)
        {
            origin.ToPascalCase().ShouldBe(dest);
        }

        [Fact]
        public void ToPascalCase_CurrentCulture_Test()
        {
            using (CultureHelper.Use("tr-TR"))
            {
                "istanbul".ToPascalCase(true).ShouldBe("İstanbul");
            }
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("HelloWorld", "helloWorld")]
        [InlineData("Istanbul", "istanbul")]
        public void ToCamelCase_Test(string origin, string dest)
        {
            origin.ToCamelCase().ShouldBe(dest);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("helloMoon", "hello-moon")]
        [InlineData("HelloWorld", "hello-world")]
        [InlineData("HelloIsparta", "hello-isparta")]
        [InlineData("ThisIsSampleText", "this-is-sample-text")]
        public void ToKebabCase_Test(string origin, string dest)
        {
            origin.ToKebabCase().ShouldBe(dest);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("helloMoon", "hello_moon")]
        [InlineData("HelloWorld", "hello_world")]
        [InlineData("HelloIsparta", "hello_isparta")]
        [InlineData("ThisIsSampleText", "this_is_sample_text")]
        public void ToSnakeCase_Test(string origin,string dest)
        {
            origin.ToSnakeCase().ShouldBe(dest);
        }

        [Fact]

        public void ToEnum_Test()
        {
            "MyValue1".ToEnum<MyEnum>().ShouldBe(MyEnum.MyValue1);
            "MyValue2".ToEnum<MyEnum>().ShouldBe(MyEnum.MyValue2);
        }

        [Theory]
        [InlineData("")]
        [InlineData("MyStringİ")]
        public void GetBytes_Test(string str)
        {
            var bytes = str.GetBytes();
            bytes.ShouldNotBeNull();
            bytes.Length.ShouldBeGreaterThanOrEqualTo(str.Length);
            Encoding.UTF8.GetString(bytes).ShouldBe(str);
        }

        [Theory]
        [InlineData("")]
        [InlineData("MyString")]
        public void GetBytes_With_Encoding_Test(string str)
        {
            var bytes = str.GetBytes(Encoding.ASCII);
            bytes.ShouldNotBeNull();
            bytes.Length.ShouldBeGreaterThanOrEqualTo(str.Length);
            Encoding.ASCII.GetString(bytes).ShouldBe(str);
        }
    }
}
