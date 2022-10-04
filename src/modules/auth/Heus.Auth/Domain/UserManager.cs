using Heus.Auth.Entities;
using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Heus.Ddd.Entities;

namespace Heus.Auth.Domain;

public class UserManager:IScopedDependency
{

    public (bool,string) CheckUserState(User? user)
    {
        var userStatus = user switch
        {
            null => UserStatus.NotFound,
          
            _ => user.Status
        };
        if (userStatus != UserStatus.Normal)
        {
            return (false,"用户" + EnumHelper.GetSummary(userStatus)) ;
        }

        return (true, "");

    }
    
}