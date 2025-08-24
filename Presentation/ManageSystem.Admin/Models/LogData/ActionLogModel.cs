using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Log;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Log
{
	/// <summary>
	/// 模型类 ，数据库表名：ActionLog 
	/// </summary>
	 [Validator(typeof(ActionLogValidator))]
	public partial class ActionLogModel : BaseEntityModel
	{

		/// <summary>
		/// 操作者浏览器名称
		/// <summary>
		[HtmlDisplayAttribute("浏览器名称","操作者浏览器名称")]
		public String BrowserName { get; set; }

		/// <summary>
		/// 操作的IP地址
		/// <summary>
		[HtmlDisplayAttribute("IP地址","操作的IP地址")]
		public String IPAddress { get; set; }

		/// <summary>
		/// 操作的用户id
		/// <summary>
		[HtmlDisplayAttribute("操作的用户id","操作的用户id")]
		public long UserinfoId { get; set; }

		/// <summary>
		/// 操作用户的姓名和登录名
		/// <summary>
		[HtmlDisplayAttribute("操作人","操作用户的姓名和登录名")]
		public String UserinfoName { get; set; }

		/// <summary>
		/// 日志内容
		/// <summary>
		[HtmlDisplayAttribute("日志内容","日志内容")]
		public String Content { get; set; }

		/// <summary>
		/// 日志类型
		/// <summary>
		[HtmlDisplayAttribute("日志类型","日志类型")]
		public String Type { get; set; }

        /// <summary>
        /// 日志类型
        /// <summary>
        [HtmlDisplayAttribute("日志类型", "日志类型")]
        public int TypeId { get; set; }

        /// <summary>
        /// 日志枚举列表
        /// </summary>
        [HtmlDisplayAttribute("日志类型", "日志类型")]
        public IList<SelectListItem> TypeList { get; set; }


        /// <summary>
        /// 日志来源
        /// <summary>
        [HtmlDisplayAttribute("日志来源", "日志来源")]
        public int Source { get; set; }

        /// <summary>
        /// 日志来源
        /// </summary>
        public IList<SelectListItem> SourceList { get; set; }
    }
}
