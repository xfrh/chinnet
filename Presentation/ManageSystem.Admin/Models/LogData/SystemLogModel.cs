using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Log;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Domain.Log;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Log
{
	/// <summary>
	/// 模型类 ，数据库表名：SystemLog 
	/// </summary>
	 [Validator(typeof(SystemLogValidator))]
	public partial class SystemLogModel : BaseEntityModel
	{

		/// <summary>
		/// 日志标题
		/// <summary>
		[HtmlDisplayAttribute("日志标题","日志简单说明")]
		public String Title { get; set; }

		/// <summary>
		/// 日志等级
		/// <summary>
		[HtmlDisplayAttribute("日志等级","请输入日志等级")]
		public int LevelId { get; set; }

        /// <summary>
		/// 日志等级名称
		/// <summary>
		[HtmlDisplayAttribute("日志等级名称", "日志等级名称")]
        public string LevelName { get; set; }

        /// <summary>
        /// 出错类
        /// <summary>
        [HtmlDisplayAttribute("出错类","出错的类")]
		public String Logger { get; set; }

        /// <summary>
        /// 错误页面地址
        /// <summary>
        [HtmlDisplayAttribute("错误页面", "错误页面地址")]
        public String Url { get; set; }

        /// <summary>
        /// 错误详细内容
        /// <summary>
        [HtmlDisplayAttribute("错误详细内容","错误详细内容")]
		public String Message { get; set; }


        /// <summary>
        /// 日志枚举列表
        /// </summary>
        [HtmlDisplayAttribute("日志等级", "选择日志等级")]
        public IList<SelectListItem> LevelList { get; set; }

        /// <summary>
        /// 操作的IP地址
        /// <summary>
        [HtmlDisplayAttribute("IP地址", "操作的IP地址")]
        public String IPAddress { get; set; }

    }
}
