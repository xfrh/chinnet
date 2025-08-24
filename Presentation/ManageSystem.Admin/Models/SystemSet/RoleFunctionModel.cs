using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.SystemSet;

namespace ManageSystem.Admin.Models.SystemSet
{
	/// <summary>
	/// 模型类 ，数据库表名：RoleFunction 
	/// </summary>
	 [Validator(typeof(RoleFunctionValidator))]
	public partial class RoleFunctionModel : BaseEntityModel
	{

		/// <summary>
		/// 角色id
		/// <summary>
		[HtmlDisplayAttribute("角色id","角色id")]
		public Int32 RoleId { get; set; }

		/// <summary>
		/// 功能id
		/// <summary>
		[HtmlDisplayAttribute("功能id","功能id")]
		public Int32 FunctionId { get; set; }



	}
}
