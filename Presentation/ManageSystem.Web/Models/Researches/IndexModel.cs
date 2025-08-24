using ManageSystem.Core.Domain.Researches;
using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Researches
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
        public  PagedList<Research> PageList { get; set; }

        /// <summary>
        /// 按会议类型查询的列表数据
        /// </summary>
        public List<ResearchType> SearchResearchTypeList { get; set; }

        /// <summary>
        /// 按地区（省份）查询的列表数据
        /// </summary>
        public List<Area> SearchAreaList { get; set; }


        /// <summary>
        /// 按天查询的列表数据
        /// </summary>
        public List<IndexSearchDay> SearchDayList { get; set; }

        /// <summary>
        /// 查询排序方式列表数据
        /// </summary>
        public List<IndexSearchOrder> SearchOrderList { get; set; }

    }


}