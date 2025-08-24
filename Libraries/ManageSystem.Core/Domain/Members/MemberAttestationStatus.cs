using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Members
{

    /// <summary>
    /// 用户认证申请状态
    /// </summary>
    public enum MemberAttestationStatus
    {
        [Description("待审核")]
        WaitCheck = 1,
        [Description("完成")]
        Finish = 2,
        [Description("取消")]
        Cancel = 3
    }

}
