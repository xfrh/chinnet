using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Web.Models.Articles
{
	/// <summary>
	/// 模型类 ，数据库表名：ArticleType 
	/// </summary>
	public partial class ArticleTypeModel : BaseEntityModel
	{

        /// <summary>
        /// 分类名称
        /// <summary>
        [HtmlDisplayAttribute("分类名称", "分类名称")]
		public String Name { get; set; }

		/// <summary>
		/// 排序
		/// <summary>
		[HtmlDisplayAttribute("排序","排序")]
		public Int32 Sort { get; set; }



	}
}
