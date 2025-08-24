using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Meetings;

namespace ManageSystem.Web.Models.Meetings
{
	/// <summary>
	/// 模型类 ，数据库表名：MeetingApply 
	/// </summary>
	 [Validator(typeof(MeetingApplyValidator))]
	public partial class MeetingApplyModel : BaseEntityModel
	{

		/// <summary>
		/// 报名用户的id
		/// <summary>
		[HtmlDisplayAttribute("报名用户的id","报名用户的id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 报名用户的姓名
		/// <summary>
		[HtmlDisplayAttribute("报名用户的姓名","报名用户的姓名")]
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
		/// 报名姓名
		/// <summary>
		[HtmlDisplayAttribute("报名姓名","报名姓名")]
		public String Name { get; set; }

		/// <summary>
		/// 邮箱地址
		/// <summary>
		[HtmlDisplayAttribute("邮箱地址","邮箱地址")]
		public String Email { get; set; }

		/// <summary>
		/// 手机号码
		/// <summary>
		[HtmlDisplayAttribute("手机号码","手机号码")]
		public String Phone { get; set; }

		/// <summary>
		/// 用户备注
		/// <summary>
		[HtmlDisplayAttribute("用户备注","用户备注")]
		public String Remark { get; set; }

		/// <summary>
		/// 报名人数
		/// <summary>
		[HtmlDisplayAttribute("报名人数","报名人数")]
		public Int32 Count { get; set; }

		/// <summary>
		/// 申请状态
		/// <summary>
		[HtmlDisplayAttribute("申请状态","申请状态")]
		public Int32 Status { get; set; }

        /// <summary>
        /// 申请状态
        /// <summary>
        [HtmlDisplayAttribute("申请状态", "申请状态")]
        public string StatusName { get; set; }

        /// <summary>
        /// 状态变更时间
        /// <summary>
        [HtmlDisplayAttribute("状态变更时间","状态变更时间")]
		public DateTime StatusTime { get; set; }

		/// <summary>
		/// 订单号
		/// <summary>
		[HtmlDisplayAttribute("订单号","订单号")]
		public String OrderSN { get; set; }

		/// <summary>
		/// 支付方的订单号
		/// <summary>
		[HtmlDisplayAttribute("支付方的订单号","支付方的订单号")]
		public String CallBackSN { get; set; }

		/// <summary>
		/// 支付方式
		/// <summary>
		[HtmlDisplayAttribute("支付方式","支付方式")]
		public Int32 PayMethod { get; set; }

		/// <summary>
		/// 报名单价
		/// <summary>
		[HtmlDisplayAttribute("报名单价","报名单价")]
		public Decimal Price { get; set; }

		/// <summary>
		/// 支付金额
		/// <summary>
		[HtmlDisplayAttribute("支付金额","支付金额")]
		public Decimal PayAmount { get; set; }

		/// <summary>
		/// 实际支付金额
		/// <summary>
		[HtmlDisplayAttribute("实际支付金额","实际支付金额")]
		public Decimal ReceivedAmount { get; set; }

		/// <summary>
		/// 支付时间
		/// <summary>
		[HtmlDisplayAttribute("支付时间","支付时间")]
		public DateTime PayTime { get; set; }



	}
}
