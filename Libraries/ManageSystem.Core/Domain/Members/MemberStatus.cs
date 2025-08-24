using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Members
{

    /// <summary>
    /// 用户帐号状态
    /// </summary>
    public enum MemberStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        WaitCheck = 1,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 2,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Stop = 3
    }

}
