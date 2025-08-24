using ManageSystem.Core.Domain.SHChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.SHChart
{
    public class SHDataSegmentDTO
    {

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// 数据段名称
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
        /// 数据类型
        /// </summary>
        public string Data_type { get; set; }
        /// <summary>
        /// 数据段所属项目
        /// </summary>
        public SHDataSegmentEnum ProjectType { get; set; }

    }
}