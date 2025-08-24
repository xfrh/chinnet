using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Members;
using System.Web.Mvc;
using ManageSystem.Core.Domain.Medicine;
using Senparc.Weixin.Annotations;

namespace ManageSystem.Admin.Models.Members
{
    /// <summary>
    /// 模型类 ，数据库表名：Member 
    /// </summary>
    [Validator(typeof(MemberValidator))]
    public partial class MemberModel : BaseEntityModel
    {
        /// <summary>
        /// 【不用了】项目类型:0为CHINET数据云，1为新项目
        /// </summary>
        [HtmlDisplayAttribute("多中心研究","如为多中心研究请勾选",false)]
        public bool _ProjectType { get; set; }

        [HtmlDisplayAttribute("项目类型", "指用户所属项目类型，如CHINET、SUGAR、XACDURO")]
        public int ProjectType { get; set; }

        /// <summary>
        /// 登录帐号
        /// <summary>
        [HtmlDisplayAttribute("登录帐号", "登录帐号", true)]
        public String LoginId { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码", true)]
        public String Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// <summary>
        [HtmlDisplayAttribute("真实姓名", "真实姓名", true)]
        public String Name { get; set; }

        public String ProjectUploadItem { get; set; }

        /// <summary>
        /// 微信OpenId
        /// <summary>
        [HtmlDisplayAttribute("微信OpenId", "微信用户的OpenId，来源于微信")]
        public String OpenId { get; set; }

        /// <summary>
        /// 用户昵称
        /// <summary>
        [HtmlDisplayAttribute("用户昵称", "用户昵称")]
        public String NickName { get; set; }
        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码", true)]
        public String Phone { get; set; }

        /// <summary>
        /// 个人邮箱
        /// <summary>
        [HtmlDisplayAttribute("个人邮箱", "个人邮箱")]
        public String Email { get; set; }

        /// <summary>
        /// 通知邮箱
        /// </summary>
        [HtmlDisplay("上传数据通知邮箱", "上传数据通知邮箱")]
        public string MedicineEmail { get; set; }

        /// <summary>
        /// 出生日期
        /// <summary>
        [HtmlDisplayAttribute("出生日期", "出生日期")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 帐号状态
        /// <summary>
        [HtmlDisplayAttribute("帐号状态", "帐号状态")]
        public Int32 Status { get; set; }

        /// <summary>
        /// 帐号状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("帐号状态", "帐号状态")]
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 用户性别
        /// <summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public Int32 Sex { get; set; }

        /// <summary>
        ///用户性别枚举列表
        /// </summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public IList<SelectListItem> SexList { get; set; }

        /// <summary>
        /// 帐号类型
        /// <summary>
        [HtmlDisplayAttribute("帐号类型", "帐号类型")]
        public Int32 Type { get; set; }

        /// <summary>
        /// 帐号类型枚举列表
        /// </summary>
        [HtmlDisplayAttribute("帐号类型", "帐号类型")]
        public IList<SelectListItem> TypeList { get; set; }

        /// <summary>
        /// 项目类型枚举列表
        /// </summary>
        [HtmlDisplayAttribute("项目类型", "项目类型")]
        public IList<SelectListItem> ProjectTypeList { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [HtmlDisplayAttribute("用户头像", "用户头像")]
        public string HeadImage { get; set; }

        /// <summary>
        /// 所属城市，关联区域表（Area）
        /// </summary>
        [HtmlDisplayAttribute("所属城市", "所属城市")]
        public long AreaId { get; set; }
        /// <summary>
        /// 所属城市列表
        /// </summary>
        [HtmlDisplayAttribute("所属城市", "所属城市")]
        public IList<SelectListItem> AreaList { get; set; }



        /// <summary>
        /// 省ID
        /// <summary>
        [HtmlDisplayAttribute("省ID", "省ID", false)]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        [HtmlDisplayAttribute("市ID", "市ID", false)]
        public long CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        [HtmlDisplayAttribute("区ID", "区ID", false)]
        public long DistrictsId { get; set; }

        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        [HtmlDisplayAttribute("省市区", "省市区", false)]
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        [HtmlDisplayAttribute("详细地址", "详细地址")]
        public String Address { get; set; }


        /// <summary>
        /// 所属医院
        /// </summary>
        [HtmlDisplayAttribute("所属医院", "所属医院")]
        public long HospitalId { get; set; }

        /// <summary>
        /// 如果医院下拉列表选择了其他，则填写其他医院的名称
        /// </summary>
        public string OtherHospital { get; set; }

        /// <summary>
        /// 所属医院列表
        /// </summary>
        [HtmlDisplayAttribute("所属医院", "所属医院")]
        public IList<SelectListItem> HospitalList { get; set; }

        /// <summary>
        /// 所属科室，关联医院科室表（HospitalDepartment）
        /// </summary>
        public long HospitalDepartmentId { get; set; }

        /// <summary>
        /// 所属科室列表
        /// </summary>
        [HtmlDisplayAttribute("所属科室", "所属科室")]
        public IList<SelectListItem> HospitalDepartmentList { get; set; }

        /// <summary>
        /// 医生职称，关联医生职称表（DoctorTitle）
        /// </summary>
        [HtmlDisplayAttribute("医生职称", "关联医生职称")]
        public long DoctorTitleId { get; set; }

        /// <summary>
        /// 医生职称列表
        /// </summary>
        [HtmlDisplayAttribute("医生职称", "医生职称")]
        public IList<SelectListItem> DoctorTitleList { get; set; }

        /// <summary>
        /// 输入的邀请码，注册的时候输入的
        /// </summary>
        [HtmlDisplayAttribute("输入的邀请码", "注册的时候输入的邀请码")]
        public string InputInviteCode { get; set; }

        /// <summary>
        /// 用户的邀请码，系统给每位用户自动产生的
        /// </summary>
        [HtmlDisplayAttribute("用户的邀请码", "系统给每位用户自动产生的")]
        public string InviteCode { get; set; }

        /// <summary>
        /// 用户积分总额，同步积分流水记录
        /// </summary>
        [HtmlDisplayAttribute("用户积分总额", "用户积分总额")]
        public decimal IntegralAmount { get; set; }

        /// <summary>
        /// 用户积分总额，同步积分流水记录
        /// </summary>
        [HtmlDisplayAttribute("测试账号", "测试账号")]
        public bool IsTest { get; set; }

        [HtmlDisplayAttribute("在线上报权限", "在线上报权限")]
        public bool _OnlineUpload { get; set; }

        public int OnlineUpload { get; set; }
        /// <summary>
        /// 已选的项目
        /// </summary>
        public List<long> SelectProject { get; set; } = new List<long>();

        /// <summary>
        /// 项目
        /// </summary>
        [HtmlDisplayAttribute("项目管理", "项目管理")]
        public IEnumerable<SelectListItem> ProjectList
        {
            get;
            set;
        }

        /// <summary>
        /// 用户积分总额，同步积分流水记录
        /// </summary>
        [HtmlDisplayAttribute("是否允许科室配置", "是否允许科室配置")]
        public bool Department { get; set; }

        /// <summary>
        /// 已选的项目
        /// </summary>
        public List<long> SelectProjectUpload { get; set; } = new List<long>();

        /// <summary>
        /// 项目
        /// </summary>
        [HtmlDisplayAttribute("数据上传所属项目", "数据上传所属项目")]
        public IEnumerable<SelectListItem> ProjectUploadList { get; set; }

        /// <summary>
        /// 已选中默认上传数据所属项目
        /// </summary>
        public long DefaultProject { get; set; }
        /// <summary>
        /// 默认选中上传数据所属项目
        /// </summary>
        [HtmlDisplay("默认选中上传所属项目", "默认选中上传所属项目")]
        public IEnumerable<SelectListItem> DefaultProjectList { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// 已选专项管理员
        /// </summary>
        public List<string> SelectSpecialManager { get; set; } = new List<string>();

        [HtmlDisplayAttribute("专项管理员", "专项管理员")]
        public IEnumerable<SelectListItem> DropSpecialManager
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "CR替加环素", Value = "cr", Selected = SelectSpecialManager.Contains("cr") },
                    new SelectListItem { Text = "CRE在线填写", Value = "2",Selected = SelectSpecialManager.Contains("2")},//杜建中新增
                    new SelectListItem { Text = "CRE表单上传", Value = "3",Selected = SelectSpecialManager.Contains("3")}//杜建中新增
                };
            }
        }


        /// <summary>
        /// 已选的选项
        /// </summary>
        public List<string> SelectMedian { get; set; } = new List<string>();
        [HtmlDisplayAttribute("折点项目", "折点项目")]
        public IEnumerable<SelectListItem> Median
        {
            get
            {
                return new List<SelectListItem>
                {                   
                    new SelectListItem { Text = "管理员", Value = "1",Selected = SelectMedian.Contains("1")},//杜建中新增
                    new SelectListItem { Text = "参与者", Value = "2",Selected = SelectMedian.Contains("2")}//杜建中新增
                };
            }
        }
    }


}
