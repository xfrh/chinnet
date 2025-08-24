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
	/// 模型类 ，数据库表名：DoctorTitle 
	/// </summary>
	 [Validator(typeof(DoctorTitleValidator))]
	public partial class DoctorTitleModel : BaseEntityModel
	{

		/// <summary>
		/// 职称名称
		/// <summary>
		[HtmlDisplayAttribute("职称名称","职称名称", true)]
		public String Name { get; set; }

		/// <summary>
		/// 排序，正序
		/// <summary>
		[HtmlDisplayAttribute("排序编号","排序，正序",true)]
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
        /// 上级职称
        /// <summary>
        [HtmlDisplayAttribute("上级职称","上级职称")]
		public long ParentId { get; set; }



	}
}
