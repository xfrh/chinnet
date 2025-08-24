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
	/// 模型类 ，数据库表名：Hospital 
	/// </summary>
	public partial class HospitalModel : BaseEntityModel
	{

		/// <summary>
		/// 医院名称
		/// <summary>
		[HtmlDisplayAttribute("医院名称","医院名称")]
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
		public bool State { get; set; }

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
        /// 医院地址
        /// <summary>
        [HtmlDisplayAttribute("医院地址","医院地址")]
		public String Address { get; set; }

		/// <summary>
		/// 医院介绍
		/// <summary>
		[HtmlDisplayAttribute("医院介绍","医院介绍")]
		public String Content { get; set; }

		/// <summary>
		/// 联系人
		/// <summary>
		[HtmlDisplayAttribute("联系人","联系人")]
		public String ContactsUser { get; set; }

		/// <summary>
		/// 联系方式
		/// <summary>
		[HtmlDisplayAttribute("联系方式","联系方式")]
		public String ContactsTel { get; set; }

		/// <summary>
		/// 备注信息
		/// <summary>
		[HtmlDisplayAttribute("备注信息","备注信息")]
		public String Describe { get; set; }



	}
}
