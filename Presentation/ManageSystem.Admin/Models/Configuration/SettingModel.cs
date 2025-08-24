using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Configuration;

namespace ManageSystem.Admin.Models.Configuration
{
	/// <summary>
	/// 模型类 ，数据库表名：SystemConfig 
	/// </summary>
	 [Validator(typeof(SettingValidator))]
	public partial class SettingModel : BaseEntityModel
	{

		/// <summary>
		/// 配置名称
		/// <summary>
		[HtmlDisplayAttribute("配置名称","配置名称")]
		public String Title { get; set; }

		/// <summary>
		/// 配置关键字
		/// <summary>
		[HtmlDisplayAttribute("配置关键字","配置关键字")]
		public String Name { get; set; }

        /// <summary>
        /// 配置所属类型
        /// </summary>
        [HtmlDisplayAttribute("配置类型", "配置类型")]
        public string Type { get; set; }

        /// <summary>
        /// 配置的值
        /// <summary>
        [HtmlDisplayAttribute("配置的值","配置的值")]
		public String Value { get; set; }

		/// <summary>
		/// 是否可以操作
		/// <summary>
		[HtmlDisplayAttribute("是否可操作", "是否可操作")]
		public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否可以操作
        /// <summary>
        [HtmlDisplayAttribute("是否缓存", "是否缓存")]
        public bool IsCache { get; set; }


	}
}
