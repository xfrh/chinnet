using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 实体类 ，数据库表名：MedicalOrganism 
	/// </summary>
	public partial class MedicalOrganism : BaseEntity
	{

		/// <summary>
		/// 细菌名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 细菌类型
		/// <summary>
		public String Type { get; set; }
		/// <summary>
		/// 细菌编码
		/// <summary>
		public String Code { get; set; }
		/// <summary>
		/// 说明
		/// <summary>
		public String Remark { get; set; }
		/// <summary>
		/// 所属细菌类型，一级分类
		/// <summary>
		public long OrganismTypeId1 { get; set; }
		/// <summary>
		/// 所属细菌类型，二级分类
		/// <summary>
		public long OrganismTypeId2 { get; set; }
		/// <summary>
		/// 排序字段
		/// <summary>
		public Int32 Sort { get; set; }

        /// <summary>
        /// 细菌所属分组名称
        /// </summary>
        public string GroupName { get; set; }

    }
}
