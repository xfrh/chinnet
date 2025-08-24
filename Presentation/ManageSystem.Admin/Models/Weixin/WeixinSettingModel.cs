using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Weixin
{
    public class WeixinSettingModel
    {

        /// <summary>
        /// 微信Token
        /// </summary>
        public string WeixinToken { get; set; }

        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WeixinAppId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        public string WeixinAppSecret { get; set; }

        /// <summary>
        /// 微信帐号类型
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// 微信帐号类型
        /// </summary>
        public IList<SelectListItem> AccountTypeList { get; set; }

        /// <summary>
        /// 微信支付商户id
        /// </summary>
        public string WeixinPayMchId { get; set; }

        /// <summary>
        ///  商户支付密钥Key
        /// </summary>
        public string WeixinPayKey { get; set; }

        /// <summary>
        ///  微信支付通知地址
        /// </summary>
        public string WeixinPayNotify { get; set; }

    }
}