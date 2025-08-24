using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Integrals;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Integrals
{
	/// <summary>
	/// 模型类 ，数据库表名：IntegralSetting 
	/// </summary>
	 [Validator(typeof(IntegralSettingValidator))]
	public partial class IntegralSettingModel : BaseEntityModel
	{
		/// <summary>
		/// 制度名称
		/// <summary>
		[HtmlDisplayAttribute("制度名称","制度名称")]
		public String Name { get; set; }

        /// <summary>
        ///操作类型，（1、上传医学数据   2、创建信息动态  3、创建科研合作）
        /// <summary>
        [HtmlDisplayAttribute("操作类型", "操作类型")]
        public int Type { get; set; }

        /// <summary>
        ///操作类型，（1、上传医学数据   2、创建信息动态  3、创建科研合作）
        /// <summary>
        [HtmlDisplayAttribute("操作类型", "操作类型")]
        public IList<SelectListItem> TypeList { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
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
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 赠送积分数量
        /// <summary>
        [HtmlDisplayAttribute("赠送积分数量", "赠送积分数量")]
        public Decimal Value { get; set; }

        /// <summary>
        /// 说明
        /// <summary>
        [HtmlDisplayAttribute("说明","说明")]
		public String Remark { get; set; }

	}
}
