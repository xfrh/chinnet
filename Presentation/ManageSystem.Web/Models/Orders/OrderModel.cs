using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Orders
{
	/// <summary>
	/// 模型类 ，数据库表名：Order 
	/// </summary>
	public partial class OrderModel : BaseEntityModel
	{

		/// <summary>
		/// 会员id
		/// <summary>
		[HtmlDisplayAttribute("会员id","会员id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 会员姓名
		/// <summary>
		[HtmlDisplayAttribute("会员姓名","会员姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 订单号
		/// <summary>
		[HtmlDisplayAttribute("订单号","订单号")]
		public String SN { get; set; }

		/// <summary>
		/// 订单总金额
		/// <summary>
		[HtmlDisplayAttribute("订单总金额","订单总金额")]
		public Decimal Amount  { get; set; }

		/// <summary>
		/// 实收金额
		/// <summary>
		[HtmlDisplayAttribute("实收金额","实收金额")]
		public Decimal ReceivedAmount  { get; set; }

		/// <summary>
		/// 订单备注（下单人备注）
		/// <summary>
		[HtmlDisplayAttribute("订单备注（下单人备注）","订单备注（下单人备注）")]
		public String Remark { get; set; }

		/// <summary>
		/// 创建时间
		/// <summary>
		[HtmlDisplayAttribute("创建时间","创建时间")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 订单状态
		/// <summary>
		[HtmlDisplayAttribute("订单状态","订单状态")]
		public Int32 Status { get; set; }

        /// <summary>
        /// 订单状态名称
        /// <summary>
        [HtmlDisplayAttribute("订单状态", "订单状态")]
        public string StatusName { get; set; }

        /// <summary>
        /// 待处理时间，状态变成待处理的时间
        /// <summary>
        [HtmlDisplayAttribute("待处理时间，状态变成待处理的时间","待处理时间，状态变成待处理的时间")]
		public DateTime StatusTime1 { get; set; }

		/// <summary>
		/// 待付款时间，状态变成待付款的时间
		/// <summary>
		[HtmlDisplayAttribute("待付款时间，状态变成待付款的时间","待付款时间，状态变成待付款的时间")]
		public DateTime StatusTime2 { get; set; }

		/// <summary>
		/// 待发货时间，状态变成待发货的时间
		/// <summary>
		[HtmlDisplayAttribute("待发货时间，状态变成待发货的时间","待发货时间，状态变成待发货的时间")]
		public DateTime StatusTime3 { get; set; }

		/// <summary>
		/// 待收货时间，状态变成待收货的时间
		/// <summary>
		[HtmlDisplayAttribute("待收货时间，状态变成待收货的时间","待收货时间，状态变成待收货的时间")]
		public DateTime StatusTime4 { get; set; }

		/// <summary>
		/// 已取消时间，状态变成待处理的时间
		/// <summary>
		[HtmlDisplayAttribute("已取消时间，状态变成待处理的时间","已取消时间，状态变成待处理的时间")]
		public DateTime StatusTime5 { get; set; }

		/// <summary>
		/// 交易完成时间，状态变成交易完成的时间
		/// <summary>
		[HtmlDisplayAttribute("交易完成时间，状态变成交易完成的时间","交易完成时间，状态变成交易完成的时间")]
		public DateTime StatusTime6 { get; set; }

		/// <summary>
		/// 付款方式  ，固定值（微信、支付宝、线下）
		/// <summary>
		[HtmlDisplayAttribute("付款方式  ，固定值（微信、支付宝、线下）","付款方式  ，固定值（微信、支付宝、线下）")]
		public String PayType { get; set; }

		/// <summary>
		/// 系统备注（程序控制）
		/// <summary>
		[HtmlDisplayAttribute("系统备注（程序控制）","系统备注（程序控制）")]
		public String SystemRemark { get; set; }

        /// <summary>
        /// 订单商品的数量，明细的数量之和
        /// </summary>
        public int OrderProductItemCount { get; set; }

        /// <summary>
        /// 订单商品种类之和
        /// </summary>
        public int OrderProductCount { get; set; }

        /// <summary>
        /// 订单的明细集合
        /// </summary>
        public List<OrderItemModel> OrderItemList { get; set; }

        /// <summary>
        /// 快递公司名称
        /// <summary>
        [HtmlDisplayAttribute("快递公司", "快递公司")]
        public String ExpressCompany { get; set; }
        /// <summary>
        /// 快递单号
        /// <summary>
        [HtmlDisplayAttribute("快递单号", "快递单号")]
        public String ExpressSN { get; set; }
        /// <summary>
        /// 快递备注信息
        /// <summary>
        [HtmlDisplayAttribute("快递备注", "快递备注")]
        public String ExpressRemark { get; set; }


    }

    /// <summary>
    /// 会员中心，我的兑换列表页面
    /// </summary>
    public class CenterOrderListModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 订单状态下拉列表
        /// </summary>
        public IList<SelectListItem> OrderStatusList { get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<OrderModel> PageList { get; set; }

    }
}
