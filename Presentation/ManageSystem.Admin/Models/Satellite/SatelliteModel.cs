using FluentValidation.Attributes;
using ManageSystem.Admin.Validators;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Satellite
{
    [Validator(typeof(SatelliteValidator))]
    public class SatelliteModel : BaseEntityModel
    { /// <summary>
      /// 省
      /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string PlaceOfWork { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string Office { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 负责人手机号码
        /// </summary>
        public string ChargePhoneNumber { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ChargeName { get; set; }

        /// <summary>
        /// 负责人email
        /// </summary>
        public string ChargeEmail { get; set; }

        /// <summary>
        /// 卫星网名称
        /// </summary>
        public string SatelliteName { get; set; }

        /// <summary>
        /// 成员单位数量
        /// </summary>
        public string UnitCount { get; set; }

        /// <summary>
        /// 项目负责人姓名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 项目负责人电话
        /// </summary>
        public string ItemPhoneNumber { get; set; }

        /// <summary>
        /// 项目负责人email
        /// </summary>
        public string ItemEmail { get; set; }

        /// <summary>
        /// 卫星网域名
        /// </summary>
        public string RealmName { get; set; }
        //public long HospitalId { get; set; }

        //[HtmlDisplayAttribute("所属医院", "所属医院")]
        //public IList<SelectListItem> HospitalList { get; set; }
        ///// <summary>
        ///// 所属科室，关联医院科室表（HospitalDepartment）
        ///// </summary>
        //public long HospitalDepartmentId { get; set; }

        ///// <summary>
        ///// 所属科室列表
        ///// </summary>
        //[HtmlDisplayAttribute("所属科室", "所属科室")]
        //public IList<SelectListItem> HospitalDepartmentList { get; set; }

        ///// <summary>
        ///// 所属科室列表
        ///// </summary>
        //[HtmlDisplayAttribute("所属省份", "所属省份")]
        //public IList<SelectListItem> ProvinceList { get; set; }

        ///// <summary>
        ///// 所属科室列表
        ///// </summary>
        //[HtmlDisplayAttribute("所属市", "所属市")]
        //public IList<SelectListItem> CityList { get; set; }

        ///// <summary>
        ///// 科室名称
        ///// </summary>
        //public string HospitalDepartmentName { get; set; }


        ///// <summary>
        ///// 医生职称，关联医生职称表（DoctorTitle）
        ///// </summary>
        //[HtmlDisplayAttribute("医生职称", "关联医生职称")]
        //public long DoctorTitleId { get; set; }

        ///// <summary>
        ///// 医生职称列表
        ///// </summary>
        //[HtmlDisplayAttribute("医生职称", "医生职称")]
        //public IList<SelectListItem> DoctorTitleList { get; set; }
    }
}