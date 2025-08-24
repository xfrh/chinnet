using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Members
{
    /// <summary>
    /// 用户注册的数据模型
    /// </summary>
    public class MemberRegistModel
    {

        /// <summary>
        /// 登录帐号
        /// <summary>
        public String LoginId { get; set; }
        
        /// <summary>
        /// 用户姓名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        public String Phone { get; set; }

        /// <summary>
        /// 短信验证码
        /// <summary>
        public String PhoneCode { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        public String Password { get; set; }

        /// <summary>
        /// 确认登录密码
        /// <summary>
        public String Password2 { get; set; }

        /// <summary>
        /// 邀请码
        /// <summary>
        public String Code { get; set; }

    }
}