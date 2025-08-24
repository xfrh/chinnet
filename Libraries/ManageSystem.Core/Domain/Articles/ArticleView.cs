using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Articles
{
	/// <summary>
	/// 实体类 ，数据库表名：ArticleView 
	/// </summary>
	public partial class ArticleView : BaseEntity
	{

        /// <summary>
        /// 会员id
        /// <summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 会员姓名
        /// <summary>
        public String MemberName { get; set; }
        /// <summary>
        /// 文章编号
        /// <summary>
        public long ArticleId { get; set; }
		/// <summary>
		/// 文章名称
		/// <summary>
		public String ArticleName { get; set; }

	}
}
