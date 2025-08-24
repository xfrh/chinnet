using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SHChart
{
    public enum ChartSHDataItemType : byte
    {
        /// <summary>
        /// 单数据
        /// </summary>
        [Description("单数据")]
        SingleData = 1,
        /// <summary>
        /// 多数据
        /// </summary>
        [Description("多数据")]
        MultipleData = 2,
        /// <summary>
        /// 表格数据
        /// </summary>
        [Description("表格")]
         TableData = 3
    }
    /// <summary>
    /// 柱状图管理
    /// </summary>
    /// <remarks>
    /// 数据库表名：Chart_SHBarChart
    /// </remarks>
    public class Chart_SHBarChart:BaseEntity
    {
        /// <summary>
        /// 数据段ID
        /// </summary>
        public long DataSegmentId { get; set; }
        /// <summary>
        /// 报表名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 报表标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 报表副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 排序编号<br />
        /// 最小值为：1，正序排列
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// PC端或移动端是否默认显示
        /// </summary>
        public bool Default { get; set; }
        /// <summary>
        /// 数值单位<br />
        /// 默认：%
        /// </summary>
        public string Unit { get; set; } = "%";
        /// <summary>
        /// 移动端显示比例<br />
        /// 单位：%
        /// </summary>
        public double MobileDisplayScale { get; set; }
        /// <summary>
        /// 报表数据类型<br />
        /// 单数据/多数据
        /// </summary>
        public ChartSHDataItemType DataItemType { get; set; }
        /// <summary>
        /// 抗生素集合<br />
        /// 仅报表数据类型为【多数据】时存在
        /// </summary>
        public string Antibiotics { get; set; } = null;
    }
}
