using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Medicine;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Medicine
{
	/// <summary>
	/// 模型类 ，数据库表名：Specimen 
	/// </summary>
	 [Validator(typeof(SpecimenValidator))]
	public partial class SpecimenModel : BaseEntityModel
	{

		/// <summary>
		/// 标本名称
		/// <summary>
		[HtmlDisplayAttribute("标本名称","标本名称")]
		public String Name { get; set; }

		/// <summary>
		/// 排序
		/// <summary>
		[HtmlDisplayAttribute("排序","排序")]
		public Int32 Sort { get; set; }

		/// <summary>
		/// 是否启用
		/// <summary>
		[HtmlDisplayAttribute("是否启用","是否启用")]
		public bool Status { get; set; }

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


        /// <summary>
        /// 备注信息
        /// <summary>
        [HtmlDisplayAttribute("备注信息","备注信息")]
		public String Describe { get; set; }



	}
}
