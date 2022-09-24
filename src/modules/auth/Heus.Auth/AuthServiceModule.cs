using Heus.Auth.Entities;
using Heus.Core.DependencyInjection;
using Heus.Ddd;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth
{
    [DependsOn(typeof(DddServiceModule))]
    internal class AuthServiceModule: ServiceModuleBase
    {
   
    }
}
