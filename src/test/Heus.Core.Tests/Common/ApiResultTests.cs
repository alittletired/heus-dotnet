﻿using Heus.Core.Common;

namespace Heus.Core.Tests.Common;
[TestClass]
public class ApiResultTests
{
    [TestMethod]
    public void Test_Error()
    {

        var result= ApiResult.Error(new Exception("error"));
        result.Code.ShouldBe(500);
        result.Message.ShouldNotBeNull();

    
    }
    [TestMethod]
    public void Test_Ok()
    {
        var ok = ApiResult.Ok(new List<string>());
        ok.Code.ShouldBe(0);
        ok.Data.ShouldNotBeNull();
    }
}