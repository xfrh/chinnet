using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine.Statistics
{
    /// <summary>
    /// 医学数据统计报表的实体数据
    /// 
    /// 报表名称： 医学数据统计 -- 按省份统计
    /// </summary>
    public class MedicalByArea
    {
        /// <summary>
        /// 省份id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 医学数据数量
        /// </summary>
        public int MedicalCount { get; set; }

    }
}
