using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Members;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using ManageSystem.Core.Domain.Medicine;

namespace ManageSystem.Web.Models.Members
{
    /// <summary>
    /// 模型类 ，数据库表名：Member 
    /// </summary>
    [Validator(typeof(MemberValidator))]
    public partial class MemberModel : BaseEntityModel
    {

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
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址")]
        public String Email { get; set; }

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
        public decimal IntegralAmount { get; set; }

        /// <summary>
        /// 折点项目权限
        /// </summary>
        public string Median { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        [HtmlDisplayAttribute("验证码", "验证码")]
        public string ValidateCode { get; set; }


        public PagedList<MemberItemModel> PageList { get; set; }

    }
    public partial class MemberItemModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 登录帐号
        /// <summary>
        [HtmlDisplayAttribute("登录帐号", "登录帐号")]
        public String LoginId { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码")]
        public String Password { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码")]
        public String NewPassword { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码")]
        public String NewPassword2 { get; set; }

        /// <summary>
        /// 真实姓名
        /// <summary>
        [HtmlDisplayAttribute("真实姓名", "真实姓名")]
        public String Name { get; set; }

        /// <summary>
        /// 用户昵称
        /// <summary>
        [HtmlDisplayAttribute("用户昵称", "用户昵称")]
        public String NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码")]
        public String Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址")]
        public String Email { get; set; }

        /// <summary>
        /// 用户性别
        /// <summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public Int32 Sex { get; set; }

        /// <summary>
        /// 用户积分总额，同步积分流水记录
        /// </summary>
        public decimal IntegralAmount { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        [HtmlDisplay("所属省份", "所属省份")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        public IList<SelectListItem> ProvinceList { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        [HtmlDisplay("所属医院名称", "所属医院名称")]
        public long HospitalId { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        public IList<SelectListItem> HospitalList { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        [HtmlDisplay("所属医院名称", "所属医院名称")]
        public string HospitalName { get; set; }


        public bool IsCheckEmail { get; set; }

        /// <summary>
        /// 折点项目权限
        /// </summary>
        public string Median { get; set; }


    }

    /// <summary>
    /// 个人中心首页的基本信息实体模型
    /// </summary>
    [Validator(typeof(CenterIndexMemberValidator))]
    public partial class CenterIndexMemberModel : BaseEntityModel
    {
        /// <summary>
        /// 登录帐号
        /// <summary>
        [HtmlDisplayAttribute("登录帐号", "登录帐号")]
        public String LoginId { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码")]
        public String Password { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码")]
        public String NewPassword { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "登录密码")]
        public String NewPassword2 { get; set; }

        /// <summary>
        /// 真实姓名
        /// <summary>
        [HtmlDisplayAttribute("真实姓名", "真实姓名")]
        public String Name { get; set; }

        /// <summary>
        /// 用户昵称
        /// <summary>
        [HtmlDisplayAttribute("用户昵称", "用户昵称")]
        public String NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码")]
        public String Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址")]
        public String Email { get; set; }

        /// <summary>
        /// 用户性别
        /// <summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public Int32 Sex { get; set; }

        /// <summary>
        /// 用户积分总额，同步积分流水记录
        /// </summary>
        public decimal IntegralAmount { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        [HtmlDisplay("所属省份", "所属省份")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        public IList<SelectListItem> ProvinceList { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        [HtmlDisplay("所属医院名称", "所属医院名称")]
        public long HospitalId { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        public IList<SelectListItem> HospitalList { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        public string HospitalName { get; set; }

   
        public bool IsCheckEmail { get; set; }

        public string ProjectUploadItem { get; set; }

    }


    /// <summary>
    /// MIC权限申请的首页基本信息实体模型
    /// </summary>
    [Validator(typeof(MICPermissionapplicationValidator))]
    public partial class MICIndexMemberModel : BaseEntityModel
    {

        /// <summary>
        /// 真实姓名
        /// <summary>
        [HtmlDisplayAttribute("真实姓名", "真实姓名")]
        public String Name { get; set; }


        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码")]
        public String Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址")]
        public String Email { get; set; }



        /// <summary>
        /// 所属省份
        /// </summary>
        [HtmlDisplay("所属省份", "所属省份")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        public IList<SelectListItem> ProvinceList { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        [HtmlDisplay("所属医院名称", "所属医院名称")]
        public long HospitalId { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        public IList<SelectListItem> HospitalList { get; set; }

        /// <summary>
        /// 所属医院名称
        /// </summary>
        public string HospitalName { get; set; }


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
        /// 科室名称
        /// </summary>
        public string HospitalDepartmentName { get; set; }


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
        /// 医生职称
        /// </summary>
        public string DoctorTitleName { get; set; }


        public bool IsCheckEmail { get; set; }

        public string ProjectUploadItem { get; set; }

        public string ProvinceName { get; set; }

    }

}
