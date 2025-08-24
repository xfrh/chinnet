using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Meetings
{
	/// <summary>
	/// 实体类 ，数据库表名：MeetingApply 
	/// </summary>
	public partial class MeetingApply : BaseEntity
	{

		/// <summary>
		/// 报名用户的id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 报名用户的姓名
		/// <summary>
		public String MemberName { get; set; }
		/// <summary>
		/// 信息动态的id
		/// <summary>
		public long MeetingId { get; set; }
		/// <summary>
		/// 信息动态的名称
		/// <summary>
		public String MeetingName { get; set; }
		/// <summary>
		/// 报名姓名
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 邮箱地址
		/// <summary>
		public String Email { get; set; }
		/// <summary>
		/// 手机号码
		/// <summary>
		public String Phone { get; set; }
		/// <summary>
		/// 用户备注
		/// <summary>
		public String Remark { get; set; }
		/// <summary>
		/// 报名人数
		/// <summary>
		public Int32 Count { get; set; }
		/// <summary>
		/// 申请状态
		/// <summary>
		public Int32 Status { get; set; }
		/// <summary>
		/// 状态变更时间
		/// <summary>
		public DateTime StatusTime { get; set; }
		/// <summary>
		/// 订单号
		/// <summary>
		public String OrderSN { get; set; }
		/// <summary>
		/// 支付方的订单号
		/// <summary>
		public String CallBackSN { get; set; }
		/// <summary>
		/// 支付方式
		/// <summary>
		public Int32 PayMethod { get; set; }
		/// <summary>
		/// 报名单价
		/// <summary>
		public Decimal Price { get; set; }
		/// <summary>
		/// 支付金额
		/// <summary>
		public Decimal PayAmount { get; set; }
		/// <summary>
		/// 实际支付金额
		/// <summary>
		public Decimal ReceivedAmount { get; set; }
		/// <summary>
		/// 支付时间
		/// <summary>
		public DateTime PayTime { get; set; }


	}
}
