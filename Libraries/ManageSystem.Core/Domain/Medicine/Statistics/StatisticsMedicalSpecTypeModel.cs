using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine.Statistics
{
    /// <summary>
    /// 统计数据，分离菌在各类标本中的分布
    /// </summary>
    public class StatisticsMedicalSpecTypeModel
    {
        /// <summary>
        /// 标本名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 统计的总数据量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 所占比例，保留2位小数
        /// </summary>
        public decimal Ratio { get; set; }

    }
}
