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
using ManageSystem.Web.Validators.Project;

namespace ManageSystem.Web.Models.Project
{
    /// <summary>
    /// 模型类 ，数据库表名：MedicalData 
    /// </summary>
    [Validator(typeof(ProjectValidator))]
    public partial class CRDataModel
    {

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 会员Id
        /// <summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 上传会员的姓名
        /// <summary>
        public String MemberName { get; set; }

        /// <summary>
        /// 医院Id
        /// <summary>
        public long HospitalId { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        public String HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// <summary>
        public String HospitalGrade { get; set; }

        /// <summary>
        /// 微生物负责人
        /// <summary>
        public String Name { get; set; }

        /// <summary>
        /// 联系电话
        /// <summary>
        public String Phone { get; set; }

        /// <summary>
        /// 邮件地址
        /// <summary>
        public String Email { get; set; }

        /// <summary>
        /// 省ID
        /// <summary>
        public long ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        public long CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        public long DistrictsId { get; set; }

        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        public String Province { get; set; }
        public String City { get; set; }
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        public String Address { get; set; }

        /// <summary>
        /// 上年CRE平均检出率
        /// <summary>
        public decimal CREDetectionRate { get; set; }

        /// <summary>
        /// 上年CRAB平均检出率
        /// <summary>
        public decimal CREDrugRate { get; set; }

        /// <summary>
        /// MH平板来源厂家
        /// </summary>
        public string MHSource { get; set; }

        /// <summary>
        /// 上传文件名称
        /// <summary>
        public String FileName { get; set; }

        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        public String UploadFilePath { get; set; }

        /// <summary>
        /// 处理过的Excel文件路径，相对路径
        /// </summary>
        public string DisposeFilePath { get; set; }

        /// <summary>
        /// 邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int EmailStatus { get; set; }

        /// <summary>
        /// 发送时间
        /// <summary>
        public DateTime EmailTime { get; set; }


        /// <summary>
        /// 医院邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int HospitalStatus { get; set; }
        /// <summary>
        /// 确认时间
        /// <summary>
        public DateTime HospitalTime { get; set; }

        public string Year { set; get; }
        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { set; get; }
    }

  

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CenterCRModel
    {
        /// <summary>
        /// 会员Id
        /// <summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 上传会员的姓名
        /// <summary>
        public String MemberName { get; set; }

        /// <summary>
        /// 医院Id
        /// <summary>
        public long HospitalId { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        public String HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// <summary>
        public String HospitalGrade { get; set; }

        /// <summary>
        /// 微生物负责人
        /// <summary>
        public String Name { get; set; }

        /// <summary>
        /// 联系电话
        /// <summary>
        public String Phone { get; set; }

        /// <summary>
        /// 邮件地址
        /// <summary>
        public String Email { get; set; }

        /// <summary>
        /// 省ID
        /// <summary>
        public Int32 ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        public Int32 CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        public Int32 DistrictsId { get; set; }

        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        public String Address { get; set; }

        /// <summary>
        /// 上年CRE平均检出率
        /// <summary>
        public decimal CREDetectionRate { get; set; }

        /// <summary>
        /// 上年CRAB平均检出率
        /// <summary>
        public decimal CREDrugRate { get; set; }

        /// <summary>
        /// 上传文件名称
        /// <summary>
        public String FileName { get; set; }

        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        public String UploadFilePath { get; set; }

        /// <summary>
        /// 处理过的Excel文件路径，相对路径
        /// </summary>
        public string DisposeFilePath { get; set; }

        /// <summary>
        /// 邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int EmailStatus { get; set; }

        /// <summary>
        /// 发送时间
        /// <summary>
        public DateTime EmailTime { get; set; }


        /// <summary>
        /// 医院邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int HospitalStatus { get; set; }
        /// <summary>
        /// 确认时间
        /// <summary>
        public DateTime HospitalTime { get; set; }

        public string Year { set; get; }
        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { set; get; }

        /// <summary>
        /// 状态下拉
        /// </summary>
        public IList<SelectListItem> EmailStatusList { get; set; }
         

        /// <summary>
        /// 季度下拉列表
        /// </summary>
        public IList<SelectListItem> HospitalStatusList { get; set; }


        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<CenterProjectItemModel> PageList { get; set; }
    }

    /// <summary>
    /// 会员中心，医学数据管理的列表数据封装 （分页数据）
    /// </summary>
    public partial class CenterCRItemModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 会员Id
        /// <summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 上传会员的姓名
        /// <summary>
        public String MemberName { get; set; }

        /// <summary>
        /// 医院Id
        /// <summary>
        public long HospitalId { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        public String HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// <summary>
        public String HospitalGrade { get; set; }

        /// <summary>
        /// 微生物负责人
        /// <summary>
        public String Name { get; set; }

        /// <summary>
        /// 联系电话
        /// <summary>
        public String Phone { get; set; }

        /// <summary>
        /// 邮件地址
        /// <summary>
        public String Email { get; set; }

        /// <summary>
        /// 省ID
        /// <summary>
        public Int32 ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        public Int32 CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        public Int32 DistrictsId { get; set; }

        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        public String Province { get; set; }
        public String City { get; set; }
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        public String Address { get; set; }

        /// <summary>
        /// 上年CRE平均检出率
        /// <summary>
        public decimal CREDetectionRate { get; set; }

        /// <summary>
        /// 上年CRAB平均检出率
        /// <summary>
        public decimal CREDrugRate { get; set; }

        /// <summary>
        /// 上传文件名称
        /// <summary>
        public String FileName { get; set; }

        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        public String UploadFilePath { get; set; }

        /// <summary>
        /// 处理过的Excel文件路径，相对路径
        /// </summary>
        public string DisposeFilePath { get; set; }

        /// <summary>
        /// 邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int EmailStatus { get; set; }

        /// <summary>
        /// 发送时间
        /// <summary>
        public DateTime EmailTime { get; set; }


        /// <summary>
        /// 医院邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int HospitalStatus { get; set; }
        

        /// <summary>
        /// 确认时间
        /// <summary>
        public DateTime HospitalTime { get; set; }

        /// <summary>
        /// 上传信息
        /// </summary>
        public string UploadMessage { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        public string Year { set; get; }
        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { set; get; }
    }


    public class CRMenber
    {
        public long MemberId { set; get; }
        public string Name { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public long HospitalId { set; get; }
        public string HospitalName { set; get; }
        public long OrderAddressId { set; get; }
        /// <summary>
        /// 省ID
        /// <summary>
        public long ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        public long CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        public long DistrictsId { get; set; }

        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        public String Address { get; set; }
    }
}

