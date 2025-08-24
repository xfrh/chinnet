using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Core;

namespace ManageSystem.Services.Products
{
	/// <summary>
	/// 操作类 ，数据库表名：ProductBrand 
	/// </summary>
	public partial class ProductBrandService :  BaseService<ProductBrand>, IProductBrandService
	{

		public ProductBrandService(IRepository<ProductBrand> repository): base(repository)
		{
			
		}

        public IPagedList<ProductBrand> QueryPage(string name, string state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(state))
            {
                int stateTemp = int.Parse(state);
                query = query.Where(m => m.Status == (stateTemp == 1));
            }

            query = query.OrderBy(m => m.Sort);

            var list = new PagedList<ProductBrand>(query.ToList(), pageIndex, pageSize);

            return list;
        }

    }
}
