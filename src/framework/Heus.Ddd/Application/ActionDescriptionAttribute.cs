using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ddd.Application;
/// <summary>
/// 描述一个功能点的编号和名称，用于标记 AppService 中的方法。
/// </summary>
public class ActionDescriptionAttribute
{

    /// <summary>
    /// 功能点编号。
    /// </summary>
    public string ActionCode { get;  }

    public ActionDescriptionAttribute(string actionName, string actionCode)
    {
        ActionCode = actionCode;
        ActionName = actionName;
    }

    /// <summary>
    /// 功能点名称。
    /// </summary>
    public string ActionName { get;  }
}
