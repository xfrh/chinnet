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
    /// 报表名称： 医学数据统计 --  细菌在标本中占比
    /// </summary>
    public class MedicalByBacteriaInSpecimen
    {
        /// <summary>
        /// 标本类型id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 标本类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标本类型编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 总计细菌数量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 细菌在总的统计数据中的占比
        /// </summary>
        public Decimal Ratio { get; set; }
    }
}
