using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Orders;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Orders
{
	/// <summary>
	/// 模型类 ，数据库表名：Order 
	/// </summary>
	 [Validator(typeof(OrderValidator))]
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
		/// 下单人会员帐号
		/// <summary>
		[HtmlDisplayAttribute("会员帐号", "会员帐号")]
        public String MemberLoginId { get; set; }

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
		[HtmlDisplayAttribute("用户订单备注","订单备注（下单人备注）")]
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
		/// 订单状态
		/// <summary>
		[HtmlDisplayAttribute("订单状态", "订单状态")]
        public string StatusName { get; set; }

        /// <summary>
        /// 订单状态变更时间
        /// <summary>
        [HtmlDisplayAttribute("变更时间", "订单状态最近一次的变更时间")]
        public DateTime StatusTime { get; set; }

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
		[HtmlDisplayAttribute("付款方式","付款方式  ，固定值（微信、支付宝、线下）")]
		public String PayType { get; set; }

		/// <summary>
		/// 系统备注（程序控制）
		/// <summary>
		[HtmlDisplayAttribute("系统备注（程序控制）","系统备注（程序控制）")]
		public String SystemRemark { get; set; }

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


        #region 收货地址相关

        /// <summary>
        /// 用户收货地址id
        /// <summary>
        [HtmlDisplayAttribute("用户收货地址id", "用户收货地址id")]
        public long AddressMemberAddressId { get; set; }

        /// <summary>
        /// 收货人姓名
        /// <summary>
        [HtmlDisplayAttribute("收货人姓名", "收货人姓名",true)]
        public String AddressName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址")]
        public String AddressEmail { get; set; }


        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        [HtmlDisplayAttribute("省市区", "省市区")]
        public String AddressArea { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        [HtmlDisplayAttribute("详细地址", "详细地址", true)]
        public String AddressAddress { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码", true)]
        public String AddressPhone { get; set; }

        /// <summary>
        /// 邮编
        /// <summary>
        [HtmlDisplayAttribute("邮编", "邮编")]
        public String AddressZipPostalCode { get; set; }

        /// <summary>
        /// 固定电话
        /// <summary>
        [HtmlDisplayAttribute("固定电话", "固定电话")]
        public String AddressTel { get; set; }

        /// <summary>
        /// 收货地址中的省份id
        /// <summary>
        [HtmlDisplayAttribute("收货地址中的省份id", "收货地址中的省份id")]
        public long AddressProvinceId { get; set; }

        /// <summary>
        /// 收货地址中的市id
        /// <summary>
        [HtmlDisplayAttribute("收货地址中的市id", "收货地址中的市id")]
        public long AddressCityId { get; set; }

        /// <summary>
        /// 收货地址中的区id
        /// <summary>
        [HtmlDisplayAttribute("收货地址中的区id", "收货地址中的区id")]
        public long AddressDistricts { get; set; }



        #endregion

        /// <summary>
        /// 订单明细集合
        /// </summary>
        public List<OrderItemModel> OrderItemList { get; set; }

        /// <summary>
        /// 订单操作记录集合
        /// </summary>
        public List<OrderLogModel> OrderLogList { get; set; }

    }


    /// <summary>
    /// 后台订单列表页面的数据模型
    /// </summary>
    public class OrderListModel
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public long IdLong { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 会员id
        /// <summary>
        [HtmlDisplayAttribute("会员id", "会员id")]
        public long MemberId { get; set; }

        /// <summary>
        /// 会员帐号
        /// <summary>
        [HtmlDisplayAttribute("会员帐号", "会员帐号")]
        public string MemberLoginId { get; set; }

        /// <summary>
        /// 订单号
        /// <summary>
        [HtmlDisplayAttribute("订单号", "订单号")]
        public String SN { get; set; }

        /// <summary>
        /// 订单总金额
        /// <summary>
        [HtmlDisplayAttribute("订单总金额", "订单总金额")]
        public Decimal Amount { get; set; }

        /// <summary>
        /// 实收金额
        /// <summary>
        [HtmlDisplayAttribute("实收金额", "实收金额")]
        public Decimal ReceivedAmount { get; set; }

        /// <summary>
        /// 订单备注（下单人备注）
        /// <summary>
        [HtmlDisplayAttribute("订单备注（下单人备注）", "订单备注（下单人备注）")]
        public String Remark { get; set; }

        /// <summary>
        /// 下单时间
        /// <summary>
        [HtmlDisplayAttribute("下单时间", "下单时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 下单时间
        /// <summary>
        public string CreateTimeString { get; set; }

        /// <summary>
        /// 订单状态
        /// <summary>
        [HtmlDisplayAttribute("订单状态", "订单状态")]
        public Int32 Status { get; set; }

        /// <summary>
        /// 订单状态名称
        /// <summary>
        [HtmlDisplayAttribute("订单状态名称", "订单状态名称")]
        public string StatusName { get; set; }

        /// <summary>
        /// 订单状态变态时间，最后一次变更状态
        /// <summary>
        public DateTime StatusTime { get; set; }

        /// <summary>
        /// 订单状态变态时间，最后一次变更状态
        /// <summary>
        public string StatusTimeString { get; set; }


        /// <summary>
        /// 支付方式
        /// </summary>
        public String PayType { get; set; }

        /// <summary>
        /// 系统备注
        /// <summary>
        public String SystemRemark { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string UserAddress { get; set; }

        /// <summary>
        /// 列表页面的按钮 html
        /// </summary>
        public string ActionHtml { get; set; }

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

    /// <summary>
    ///后台订单列表页面查询条件的数据模型 
    /// </summary>
    public class OrderListSearchModel
    {
        /// <summary>
        /// 订单号
        /// <summary>
        [HtmlDisplayAttribute("订单号", "订单号")]
        public String SN { get; set; }

      /// <summary>
        /// 订单状态
        /// <summary>
        [HtmlDisplayAttribute("订单状态", "订单状态")]
        public Int32 Status { get; set; }

        /// <summary>
        /// 订单状态
        /// <summary>
        [HtmlDisplayAttribute("订单状态", "订单状态")]
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 下单时间 开始
        /// <summary>
        [HtmlDisplayAttribute("下单时间 (开始)", "下单时间")]
        public string CreateTimeStart { get; set; }

        /// <summary>
        /// 下单时间 结束
        /// <summary> 
        [HtmlDisplayAttribute("下单时间 (结束)", "下单时间")]
        public string CreateTimeEnd { get; set; }

        /// <summary>
        /// 收货人
        /// <summary>
        [HtmlDisplayAttribute("收货人", "收货人")]
        public string UserName { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码")]
        public string UserPhone { get; set; }

        /// <summary>
        /// 会员帐号
        /// <summary>
        [HtmlDisplayAttribute("会员帐号", "会员帐号")]
        public string MemberLoginId { get; set; }
    }
}
