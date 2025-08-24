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
using ManageSystem.Services.Products;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Core.Domain.Products;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Members;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.Users;
using ManageSystem.Core.Extensions;
using System.IO;
using ManageSystem.Admin.App_Start;
using System.Text.RegularExpressions;
using ManageSystem.Services.Security;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.SystemSet;
using ManageSystem.Framework.Upload;

namespace ManageSystem.Admin.Controllers
{

    public class ProductsController : AdminBaseController
    {
        private readonly IProductService ProductService;
        private readonly IProductBrandService ProductBrandService;
        private readonly IProductCategoryService ProductCategoryService;
        private readonly IProductImageService ProductImageService;
        public ProductsController(
            IProductService _productService,
             IProductBrandService _productBrandService,
               IProductCategoryService _productCategoryService,
                IProductImageService _productImageService
        )
        {
            this.ProductService = _productService;
            this.ProductBrandService = _productBrandService;
            this.ProductCategoryService = _productCategoryService;
            this.ProductImageService = _productImageService;
        }

        #region 商品

        /// <summary>
        /// 商品 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductList()
        {
            ProductModel model = new ProductModel();
            model.StatusList = ProductStatusEnum.Putaway.ToSelectList().ToList();
            model.ProductBrandList = this.ProductBrandService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProductCategoryList = this.ProductCategoryService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProductBrandList.Insert(0, new SelectListItem() { Text = "全部品牌", Value = "0" });
            model.ProductCategoryList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "0" });

            return View(model);
        }

        /// <summary>
        /// 商品 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProductList(DataSourceRequest command, ProductModel model)
        {
            //获得数据
            var list = this.ProductService.QueryPage(model.Name, model.ProductCategoryId, model.ProductBrandId, model.Status, command.Page - 1, command.PageSize);

            var data = list.Select(x => x.ToModel()).ToList();

            //填充分类的数据
            IList<long> categoryIds = data.Select(m => m.ProductCategoryId).ToList();
            var categoryList = this.ProductCategoryService.Query(m => categoryIds.Contains(m.Id));
            if (categoryList != null && categoryList.Any())
            {
                foreach (var item in data)
                {
                    var categoryTemp = categoryList.Where(m => m.Id == item.ProductCategoryId).FirstOrDefault();
                    if (categoryTemp != null && categoryTemp.Id > 0)
                        item.ProductCategoryName = categoryTemp.Name;
                }
            }

            //填充品牌的数据
            IList<long> brandIds = data.Select(m => m.ProductBrandId).ToList();
            var brandList = this.ProductBrandService.Query(m => brandIds.Contains(m.Id));
            if (brandList != null && brandList.Any())
            {
                foreach (var item in data)
                {
                    var brandTemp = brandList.Where(m => m.Id == item.ProductBrandId).FirstOrDefault();
                    if (brandTemp != null && brandTemp.Id > 0)
                        item.ProductBrandName = brandTemp.Name;
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = data.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        ProductCategoryName = x.ProductCategoryName,
                        ProductBrandName = x.ProductBrandName,
                        Count = x.Count,
                        MarketPrice = x.MarketPrice,
                        Code = x.Code,
                        Name = x.Name,
                        CostPrice = x.CostPrice,
                        Status = ((ProductStatusEnum)x.Status).GetDescription(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 商品 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductModel SetProductCreateData()
        {
            ProductModel model = new ProductModel();
            model.ProductBrandList = this.ProductBrandService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProductCategoryList = this.ProductCategoryService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.StatusList = ProductStatusEnum.Putaway.ToSelectList(true, false).ToList();
            model.Id = CommonHelper.GuidToLongID;

            return model;
        }

        /// <summary>
        /// 商品 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCreate()
        {
            return View(this.SetProductCreateData());

        }

        /// <summary>
        /// 商品 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductCreate(ProductModel model)
        {
            List<ProductImage> imgageList = model.ProductImageJson.DeserializeObject<List<ProductImage>>();
            if (imgageList == null || !imgageList.Any())
                ModelState.AddModelError("ProductImageJson", "选择的商品图片错误，请重试");

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = model.ToEntity();
                    this.ProductService.Insert(entity, imgageList, base.LoginUserinfo);

                    base.SuccessNotification("添加商品成功，商品名称：" + model.Name);

                    return this.RedirectToAction("ProductCreate");
                }
                catch (Exception ex)
                {
                    base.ErrorNotification(ex.Message);
                }
            }

            return View(this.SetProductCreateData());
        }


        /// <summary>
        /// 商品 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductModel SetProductEditData(long id)
        {
            var entity = this.ProductService.QueryEntity(id);

            var model = entity.ToModel();
            model.ProductBrandList = this.ProductBrandService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProductCategoryList = this.ProductCategoryService.Query(m => m.ParentId == 0 && m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.StatusList = ((ProductStatusEnum)model.Status).ToSelectList(true, false).ToList();

            model.ProductImageList = this.ProductImageService.Query(m => m.ProductId == model.Id && m.Mark > 0).OrderBy(m => m.Sort).ToList();
            return model;
        }

        /// <summary>
        /// 商品 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductEdit(long id)
        {
            ProductModel model = this.SetProductEditData(id);
            model.ProductImageJson = "test";  //随便设置一个值，保证如果有图片但是不选择的验证

            return this.View(model);
        }

        /// <summary>
        /// 商品 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductEdit(ProductModel model)
        {
            List<ProductImage> imgageList = model.ProductImageJson.DeserializeObject<List<ProductImage>>();
            if (imgageList == null || !imgageList.Any())
                ModelState.AddModelError("ProductImageJson", "选择的商品图片错误，请重试");

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = this.ProductService.QueryEntity(model.Id);
                    base.SetDefaultValue(model, entity);
                    entity = model.ToEntity(entity);

                    this.ProductService.Update(entity, imgageList, base.LoginUserinfo);
                    base.SuccessNotification("修改商品成功，商品名称：" + model.Name);

                    return this.RedirectToAction("ProductList");
                }
                catch (Exception ex)
                {
                    base.ErrorNotification(ex.Message);
                }
            }
            return this.View(this.SetProductEditData(model.Id));
        }


        /// <summary>
        /// 商品 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductView(long id)
        {
            return this.View(this.SetProductEditData(id));
        }


        /// <summary>
        /// 商品 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ProductList");
            }

            this.ProductService.Delete(selectedIds);

            string logContent = "【手动】删除商品，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ProductList");
        }


        /// <summary>
        /// 上传商品的图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(true, false)]
        public ContentResult UploadProductImage(long productId)
        {
            try
            {
                if (productId <= 0) throw new Exception("商品信息不能为空，请刷新页面重试！");

                UploadParameter par = new UploadParameter();
                UploadResult model = new UploadifyUpload().Upload(new UploadParameter()
                {
                    Extension = "jpg,jpeg,png,bmp",
                    FilePath = "/Content/Upload/Product/",
                    HttpFile = this.HttpContext.Request.Files,
                    MaxLength = 2 * 1024, //10M
                    NewFileName = Guid.NewGuid() + "_" + new Random().Next(10000, 99999),
                    Type = UploadTypeEnum.Image
                });

                if (model.IsSuccess)
                {
                    //保存信息到数据库
                    var entity = new ProductImage()
                    {
                        NewFileName = model.FileNewName,
                        IsMain = false,
                        OldFileName = model.FileName,
                        Sort = 100,
                        Path = model.FilePath,
                        ProductId = productId
                    };

                    this.ProductImageService.Insert(entity);
                    model.CustomValue1 = entity.Id.ToString();

                    return this.Content(model.SerializeObject());
                }
                else
                {
                    throw new Exception("上传图片失败，请刷新页面重试！");
                }
            }
            catch (Exception ex)
            {
                var errorModel = new UploadResult() { IsSuccess = false, ErrorMessage = ex.Message };
                return this.Content(errorModel.SerializeObject());
            }
        }

        /// <summary>
        /// 删除商品的图片
        /// </summary>
        /// <param name="id">商品id</param>
        /// <param name="imageId">商品图片的id</param>
        /// <returns></returns>
        public ContentResult ProductDeleteImage(long id, long imageId)
        {
            if (id <= 0 || imageId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "请选择需要删除的图片"));

            var entity = this.ProductImageService.QueryEntity(m => m.Id == imageId && m.ProductId == id);

            if (entity != null && entity.Id > 0)
            {
                this.ProductImageService.Delete(entity);
                base.InsetActionLog(ActionType.Delete, "【手动】删除商品图片，商品id：" + id + "  图片id：" + imageId + "", "");
            }

            return this.Content(JsonHelper.GetBaseMessage(true, ""));
        }

        #endregion

        #region 商品分类

        /// <summary>
        /// 商品分类 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCategoryList()
        {
            ProductCategoryModel model = new ProductCategoryModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);
        }

        /// <summary>
        /// 商品分类 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProductCategoryList(DataSourceRequest command, ProductCategoryModel model)
        {

            var list = this.ProductCategoryService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        Sort = x.Sort,
                        Status = x.Status ? "启用" : "禁用",
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 商品分类 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductCategoryModel SetProductCategoryCreateData()
        {
            ProductCategoryModel model = new ProductCategoryModel();

            return model;
        }

        /// <summary>
        /// 商品分类 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCategoryCreate()
        {
            return View(this.SetProductCategoryCreateData());

        }

        /// <summary>
        /// 商品分类 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProductCategoryCreate(ProductCategoryModel model)
        {
            model.ParentId = 0;
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ProductCategoryService.Insert(entity);

                string logContent = "添加商品分类，分类名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ProductCategoryCreate");
            }

            return View(this.SetProductCategoryCreateData());
        }


        /// <summary>
        /// 商品分类 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductCategoryModel SetProductCategoryEditData(long id)
        {
            var entity = this.ProductCategoryService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 商品分类 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCategoryEdit(long id)
        {
            return this.View(this.SetProductCategoryEditData(id));
        }

        /// <summary>
        /// 商品分类 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProductCategoryEdit(ProductCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.ProductCategoryService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                model.ParentId = entity.ParentId;
                entity = model.ToEntity(entity);

                this.ProductCategoryService.Update(entity);

                string logContent = "修改商品分类，分类名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ProductCategoryList");
            }
            return this.View(this.SetProductCategoryEditData(model.Id));
        }


        /// <summary>
        /// 商品分类 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCategoryView(long id)
        {
            return this.View(this.SetProductCategoryEditData(id));
        }


        /// <summary>
        /// 商品分类 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductCategoryDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ProductCategoryList");
            }

            this.ProductCategoryService.Delete(selectedIds);

            string logContent = "【手动】删除商品分类，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ProductCategoryList");
        }


        #endregion

        #region 商品品牌

        /// <summary>
        /// 商品品牌 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductBrandList()
        {
            ProductBrandModel model = new ProductBrandModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);
        }

        /// <summary>
        /// 商品品牌 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProductBrandList(DataSourceRequest command, ProductBrandModel model)
        {
            var list = this.ProductBrandService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        Sort = x.Sort,
                        Status = x.Status ? "启用" : "禁用",
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 商品品牌 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductBrandModel SetProductBrandCreateData()
        {
            ProductBrandModel model = new ProductBrandModel();

            return model;
        }

        /// <summary>
        /// 商品品牌 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductBrandCreate()
        {
            return View(this.SetProductBrandCreateData());

        }

        /// <summary>
        /// 商品品牌 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProductBrandCreate(ProductBrandModel model)
        {
            model.ParentId = 0;

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ProductBrandService.Insert(entity);

                string logContent = "添加商品品牌，品牌名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ProductBrandCreate");
            }

            return View(this.SetProductBrandCreateData());
        }


        /// <summary>
        /// 商品品牌 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductBrandModel SetProductBrandEditData(long id)
        {
            var entity = this.ProductBrandService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 商品品牌 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductBrandEdit(long id)
        {
            return this.View(this.SetProductBrandEditData(id));
        }

        /// <summary>
        /// 商品品牌 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProductBrandEdit(ProductBrandModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.ProductBrandService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                model.ParentId = entity.ParentId;
                entity = model.ToEntity(entity);

                this.ProductBrandService.Update(entity);

                string logContent = "修改商品品牌，品牌名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ProductBrandList");
            }
            return this.View(this.SetProductBrandEditData(model.Id));
        }


        /// <summary>
        /// 商品品牌 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductBrandView(long id)
        {
            return this.View(this.SetProductBrandEditData(id));
        }


        /// <summary>
        /// 商品品牌 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductBrandDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ProductBrandList");
            }

            this.ProductBrandService.Delete(selectedIds);

            string logContent = "【手动】删除商品品牌，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ProductBrandList");
        }


        #endregion

        #region 商品图片

        /// <summary>
        /// 商品图片 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductImageList()
        {
            return View();
        }

        /// <summary>
        /// 商品图片 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProductImageList(DataSourceRequest command, ProductImageModel model)
        {

            //获得数据
            var list = this.ProductImageService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 商品图片 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductImageModel SetProductImageCreateData()
        {
            ProductImageModel model = new ProductImageModel();

            return model;
        }

        /// <summary>
        /// 商品图片 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductImageCreate()
        {
            return View(this.SetProductImageCreateData());

        }

        /// <summary>
        /// 商品图片 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProductImageCreate(ProductImageModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ProductImageService.Insert(entity);

                string logContent = "添加商品图片 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ProductImageCreate");
            }

            return View(this.SetProductImageCreateData());
        }


        /// <summary>
        /// 商品图片 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ProductImageModel SetProductImageEditData(long id)
        {
            var entity = this.ProductImageService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 商品图片 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductImageEdit(long id)
        {
            return this.View(this.SetProductImageEditData(id));
        }

        /// <summary>
        /// 商品图片 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProductImageEdit(ProductImageModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.ProductImageService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.ProductImageService.Update(entity);

                string logContent = "修改商品图片 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ProductImageEdit");
            }
            return this.View(this.SetProductImageEditData(model.Id));
        }


        /// <summary>
        /// 商品图片 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductImageView(long id)
        {
            return this.View(this.SetProductImageEditData(id));
        }


        /// <summary>
        /// 商品图片 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductImageDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ProductImageList");
            }

            this.ProductImageService.Delete(selectedIds);

            string logContent = "【手动】删除商品图片，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ProductImageList");
        }

        #endregion

    }
}
