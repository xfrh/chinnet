using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Articles
{
	/// <summary>
	/// 实体类 ，数据库表名：ArticleAttachment 
	/// </summary>
	public partial class ArticleAttachment : BaseEntity
	{

		/// <summary>
		/// 文件名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 文件路径
		/// <summary>
		public String Path { get; set; }
		/// <summary>
		/// 所属文章
		/// <summary>
		public long ArticleId { get; set; }
		/// <summary>
		/// 文件类型
		/// <summary>
		public Int32 Type { get; set; }

	}
}
