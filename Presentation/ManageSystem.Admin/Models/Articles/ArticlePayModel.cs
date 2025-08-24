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
	/// 模型类 ，数据库表名：ArticlePay 
	/// </summary>
	 [Validator(typeof(ArticlePayValidator))]
	public partial class ArticlePayModel : BaseEntityModel
	{

        /// <summary>
        /// 支付用户
        /// <summary>
        [HtmlDisplayAttribute("支付用户", "支付用户")]
		public long MemberId { get; set; }

		/// <summary>
		/// 用户姓名
		/// <summary>
		[HtmlDisplayAttribute("支付人姓名", "支付人姓名")]
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


		/// <summary>
		/// 付费金额
		/// <summary>
		[HtmlDisplayAttribute("付费金额","付费金额")]
		public Decimal Price { get; set; }

		/// <summary>
		/// 是否有效
		/// <summary>
		[HtmlDisplayAttribute("是否有效","是否有效")]
		public bool IsEffective { get; set; }



	}
}
