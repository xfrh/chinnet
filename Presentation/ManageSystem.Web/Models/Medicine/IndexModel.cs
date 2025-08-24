using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Medicine
{
    /// <summary>
    /// 医学信息首页数据封装
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        /// 查询条件的数据封装
        /// </summary>
        public IndexSearchModel SearchModel { get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public Webdiyer.WebControls.Mvc.PagedList<MedicalData> PageList { get; set; }

        /// <summary>
        /// 按医院查询的列表数据
        /// </summary>
        public List<Hospital> SearchHospitalList { get; set; }

        /// <summary>
        /// 按地区（省份）查询的列表数据
        /// </summary>
        public List<Area> SearchAreaList { get; set; }


        /// <summary>
        /// 按细菌类型查询的列表数据
        /// </summary>
        public List<BacteriaType> SearchBacteriaTypeList { get; set; }

        /// <summary>
        /// 按标本查询的列表数据
        /// </summary>
        public List<Specimen> SearchSpecimenList { get; set; }

        /// <summary>
        /// 按医院科室查询的列表数据
        /// </summary>
        public List<HospitalDepartment> SearchDepartmentList { get; set; }
        
    }

    /// <summary>
    ///  医学信息合作首页查询条件
    /// </summary>
    public class IndexSearchModel
    {
        /// <summary>
        /// 所在区域
        /// </summary>
        public long Area { get; set; }

        /// <summary>
        ///  医院id
        /// </summary>
        public long Hospital { get; set; }

        /// <summary>
        ///  细菌类
        /// </summary>
        public long Bacteria { get; set; }

        /// <summary>
        /// 标本
        /// </summary>
        public long Specimen { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public long Department { get; set; }

        /// <summary>
        /// 分页，页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页，每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public int Order { get; set; }


    }
}