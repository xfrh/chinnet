using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Products
{
	/// <summary>
	/// 模型类 ，数据库表名：ProductCategory 
	/// </summary>
	public partial class ProductCategoryModel : BaseEntityModel
	{

		/// <summary>
		/// 分类名称
		/// <summary>
		[HtmlDisplayAttribute("分类名称","分类名称")]
		public String Name { get; set; }

		/// <summary>
		/// 排序，正序
		/// <summary>
		[HtmlDisplayAttribute("排序","排序，正序")]
		public Int32 Sort { get; set; }

		/// <summary>
		/// 是否启用  0：禁用  1：启用
		/// <summary>
		[HtmlDisplayAttribute("是否启用","是否启用  0：禁用  1：启用")]
		public bool Status { get; set; }

		/// <summary>
		/// 上级分类
		/// <summary>
		[HtmlDisplayAttribute("上级分类","上级分类")]
		public long ParentId { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
        public string StateValue { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
        public IList<SelectListItem> StateList { get; set; }

    }
}
