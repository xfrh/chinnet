using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 实体类 ，数据库表名：Specimen 
	/// </summary>
	public partial class Specimen : BaseEntity
	{

		/// <summary>
		/// 标本名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 是否启用
		/// <summary>
		public bool Status { get; set; }


	}
}
