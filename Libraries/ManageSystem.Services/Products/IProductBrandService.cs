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
	/// 操作接口类 ，数据库表名：ProductBrand 
	/// </summary>
	public  partial interface IProductBrandService : IBaseService<ProductBrand>
	{
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ProductBrand> QueryPage(string name, string stateValue, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
