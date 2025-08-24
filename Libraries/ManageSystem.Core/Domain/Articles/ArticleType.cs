using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Articles
{
	/// <summary>
	/// 实体类 ，数据库表名：ArticleType 
	/// </summary>
	public partial class ArticleType : BaseEntity
	{

		/// <summary>
		/// 类型名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序
		/// <summary>
		public Int32 Sort { get; set; }

	}
}
