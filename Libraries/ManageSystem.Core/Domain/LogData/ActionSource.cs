using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Log
{


    /// <summary>
     /// 日志来源  
     /// </summary>
    public enum ActionSource
    {
        [Description("后台")]
        Admin = 1,
        [Description("微信")]
        Weixin = 2,
        [Description("网站")]
        Web = 3,
        [Description("移动端")]
        Mobile = 4,
        [Description("InnerApi")]
        InnerApi = 5
    }

   

}
