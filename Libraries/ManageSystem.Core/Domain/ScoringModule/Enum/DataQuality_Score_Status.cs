using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule.Enum
{
    /// <summary>
    /// 自动评分状态
    /// </summary>
    public enum DataQuality_Score_Status : int
    {
        /// <summary>
        /// 待开始
        /// </summary>
        [Description("待开始")]
        NotStarted = 1,

        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Underway = 2,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Finish = 5,

        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancel = 0
    }
}
