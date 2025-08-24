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
	/// 
	/// </summary>
	 [Validator(typeof(PasswordValidator))]
	public partial class PasswordModel : BaseEntityModel
	{

		/// <summary>
		/// 原始密码
		/// <summary>
		[HtmlDisplayAttribute("原始密码", "原始密码")]
		public String OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// <summary>
        [HtmlDisplayAttribute("新密码", "新密码")]
		public String NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// <summary>
        [HtmlDisplayAttribute("确认密码", "确认密码")]
		public String ConfirmPassword { get; set; }

    }
}
