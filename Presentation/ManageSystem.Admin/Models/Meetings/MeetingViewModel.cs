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
	/// 模型类 ，数据库表名：MeetingView 
	/// </summary>
	 [Validator(typeof(MeetingViewValidator))]
	public partial class MeetingViewModel : BaseEntityModel
	{

		/// <summary>
		/// 报名用户的id
		/// <summary>
		[HtmlDisplayAttribute("会员Id", "会员Id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 报名用户的姓名
		/// <summary>
		[HtmlDisplayAttribute("会员姓名", "会员姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 信息动态的id
		/// <summary>
		[HtmlDisplayAttribute("会议Id", "信息动态Id")]
		public long MeetingId { get; set; }

		/// <summary>
		/// 信息动态的名称
		/// <summary>
		[HtmlDisplayAttribute("会议名称","会议名称")]
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
