using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SHChart
{
    /// <summary>
    /// 数据段所属项目类型
    /// </summary>
    public enum SHDataSegmentEnum : byte
    {
       
        /// <summary>
        /// 上海柱状图管理
        /// </summary>
        [Description("上海往图表管理")]
        SHBarChart = 1
    }
}
