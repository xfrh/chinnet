using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Users;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Users
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
        /// 登录密码
        /// <summary>
        [HtmlDisplayAttribute("登录密码", "请输入密码")]
        public String Password { get; set; }

        /// <summary>
        /// 登录验证码
        /// <summary>
        [HtmlDisplayAttribute("登录验证码", "请输入登录验证码")]
        public String Captcha { get; set; }

        /// <summary>
        /// 姓名
        /// <summary>
        [HtmlDisplayAttribute("姓名", "请输入姓名")]
        public String Name { get; set; }

        /// <summary>
        /// 用户昵称
        /// <summary>
        [HtmlDisplayAttribute("用户昵称", "请输入用户昵称")]
        public String NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "请输入手机号码")]
        public String Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "请输入邮箱地址")]
        public String Email { get; set; }

    }
}