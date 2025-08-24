using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Members;
using ManageSystem.Services.Orders;
using ManageSystem.Services.Products;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Mall;
using ManageSystem.Web.Models.Members;
using ManageSystem.Web.Models.Orders;
using ManageSystem.Web.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class MallController : WebBaseController
    {

        private readonly IProductService ProductService;
        private readonly IProductBrandService ProductBrandService;
        private readonly IProductCategoryService ProductCategoryService;
        private readonly IProductImageService ProductImageService;
        private readonly IMemberAddressService MemberAddressService;
        private readonly IMemberCartService MemberCartService;
        private readonly IMemberService MemberService;
        private readonly IOrderService OrderService;
        private readonly IOrderItemService OrderItemService;
        private readonly IOrderAddressService OrderAddressService;

        public MallController(
            IProductService _productService,
             IProductBrandService _productBrandService,
               IProductCategoryService _productCategoryService,
                IProductImageService _productImageService,
               IMemberAddressService _memberAddressService,
              IMemberCartService _memberCartService,
              IMemberService _memberService,
              IOrderService _orderService,
               IOrderItemService _orderItemService,
                  IOrderAddressService _orderAddressService
        )
        {
            this.ProductService = _productService;
            this.ProductBrandService = _productBrandService;
            this.ProductCategoryService = _productCategoryService;
            this.ProductImageService = _productImageService;
            this.MemberAddressService = _memberAddressService;
            this.MemberCartService = _memberCartService;
            this.MemberService = _memberService;
            this.OrderService = _orderService;
            this.OrderAddressService = _orderAddressService;
            this.OrderItemService = _orderItemService;
        }

        #region 积分兑换首页数据

        public ActionResult Index(IndexSearchModel model)
        {
            var result = this.SetIndexData(model);

            return View(result);
        }

        private IndexModel SetIndexData(IndexSearchModel searchModel)
        {
            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();
            model.SearchProductBrandList = this.ProductBrandService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList();
            model.SearchProductCategoryList = this.ProductCategoryService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList();
            model.SearchOrderList = this.GetSearchOrderList(); //排序条件

            var data = this.ProductService.Query(searchModel.Category, searchModel.Brand, searchModel.Order);
            //分页数据
            model.PageList = data.Select(x => new ProductModel()
            {
                Id = x.Id,
                MarketPrice = x.MarketPrice,
                CostPrice = x.CostPrice,
                Count = x.Count,
                BuyCount = x.BuyCount,
                Name = x.Name,
                InsertTime = x.InsertTime,
                Status = x.Status,
                ProductImage = x.MainImage
            }).ToPagedList<ProductModel>(searchModel.PageIndex, MvcPagerExtensions.PageSize);

            return model;
        }

        private List<IndexSearchOrder> GetSearchOrderList()
        {
            List<IndexSearchOrder> list = new List<IndexSearchOrder>();
            list.Add(new IndexSearchOrder() { Text = "新品", Value = 0 });
            list.Add(new IndexSearchOrder() { Text = "按兑换排行", Value = 1 });
            list.Add(new IndexSearchOrder() { Text = "按积分值", Value = 2 });

            return list;
        }

        #endregion

        #region  积分兑换详细页面

        /// <summary>
        /// 商品的详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Item(long id)
        {
            var entity = this.ProductService.QueryEntity(id);
            if (entity == null || entity.Id <= 0) return this.View(new ProductModel());

            var model = entity.ToModel();
            model.ProductImageList = this.ProductImageService.GetProductImageList(model.Id);

            model.ProductImage = model.MainImage;

            return View(model);
        }


        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ContentResult AddCart(MemberCart cartEntity)
        {
            try
            {
                if (cartEntity.ProductId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "商品不存在"));

                var member = base.LoginUserinfo;
                cartEntity.MemberId = member.Id;
                cartEntity.Count = cartEntity.Count <= 0 ? 1 : cartEntity.Count;

                if (this.MemberCartService.AddCart(cartEntity, member, ActionSource.Web))
                {
                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }
                return this.Content(JsonHelper.GetBaseMessage(false, "添加购物车失败"));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "商品不存在"));
            }
        }

        #endregion

        #region 购物车页面

        /// <summary>
        /// 用户购物车页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Cart()
        {
            List<MemberCartModel> cartList = new List<MemberCartModel>();
            try
            {
                cartList = this.MemberCartService.QueryByMember(base.LoginUserinfo.Id).Select(x => x.ToModel()).OrderByDescending(m => m.InsertTime).ToList();
                if (cartList == null || !cartList.Any()) return this.View(cartList);

                IList<long> productIds = cartList.Select(m => m.ProductId).ToList();

                //填充商品信息
                var productList = this.ProductService.Query(m => productIds.Contains(m.Id));
                if (productList != null && productList.Any())
                {
                    foreach (var item in cartList)
                    {
                        var temp = productList.Where(m => m.Id == item.ProductId).FirstOrDefault();
                        if (temp == null || temp.Id <= 0)
                        {
                            item.StatusMessage = "不存在";
                            item.Status = false;
                            continue;
                        }

                        if (temp.Status == (int)ProductStatusEnum.Soldout)
                        {
                            item.StatusMessage = "已下架";
                            item.Status = false;
                            item.MarketPrice = 0;
                            item.Amount = 0;
                            continue;
                        }

                        item.Status = true;
                        item.ProductName = temp.Name;
                        item.ProductImage = temp.MainImage;
                        item.MarketPrice = temp.MarketPrice;
                        item.Amount = Math.Round(temp.MarketPrice * item.Count, 2);

                    }
                }
            }
            catch (Exception)
            {

            }

            return this.View(cartList);
        }

        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DeleteCart(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在"));

                var cart = this.MemberCartService.QueryEntity(id);
                if (cart == null || cart.Id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在"));
                if (cart.MemberId != base.LoginUserinfo.Id) return this.Content(JsonHelper.GetBaseMessage(false, "无权操作"));

                this.MemberCartService.Delete(cart);

                base.InsetActionLog(ActionType.Delete, "【手动】删除购物车数据，商品id：" + cart.ProductId, cart.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "系统错误，请重试"));
            }
        }

        /// <summary>
        /// 购物车页面提交去结算的数据，将数据存储在缓存中
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ContentResult CartSettlement(List<CartSubmitModel> data)
        {
            try
            {
                if (data == null || !data.Any()) return this.Content(JsonHelper.GetBaseMessage(false, "请勾选需要结算的明细"));

                //将数据缓存24小时
                string key = Guid.NewGuid().ToString();
                base.CacheManager.Set(key, data, 60 * 24);

                if (!base.CacheManager.IsSet(key))
                    return this.Content(JsonHelper.GetBaseMessage(false, "提交结算失败，请重试！"));

                return this.Content(JsonHelper.GetBaseMessage(true, key));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "系统错误，请重试"));
            }
        }


        #endregion

        #region 订单结算页面

        /// <summary>
        /// 订单结算页面数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Settlement(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                base.ErrorNotification("数据错误，请重新操作");
                return this.RedirectToAction("Mall", "Cart");
            }

            List<CartSubmitModel> data = base.CacheManager.Get<List<CartSubmitModel>>(key);
            if (data == null || !data.Any())
            {
                base.ErrorNotification("结算数据不能为空");
                return this.RedirectToAction("Cart", "Mall");
            }

            var memberEntity = base.LoginUserinfo;
            var cartIds = data.Select(m => m.Id).ToList();

            SettlementModel model = new SettlementModel();
            model.AddressList = this.MemberAddressService.Query(m => m.MemberId == memberEntity.Id && m.Mark > 0).Select(x => x.ToModel()).OrderByDescending(m=>m.InsertTime).ToList();
            model.CartItemList = this.MemberCartService.Query(m => m.Mark > 0 && cartIds.Contains(m.Id)).Select(x => x.ToModel()).ToList();

            //填充商品信息
            IList<long> productIds = model.CartItemList.Select(m => m.ProductId).ToList();
            var productList = this.ProductService.Query(m => productIds.Contains(m.Id));
            foreach (var item in model.CartItemList)
            {
                var cartTemp = data.Where(m => m.Id == item.Id).FirstOrDefault();
                if (cartTemp == null || cartTemp.Id <= 0) continue;

                item.Count = cartTemp.Count;

                var temp = productList.Where(m => m.Id == item.ProductId).FirstOrDefault();
                if (temp == null || temp.Id <= 0 || temp.Status == (int)ProductStatusEnum.Soldout || item.Count > temp.Count)
                    continue;

                item.Status = true;
                item.ProductName = temp.Name;
                item.MarketPrice = temp.MarketPrice;
                item.Amount = Math.Round(temp.MarketPrice * item.Count, 2);
                item.ProductImage = temp.MainImage;

                model.Count += item.Count;
                model.ProductAmount += item.Amount;
                model.OrderAmount += item.Amount;
                model.Key = key;
                model.IntegralAmount = this.MemberService.QueryEntity(memberEntity.Id).IntegralAmount;
            }

            return this.View(model);
        }


        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContentResult SubmitOrder(SubmitOrderModel model)
        {
            try
            {
                //初步数据检查
                if (model == null || model.ItemList == null || !model.ItemList.Any() || model.OrderAmount <= 0)
                    throw new Exception("提交的订单数据错误，请重试");

                if (model.AddressId <= 0)
                    throw new Exception("请选择收货地址");

                var member = this.MemberService.QueryEntity(base.LoginUserinfo.Id);
                if (member == null || member.Id <= 0)
                    throw new Exception("会员不存在");

                if (model.OrderAmount > member.IntegralAmount)
                    throw new Exception("用户的可用积分不够本次支付");

                //将提交的数据转换成订单数据
                Order orderEntity = new Order();
                orderEntity.Amount = model.OrderAmount;
                orderEntity.Remark = model.Remark;

                //提交数据
                this.OrderService.Insert(orderEntity, model.ItemList, model.AddressId, base.LoginUserinfo, ActionSource.Web);

                return this.Content(JsonHelper.GetBaseMessage(true, orderEntity.SN));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        #endregion

        #region  订单提交结果页面

        public ActionResult Result()
        {
            return View();
        }


        #endregion

    }

}