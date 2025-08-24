using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Medicine;
using System.Web.Mvc;
using ManageSystem.Core.Domain.Medicine;

namespace ManageSystem.Admin.Models.Medicine
{
    /// <summary>
    /// 模型类 ，数据库表名：MedicalData 
    /// </summary>
    [Validator(typeof(MedicalDataValidator))]
    public partial class MedicalDataModel : BaseEntityModel
    {

        /// <summary>
        /// 医院科室Id
        /// <summary>
        [HtmlDisplayAttribute("医院科室Id", "医院科室Id")]
        public long HospitalDepartmentId { get; set; }

        /// <summary>
        /// 医院科室名称
        /// <summary>
        [HtmlDisplayAttribute("医院科室名称", "医院科室名称")]
        public String HospitalDepartmentName { get; set; }

        /// <summary>
        /// 所属医院
        /// <summary>
        [HtmlDisplayAttribute("所属医院", "所属医院")]
        public long HospitalId { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        [HtmlDisplayAttribute("医院名称", "医院名称")]
        public String HospitalName { get; set; }

        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        [HtmlDisplayAttribute("上传数据的文件路径", "上传数据的文件路径")]
        public String UploadFilePath { get; set; }

        /// <summary>
        /// 上报数据所属年度
        /// <summary>
        [HtmlDisplayAttribute("年度", "上报数据所属年度")]
        public Int32 Year { get; set; }

        /// <summary>
        /// 上报数据所属季度
        /// <summary>
        [HtmlDisplayAttribute("季度", "上报数据所属季度")]
        public Int32 Quarter { get; set; }

        /// <summary>
        /// 数据所属项目
        /// </summary>
        [HtmlDisplayAttribute("所属项目", "上报数据所属项目")]
        public long ProjectType { get; set; }

        /// <summary>
        /// 标本类型Id
        /// <summary>
        [HtmlDisplayAttribute("标本类型Id", "标本类型Id")]
        public long SpecimenId { get; set; }

        /// <summary>
        /// 标本类型名称
        /// <summary>
        [HtmlDisplayAttribute("标本类型名称", "标本类型名称")]
        public String SpecimenName { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        [HtmlDisplayAttribute("状态，1：有效    2：无效", "状态，1：有效    2：无效")]
        public Int32 Status { get; set; }

        /// <summary>
        /// 系统编号
        /// <summary>
        [HtmlDisplayAttribute("编号", "系统编号")]
        public String SN { get; set; }

        /// <summary>
        /// 上传信息
        /// </summary>
        public string UploadMessage { get; set; }

        /// <summary>
        /// 上传文件的原始文件名称
        /// </summary>
        [HtmlDisplayAttribute("上传文件的原始文件名称", "上传文件的原始文件名称")]
        public string FileName { get; set; }

        /// <summary>
        /// 所属地区，省份
        /// <summary>
        [HtmlDisplayAttribute("所属区域", "所属区域")]
        public long AreaId { get; set; }

        /// <summary>
        /// 所属地区名称
        /// <summary>
        public String AreName { get; set; }

        /// <summary>
        /// 二维码图片地址
        /// </summary>
        public string CodeFilePath { get; set; }

        /// <summary>
        /// 区域下拉列表，省份
        /// </summary>
        public IList<SelectListItem> AreaList { get; set; }

        /// <summary>
        /// 医院下拉列表
        /// </summary>
        public IList<SelectListItem> HospitalList { get; set; }

        /// <summary>
        /// 年份下拉列表
        /// </summary>
        public IList<SelectListItem> YearList { get; set; }

        /// <summary>
        /// 季度下拉列表
        /// </summary>
        public IList<SelectListItem> QuarterList { get; set; }

        /// <summary>
        /// 上传数据的信息
        /// </summary>
        public UploadMessageModel UploadMessageEntity { get; set; }

        /// <summary>
        /// 对应的详细数据
        /// </summary>
        public IList<MedicalDataItem> ItemList { get; set; }

        /// <summary>
        /// 数据所属项目对应下拉选项
        /// </summary>
        public IList<SelectListItem> ProjectTypeItems { get; set; }

        /// <summary>
        /// 对应的验证信息
        /// </summary>
        public IList<MedicalDataItemValidate> ValidateList { get; set; }

        /// <summary>
        /// 验证中发生的错误数量
        /// </summary>
        public int ValidateErrorCount { get; set; }

        /// <summary>
        /// 验证中发生的警告数量
        /// </summary>
        public int ValidateWarningCount { get; set; }

    }
}
