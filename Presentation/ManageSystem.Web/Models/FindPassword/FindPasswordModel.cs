using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.FindPassword
{
    /// <summary>
    ///找回数据封装
    /// </summary>
    public class FindPasswordModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string ValidateCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string Password2 { get; set; }

  
    }

}