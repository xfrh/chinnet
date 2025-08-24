using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Orders
{
    /// <summary>
    /// 订单详细页数据封装
    /// </summary>
    public class OrderDetailModel
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderModel OrderModel { get; set; }

        /// <summary>
        /// 订单明细集合
        /// </summary>
        public IList<OrderItemModel> OrderItemList { get; set; }

        /// <summary>
        /// 订单收货地址
        /// </summary>
        public OrderAddress AddressModel { get; set; }


    }
}