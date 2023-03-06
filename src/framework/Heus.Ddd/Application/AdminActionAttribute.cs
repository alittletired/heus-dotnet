using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ddd.Application;
/// <summary>
/// 描述一个功能点的编号和名称，用于标记 AppService 中的方法。
/// </summary>
public class AdminActionAttribute:Attribute
{

    /// <summary>
    /// 功能点编号。
    /// </summary>
    public string ActionCode { get;  }
    /// <summary>
    /// 功能点名称。
    /// </summary>
    public string ActionName { get;  }
    public string Description { get;  }
    public AdminActionAttribute(string actionCode,string actionName,string description="")
    {
        ActionCode = actionCode;
        ActionName = actionName;
        Description = description;
    }

   
   
}
