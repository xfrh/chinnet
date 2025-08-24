using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Chart
{
    /// <summary>
    /// 数据段
    /// </summary>
    /// <remarks>
    /// 数据库表名：CRProject 
    /// </remarks>
    public class Chart_DataSegment : BaseEntity
    {
        /// <summary>
        /// 数据段所属项目类型
        /// </summary>
        public DataSegmentEnum ProjectType { get; set; }

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
    }
}
