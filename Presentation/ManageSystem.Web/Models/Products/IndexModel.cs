using ManageSystem.Core.Domain.Products;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Products
{
    public class IndexModel
    {
        /// <summary>
        /// 查询条件的数据封装
        /// </summary>
        public IndexSearchModel SearchModel { get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public  PagedList<ProductModel> PageList { get; set; }

        /// <summary>
        /// 按商品类型查询的列表数据
        /// </summary>
        public List<ProductCategory> SearchProductCategoryList { get; set; }

        /// <summary>
        /// 按商品品牌查询的列表数据
        /// </summary>
        public List<ProductBrand> SearchProductBrandList { get; set; }

        /// <summary>
        /// 查询排序方式列表数据
        /// </summary>
        public List<IndexSearchOrder> SearchOrderList { get; set; }

    }


}