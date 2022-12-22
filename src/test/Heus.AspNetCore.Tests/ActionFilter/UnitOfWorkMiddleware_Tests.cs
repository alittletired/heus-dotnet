﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heus.AspNetCore.Tests.ActionFilter;
public class UnitOfWorkMiddleware_Tests : IClassFixture<AspNetCoreIntegratedTest>
{
    private readonly AspNetCoreIntegratedTest _factory;
    public UnitOfWorkMiddleware_Tests(AspNetCoreIntegratedTest factory)
    {
        _factory = factory;
    }
    [Fact]
    public async Task Get_Actions_Should_Not_Be_Transactional()
    {
        var res=await _factory.HttpClient.GetAsync("/api/unitofwork-test/UowWithoutTransaction");
        res.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task Non_Get_Actions_Should_Be_Transactional()
    {
        var result = await _factory.HttpClient.PostAsync("/api/unitofwork-test/UowWithTransaction", null);
        result.IsSuccessStatusCode.ShouldBeTrue();
    }
}