using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Products;

namespace ManageSystem.Services.Products
{
	/// <summary>
	/// 操作接口类 ，数据库表名：Product 
	/// </summary>
	public  partial interface IProductService : IBaseService<Product>
	{
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="imgageList">对应的商品图片</param>
        /// <param name="account">操作用户数据</param>
        /// <returns></returns>
        bool Insert(Product entity, List<ProductImage> imgageList, Account account);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="imgageList">对应的商品图片</param>
        /// <param name="account">操作用户数据</param>
        /// <returns></returns>
        bool Update(Product entity, List<ProductImage> imgageList, Account account);


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
        IPagedList<Product> QueryPage(string name, long categoryId, long brandId, int status, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 分页查询  网页
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<Product> Query(long categoryId, long brandId,  int orderBy);
    }
}
