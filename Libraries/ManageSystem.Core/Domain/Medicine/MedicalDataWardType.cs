using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
	/// 实体类 ，数据库表名：MedicalDataWardType 
	/// </summary>
	public partial class MedicalDataWardType : BaseEntity
    {
        /// <summary>
        /// 科室类别名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 科室类别编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sort { get; set; }

    }
}
