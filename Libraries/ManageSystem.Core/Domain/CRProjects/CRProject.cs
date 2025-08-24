using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Core.Domain.CRProjects
{
    /// <summary>
    ///CR项目信息  CR项目信息-CR项目管理  实体类 ，数据库表名：CRProject 
    /// </summary>
    public partial class CRProject : BaseEntity
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

        /// <summary>
        /// 上年CRE平均检出率
        /// <summary>
        public String CREDetectionRate { get; set; }

        /// <summary>
        /// 上年CRAB平均检出率
        /// <summary>
        public String CREDrugRate { get; set; }

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
        /// 邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int EmailStatus { get; set; }

        /// <summary>
        /// 发送时间
        /// <summary>
        public DateTime EmailTime { get; set; }

        /// <summary>
        /// 确认时间
        /// <summary>
        public DateTime HospitalTime { get; set; }

        /// <summary>
        /// 邮件确认加密
        /// </summary>
        public string EmailStr { set; get; }

    }



    #region 前台上传数据信息以后的返回结果数据封装  不是数据库

    /// <summary>
    /// 前台上传数据信息以后的返回结果数据封装
    /// </summary>
    public class UploadProjectResult
    {
        /// <summary>
        /// 上传数据对象
        /// </summary>
        public CRProject MedicalEntity { get; set; }

        /// <summary>
        /// 基础信息
        /// </summary>
        public UploadMedicalResultBase BaseMessage { get; set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public List<CRProjectItemValidate> ValidateList { get; set; }

        /// <summary>
        /// 上传的Excel文件绝对路径，用于读取数据
        /// </summary>
        public string ExcelFilePath { get; set; }

        /// <summary>
        /// 读取Excel中Item数据集合
        /// </summary>
        public List<CRProjectItem> ExcelItemList { get; set; }

        /// <summary>
        /// 需要保存到数据库的中Item数据集合
        /// </summary>
        public List<CRProjectItem> ItemList { get; set; }

        /// <summary>
        /// 医学数据 细菌（基础数据）
        /// </summary>
        public List<CRProjectItem> OrganismList { get; set; }

        /// <summary>
        /// 医学数据 抗生素（基础数据）
        /// </summary>
        public List<CRProjectItem> AntibioticList { get; set; }


    }

    /// <summary>
    /// 前台上传数据的基础信息
    /// </summary>
    public class UploadMedicalResultBase
    {
        /// <summary>
        /// 医学数据主表的Id
        /// </summary>
        public long MedicalDataId { get; set; }

        /// <summary>
        ///用户上传的文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小 单位：KB
        /// </summary>
        public string FileLength { get; set; }

        /// <summary>
        /// 数据所属时段
        /// </summary>
        public string DataTime { get; set; }

        /// <summary>
        /// 数据上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 细菌类型
        /// </summary>
        public string BacteriaTypeName { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        public string SpecimenName { get; set; }

        /// <summary>
        /// 本次读取的总行数
        /// </summary>
        public int ReadCount { get; set; }

        /// <summary>
        /// 共成功上传的总行数
        /// </summary>
        public int SuccessCount { get; set; }

    }

    /// <summary>
    /// 前台上传数据的错误信息
    /// </summary>
    public class UploadMedicalResultError
    {
        /// <summary>
        /// 错误信息的名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 前台上传数据的验证信息
    /// </summary>
    public class UploadMedicalResultValidate
    {
        /// <summary>
        /// 字段名称，excel的字段
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 验证信息的名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 存储上传的基本数据
    /// </summary>
    public class UploadMessageModel
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public UploadMedicalResultBase BaseMessage { get; set; }

    }


    #endregion
}
