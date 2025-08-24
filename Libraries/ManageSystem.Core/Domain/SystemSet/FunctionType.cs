using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SystemSet
{


    /// <summary>
     /// 系统功能类型
     /// </summary>
    public enum FunctionType
    {
        [Description("页面")]
        Page = 1,
        [Description("按钮")]
        Button = 2,
        [Description("页面/按钮")]
        Mixture = 3
    }

   

}
