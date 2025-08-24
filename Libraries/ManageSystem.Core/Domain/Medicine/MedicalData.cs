using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
    /// 实体类 ，数据库表名：MedicalData 
    /// </summary>
    public partial class MedicalData : BaseEntity
    {
        /// <summary>
        /// 编号
        /// <summary>
        public string SN { get; set; }

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
        public Int32 Year { get; set; }
        /// <summary>
        /// 上报数据所属季度
        /// <summary>
        public Int32 Quarter { get; set; }
        /// <summary>
        /// 标本类型Id
        /// <summary>
        public long SpecimenId { get; set; }
        /// <summary>
        /// 标本类型名称
        /// <summary>
        public String SpecimenName { get; set; }
        /// <summary>
        /// 状态，1：有效    2：无效
        /// <summary>
        public Int32 Status { get; set; }
        /// <summary>
        /// 细菌鉴定系统Id集合
        /// <summary>
        public String BacteriaTypeIds { get; set; }
        /// <summary>
        /// 细菌鉴定的名称
        /// <summary>
        public String BacteriaTypeName { get; set; }

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
        /// 上传会员的id
        /// <summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 上传会员的姓名
        /// <summary>
        public String MemberName { get; set; }

        /// <summary>
        /// 上传文件的原始文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 二维码图片地址
        /// </summary>
        public string CodeFilePath { get; set; }

        /// <summary>
        /// 上传数据耐药生成结果状态，1：待处理    2：处理中   3：成功  4：失败
        /// </summary>
        public int AntibioticResultStatue { get; set; }

        /// <summary>
        /// 上传数据耐药生成结果时间，也就是生成结果状态的处理时间
        /// </summary>
        public DateTime AntibioticResultTime { get; set; }

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
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 发生异常
        /// </summary>
        public string ExceptionMessage { get; set; } = null;

        /// <summary>
        /// 异常堆栈信息
        /// </summary>
        public string ExceptionStackTrace { get; set; } = null;
    }


    #region 前台上传数据信息以后的返回结果数据封装  不是数据库

    /// <summary>
    /// 前台上传数据信息以后的返回结果数据封装
    /// </summary>
    public class UploadMedicalResult
    {
        /// <summary>
        /// 上传数据对象
        /// </summary>
        public MedicalData MedicalEntity { get; set; }

        /// <summary>
        /// 基础信息
        /// </summary>
        public UploadMedicalResultBase BaseMessage { get; set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public List<MedicalDataItemValidate> ValidateList { get; set; }

        /// <summary>
        /// 上传的Excel文件绝对路径，用于读取数据
        /// </summary>
        public string ExcelFilePath { get; set; }

        /// <summary>
        /// 读取Excel中Item数据集合
        /// </summary>
        public List<MedicalDataItem> ExcelItemList { get; set; }

        /// <summary>
        /// 需要保存到数据库的中Item数据集合
        /// </summary>
        public List<MedicalDataItem> ItemList { get; set; }

        /// <summary>
        /// 医学数据 细菌（基础数据）
        /// </summary>
        public List<MedicalOrganism> OrganismList { get; set; }

        /// <summary>
        /// 医学数据 抗生素（基础数据）
        /// </summary>
        public List<MedicalAntibiotic> AntibioticList { get; set; }

        /// <summary>
        /// 医学数据 抗生素规则（基础数据）
        /// </summary>
        public List<MedicalAntibioticRule> AntibioticRuleList { get; set; }

        /// <summary>
        /// 医学数据 科室类别 数据 （基础数据）
        /// </summary>
        public List<MedicalDataWardType> WardTypeList { get; set; }

        /// <summary>
        /// 医学数据 系统内置标准科室分类 数据 （基础数据）
        /// </summary>
        public List<MedicalDepartmentType> DepartmentTypeList { get; set; }

        /// <summary>
        /// 医学数据 标本类型（基础数据）
        /// </summary>
        public List<MedicalSpecType> SpecTypeList { get; set; }

        /// <summary>
        /// 医学数据 细菌类型（基础数据）
        /// </summary>
        public List<MedicalOrganismType> OrganismTypeList { get; set; }

        /// <summary>
        /// 对应医院的编码替换数据（基础数据）
        /// </summary>
        public List<HospitalWardLocation> HospitalWardList { get; set; }

        ///// <summary>
        ///// 所有医院的科室，用于验证
        ///// </summary>
        //public List<HospitalDepartment> HospitalDepartmentList { get; set; }

        /// <summary>
        /// 当前用户所属的医院（基础数据）
        /// </summary>
        public Hospital HospitalEntity { get; set; }

        /// <summary>
        /// 当前登录用户（基础数据）
        /// </summary>
        public Member MemberEntity { get; set; }

        /// <summary>
        /// 上传数据成功以后的操作日志记录
        /// </summary>
        public string UploadResultLog { get; set; }

        /// <summary>
        /// 上传的原始文件的完整url地址，用于邮件通知的下载
        /// </summary>
        public string UploadExcelFileUrl { get; set; }

        /// <summary>
        /// 数据处理过后的文件的完整url地址，用于邮件通知的下载
        /// </summary>
        public string DisposeExcelFileUrl { get; set; }

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
