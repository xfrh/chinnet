using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.SHChart
{
    public class SHBarChartListDTO : SHDataSegmentDTO
    {
        public IEnumerable<SHBarChartItemDTO> DataItems { get; set; }
    }

    public class SHBarChartItemDTO
    {
        /// <summary>
        /// 报表ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 报表名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 主标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// 是否默认显示
        /// </summary>
        public bool Default { get; set; }
    }
}