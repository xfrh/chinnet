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
	/// 模型类 ，数据库表名：UserRole
	/// </summary>
	 [Validator(typeof(UserRoleValidator))]
	public partial class UserRoleModel : BaseEntityModel
	{

		/// <summary>
		/// 角色id
		/// <summary>
		[HtmlDisplayAttribute("角色id","角色id")]
		public Int32 RoleId { get; set; }

		/// <summary>
		/// 用户id
		/// <summary>
		[HtmlDisplayAttribute("用户id","用户id")]
		public Int32 UserinfoId { get; set; }



	}
}
