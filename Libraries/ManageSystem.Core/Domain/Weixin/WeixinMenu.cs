using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Weixin
{
	/// <summary>
	/// 实体类 ，数据库表名：WeixinMenu 
	/// </summary>
	public partial class WeixinMenu : BaseEntity
	{

		/// <summary>
		/// 菜单名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 菜单类型
		/// <summary>
		public Int32 Type { get; set; }
		/// <summary>
		/// 菜单关键字
		/// <summary>
		public String Key { get; set; }
		/// <summary>
		/// 排序编号
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 上级id
		/// <summary>
		public long ParentId { get; set; }
		/// <summary>
		/// 菜单值
		/// <summary>
		public String Value { get; set; }


	}
}
