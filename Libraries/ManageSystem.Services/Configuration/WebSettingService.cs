using ManageSystem.Core.Domain.Configuration;
using ManageSystem.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Configuration
{

    /// <summary>
    /// 微信设置相关的配置
    /// </summary>
    public class WebSettingService
    {

        /// <summary>
        /// 配置底层服务
        /// </summary>
        protected static readonly ISettingService SettingService = EngineContext.Current.Resolve<ISettingService>();


        #region 微信相关的配置


        /// <summary>
        ///  获取微信的AppId
        /// </summary>
        /// <returns></returns>
        public static string GetAppId()
        {
            return SettingService.QueryValue<string>("weixin.app.id");
        }

        /// <summary>
        ///  获取微信的Token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            return SettingService.QueryValue<string>("weixin.app.token");
        }

        /// <summary>
        ///  获取微信的Token
        /// </summary>
        /// <returns></returns>
        public static string GetAppSecret()
        {
            return SettingService.QueryValue<string>("weixin.app.secret");
        }

        /// <summary>
        ///  获取微信的与EncodingAESKey
        /// </summary>
        /// <returns></returns>
        public static string GetEncodingAESKey()
        {
            return SettingService.QueryValue<string>("weixin.app.aeskey");
        }

        /// <summary>
        ///  获取微信的关注推送对象
        /// </summary>
        /// <returns></returns>
        public static Setting GetSubscribe()
        {
            return SettingService.QueryEntity("weixin.subscribe");
        }

        /// <summary>
        ///  微信端的申请服务协议
        /// </summary>
        public static string GetWeixinAgreement()
        {
            return SettingService.QueryValue<string>("web.agreement");
        }


        #endregion


        #region  网站相关的配置

        /// <summary>
        /// 百度App key  （通用）
        /// </summary>
        public static string GetBaiduAppKey()
        {
            return SettingService.QueryValue<string>("baidu.app.key");
        }

        /// <summary>
        /// 网站名称
        /// </summary>
        public static string GetWebName()
        {
            return SettingService.QueryValue<string>("web.name");
        }

        /// <summary>
        /// SEO网站标题
        /// </summary>
        /// <returns></returns>
        public static string WebSEO_Title
        {
            get
            {
                string title = SettingService.QueryValue<string>("web.seo.title");
                if (string.IsNullOrWhiteSpace(title))
                {
                    title = GetWebName();
                }
                return title;
            }
        }

        /// <summary>
        /// SEO网站关键词
        /// </summary>
        public static string WebSEO_Key
        {
            get
            {
                return SettingService.QueryValue<string>("web.seo.key");
            }
        }
        /// <summary>
        /// SEO网站描述
        /// </summary>
        public static string WebSEO_Describe
        {
            get
            {
                return SettingService.QueryValue<string>("web.seo.describe");
            }
        }

        /// <summary>
        ///  后台的URL地址
        /// </summary>
        public static string GetAdminUrl()
        {
            return SettingService.QueryValue<string>("web.admin.url");
        }

        /// <summary>
        ///  weixin端的URL地址
        /// </summary>
        public static string GetWeixinUrl()
        {
            return SettingService.QueryValue<string>("web.weixin.url");
        }

        /// <summary>
        ///  PC 的URL地址
        /// </summary>
        public static string GetWebUrl()
        {
            return SettingService.QueryValue<string>("web.web.url");
        }

        /// <summary>
        ///  网站发送短信的key
        /// </summary>
        public static string GetWebSMS()
        {
            return SettingService.QueryValue<string>("web.sms.key");
        }

        /// <summary>
        ///  后台允许访问的ip
        /// </summary>
        public static string GetWebAdminAllowIp()
        {
            return SettingService.QueryValue<string>("web.admin.allow.ip");
        }



        #endregion

    }
}
