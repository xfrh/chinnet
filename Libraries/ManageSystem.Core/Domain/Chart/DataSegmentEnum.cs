using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Chart
{
    /// <summary>
    /// 数据段所属项目类型
    /// </summary>
    public enum DataSegmentEnum : byte
    {
        /// <summary>
        /// 热图管理
        /// </summary>
        [Description("热图管理")]
        Heatmap = 1,

        /// <summary>
        /// 柱状图管理
        /// </summary>
        [Description("柱状图管理")]
        BarChart = 2,

        /// <summary>
        /// 趋势图管理
        /// </summary>
        [Description("趋势图管理")]
        TrendChart = 3,

        /// <summary>
        /// 上海柱状图管理
        /// </summary>
        [Description("上海柱状图管理")]
        SHBarChart = 4
    }
}
