using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Members
{
    /// <summary>
     /// 会员帐号类型
     /// </summary>
    public enum MemberType
    {
        /// <summary>
        /// 未认证会员
        /// </summary>
        [Description("未认证会员")]
        Unauthorized = 0,
        /// <summary>
        /// 认证会员
        /// </summary>
        [Description("认证会员")]
        Authentication = 1,
        /// <summary>
        /// 医生
        /// </summary>
        [Description("医生")]
        Doctor = 2,
        /// <summary>
        /// 主任
        /// </summary>
        [Description("主任")]
        Director = 3
    }
}
