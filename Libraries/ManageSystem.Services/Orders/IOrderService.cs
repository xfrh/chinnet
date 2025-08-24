using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Log;

namespace ManageSystem.Services.Orders
{
	/// <summary>
	/// 操作接口类 ，数据库表名：Order 
	/// </summary>
	public  partial interface IOrderService : IBaseService<Order>
	{
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderEntity">订单数据实体</param>
        /// <param name="cartList">订单明细，购物车数据</param>
        /// <param name="addressId">用户的收货地址id</param>
        /// <returns></returns>
        bool Insert(Order orderEntity, List<MemberCart> cartList, long addressId, Member member, ActionSource actionSource);


        /// <summary>
        /// 根据订单号查询数据
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        Order QueryEntityBySn(string sn);
        
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="address"></param>
        /// <param name="member"></param>
        /// <param name="actionSource"></param>
        /// <returns></returns>
        bool Update(Order order, OrderAddress address, Account member, ActionSource actionSource);

        /// <summary>
        /// 确认发货
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="remark">操作备注</param>
        /// <param name="expressCompany">快递公司名称</param>
        /// <param name="expressSN">快递单号</param>
        /// <param name="expressRemark">快递备注信息</param>
        /// <param name="actionSource">操作来源</param>
        /// <param name="account">操作帐号</param>
        /// <returns></returns>
        bool Send(long orderId, string remark,string expressCompany, string expressSN, string expressRemark, ActionSource actionSource, Account account);


        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="remark">操作备注</param>
        /// <param name="actionSource">操作来源</param>
        /// <param name="account">操作帐号</param>
        /// <returns></returns>
        bool Receive(long orderId, string remark, ActionSource actionSource, Account account);


        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="remark">操作备注</param>
        /// <param name="actionSource">操作来源</param>
        /// <param name="account">操作帐号</param>
        /// <returns></returns>
        bool Cancel(long orderId, string remark, ActionSource actionSource, Account account);


        /// <summary>
        /// 会员中心，我的兑换记录，分页查询数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="status"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        IQueryable<Order> Query(long memberId, int status, string   sn );


        /// <summary>
        /// 分页查询 后台
        /// </summary>
        /// <param name="sn">订单号</param>
        /// <param name="state">订单状态</param>
        /// <param name="memberLoginId">会员帐号</param>
        /// <param name="userName">收货人姓名</param>
        /// <param name="userPhone">收货人手机号码</param>
        /// <param name="createTimeStart">下单开始时间</param>
        /// <param name="createTimeEnd">下单结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Order> QueryPage(string sn, int state, string memberLoginId, string userName, string userPhone, string createTimeStart, string createTimeEnd, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
