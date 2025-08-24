using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Chart
{
    /// <summary>
    ///趋势图管理
    /// </summary>
    /// <remarks>
    /// 数据库表名：Chart_TrendChartWithItemData
    /// </remarks>
    public class Chart_TrendChartWithItemData : BaseEntity
    {
        /// <summary>
        /// 数据报表ID<br />
        /// Chart_TrendChart.Id
        /// </summary>
        public long TrendChartId { get; set; }
        /// <summary>
        /// 报表类型
        /// </summary>
        public ChartTypeEnum ChartType { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string ChartColor { get; set; }
        /// <summary>
        /// 数据项名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
