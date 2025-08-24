using ManageSystem.Core.Domain.Members;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Mall
{
    /// <summary>
    /// 订单结算页面的数据封装
    /// </summary>
    public class SettlementModel
    {
        /// <summary>
        /// 用户的收货地址集合
        /// </summary>
        public List<MemberAddressModel> AddressList { get; set; }

        /// <summary>
        /// 购买的商品集合
        /// </summary>
        public List<MemberCartModel> CartItemList { get; set; }

        /// <summary>
        /// 购买商品总数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 订单商品总金额
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 订单应付总金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 用户可用积分总数
        /// </summary>
        public decimal IntegralAmount { get; set; }


        /// <summary>
        /// 购物车的缓存Key
        /// </summary>
        public string Key { get; set; }

    }


    /// <summary>
    /// 提交订单
    /// </summary>
    public class SubmitOrderModel
    {
        /// <summary>
        /// 订单商品的详细数据
        /// </summary>
        public List<MemberCart> ItemList { get; set; }

        /// <summary>
        /// 订单应付总金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 商品总数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 购物车的缓存Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 用户收货地址id
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public string Remark { get; set; }


    }


    /// <summary>
    /// 提交订单的商品详细
    /// </summary>
    public class SubmitOrderItemModel
    {
        /// <summary>
        /// 购物车表的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
    }

}
