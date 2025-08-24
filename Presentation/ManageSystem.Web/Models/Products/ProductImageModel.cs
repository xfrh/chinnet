using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Web.Models.Products
{
	/// <summary>
	/// 模型类 ，数据库表名：ProductImage 
	/// </summary>
	public partial class ProductImageModel : BaseEntityModel
	{

		/// <summary>
		/// 原始图片名称
		/// <summary>
		[HtmlDisplayAttribute("原始图片名称","原始图片名称")]
		public String OldFileName { get; set; }

		/// <summary>
		/// 新文件名称，上传以后在服务器上的文件名称
		/// <summary>
		[HtmlDisplayAttribute("新文件名称，上传以后在服务器上的文件名称","新文件名称，上传以后在服务器上的文件名称")]
		public String NewFileName { get; set; }

		/// <summary>
		/// 图片路径，相对路径
		/// <summary>
		[HtmlDisplayAttribute("图片路径，相对路径","图片路径，相对路径")]
		public String Path { get; set; }

		/// <summary>
		/// 排序，正序
		/// <summary>
		[HtmlDisplayAttribute("排序，正序","排序，正序")]
		public Int32 Sort { get; set; }

		/// <summary>
		/// 是否主图  0：否  1：是，一个商品只能有一个主图
		/// <summary>
		[HtmlDisplayAttribute("是否主图  0：否  1：是，一个商品只能有一个主图","是否主图  0：否  1：是，一个商品只能有一个主图")]
		public bool IsMain { get; set; }

		/// <summary>
		/// 商品Id
		/// <summary>
		[HtmlDisplayAttribute("商品Id","商品Id")]
		public long ProductId { get; set; }



	}
}
