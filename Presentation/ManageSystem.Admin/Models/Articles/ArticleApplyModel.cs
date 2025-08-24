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
	/// 模型类 ，数据库表名：ArticleApply 
	/// </summary>
	 [Validator(typeof(ArticleApplyValidator))]
	public partial class ArticleApplyModel : BaseEntityModel
	{

		/// <summary>
		/// 姓名
		/// <summary>
		[HtmlDisplayAttribute("姓名","姓名")]
		public String Name { get; set; }

		/// <summary>
		/// 手机号码
		/// <summary>
		[HtmlDisplayAttribute("手机号码","手机号码")]
		public String Phone { get; set; }

		/// <summary>
		/// 所属文章
		/// <summary>
		[HtmlDisplayAttribute("文章编号", "文章编号")]
		public long ArticleId { get; set; }

        /// <summary>
        /// 文章名称
        /// </summary>
        [HtmlDisplayAttribute("文章名称", "文章名称")]
        public string ArticleName { get; set; }

        /// <summary>
        /// 用户备注
        /// <summary>
        [HtmlDisplayAttribute("用户备注","用户备注")]
		public String Remark { get; set; }


	}
}
