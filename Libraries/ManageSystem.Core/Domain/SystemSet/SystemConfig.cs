using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
	/// <summary>
	/// 实体类 ，数据库表名：SystemConfig 
	/// </summary>
	public partial class SystemConfig : BaseEntity
	{

		/// <summary>
		/// 配置名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 配置关键字
		/// <summary>
		public String Key { get; set; }
		/// <summary>
		/// 配置的值
		/// <summary>
		public String Value { get; set; }
		/// <summary>
		/// 是否可以操作
		/// <summary>
		public bool IsUser { get; set; }


	}
}
