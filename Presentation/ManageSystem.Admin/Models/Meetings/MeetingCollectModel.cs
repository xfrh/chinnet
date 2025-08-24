using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Meetings;

namespace ManageSystem.Admin.Models.Meetings
{
	/// <summary>
	/// 模型类 ，数据库表名：MeetingCollect 
	/// </summary>
	 [Validator(typeof(MeetingCollectValidator))]
	public partial class MeetingCollectModel : BaseEntityModel
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
		/// 信息动态的id
		/// <summary>
		[HtmlDisplayAttribute("信息动态的id","信息动态的id")]
		public long MeetingId { get; set; }

		/// <summary>
		/// 信息动态的名称
		/// <summary>
		[HtmlDisplayAttribute("信息动态的名称","信息动态的名称")]
		public String MeetingName { get; set; }

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
