using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Login;
using ManageSystem.Web.Validators.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Login
{
    /// <summary>
    /// 模型类 ，数据库表名：Userinfo 
    /// </summary>
    [Validator(typeof(LoginValidator))]
    public partial class LoginModel : BaseModel
    {

        /// <summary>
        /// 登录帐号
        /// <summary>
        [HtmlDisplayAttribute("登录帐号", "请输入登录帐号")]
        public String LoginId { get; set; }

        /// <summary>
        /// 验证码
        /// <summary>
        [HtmlDisplayAttribute("验证码", "验证码")]
        public String ValidateCode { get; set; }

    }
}