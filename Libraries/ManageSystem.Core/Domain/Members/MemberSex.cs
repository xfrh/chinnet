using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Members
{

    /// <summary>
    /// 用户帐号性别
    /// </summary>
    public enum MemberSex
    {
        /// <summary>
        /// 保密
        /// </summary>
        [Description("保密")]
        Unknown = 3,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female = 2
    }
    
    /// <summary>
    /// 项目类型枚举
    /// </summary>
    public enum MemberProjectType
    {
        /// <summary>
        /// CHINET数据云
        /// </summary>
        [Description("CHINET数据云")]
        CHINET= 0,
        /// <summary>
        /// SUGAR多中心
        /// </summary>
        [Description("SUGAR多中心")]
        SUGAR= 1,
        /// <summary>
        /// CRAB多中心
        /// </summary>
        [Description("CRAB多中心")]
        XACDURO = 2,
        /// <summary>
        /// ERA多中心
        /// </summary>
        [Description("ERA多中心")]
        MDR=3,
        /// <summary>
        /// CRE多中心
        /// </summary>
        [Description("CRE多中心")]
        CRE=4,
    }
}
