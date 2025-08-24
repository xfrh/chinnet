using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Configuration
{
    /// <summary>
     ///微信帐号类型
     /// </summary>
    public enum WeixinAccountType
    {
        [Description("未认证订阅号")]
        NoSubscribe = 1,
        [Description("认证订阅号")]
        Subscribe = 2,
        [Description("未认证服务号")]
        NoServe = 3,
        [Description("认证服务号")]
        Serve = 4,
        [Description("企业号")]
        Enterprise = 5
    }

}
