using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Members
{
    public class MemberSettingModel
    {
        /// <summary>
        /// 认证金额
        /// </summary>
        public decimal AuthenticationAmount { get; set; }

        /// <summary>
        /// 微信信息接收管理员微信会员Ids，多个使用逗号分割
        /// </summary>
        public string MemberWeixinMessageAdmin { get; set; }
        

    }
}