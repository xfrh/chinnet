using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Products
{
    /// <summary>
    ///  积分兑换首页查询条件
    /// </summary>
    public class IndexSearchModel
    {
        /// <summary>
        ///  商品的品牌
        /// </summary>
        public long Brand { get; set; }

        /// <summary>
        /// 商品的分类
        /// </summary>
        public long Category { get; set; }

        /// <summary>
        /// 分页，页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页，每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序方式 , 0：按时间    1：按兑换排行    2：按积分值
        /// </summary>
        public int Order { get; set; }


    }
}