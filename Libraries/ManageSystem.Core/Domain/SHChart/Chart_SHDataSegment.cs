using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SHChart
{
    public class Chart_SHDataSegment: BaseEntity
    {
        /// <summary>
        /// 数据段所属项目类型
        /// </summary>
        public SHDataSegmentEnum ProjectType { get; set; }

        /// <summary>
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
    }
}
