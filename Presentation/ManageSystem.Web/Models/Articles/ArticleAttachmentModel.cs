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
	/// 模型类 ，数据库表名：ArticleAttachment 
	/// </summary>
	public partial class ArticleAttachmentModel : BaseEntityModel
	{

		/// <summary>
		/// 文件名称
		/// <summary>
		[HtmlDisplayAttribute("文件名称","文件名称")]
		public String Name { get; set; }

		/// <summary>
		/// 文件路径
		/// <summary>
		[HtmlDisplayAttribute("文件路径","文件路径")]
		public String Path { get; set; }

		/// <summary>
		/// 所属文章
		/// <summary>
		[HtmlDisplayAttribute("所属文章","所属文章")]
		public long ArticleId { get; set; }


		/// <summary>
		/// 文件类型
		/// <summary>
		[HtmlDisplayAttribute("文件类型","文件类型")]
		public Int32 Type { get; set; }



	}
}
