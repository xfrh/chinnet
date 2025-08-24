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
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Admin.Models.SystemSet
{
	/// <summary>
	/// 模型类 ，数据库表名：Function 
	/// </summary>
	 [Validator(typeof(FunctionValidator))]
	public partial class FunctionModel : BaseEntityModel
	{

        public FunctionModel() {
            AvailableFunctions = new List<SelectListItem>();
        }

		/// <summary>
		/// 功能名称
		/// <summary>
		[HtmlDisplayAttribute("功能名称","功能名称")]
		public String Name { get; set; }

        /// <summary>
        /// 面包屑名称
        /// </summary>
        public string Breadcrumb { get; set; }

        /// <summary>
        /// 排序编号
        /// <summary>
        [HtmlDisplayAttribute("排序编号","排序编号")]
		public int? Sort { get; set; }

		/// <summary>
		/// 页面地址
		/// <summary>
		[HtmlDisplayAttribute("页面地址","页面地址")]
		public String Url { get; set; }

		/// <summary>
		/// 功能类型
		/// <summary>
		[HtmlDisplayAttribute("功能类型","功能类型")]
		public int Type { get; set; }

        /// <summary>
        /// 功能类型名称
        /// <summary>
        [HtmlDisplayAttribute("功能类型名称", "功能类型名称")]
        public string TypeName { get; set; }

        /// <summary>
        /// 页面控件Id
        /// <summary>
        [HtmlDisplayAttribute("页面控件Id", "页面控件Id")]
		public String ControlId { get; set; }

        /// <summary>
        /// 功能图标
        /// <summary>
        [HtmlDisplayAttribute("功能图标", "功能图标")]
		public String Image { get; set; }

        /// <summary>
        /// 页面class名称
        /// <summary>
        [HtmlDisplayAttribute("页面class名称", "页面class名称")]
		public String CustomClass { get; set; }

		/// <summary>
		/// 上级功能id
		/// <summary>
		[HtmlDisplayAttribute("上级功能","上级功能")]
		public long FunctionId { get; set; }

        /// <summary>
        /// 所有功能集合
        /// </summary>
        public IList<SelectListItem> AvailableFunctions { get; set; }

        /// <summary>
		/// 上级功能名称
		/// <summary>
		[HtmlDisplayAttribute("上级功能名称", "上级功能名称")]
        public string FunctionName { get; set; }



		/// <summary>
		/// 创建人姓名
		/// <summary>
		[HtmlDisplayAttribute("创建人姓名","创建人姓名")]
		public String CreateName { get; set; }

        /// <summary>
        /// 功能类型枚举列表
        /// </summary>
        [HtmlDisplayAttribute("功能类型", "选择功能类型")]
        public IList<SelectListItem> FunctionTypeList { get; set; }

        /// <summary>
        /// 是否菜单，如果是将在左边显示菜单中
        /// </summary>
        [HtmlDisplayAttribute("是否菜单", "如果是将在左边显示菜单中")]
        public bool IsMenu { get; set; }

        /// <summary>
        /// 页面上那个菜单被选中的效果。 只用于前台显示
        /// </summary>
        public long ShowFunctinId { get; set; }


    }
}
