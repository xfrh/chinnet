using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Document
{
    public class IndexModel
    {
        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public  PagedList<ManageSystem.Core.Domain.Documents.Document> PageList { get; set; }

        /// <summary>
        /// 搜索条件
        /// </summary>
        public IndexSearchModel SearchModel { get; set; }
    }


}