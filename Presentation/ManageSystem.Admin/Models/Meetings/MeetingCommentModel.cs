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
	/// 模型类 ，数据库表名：MeetingComment 
	/// </summary>
	 [Validator(typeof(MeetingCommentValidator))]
	public partial class MeetingCommentModel : BaseEntityModel
	{

		/// <summary>
		/// 用户的id
		/// <summary>
		[HtmlDisplayAttribute("会员Id", "会员Id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 用户的姓名
		/// <summary>
		[HtmlDisplayAttribute("会员姓名", "会员姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 信息动态的id
		/// <summary>
		[HtmlDisplayAttribute("会议id","会议id")]
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
		[HtmlDisplayAttribute("浏览器名称","浏览器名称")]
		public String BrowserName { get; set; }

		/// <summary>
		/// 评论内容
		/// <summary>
		[HtmlDisplayAttribute("评论内容","评论内容")]
		public String Content { get; set; }

		/// <summary>
		/// 上级评论
		/// <summary>
		[HtmlDisplayAttribute("上级评论","上级评论")]
		public long ParentId { get; set; }

        /// <summary>
        /// 该评论的下级评论数量，只记录下一级的。多级不处理
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 评论类型，1：普通会员   2：会议主办方  3：后台管理员
        /// </summary>
        [HtmlDisplayAttribute("评论类型", "评论类型")]
        public int Type { get; set; }

        public string TypeName { get; set; }

        /// <summary>
        /// 当前评论所处的深度，0表示顶级评论，1表示1级，以此内推
        /// </summary>
        public int Level { get; set; }


    }
}
