using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Document
{
    /// <summary>
    /// 首页查询条件
    /// </summary>
    public class IndexSearchModel
    {
        /// <summary>
        /// 分页，页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页，每页数量
        /// </summary>
        public int PageSize { get; set; }
    }
}