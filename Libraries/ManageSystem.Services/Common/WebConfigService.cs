using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ManageSystem.Services.Common
{
   public class WebConfigService
    {
        /// <summary>
        /// 百度App key  （通用）
        /// </summary>
        public static string BaiduAppKey = ConfigurationManager.AppSettings["BaiduAppKey"];

        /// <summary>
        /// 图片地址的url地址，因为前后台分离的原因
        /// </summary>
        public static string ImageUrl = ConfigurationManager.AppSettings["ImageUrl"];

        /// <summary>
        /// 微信服务器的网站的域名地址
        /// </summary>
        public static string WeixinWebUrl = ConfigurationManager.AppSettings["WeixinWebUrl"];

        /// <summary>
        /// 微信的 AccessToken
        /// </summary>
        public static string AccessToken = "";

        /// <summary>
        /// 与微信公众账号后台的Token设置保持一致，区分大小写。
        /// </summary>
  //      public static string WeixinToken = ConfigurationManager.AppSettings["WeixinToken"];

        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
     //   public static string WeixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];

        /// <summary>
        /// 与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        /// </summary>
      //  public static string WeixinEncodingAESKey = ConfigurationManager.AppSettings["WeixinEncodingAESKey"];

        /// <summary>
        /// 与微信公众账号后台的AppSecret设置保持一致，区分大小写。
        /// </summary>
      //  public static string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];




    }
}
