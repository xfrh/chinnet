using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Members
{
    /// <summary>
    /// 实体类 ，数据库表名：Member
    /// </summary>
    public partial class Member : Account
    {
        private Int32 _ProjectType = 0;

        /// <summary>
        /// 项目类型:0/CHINET，1/SUGAR多中心，2/CRAB多中心，3/ERA多中心，4/CRE多中心
        /// </summary>
        public Int32 ProjectType { get { return _ProjectType; } set{ _ProjectType = value; } }

        /// <summary>
        /// 微信OpenId
        /// <summary>
        public String OpenId { get; set; }

        /// <summary>
        /// 用户昵称
        /// <summary>
        public String NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        public String Phone { get; set; }
        /// <summary>
        /// 邮箱地址
        /// <summary>
        public String Email { get; set; }

        /// <summary>
        /// 出生日期
        /// <summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 帐号状态
        /// <summary>
        public Int32 Status { get; set; }
        /// <summary>
        /// 用户性别
        /// <summary>
        public Int32 Sex { get; set; }

        /// <summary>
        /// 帐号类型：未认证会员、认证会员、医生、主任
        /// <summary>
        public Int32 Type { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 所属城市，关联区域表（Area）
        /// </summary>
        public long AreaId { get; set; }

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
        /// 所属医院，关联医院表（Hospital）
        /// </summary>
        public long HospitalId { get; set; }

        /// <summary>
        /// 如果医院下拉列表选择了其他，则填写其他医院的名称
        /// </summary>
        public string OtherHospital { get; set; }

        /// <summary>
        /// 所属科室，关联医院科室表（HospitalDepartment）
        /// </summary>
        public long HospitalDepartmentId { get; set; }

        /// <summary>
        /// 医生职称，关联医生职称表（DoctorTitle）
        /// </summary>
        public long DoctorTitleId { get; set; }

        /// <summary>
        /// 输入的邀请码，注册的时候输入的
        /// </summary>
        public string InputInviteCode { get; set; }

        /// <summary>
        /// 用户的邀请码，系统给每位用户自动产生的
        /// </summary>
        public string InviteCode { get; set; }

        /// <summary>
        /// 用户积分总额，同步积分流水记录，MemberIntegralLog
        /// </summary>
        public decimal IntegralAmount { get; set; }

        /// <summary>
        /// 医学数据上传成功以后接受提醒的邮箱
        /// </summary>
        public string MedicineEmail { get; set; }

        /// <summary>
        /// 管理的项目
        /// </summary>
        public string ProjectItem { get; set; } = "[]";

        /// <summary>
        /// 默认项目
        /// </summary>
        public long DefaultProject { get; set; }

        /// <summary>
        /// 是否测试账号
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// 是否已设置密码
        /// </summary>
        public bool IsSettingPassword { get; set; }

        /// <summary>
        /// 专项管理员<br />
        /// List<int>格式<br />
        /// 1: 表示“CRE专项管理员”
        /// </summary>
        public string SpecialManager { get; set; } = "[]";

        /// <summary>
        /// 是否验证邮箱
        /// </summary>
        public bool IsCheckEmail { get; set; } = false;

        /// <summary>
        /// 是否支持上传
        /// </summary>
        public string ProjectUploadItem { get; set; } = "[]";


        /*杜建中新增两个字段模型*/
        /// <summary>
        /// 在线填写
        /// </summary>
        public string Crefillonline { get; set; }

        /// <summary>
        /// 表单上传
        /// </summary>
        public string Creformupload { get; set; }

        /// <summary>
        /// 科室配置
        /// </summary>
        public bool Department { get; set; }

        /// <summary>
        /// 折点项目
        /// </summary>
        public string Median { get; set; }

        /// <summary>
        /// MIC权限
        /// </summary>
        public int MICjurisdiction { get; set; }

        /// <summary>
        /// 在线上报权限
        /// </summary>
        public int OnlineUpload { get; set; }
    }
}
