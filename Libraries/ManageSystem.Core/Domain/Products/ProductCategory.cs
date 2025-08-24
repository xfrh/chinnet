using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Products
{
	/// <summary>
	/// 实体类 ，数据库表名：ProductCategory 
	/// </summary>
	public partial class ProductCategory : BaseEntity
	{

		/// <summary>
		/// 分类名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序，正序
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 是否启用  0：禁用  1：启用
		/// <summary>
		public bool Status { get; set; }
		/// <summary>
		/// 上级分类
		/// <summary>
		public long ParentId { get; set; }


	}
}
