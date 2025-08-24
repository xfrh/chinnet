using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Users;
using FluentValidation.Attributes;
using ManageSystem.Core.Domain.Users;
using System.Web.Mvc;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Admin.Validators.SystemSet;

namespace ManageSystem.Admin.Models.SystemSet
{
    /// <summary>
    /// 模型类 ，数据库表名：Userinfo 
    /// </summary>
    [Validator(typeof(UserProfileValidator))]
    public partial class UserProfileModel : BaseEntityModel
    {

        /// <summary>
        /// 登录帐号
        /// <summary>
        [HtmlDisplayAttribute("登录帐号", "请输入登录帐号")]
        public String LoginId { get; set; }

    
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
        /// 出生日期
        /// <summary>
        [HtmlDisplayAttribute("出生日期", "出生日期")]
        public DateTime? Birthday { get; set; }

        /// 出生日期
        /// <summary>
        [HtmlDisplayAttribute("出生日期", "出生日期")]
        public string BirthdayString
        {
            get
            {
                try
                {
                    if (this.Birthday == null) return "";

                    if (this.Birthday.GetValueOrDefault() <= DateTime.Parse("1910-1-1")) return "";

                    return this.Birthday.GetValueOrDefault().ToString("yyyy-MM-dd");

                }
                catch (Exception)
                {
                    return "";
                }

            }
        }

     
        /// <summary>
        /// 性别
        /// <summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public int Sex { get; set; }



    }
}
