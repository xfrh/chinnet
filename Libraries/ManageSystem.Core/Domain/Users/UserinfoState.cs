using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Users
{
    
    /// <summary>
    /// 用户帐号状态
    /// </summary>
    public enum UserinfoState
    {
        [Description("待审核")]
        WaitCheck = 1,
        [Description("正常")]
        Normal = 2,
        [Description("停用")]
        Stop = 3
    }

}
