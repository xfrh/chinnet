using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Products;

namespace ManageSystem.Services.Products
{
	/// <summary>
	/// 操作类 ，数据库表名：ProductImage 
	/// </summary>
	public partial class ProductImageService :  BaseService<ProductImage>, IProductImageService
	{

		public ProductImageService(IRepository<ProductImage> repository): base(repository)
		{
			
		}

        /// <summary>
        /// 获取指定商品的图片集合
        /// </summary>
        /// <param name="productId">商品id</param>
        /// <returns></returns>
        public List<ProductImage> GetProductImageList(long productId)
        {
            if (productId <= 0) return null;

            var data = this.Query(m => m.Mark > 0 && m.ProductId == productId ).OrderBy(m => m.Sort);

            if (data == null || !data.Any()) return null;
            
            List<ProductImage> result = new List<ProductImage>();
            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item.Path)) continue;
                item.Path = ProductExtensions.GetProductImage(item.Path);

                result.Add(item);
            }

            return result;
        }
    }
}
