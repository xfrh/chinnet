using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Setting;

namespace ManageSystem.Admin.Models.Setting
{
	/// <summary>
	/// 模型类 ，数据库表名：Menu 
	/// </summary>
	 [Validator(typeof(MenuValidator))]
	public partial class MenuModel : BaseEntityModel
	{

		/// <summary>
		/// 日志标题
		/// <summary>
		[HtmlDisplayAttribute("日志标题","日志标题")]
		public String Title { get; set; }

		/// <summary>
		/// 日志等级
		/// <summary>
		[HtmlDisplayAttribute("日志等级","日志等级")]
		public String Level { get; set; }

		/// <summary>
		/// 出错类
		/// <summary>
		[HtmlDisplayAttribute("出错类","出错类")]
		public String Logger { get; set; }

		/// <summary>
		/// 错误详细内容
		/// <summary>
		[HtmlDisplayAttribute("错误详细内容","错误详细内容")]
		public String Message { get; set; }

		/// <summary>
		/// 创建时间
		/// <summary>
		[HtmlDisplayAttribute("创建时间","创建时间")]
		public DateTime CreateTime { get; set; }



	}
}
