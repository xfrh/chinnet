using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Infrastructure;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Products
{
    /// <summary>
    /// 操作类 ，数据库表名：Product 
    /// </summary>
    public partial class ProductService : BaseService<Product>, IProductService
    {
        public readonly IProductImageService ProductImageService;
        private readonly IActionLogService ActionLogService;

        public ProductService(IRepository<Product> repository,
              IActionLogService actionLogService,
            IProductImageService productImageService
            ) : base(repository)
        {
            this.ProductImageService = productImageService;
            this.ActionLogService = actionLogService;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="imgageList">对应的商品图片</param>
        /// <param name="account">操作用户数据</param>
        /// <returns></returns>
        public bool Insert(Product entity, List<ProductImage> imgageList, Account account)
        {
            if (entity == null) throw new Exception("保存数据不能为空");
            if (imgageList == null || !imgageList.Any()) throw new Exception("商品图片不能为空");

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                imgageList = imgageList.OrderBy(m => m.Sort).ToList();

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //1、遍历商品图片，设置排序和默认
                    for (int i = 0; i < imgageList.Count; i++)
                    {
                        var imgModel = imgageList[i];
                        if (imgModel.Id <= 0) continue;

                        if (i == 0)
                        {
                            entity.MainImage = imgModel.Path;  //将主图的地址冗余到商品表中
                            imgModel.IsMain = true;
                        }

                        this.ProductImageService.Update(m => m.Id == imgModel.Id, p => new ProductImage { IsMain = imgModel.IsMain, Sort = imgModel.Sort, UpdateTime = DateTime.Now });
                    }

                    //2、保存商品主表
                    this.Insert(entity);
                    if (entity.Id <= 0) throw new Exception("保存数据失败，请重试");

                    //3、保存日志数据
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Admin, account.Id, account.Name + "（" + account.LoginId + "）", "添加商品成功，商品名称：" + entity.Name, entity.SerializeObject());

                    tran.Commit();
                }
                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="imgageList"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool Update(Product entity, List<ProductImage> imgageList, Account account)
        {
            if (entity == null) throw new Exception("保存数据不能为空");
            if (imgageList == null || !imgageList.Any()) throw new Exception("商品图片不能为空");

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                imgageList = imgageList.OrderBy(m => m.Sort).ToList();

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //1、遍历商品图片，设置排序和默认
                    for (int i = 0; i < imgageList.Count; i++)
                    {
                        var imgModel = imgageList[i];
                        if (imgModel.Id <= 0) continue;

                        if (i == 0)
                        {
                            var mainImageTemp = this.ProductImageService.QueryEntity(imgModel.Id);
                            entity.MainImage = mainImageTemp.Path;  //将主图的地址冗余到商品表中
                            imgModel.IsMain = true;
                        }

                        this.ProductImageService.Update(m => m.Id == imgModel.Id, p => new ProductImage { IsMain = imgModel.IsMain, Sort = imgModel.Sort, UpdateTime = DateTime.Now });
                    }

                    //2、保存商品主表
                    this.Update(entity);
                    if (entity.Id <= 0) throw new Exception("修改数据失败，请重试");

                    //3、保存日志数据
                    this.ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, account.Id, account.Name + "（" + account.LoginId + "）", "修改商品成功，商品名称：" + entity.Name, entity.SerializeObject());

                    tran.Commit();
                }
                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Product> QueryPage(string name, long categoryId, long brandId, int status, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (categoryId>0)
                query = query.Where(m => m.ProductCategoryId == categoryId);

            if (brandId > 0)
                query = query.Where(m => m.ProductBrandId == brandId);

            if (status > 0)
                query = query.Where(m => m.Status == status);

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Product>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IQueryable<Product> Query(long categoryId, long brandId, int orderBy)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.Status == (int)ProductStatusEnum.Putaway);

            if (categoryId > 0)
                query = query.Where(m => m.ProductCategoryId == categoryId);

            if (brandId > 0)
                query = query.Where(m => m.ProductBrandId == brandId);

            if (orderBy == 0)
            {
                //按新品
                query = query.OrderByDescending(m => m.InsertTime).ThenByDescending(m => m.Id);
            }
            else if (orderBy == 1)
            {
                //按兑换排行
                query = query.OrderByDescending(m => m.BuyCount).ThenByDescending(m => m.Id);
            }
            else if (orderBy == 2)
            {
                //按积分值
                query = query.OrderBy(m => m.MarketPrice).ThenByDescending(m => m.Id);
            }

            return query;
        }
    }
}
