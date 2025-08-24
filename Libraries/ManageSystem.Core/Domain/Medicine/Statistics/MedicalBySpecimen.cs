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
    /// 报表名称： 医学数据统计 -- 按标本类型统计
    /// </summary>
    public class MedicalBySpecimen
    {
        /// <summary>
        /// 标本类型id
        /// </summary>
        public long SpecimenId { get; set; }

        /// <summary>
        /// 标本类型名称
        /// </summary>
        public string SpecimenName { get; set; }

        /// <summary>
        /// 医学数据数量
        /// </summary>
        public int MedicalCount { get; set; }

    }
}
