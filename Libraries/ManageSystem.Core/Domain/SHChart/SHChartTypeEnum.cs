using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SHChart
{
    public enum SHChartTypeEnum : byte
    {
        /// <summary>
        /// 地图
        /// </summary>
        [Description("地图")]
        ChinaMap = 1,
        /// <summary>
        /// 柱状图
        /// </summary>
        [Description("柱状图")]
        Bar = 2,
        /// <summary>
        /// 折线图
        /// </summary>
        [Description("折线图")]
        Line = 3
    }
}
