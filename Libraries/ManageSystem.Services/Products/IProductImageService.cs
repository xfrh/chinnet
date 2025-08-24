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
	/// 操作接口类 ，数据库表名：ProductImage 
	/// </summary>
	public  partial interface IProductImageService : IBaseService<ProductImage>
	{
        /// <summary>
        /// 获取指定商品的图片集合
        /// </summary>
        /// <param name="productId">商品id</param>
        /// <returns></returns>
        List<ProductImage> GetProductImageList(long productId);
	}
}
