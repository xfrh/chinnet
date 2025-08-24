using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
     /// 医学数据状态
     /// </summary>
    public enum MedicalDataStatusEnum
    {
        /// <summary>
        /// 医学数据状态: 有效
        /// </summary>
        [Description("已处理")]
        Effective = 1,
        /// <summary>
        /// 医学数据状态: 无效
        /// </summary>
        [Description("已处理")]
        Invalid = 2,
        /// <summary>
        /// 医学数据状态: 待处理
        /// </summary>
        [Description("待处理")]
        Wait = 3
    }
}
