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
	/// 模型类 ，数据库表名：ArticleView 
	/// </summary>
	 [Validator(typeof(ArticleViewValidator))]
	public partial class ArticleViewModel : BaseEntityModel
	{

		/// <summary>
		/// 查看用户
		/// <summary>
		[HtmlDisplayAttribute("查看用户","查看用户")]
		public long MemberId { get; set; }

		/// <summary>
		/// 用户姓名
		/// <summary>
		[HtmlDisplayAttribute("查看人姓名", "查看人姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 文章编号
		/// <summary>
		[HtmlDisplayAttribute("文章编号","文章编号")]
		public long ArticleId { get; set; }


		/// <summary>
		/// 文章名称
		/// <summary>
		[HtmlDisplayAttribute("文章名称","文章名称")]
		public String ArticleName { get; set; }

	}
}
