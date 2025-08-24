using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Products;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Products
{
	/// <summary>
	/// 模型类 ，数据库表名：ProductBrand 
	/// </summary>
	 [Validator(typeof(ProductBrandValidator))]
	public partial class ProductBrandModel : BaseEntityModel
	{

		/// <summary>
		/// 品牌名称
		/// <summary>
		[HtmlDisplayAttribute("品牌名称","品牌名称")]
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
		/// 上级品牌
		/// <summary>
		[HtmlDisplayAttribute("上级品牌","上级品牌")]
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
