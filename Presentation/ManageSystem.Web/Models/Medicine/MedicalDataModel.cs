using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Medicine;
using ManageSystem.Core.Domain.Medicine;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Medicine
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
        public long HospitalDepartmentId { get; set; }
        /// <summary>
		/// 医院科室名称
		/// <summary>
		public String HospitalDepartmentName { get; set; }
        /// <summary>
		/// 所属医院
		/// <summary>
		public long HospitalId { get; set; }
        /// <summary>
		/// 医院名称
		/// <summary>
		public String HospitalName { get; set; }
        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        public String UploadFilePath { get; set; }
        /// <summary>
        /// 上报数据所属年度
        /// <summary>
        public int Year { get; set; }
        /// <summary>
        /// 上报数据所属季度
        /// <summary>
        public int Quarter { get; set; }
        /// <summary>
        /// 标本类型Id
        /// <summary>
        public long SpecimenId { get; set; }
        /// <summary>
		/// 标本类型名称
		/// <summary>
		public String SpecimenName { get; set; }

        /// <summary>
        /// 系统编号
        /// <summary>
        [HtmlDisplayAttribute("系统编号", "系统编号")]
        public String SN { get; set; }

        /// <summary>
        /// 上传信息
        /// </summary>
        public string UploadMessage { get; set; }

        /// <summary>
        /// 所属地区，省份
        /// <summary>
        public long AreaId { get; set; }

        /// <summary>
        /// 所属地区名称
        /// <summary>
        public String AreName { get; set; }

        /// <summary>
        /// 上传文件的原始文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 二维码图片地址
        /// </summary>
        public string CodeFilePath { get; set; }

        /// <summary>
        /// 上传数据的信息
        /// </summary>
        public UploadMessageModel UploadMessageEntity { get; set; }

        /// <summary>
        /// 对应的详细数据
        /// </summary>
        public IList<MedicalDataItem> ItemList { get; set; }

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

        /// <summary>
        /// 处理过的Excel文件路径，相对路径
        /// </summary>
        public string DisposeFilePath { get; set; }

        /// <summary>
        /// 处理完成后生成的word文件地址
        /// </summary>
        public string WordFile { get; set; }

        /// <summary>
        /// 数据上传的项目类型，固定值：CHINET中国细菌耐药检测网、上海市细菌真菌耐药监测网、其他监测数据
        /// </summary>
        public long ProjectType { get; set; }

        /// <summary>
        ///  数据上传的项目类型，固定值：CHINET中国细菌耐药检测网、上海市细菌真菌耐药监测网、其他监测数据
        /// </summary>
        public string ProjectTypeName { get; set; }
    }

    /// <summary>
    /// 上传数据页面的数据封装
    /// </summary>
    [Validator(typeof(UploadMedicalDataValidator))]
    public partial class UploadMedicalDataModel : BaseEntityModel
    {
        //2025.02.14 新增：报告数量、报告形式Number of report
        private string _ReportNumber = string.Empty;
        private string _ReportType = string.Empty;
        private string _ReportRemark = string.Empty;

        /// <summary>
        /// 报告数量
        /// </summary>
        public string ReportNumber { get { return _ReportNumber; } set { _ReportNumber = value; } }
        /// <summary>
        /// 报告形式：纸质报告、电子报告、报告中加备注、口头报告、 其他_______
        /// </summary>
        public string ReportType { get { return _ReportType; } set { _ReportType = value; } }
        /// <summary>
        /// 其他报告形式
        /// </summary>
        public string ReportRemark { get { return _ReportRemark; } set { _ReportRemark = value; } }

        /// <summary>
        /// 细菌类型集合
        /// </summary>
        public IList<BacteriaType> BacteriaTypeList { get; set; }

        /// <summary>
        ///标本类型集合
        /// </summary>
        public IList<SelectListItem> SpecimenList { get; set; }

        /// <summary>
        /// 标本类型
        /// </summary>
        [HtmlDisplayAttribute("标本类型", "标本类型")]
        public long SpecimenId { get; set; }

        /// <summary>
        /// 细菌鉴定系统名称
        /// </summary>
        [HtmlDisplayAttribute("细菌鉴定系统名称", "细菌鉴定系统名称")]
        public string BacteriaIds { get; set; }

        /// <summary>
        ///上报数据所属年度集合
        /// </summary>
        public IList<SelectListItem> YearList { get; set; }

        /// <summary>
        /// 上报数据所属年度
        /// </summary>
        [HtmlDisplayAttribute("上报数据所属年度", "上报数据所属年度")]
        public int Year { get; set; }

        /// <summary>
        ///上报数据所属季度集合
        /// </summary>
        public IList<SelectListItem> QuarterList { get; set; }

        /// <summary>
        /// 用于接受上传结果的邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 上报数据所属季度
        /// </summary>
        [HtmlDisplayAttribute("上报数据所属季度", "上报数据所属季度")]
        public int Quarter { get; set; }

        /// <summary>
        /// 上传的Excel路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 上传文件的原始文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上传文件的大小，KB
        /// </summary>
        public double FileSize { get; set; }

        /// <summary>
        /// 数据上传的项目类型
        /// </summary>
        public IList<SelectListItem> ProjectTypeList { get; set; }

        /// <summary>
        /// 处理完成后生成的word文件地址
        /// </summary>
        public string WordFile { get; set; }

        /// <summary>
        /// 数据上传的项目类型，固定值：CHINET中国细菌耐药检测网、上海市细菌真菌耐药监测网、其他监测数据
        /// </summary>
        public long ProjectType { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        public long HospitalId { get; set; }

        public List<string> FilePathList { get; set; }

        /// <summary>
        /// 是否为卫星网上传
        /// </summary>
        public int IsSatellite { get; set; }

    }

    #region  医学数据管理

    /// <summary>
    /// 会员中心，医学数据管理的数据封装
    /// </summary>
    [Serializable]
    public class CenterMedicineManageModel
    {
        /// <summary>
        /// 搜索的 年 
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 状态下拉
        /// </summary>
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 年份下拉列表
        /// </summary>
        public IList<SelectListItem> YearList { get; set; }

        /// <summary>
        /// 搜索的 季度
        /// </summary>
        public int Quarter { get; set; }

        /// <summary>
        /// 季度下拉列表
        /// </summary>
        public IList<SelectListItem> QuarterList { get; set; }

        /// <summary>
        ///搜索的文件名称
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<CenterMedicineManageItemModel> PageList { get; set; }

    }

    /// <summary>
    /// 会员中心，医学数据管理的列表数据封装 （分页数据）
    /// </summary>
    public partial class CenterMedicineManageItemModel
    {
        /// <summary>
        /// 医学数据的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public int Quarter { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 上传信息
        /// </summary>
        public string UploadMessage { get; set; }

    }


    /// <summary>
    /// 实体类 ，数据库表名：MedicalDataProject 
    /// </summary>
    [Serializable]
    public partial class MedicalDataProjectModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        public int Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }

    /// <summary>
    /// 会员中心，项目医学数据管理的数据封装
    /// </summary>
    [Serializable]
    public class ProjectDataManageManageModel
    {
        /// <summary>
        /// 搜索的 年 
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 年份下拉列表
        /// </summary>
        public IList<SelectListItem> YearList { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 状态下拉
        /// </summary>
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 所属项目
        /// <summary>
        public long Project { get; set; }

        /// <summary>
        /// 项目下拉列表
        /// </summary>
        public IList<SelectListItem> ProjectList { get; set; }

        /// <summary>
        ///搜索的医院名称
        /// </summary>
        public String Hospital { get; set; }

        /// <summary>
        ///搜索的文件名称
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<ProjectDataManageManageItemModel> PageList { get; set; }
    }

    /// <summary>
    /// 会员中心，项目医学数据管理的数据封装 （分页数据）
    /// </summary>
    public partial class ProjectDataManageManageItemModel
    {
        /// <summary>
        /// 医学数据的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///医院名称
        /// </summary>
        public String Hospital { get; set; }

        /// <summary>
        ///所属项目名称ID
        /// </summary>
        public long Project { get; set; }

        /// <summary>
        ///所属项目名称
        /// </summary>
        public String ProjectName { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 上传信息
        /// </summary>
        public string UploadMessage { get; set; }

        /// <summary>
        /// 搜索的 年 
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public int Quarter { get; set; }

    }

    #endregion
    #region  科室数据管理
    /// <summary>
    /// 科室管理的数据封装
    /// </summary>
    [Serializable]
    public class HospitalWardLocationModel
    {       

        public PagedList<HospitalWardLocationItemModel> PageList { get; set; }
    }
    /// <summary>
    /// 科室管理数据列表页的分页
    /// </summary>
    public partial class HospitalWardLocationItemModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 医院名
        /// </summary>
        public string Name { get; set; }
        public string Ward { get; set; }
        public string Department_CN { get; set; }
        public string Department_EN { get; set; }
        public string Location { get; set; }
        public string Location_Type { get; set; }
        public int Sort { get; set; }

        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        public long HospitalId { get; set; }
    }
    #endregion


}

