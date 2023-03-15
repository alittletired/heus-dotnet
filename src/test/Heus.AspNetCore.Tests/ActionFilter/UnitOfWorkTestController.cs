using Heus.Ddd.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heus.AspNetCore.Tests.ActionFilter;
[Route("api/unitofwork-test")]
[AllowAnonymous]
public class UnitOfWorkTestController : Controller
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkTestController(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    [HttpGet]
    [UnitOfWork]
    [Route("UowWithoutTransaction")]
    public ActionResult UowWithoutTransactional()
    {
        _unitOfWorkManager.Current.ShouldNotBeNull();
        _unitOfWorkManager.Current.Options.IsTransactional.ShouldBeFalse();

        return Content("OK");
    }
    [HttpPost]
    [UnitOfWork]
    [Route("UowWithTransaction")]
    public ActionResult UowWithTransactional()
    {
        _unitOfWorkManager.Current.ShouldNotBeNull();
        _unitOfWorkManager.Current.Options.IsTransactional.ShouldBeTrue();
        return Content("OK");
    }
}
