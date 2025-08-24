using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 实体类 ，数据库表名：MedicalAntibiotic 
	/// </summary>
	public partial class MedicalAntibiotic : BaseEntity
	{
        /// <summary>
        /// 抗生素名称（中文）
        /// <summary>
        public String Name { get; set; }

        /// <summary>
        /// 抗生素名称（英文）
        /// <summary>
        public String NameEn { get; set; }

        /// <summary>
        /// 抗生素编码
        /// <summary>
        public String Code { get; set; }

		/// <summary>
		/// 说明
		/// <summary>
		public String Remark { get; set; }

	}
}
