using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Admin.Extensions;
using ManageSystem.Framework.Controllers;
using ManageSystem.Framework.Kendoui;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Services.Orders;
using ManageSystem.Admin.Models.Orders;
using ManageSystem.Core.Domain.Orders;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Products;
using ManageSystem.Services.Members;
using ManageSystem.Core.Extensions;

namespace ManageSystem.Admin.Controllers
{

    public class OrdersController : AdminBaseController
    {
        private readonly IProductService ProductService;
        private readonly IProductBrandService ProductBrandService;
        private readonly IProductCategoryService ProductCategoryService;
        private readonly IProductImageService ProductImageService;
        private readonly IMemberAddressService MemberAddressService;
        private readonly IMemberCartService MemberCartService;
        private readonly IMemberService MemberService;
        private readonly IOrderItemService OrderItemService;
        private readonly IOrderLogService OrderLogService;
        private readonly IMemberIntegralLogService MemberIntegralLogService;
        private readonly IOrderAddressService OrderAddressService;
        private readonly IOrderService OrderService;

        public OrdersController(
              IProductService _productService,
              IProductBrandService _productBrandService,
              IProductCategoryService _productCategoryService,
              IProductImageService _productImageService,
              IMemberAddressService _memberAddressService,
              IMemberCartService _memberCartService,
              IMemberService _memberService,
              IOrderItemService _orderItemService,
              IOrderLogService _orderLogService,
              IMemberIntegralLogService _memberIntegralLogService,
              IOrderAddressService _orderAddressService,
                   IOrderService _orderService
              )
        {
            this.ProductService = _productService;
            this.ProductBrandService = _productBrandService;
            this.ProductCategoryService = _productCategoryService;
            this.ProductImageService = _productImageService;
            this.MemberAddressService = _memberAddressService;
            this.MemberCartService = _memberCartService;
            this.MemberService = _memberService;
            this.OrderLogService = _orderLogService;
            this.MemberIntegralLogService = _memberIntegralLogService;
            this.OrderAddressService = _orderAddressService;
            this.OrderItemService = _orderItemService;
            this.OrderService = _orderService;
        }



        #region 订单管理

        private bool OrderEditRole = false; //编辑订单
        private bool OrderViewRole = false;//查看到达
        private bool OrderSendRole = false;//订单发货
        private bool OrderReceiveRole = false;//订单收货
        private bool OrderFinishRole = false;//订单完成
        private bool OrderCancelRole = false;//订单取消

        /// <summary>
        /// 订单 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderList()
        {
            OrderListSearchModel model = new OrderListSearchModel();
            model.StatusList = OrderStatusEnum.Cancel.ToSelectList(false, true).ToList();

            return View(model);
        }

        /// <summary>
        /// 订单 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderList(DataSourceRequest command, OrderListSearchModel model)
        {
            try
            {
                //检查权限
                this.OrderEditRole = ManageSystem.Services.Users.UserinfoExtensions.CheckFunction("/Orders/OrderEdit");
                this.OrderViewRole = ManageSystem.Services.Users.UserinfoExtensions.CheckFunction("/Orders/OrderView");
                this.OrderSendRole = ManageSystem.Services.Users.UserinfoExtensions.CheckFunction("/Orders/OrderSend");
                this.OrderReceiveRole = ManageSystem.Services.Users.UserinfoExtensions.CheckFunction("/Orders/OrderReceive");
                this.OrderFinishRole = ManageSystem.Services.Users.UserinfoExtensions.CheckFunction("/Orders/OrderFinish");
                this.OrderCancelRole = ManageSystem.Services.Users.UserinfoExtensions.CheckFunction("/Orders/OrderCancel");

                //获得数据
                var list = this.OrderService.QueryPage(model.SN, model.Status, model.MemberLoginId, model.UserName, model.UserPhone, model.CreateTimeStart, model.CreateTimeEnd, command.Page - 1, command.PageSize);
                var data = list.Select(x =>
                {
                    return new OrderListModel()
                    {
                        Id = x.Id.ToString(),
                        IdLong = x.Id,
                        Amount = x.Amount,
                        CreateTime = x.CreateTime,
                        CreateTimeString = x.CreateTime.GetNormalString(),
                        InsertTime = x.InsertTime,
                        MemberId = x.MemberId,
                        MemberLoginId = "",
                        PayType = x.PayType,
                        ReceivedAmount = x.ReceivedAmount,
                        Remark = x.Remark,
                        SN = x.SN,
                        Status = x.Status,
                        StatusTime = x.StatusTime,
                        StatusTimeString = x.StatusTime.GetNormalString(),
                        SystemRemark = x.SystemRemark,
                        StatusName = ((OrderStatusEnum)x.Status).GetDescription(),
                        UserAddress = "",
                        UserPhone = "",
                        UserName = "",
                    };
                }).ToList();

                //填充部分数据

                //订单收货地址
                List<long> orderIds = data.Select(m => m.IdLong).ToList();
                var addressList = this.OrderAddressService.Query(m => m.Mark > 0 && orderIds.Contains(m.OrderId));

                //填充订单会员帐号
                List<long> memberIds = data.Select(m => m.MemberId).ToList();
                var memberList = this.MemberService.Query(m => m.Mark > 0 && memberIds.Contains(m.Id));

                foreach (var item in data)
                {
                    //收货地址
                    var addressTemp = addressList.Where(m => m.OrderId == item.IdLong).FirstOrDefault();
                    if (addressTemp != null && addressTemp.Id > 0)
                    {
                        item.UserName = addressTemp.Name;
                        item.UserPhone = addressTemp.Phone;
                        item.UserAddress = addressTemp.Address;
                    }

                    //会员信息
                    var memberTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                    if (memberTemp != null && memberTemp.Id > 0)
                        item.MemberLoginId = memberTemp.LoginId;

                    //操作按钮
                    item.ActionHtml = this.GetReferralActionHtml(item);
                }

                var gridModel = new DataSourceResult
                {
                    Data = data,
                    Total = list.TotalCount
                };

                return new JsonResult { Data = gridModel };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = null };
            }
        }

        /// <summary>
        /// 获取页面的权限按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetReferralActionHtml(OrderListModel model)
        {
            StringBuilder sb = new StringBuilder();

            if (this.OrderViewRole)
                sb.Append("<a href=\"/Orders/OrderView?id=" + model.Id + "\" >查看</a>&nbsp;&nbsp;");

            switch ((OrderStatusEnum)model.Status)
            {
                case OrderStatusEnum.WaitPay:

                    if (this.OrderEditRole)
                        sb.Append("<a href=\"/Orders/OrderEdit?id=" + model.Id + "\" >编辑</a>&nbsp;&nbsp;");

                    if (this.OrderCancelRole)
                        sb.Append("<a href=\"javascript:OrderCancelWindow('" + model.Id + "')\" >取消</a>&nbsp;&nbsp;");

                    break;
                case OrderStatusEnum.WaitSend:
                    if (this.OrderEditRole)
                        sb.Append("<a href=\"/Orders/OrderEdit?id=" + model.Id + "\" >编辑</a>&nbsp;&nbsp;");

                    if (this.OrderSendRole)
                        sb.Append("<a href=\"javascript:OrderSendWindow('" + model.Id + "')\" >发货</a>&nbsp;&nbsp;");

                    if (this.OrderCancelRole)
                        sb.Append("<a href=\"javascript:OrderCancelWindow('" + model.Id + "')\" >取消</a>&nbsp;&nbsp;");
                    break;
                case OrderStatusEnum.WaitReceive:
                    if (this.OrderEditRole)
                        sb.Append("<a href=\"/Orders/OrderEdit?id=" + model.Id + "\" >编辑</a>&nbsp;&nbsp;");

                    if (this.OrderReceiveRole)
                        sb.Append("<a href=\"javascript:OrderReceiveWindow('" + model.Id + "')\" >收货</a>&nbsp;&nbsp;");

                    if (this.OrderCancelRole)
                        sb.Append("<a href=\"javascript:OrderCancelWindow('" + model.Id + "')\" >取消</a>&nbsp;&nbsp;");
                    break;
                case OrderStatusEnum.Cancel:
                    break;
                case OrderStatusEnum.Finish:
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 订单 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderModel SetOrderCreateData()
        {
            OrderModel model = new OrderModel();

            return model;
        }

        /// <summary>
        /// 订单 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderCreate()
        {
            return View(this.SetOrderCreateData());

        }

        /// <summary>
        /// 订单 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderCreate(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();

                this.OrderService.Insert(entity);

                string logContent = "添加订单 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderCreate");
            }

            return View(this.SetOrderCreateData());
        }


        /// <summary>
        /// 订单 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderModel SetOrderEditData(long id)
        {
            var entity = this.OrderService.QueryEntity(id);

            var model = entity.ToModel();
            model.StatusName = ((OrderStatusEnum)model.Status).GetDescription();

            //会员信息
            var memberEnity = this.MemberService.QueryEntity(model.MemberId);
            model.MemberLoginId = memberEnity == null ? "" : memberEnity.LoginId;

            //收货信息
            var addressEntity = this.OrderAddressService.QueryEntity(m => m.OrderId == model.Id);
            if (addressEntity != null && addressEntity.Id > 0)
            {
                model.AddressAddress = addressEntity.Address;
                model.AddressArea = addressEntity.Area;
                model.AddressCityId = addressEntity.CityId;
                model.AddressDistricts = addressEntity.DistrictsId;
                model.AddressEmail = addressEntity.Email;
                model.AddressMemberAddressId = addressEntity.MemberAddressId;
                model.AddressName = addressEntity.Name;
                model.AddressPhone = addressEntity.Phone;
                model.AddressProvinceId = addressEntity.ProvinceId;
                model.AddressTel = addressEntity.Tel;
                model.AddressZipPostalCode = addressEntity.ZipPostalCode;
            }

            //订单明细
            model.OrderItemList = this.OrderItemService.Query(m => m.OrderId == model.Id && m.Mark > 0).OrderBy(m => m.InsertTime).Select(x => x.ToModel()).ToList();

            //订单操作备注
            model.OrderLogList = this.OrderLogService.Query(m => m.OrderId == model.Id && m.Mark > 0).OrderBy(m => m.InsertTime).Select(x => x.ToModel()).ToList();

            return model;
        }

        /// <summary>
        /// 订单 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderEdit(long id)
        {
            return this.View(this.SetOrderEditData(id));
        }

        /// <summary>
        /// 订单 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderEdit(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.OrderService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);

                //订单信息
                entity.Describe = model.Describe;
                entity.ExpressCompany = model.ExpressCompany;
                entity.ExpressRemark = model.ExpressRemark;
                entity.ExpressSN = model.ExpressSN;

                //收货地址信息
                var orderAddressEntity = this.OrderAddressService.QueryEntity(m => m.OrderId == entity.Id && m.Mark > 0);
                orderAddressEntity.Name = model.AddressName;
                orderAddressEntity.Phone = model.AddressPhone;
                orderAddressEntity.Tel = model.AddressTel;
                orderAddressEntity.Email = model.AddressEmail;
                orderAddressEntity.Address = model.AddressAddress;


                try
                {
                    this.OrderService.Update(entity, orderAddressEntity, base.LoginUserinfo, ActionSource.Admin);
                    base.SuccessNotification("修改订单成功");
                    return this.RedirectToAction("OrderList");
                }
                catch (Exception ex)
                {
                    base.ErrorNotification(ex.Message);
                }
            }

            return this.View(this.SetOrderEditData(model.Id));
        }


        /// <summary>
        /// 订单 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderView(long id)
        {
            return this.View(this.SetOrderEditData(id));
        }


        /// <summary>
        /// 订单 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("OrderList");
            }

            this.OrderService.Delete(selectedIds);

            string logContent = "【手动】删除订单，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("OrderList");
        }


        /// <summary>
        /// 订单确认发货
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSend(long id)
        {
            OrderModel model = new OrderModel();

            var entity = this.OrderService.QueryEntity(id);

            model = entity.ToModel();
            return this.View(model);
        }

        /// <summary>
        /// 订单确认发货
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult OrderSend(OrderModel model)
        {
            if (model.Id <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "订单数据不能为空"));

            try
            {
                if (this.OrderService.Send(model.Id, model.Describe,model.ExpressCompany,model.ExpressSN,model.ExpressRemark, ActionSource.Admin, base.LoginUserinfo))
                    return this.Content(JsonHelper.GetBaseMessage(true, "确认发货成功"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "确认发货失败，失败原因：" + ex.Message));
            }

            return this.Content(JsonHelper.GetBaseMessage(false, "确认发货失败"));
        }

        /// <summary>
        /// 订单确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderReceive(long id)
        {
            OrderModel model = new OrderModel();

            var entity = this.OrderService.QueryEntity(id);

            model = entity.ToModel();
            return this.View(model);
        }

        /// <summary>
        /// 订单确认收货
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult OrderReceive(OrderModel model)
        {
            if (model.Id <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "订单数据不能为空"));

            try
            {
                if (this.OrderService.Receive(model.Id, model.Describe, ActionSource.Admin, base.LoginUserinfo))
                    return this.Content(JsonHelper.GetBaseMessage(true, "确认收货成功"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "确认收货失败，失败原因：" + ex.Message));
            }

            return this.Content(JsonHelper.GetBaseMessage(false, "确认收货失败"));
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderCancel(long id)
        {
            OrderModel model = new OrderModel();

            var entity = this.OrderService.QueryEntity(id);

            model = entity.ToModel();
            return this.View(model);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult OrderCancel(OrderModel model)
        {
            if (model.Id <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "订单数据不能为空"));

            try
            {
                if (this.OrderService.Cancel(model.Id, model.Describe, ActionSource.Admin, base.LoginUserinfo))
                    return this.Content(JsonHelper.GetBaseMessage(true, "取消订单成功"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "取消订单失败，失败原因：" + ex.Message));
            }

            return this.Content(JsonHelper.GetBaseMessage(false, "取消订单失败"));
        }


        #endregion

        #region 订单收货信息

        /// <summary>
        /// 订单收货信息 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderAddressList()
        {
            return View();
        }

        /// <summary>
        /// 订单收货信息 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderAddressList(DataSourceRequest command, OrderAddressModel model)
        {

            //获得数据
            var list = this.OrderAddressService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 订单收货信息 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderAddressModel SetOrderAddressCreateData()
        {
            OrderAddressModel model = new OrderAddressModel();

            return model;
        }

        /// <summary>
        /// 订单收货信息 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderAddressCreate()
        {
            return View(this.SetOrderAddressCreateData());

        }

        /// <summary>
        /// 订单收货信息 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderAddressCreate(OrderAddressModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.OrderAddressService.Insert(entity);

                string logContent = "添加订单收货信息 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderAddressCreate");
            }

            return View(this.SetOrderAddressCreateData());
        }


        /// <summary>
        /// 订单收货信息 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderAddressModel SetOrderAddressEditData(long id)
        {
            var entity = this.OrderAddressService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 订单收货信息 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderAddressEdit(long id)
        {
            return this.View(this.SetOrderAddressEditData(id));
        }

        /// <summary>
        /// 订单收货信息 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderAddressEdit(OrderAddressModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.OrderAddressService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.OrderAddressService.Update(entity);

                string logContent = "修改订单收货信息 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderAddressEdit");
            }
            return this.View(this.SetOrderAddressEditData(model.Id));
        }


        /// <summary>
        /// 订单收货信息 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderAddressView(long id)
        {
            return this.View(this.SetOrderAddressEditData(id));
        }


        /// <summary>
        /// 订单收货信息 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderAddressDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("OrderAddressList");
            }

            this.OrderAddressService.Delete(selectedIds);

            string logContent = "【手动】删除订单收货信息，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("OrderAddressList");
        }


        #endregion

        #region 订单明细

        /// <summary>
        /// 订单明细 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderItemList()
        {
            return View();
        }

        /// <summary>
        /// 订单明细 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderItemList(DataSourceRequest command, OrderItemModel model)
        {

            //获得数据
            var list = this.OrderItemService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 订单明细 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderItemModel SetOrderItemCreateData()
        {
            OrderItemModel model = new OrderItemModel();

            return model;
        }

        /// <summary>
        /// 订单明细 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderItemCreate()
        {
            return View(this.SetOrderItemCreateData());

        }

        /// <summary>
        /// 订单明细 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderItemCreate(OrderItemModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.OrderItemService.Insert(entity);

                string logContent = "添加订单明细 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderItemCreate");
            }

            return View(this.SetOrderItemCreateData());
        }


        /// <summary>
        /// 订单明细 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderItemModel SetOrderItemEditData(long id)
        {
            var entity = this.OrderItemService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 订单明细 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderItemEdit(long id)
        {
            return this.View(this.SetOrderItemEditData(id));
        }

        /// <summary>
        /// 订单明细 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderItemEdit(OrderItemModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.OrderItemService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.OrderItemService.Update(entity);

                string logContent = "修改订单明细 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderItemEdit");
            }
            return this.View(this.SetOrderItemEditData(model.Id));
        }


        /// <summary>
        /// 订单明细 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderItemView(long id)
        {
            return this.View(this.SetOrderItemEditData(id));
        }


        /// <summary>
        /// 订单明细 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderItemDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("OrderItemList");
            }

            this.OrderItemService.Delete(selectedIds);

            string logContent = "【手动】删除订单明细，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("OrderItemList");
        }


        #endregion

        #region 订单日志

        /// <summary>
        /// 订单日志 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderLogList()
        {
            return View();
        }

        /// <summary>
        /// 订单日志 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderLogList(DataSourceRequest command, OrderLogModel model)
        {

            //获得数据
            var list = this.OrderLogService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 订单日志 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderLogModel SetOrderLogCreateData()
        {
            OrderLogModel model = new OrderLogModel();

            return model;
        }

        /// <summary>
        /// 订单日志 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderLogCreate()
        {
            return View(this.SetOrderLogCreateData());

        }

        /// <summary>
        /// 订单日志 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderLogCreate(OrderLogModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.OrderLogService.Insert(entity);

                string logContent = "添加订单日志 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderLogCreate");
            }

            return View(this.SetOrderLogCreateData());
        }


        /// <summary>
        /// 订单日志 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public OrderLogModel SetOrderLogEditData(long id)
        {
            var entity = this.OrderLogService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 订单日志 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderLogEdit(long id)
        {
            return this.View(this.SetOrderLogEditData(id));
        }

        /// <summary>
        /// 订单日志 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderLogEdit(OrderLogModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.OrderLogService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.OrderLogService.Update(entity);

                string logContent = "修改订单日志 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("OrderLogEdit");
            }
            return this.View(this.SetOrderLogEditData(model.Id));
        }


        /// <summary>
        /// 订单日志 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderLogView(long id)
        {
            return this.View(this.SetOrderLogEditData(id));
        }


        /// <summary>
        /// 订单日志 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderLogDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("OrderLogList");
            }

            this.OrderLogService.Delete(selectedIds);

            string logContent = "【手动】删除订单日志，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("OrderLogList");
        }


        #endregion

    }
}
