using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Researches;

namespace ManageSystem.Admin.Models.Researches
{
    /// <summary>
    /// 模型类 ，数据库表名：ResearchCollect 
    /// </summary>
    [Validator(typeof(ResearchCollectValidator))]
	public partial class ResearchCollectModel : BaseEntityModel
	{

		/// <summary>
		/// 用户的id
		/// <summary>
		[HtmlDisplayAttribute("会员的id", "会员的id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 用户的姓名
		/// <summary>
		[HtmlDisplayAttribute("会员的姓名", "会员的姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 科研合作的id
		/// <summary>
		[HtmlDisplayAttribute("科研合作的id","科研合作的id")]
		public long ResearchId { get; set; }

		/// <summary>
		/// 科研合作的名称
		/// <summary>
		[HtmlDisplayAttribute("科研合作的名称","科研合作的名称")]
		public String ResearchName { get; set; }

		/// <summary>
		/// Ip地址
		/// <summary>
		[HtmlDisplayAttribute("Ip地址","Ip地址")]
		public String Ip { get; set; }

		/// <summary>
		/// 操作者浏览器名称
		/// <summary>
		[HtmlDisplayAttribute("操作者浏览器名称","操作者浏览器名称")]
		public String BrowserName { get; set; }



	}
}
