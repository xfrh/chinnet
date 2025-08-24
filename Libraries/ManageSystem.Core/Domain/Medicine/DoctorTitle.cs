using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 实体类 ，数据库表名：DoctorTitle 
	/// </summary>
	public partial class DoctorTitle : BaseEntity
	{

		/// <summary>
		/// 职称名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序，正序
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 是否启用
		/// <summary>
		public bool Status { get; set; }
		/// <summary>
		/// 上级职称
		/// <summary>
		public long ParentId { get; set; }


	}
}
