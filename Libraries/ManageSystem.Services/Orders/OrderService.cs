using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.Products;
using ManageSystem.Services.Members;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Data;
using System.Data.Common;
using ManageSystem.Core.Infrastructure;
using System.Data.Entity.Infrastructure;
using ManageSystem.Services.SystemSet;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Log;
using ManageSystem.Core.Extensions;
using ManageSystem.Core;

namespace ManageSystem.Services.Orders
{
    /// <summary>
    /// 操作类 ，数据库表名：Order 
    /// </summary>
    public partial class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IProductService ProductService;
        private readonly IProductBrandService ProductBrandService;
        private readonly IProductCategoryService ProductCategoryService;
        private readonly IProductImageService ProductImageService;
        private readonly IMemberAddressService MemberAddressService;
        private readonly IMemberCartService MemberCartService;
        private readonly IMemberService MemberService;
        private readonly IAutoCodeService AutoCodeService;
        private readonly IOrderItemService OrderItemService;
        private readonly IOrderLogService OrderLogService;
        private readonly IActionLogService ActionLogService;
        private readonly IMemberIntegralLogService MemberIntegralLogService;
        private readonly IOrderAddressService OrderAddressService;

        public OrderService(
              IRepository<Order> repository,
              IProductService _productService,
              IProductBrandService _productBrandService,
              IProductCategoryService _productCategoryService,
              IProductImageService _productImageService,
              IMemberAddressService _memberAddressService,
              IMemberCartService _memberCartService,
              IMemberService _memberService,
              IAutoCodeService _autoCodeService,
              IOrderItemService _orderItemService,
              IOrderLogService _orderLogService,
              IActionLogService _actionLogService,
              IMemberIntegralLogService _memberIntegralLogService,
              IOrderAddressService _orderAddressService
              ) : base(repository)
        {
            this.ProductService = _productService;
            this.ProductBrandService = _productBrandService;
            this.ProductCategoryService = _productCategoryService;
            this.ProductImageService = _productImageService;
            this.MemberAddressService = _memberAddressService;
            this.MemberCartService = _memberCartService;
            this.MemberService = _memberService;
            this.AutoCodeService = _autoCodeService;
            this.OrderLogService = _orderLogService;
            this.ActionLogService = _actionLogService;
            this.MemberIntegralLogService = _memberIntegralLogService;
            this.OrderAddressService = _orderAddressService;
            this.OrderItemService = _orderItemService;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderEntity">订单数据实体</param>
        /// <param name="cartList">订单明细，购物车数据</param>
        /// <param name="addressId">用户的收货地址id</param>
        /// <param name="member">下单用户的数据</param>
        /// <param name="actionSource">操作来源</param>
        /// <returns></returns>
        public bool Insert(Order orderEntity, List<MemberCart> cartList, long addressId, Member member, ActionSource actionSource)
        {
            try
            {
                MemberAddress memberAddress = null; //用户选择的收货地址
                List<OrderItem> orderItemList = new List<OrderItem>(); //订单明细
                List<Product> productList = new List<Product>(); //本次订单明细中操作的商品集合

                decimal orderSubmitAmount = orderEntity.Amount;//前台提交的订单总金额，和填充好数据以后的总金额对比的时候使用。

                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    orderEntity.Id = CommonHelper.GuidToLongID;

                    #region  数据验证 和 数据封装

                    if (member == null || member.Id <= 0)
                        throw new Exception("会员不存在");

                    if (orderEntity == null || orderEntity.Amount <= 0)
                        throw new Exception("订单数据错误");

                    if (cartList == null || !cartList.Any())
                        throw new Exception("订单明细数据错误");

                    if (addressId <= 0)
                        throw new Exception("请选择收货地址");

                    memberAddress = this.MemberAddressService.QueryEntity(m => m.Id == addressId && m.Mark > 0 && m.MemberId == member.Id);
                    if (memberAddress == null || memberAddress.Id <= 0)
                        throw new Exception("请选择收货地址");

                    //拆解购物车的数据获取订单明细
                    var cartIds = cartList.Select(o => o.Id).ToList();
                    var oldCartList = this.MemberCartService.Query(m => cartIds.Contains(m.Id) && m.MemberId == member.Id); //原始购物车数据
                    if (oldCartList == null || !oldCartList.Any())
                        throw new Exception("订单明细数据错误");

                    orderEntity.Amount = 0; //在前面已经赋值过一次了，这里清零以后重新计算
                    foreach (var item in cartList)
                    {
                        var cartItem = oldCartList.Where(m => m.Id == item.Id).FirstOrDefault();
                        if (cartItem == null || cartItem.Id <= 0 || cartItem.ProductId <= 0)
                            throw new Exception("订单明细数据错误");

                        var productEntity = this.ProductService.QueryEntity(cartItem.ProductId);
                        if (productEntity == null || productEntity.Id <= 0)
                            throw new Exception("商品数据错误");

                        if (productEntity.Status == (int)ProductStatusEnum.Soldout)
                            throw new Exception(productEntity.Name + " 已下架");

                        if (productEntity.Count < item.Count)
                            throw new Exception(productEntity.Name + " 库存不足");

                        //更新商品的库存
                        productEntity.BuyCount += 1; //购买次数
                        productEntity.Count -= item.Count; //商品库存
                        productList.Add(productEntity);

                        //更新用户购物车的数据
                        cartItem.Count = cartItem.Count > item.Count ?cartItem.Count - item.Count:0;

                        OrderItem orderItem = new OrderItem()
                        {
                            Amount = item.Count * productEntity.MarketPrice,
                            CostPrice = productEntity.CostPrice,
                            Count = item.Count,
                            MarketPrice = productEntity.MarketPrice,
                            OrderId = orderEntity.Id,
                            ProductCode = productEntity.Code,
                            ProductId = productEntity.Id,
                            ProductName = productEntity.Name,
                            ProductImage=productEntity.MainImage,
                            Remark = ""
                        };

                        orderItemList.Add(orderItem);

                        orderEntity.Amount += orderItem.Amount;
                    }

                    if (orderEntity.Amount != orderSubmitAmount)
                        throw new Exception("订单提交的总金额数据错误，请重新");

                    if (orderEntity.Amount > member.IntegralAmount)
                        throw new Exception("用户的可用积分不够本次支付");

                    //设置订单主表数据
                    orderEntity.SN = this.AutoCodeService.GetCode(AutoCodeType.ProductOrder);
                    orderEntity.MemberId = member.Id;
                    orderEntity.MemberName = member.Name;
                    orderEntity.PayType = "积分";
                    orderEntity.ReceivedAmount = orderEntity.Amount;
                    orderEntity.CreateTime = DateTime.Now;
                    orderEntity.Status = (int)OrderStatusEnum.WaitSend;
                    orderEntity.StatusTime = DateTime.Now;
                    orderEntity.StatusTime3 = DateTime.Now;
                    orderEntity.StatusTime1 = DateHelper.DefaultValue();
                    orderEntity.StatusTime2 = DateHelper.DefaultValue();
                    orderEntity.StatusTime4 = DateHelper.DefaultValue();
                   orderEntity.StatusTime5 = DateHelper.DefaultValue();
                    orderEntity.StatusTime6 = DateHelper.DefaultValue();
                    orderEntity.Remark = string.IsNullOrWhiteSpace(orderEntity.Remark) ? "" : orderEntity.Remark;
                    orderEntity.SystemRemark = string.IsNullOrWhiteSpace(orderEntity.SystemRemark) ? "" : orderEntity.SystemRemark;

                    #endregion

                    #region 提交订单

                    //1、提交订单主表
                    this.Insert(orderEntity);

                    //2、提交订单明细表
                    foreach (var item in orderItemList)
                        this.OrderItemService.Insert(item);
                    
                    //3、订单收货地址表
                    this.OrderAddressService.Insert(new OrderAddress()
                    {
                        Area = memberAddress.Area,
                        Address =memberAddress.Address,
                        CityId = memberAddress.CityId,
                        DistrictsId = memberAddress.DistrictsId,
                        Email = memberAddress.Email,
                        MemberAddressId = memberAddress.Id,
                        MemberId = memberAddress.MemberId,
                        Name = memberAddress.Name,
                        OrderId = orderEntity.Id,
                        Phone = memberAddress.Phone,
                        ProvinceId = memberAddress.ProvinceId,
                        Remark = "",
                        Tel = memberAddress.Tel,
                        ZipPostalCode = memberAddress.ZipPostalCode
                    });

                    //3、修改商品库存和购买次数
                    foreach (var item in productList)
                        this.ProductService.Update(item); //库存数量在上面已经减过了，所以这里直接更新商品

                    //4、修改用户购物车，数量在上面已经减过了，所以这里直接更新数据库
                    foreach (var item in oldCartList)
                    {
                        if (item.Count <= 0)
                            this.MemberCartService.Delete(item);
                        else
                            this.MemberCartService.Update(item);
                    }

                    //5、修改用户积分总额
                    member.IntegralAmount -= orderEntity.Amount;
                    this.MemberService.Update(member);

                    //6、添加用户积分使用流水
                    this.MemberIntegralLogService.Insert(new MemberIntegralLog()
                    {
                        MemberId = member.Id,
                        Remark = "订单号："+orderEntity.SN,
                        Type = "下单扣除",
                        Source = actionSource.GetDescription(),
                        Value = -orderEntity.Amount,
                        DataId = orderEntity.Id
                    });

                    //7、添加订单操作日志
                    OrderLog orderLog = new OrderLog()
                    {
                        Content = "用户提交订单成功",
                        OrderId = orderEntity.Id,
                        Source = actionSource.GetDescription(), //操作来源，1：前台   2：后台
                        Type = "提交订单",
                        UserId = member.Id,
                        UserName = member.Name + "（" + member.LoginId + "）",
                        JsonData = orderEntity.SerializeObject()
                    };
                    this.OrderLogService.Insert(orderLog);

                    //8、添加系统操作日志
                    this.ActionLogService.Insert(ActionType.Create, actionSource, member.Id, member.Name + "（" + member.LoginId + "）",
                        "提交订单，订单号：" + orderEntity.SN, "提交订单数据：" + orderEntity.SerializeObject() + "，提交购物车明细：" + cartList.SerializeObject() + "，收货地址数据：" + memberAddress.SerializeObject() + "，会员信息：" + member.Id);

                    #endregion

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// 根据订单号查询数据
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public Order QueryEntityBySn(string sn)
        {
            try
            {
                return this.QueryEntity(m => m.SN.Equals(sn));
            }
            catch (Exception)
            {

                return null;
            }
        }


        /// <summary>
        /// 会员中心，我的兑换记录，分页查询数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="status"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        public IQueryable<Order> Query(long memberId, int status, string sn)
        {
            if (memberId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId);

            if (!string.IsNullOrWhiteSpace(sn))
                query = query.Where(m => m.SN.Contains(sn));

            if (status > 0)
                query = query.Where(m => m.Status == status);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }
        
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
        public IPagedList<Order> QueryPage(string sn, int status, string memberLoginId, string userName, string userPhone, string createTimeStart, string createTimeEnd, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(sn))
                query = query.Where(m => m.SN.Equals(sn.Trim()));

            if (status > 0)
                query = query.Where(m => m.Status == status);

            if (!string.IsNullOrWhiteSpace(memberLoginId))
            {
                var member = this.MemberService.QueryModelByLoginId(memberLoginId);
                query = (member != null && member.Id > 0)? query.Where(m => m.MemberId == member.Id) 
                                                                                        : query.Where(m => m.MemberId == -1);
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var orderAddress = this.OrderAddressService.Query(m => m.Name.Equals(userName) && m.Mark > 0);
                if (orderAddress != null && orderAddress.Any())
                {
                    var orderIds = orderAddress.Select(m => m.OrderId).ToList();
                    query = query.Where(m => orderIds.Contains(m.Id));
                }
                else
                {
                    query = query.Where(m => m.Id == -1);
                }
            }

            if (!string.IsNullOrWhiteSpace(userPhone))
            {
                var orderAddress = this.OrderAddressService.Query(m => m.Phone.Equals(userPhone) && m.Mark>0);
                if (orderAddress != null && orderAddress.Any())
                {
                    var orderIds = orderAddress.Select(m => m.OrderId).ToList();
                    query = query.Where(m => orderIds.Contains(m.Id));
                }
                else
                {
                    query = query.Where(m => m.Id == -1);
                }
            }

            if (!string.IsNullOrWhiteSpace(createTimeStart) && createTimeStart.IsDateTime2())
            {
                DateTime temp = DateTime.Parse(createTimeStart);
               query = query.Where(m => m.CreateTime >= temp);
            }

            if (!string.IsNullOrWhiteSpace(createTimeEnd) && createTimeEnd.IsDateTime2())
            {
                DateTime temp = DateTime.Parse(createTimeEnd);
                query = query.Where(m => m.CreateTime <= temp);
            }

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Order>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="address"></param>
        /// <param name="member"></param>
        /// <param name="actionSource"></param>
        /// <returns></returns>
        public bool Update(Order order, OrderAddress address, Account member, ActionSource actionSource)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //1、修改订单信息
                    this.Update(order);

                    //2、修改订单收货地址信息
                    this.OrderAddressService.Update(address);

                    //3、添加订单日志
                    this.OrderLogService.Insert(new OrderLog()
                    {
                        Content = "修改订单信息",
                        OrderId = order.Id,
                        Source = actionSource.GetDescription(), //操作来源，1：前台   2：后台
                        Type = "修改订单",
                        UserId = member.Id,
                        UserName = member.Name + "（" + member.LoginId + "）",
                        JsonData = order.SerializeObject()
                    });

                    //4、添加系统日志
                    this.ActionLogService.Insert(ActionType.Create, actionSource, member.Id, member.Name + "（" + member.LoginId + "）","修改订单信息，订单号："+order.SN,order.SerializeObject() );

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
          
        }

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
        public bool Send(long orderId, string remark, string expressCompany, string expressSN, string expressRemark, ActionSource actionSource, Account account)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                   
                    //1、修改订单信息
                    var order = this.QueryEntity(orderId);
                    if (order == null || order.Id <= 0)
                        throw new Exception("订单不存在");

                    if (order.Status != (int)OrderStatusEnum.WaitSend)
                        throw new Exception("只有等待发货的订单才能操作");

                    order.Status = (int)OrderStatusEnum.WaitReceive;
                    order.StatusTime4 = DateTime.Now;
                    order.StatusTime = DateTime.Now;
                    order.ExpressCompany = expressCompany;
                    order.ExpressRemark = expressRemark;
                    order.ExpressSN = expressSN;
                    this.Update(order);

                    //2、添加订单日志
                    this.OrderLogService.Insert(new OrderLog()
                    {
                        Content = "发货"+(string.IsNullOrWhiteSpace(remark)?"":"，操作备注："+remark),
                        OrderId = order.Id,
                        Source = actionSource.GetDescription(), //操作来源，1：前台   2：后台
                        Type = "发货",
                        UserId = account.Id,
                        UserName = account.Name + "（" + account.LoginId + "）",
                        JsonData = order.SerializeObject()
                    });

                    //3、添加系统日志
                    this.ActionLogService.Insert(ActionType.Create, actionSource, account.Id, account.Name + "（" + account.LoginId + "）", "订单确认发货，订单号：" + order.SN, order.SerializeObject());

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="remark">操作备注</param>
        /// <param name="actionSource">操作来源</param>
        /// <param name="account">操作帐号</param>
        /// <returns></returns>
        public bool Receive(long orderId, string remark, ActionSource actionSource, Account account)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //1、修改订单信息
                    var order = this.QueryEntity(orderId);
                    if (order == null || order.Id <= 0)
                        throw new Exception("订单不存在");

                    if (order.Status != (int)OrderStatusEnum.WaitReceive)
                        throw new Exception("只有等待收货的订单才能操作");

                    order.Status = (int)OrderStatusEnum.Finish;
                    order.StatusTime6 = DateTime.Now;
                    order.StatusTime = DateTime.Now;
                    this.Update(order);

                    //2、添加订单日志
                    this.OrderLogService.Insert(new OrderLog()
                    {
                        Content = "收货" + (string.IsNullOrWhiteSpace(remark) ? "" : "，操作备注：" + remark),
                        OrderId = order.Id,
                        Source = actionSource.GetDescription(), //操作来源，1：前台   2：后台
                        Type = "收货",
                        UserId = account.Id,
                        UserName = account.Name + "（" + account.LoginId + "）",
                        JsonData = order.SerializeObject()
                    });

                    //3、添加系统日志
                    this.ActionLogService.Insert(ActionType.Create, actionSource, account.Id, account.Name + "（" + account.LoginId + "）", "订单确认收货，订单号：" + order.SN, order.SerializeObject());

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="remark">操作备注</param>
        /// <param name="actionSource">操作来源</param>
        /// <param name="account">操作帐号</param>
        /// <returns></returns>
        public bool Cancel(long orderId, string remark, ActionSource actionSource, Account account)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //1、修改订单信息
                    var order = this.QueryEntity(orderId);
                    if (order == null || order.Id <= 0)
                        throw new Exception("订单不存在");

                    if (order.Status == (int)OrderStatusEnum.Cancel || order.Status == (int)OrderStatusEnum.Finish)
                        throw new Exception("已取消或已完成的订单不运行操作");

                    order.Status = (int)OrderStatusEnum.Cancel;
                    order.StatusTime5 = DateTime.Now;
                    order.StatusTime = DateTime.Now;
                    this.Update(order);

                    //2、添加订单日志
                    this.OrderLogService.Insert(new OrderLog()
                    {
                        Content = "取消" + (string.IsNullOrWhiteSpace(remark) ? "" : "，操作备注：" + remark),
                        OrderId = order.Id,
                        Source = actionSource.GetDescription(), //操作来源，1：前台   2：后台
                        Type = "取消",
                        UserId = account.Id,
                        UserName = account.Name + "（" + account.LoginId + "）",
                        JsonData = order.SerializeObject()
                    });

                    //3、添加系统日志
                    this.ActionLogService.Insert(ActionType.Create, actionSource, account.Id, account.Name + "（" + account.LoginId + "）", "取消订单，订单号：" + order.SN, order.SerializeObject());

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
