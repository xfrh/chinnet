using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SystemSet
{
    /// <summary>
     /// 数据来源： 1：后台  2：微信 
     /// </summary>
    public enum ValidateCodeSource
    {
        [Description("后台")]
        Admin = 1,
        [Description("微信")]
        Weixin = 2,
        [Description("PC")]
        PC = 3
    }


}
