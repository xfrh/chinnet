using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Orders
{
	/// <summary>
	/// 实体类 ，数据库表名：Order 
	/// </summary>
	public partial class Order : BaseEntity
	{

		/// <summary>
		/// 会员id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 会员姓名
		/// <summary>
		public String MemberName { get; set; }
		/// <summary>
		/// 订单号
		/// <summary>
		public String SN { get; set; }
		/// <summary>
		/// 订单总金额
		/// <summary>
		public Decimal Amount  { get; set; }
		/// <summary>
		/// 实收金额
		/// <summary>
		public Decimal ReceivedAmount  { get; set; }
		/// <summary>
		/// 订单备注（下单人备注）
		/// <summary>
		public String Remark { get; set; }

        /// <summary>
        /// 下单时间
        /// <summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单状态
        /// <summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 订单状态变态时间，最后一次变更状态
        /// <summary>
        public DateTime StatusTime { get; set; }


        /// <summary>
        /// 待处理时间，状态变成待处理的时间
        /// <summary>
        public DateTime StatusTime1 { get; set; }
		/// <summary>
		/// 待付款时间，状态变成待付款的时间
		/// <summary>
		public DateTime StatusTime2 { get; set; }
		/// <summary>
		/// 待发货时间，状态变成待发货的时间
		/// <summary>
		public DateTime StatusTime3 { get; set; }
		/// <summary>
		/// 待收货时间，状态变成待收货的时间
		/// <summary>
		public DateTime StatusTime4 { get; set; }
		/// <summary>
		/// 已取消时间，状态变成待处理的时间
		/// <summary>
		public DateTime StatusTime5 { get; set; }
		/// <summary>
		/// 交易完成时间，状态变成交易完成的时间
		/// <summary>
		public DateTime StatusTime6 { get; set; }
		/// <summary>
		/// 付款方式  ，固定值（微信、支付宝、线下）
		/// <summary>
		public String PayType { get; set; }
		/// <summary>
		/// 系统备注（程序控制）
		/// <summary>
		public String SystemRemark { get; set; }

        /// <summary>
		/// 快递公司名称
		/// <summary>
		public String ExpressCompany { get; set; }
        /// <summary>
		/// 快递单号
		/// <summary>
		public String ExpressSN { get; set; }
        /// <summary>
		/// 快递备注信息
		/// <summary>
		public String ExpressRemark { get; set; }

    }
}
