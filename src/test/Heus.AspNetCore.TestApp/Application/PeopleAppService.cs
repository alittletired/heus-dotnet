using Heus.AspNetCore.TestApp.Domain;
using Heus.Ddd.Application;

namespace Heus.AspNetCore.TestApp.Application;

public  interface IPeopleAppService:IAdminApplicationService<Person>{}
public class PeopleAppService:AdminApplicationService<Person>,IPeopleAppService
{
    
}