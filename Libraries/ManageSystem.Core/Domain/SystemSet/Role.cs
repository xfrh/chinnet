using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
	/// <summary>
	/// 实体类 ，数据库表名：Role 
	/// </summary>
	public partial class Role : BaseEntity
	{

		/// <summary>
		/// 角色名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序编号
		/// <summary>
		public Int32 Sort { get; set; }
		/// 创建人姓名
		/// <summary>
		public String CreateName { get; set; }

		public string SatelliteId { get; set; }
	}
}
