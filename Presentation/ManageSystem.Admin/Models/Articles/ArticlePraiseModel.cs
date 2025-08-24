using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Articles;

namespace ManageSystem.Admin.Models.Articles
{
	/// <summary>
	/// 模型类 ，数据库表名：ArticlePraise 
	/// </summary>
	 [Validator(typeof(ArticlePraiseValidator))]
	public partial class ArticlePraiseModel : BaseEntityModel
	{

		/// <summary>
		/// 点赞用户
		/// <summary>
		[HtmlDisplayAttribute("点赞用户","点赞用户")]
		public long MemberId { get; set; }

		/// <summary>
		/// 点赞姓名
		/// <summary>
		[HtmlDisplayAttribute("点赞姓名","点赞姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 所属文章
		/// <summary>
		[HtmlDisplayAttribute("文章Id","所属文章的Id")]
		public long ArticleId { get; set; }

        /// <summary>
        /// 文章名称
        /// </summary>
        [HtmlDisplayAttribute("文章名称", "文章名称")]
        public string ArticleName { get; set; }


    }
}
