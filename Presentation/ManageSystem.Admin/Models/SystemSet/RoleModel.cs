using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.SystemSet;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.SystemSet
{
	/// <summary>
	/// 模型类 ，数据库表名：Role 
	/// </summary>
	 [Validator(typeof(RoleValidator))]
	public partial class RoleModel : BaseEntityModel
	{

		/// <summary>
		/// 角色名称
		/// <summary>
		[HtmlDisplayAttribute("角色名称","角色名称")]
		public String Name { get; set; }

		/// <summary>
		/// 排序编号
		/// <summary>
		[HtmlDisplayAttribute("排序编号","排序编号")]
		public int? Sort { get; set; }

		/// <summary>
		/// 创建人姓名
		/// <summary>
		[HtmlDisplayAttribute("创建人姓名","创建人姓名")]
		public String CreateName { get; set; }

        /// <summary>
		/// 功能列表Html代码
		/// <summary>
		[HtmlDisplayAttribute("功能列表Html代码", "功能列表Html代码")]
        public String FunctionHtml { get; set; }

        /// <summary>
		/// 功能id集合，使用英文逗号分割
		/// <summary>
		[HtmlDisplayAttribute("功能id集合", "功能id集合")]
        public String FunctionIds { get; set; }

		[HtmlDisplayAttribute("所属卫星网", "所属卫星网")]
		public string SatelliteId { get; set; }
		/// <summary>
		/// 医生职称列表
		/// </summary>
		[HtmlDisplayAttribute("所属卫星网", "所属卫星网")]
		public IList<SelectListItem> SatelliteList { get; set; }
	}
}
