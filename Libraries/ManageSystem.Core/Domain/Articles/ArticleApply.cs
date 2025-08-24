using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Articles
{
	/// <summary>
	/// 实体类 ，数据库表名：ArticleApply 
	/// </summary>
	public partial class ArticleApply : BaseEntity
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
        /// 姓名
        /// <summary>
        public String Name { get; set; }
		/// <summary>
		/// 手机号码
		/// <summary>
		public String Phone { get; set; }
		/// <summary>
		/// 所属文章
		/// <summary>
		public long ArticleId { get; set; }
		/// <summary>
		/// 用户备注
		/// <summary>
		public String Remark { get; set; }


        /// <summary>
        /// 文章名称
        /// </summary>
        public string ArticleName { get; set; }

    }
}
