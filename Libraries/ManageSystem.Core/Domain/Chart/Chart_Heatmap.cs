using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Chart
{
    /// <summary>
    /// 热图管理
    /// </summary>
    /// <remarks>
    /// 数据库表名：Chart_Heatmap
    /// </remarks>
    public class Chart_Heatmap : BaseEntity
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
        /// 排序
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
        /// 图标类型<br />
        /// 暂时取值有且仅有 1: 地图<br />
        /// 其他值无效
        /// </summary>
        public byte ChartType { get; set; }

        /// <summary>
        /// 卫星网上传
        /// </summary>
        public string SatelliteId { get; set; }
    }

    /// <summary>
    /// 热图数据项
    /// </summary>
    public class Chart_HeatmapItem : BaseEntity
    {
        /// <summary>
        /// 热图报表Id
        /// </summary>
        public long HeatmapId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
