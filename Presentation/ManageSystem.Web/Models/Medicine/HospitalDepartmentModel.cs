using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Medicine
{
	/// <summary>
	/// 模型类 ，数据库表名：HospitalDepartment 
	/// </summary>
	public partial class HospitalDepartmentModel : BaseEntityModel
	{

		/// <summary>
		/// 科室名称
		/// <summary>
		[HtmlDisplayAttribute("科室名称","科室名称")]
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



    }
}
