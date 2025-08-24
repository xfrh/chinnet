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
    public class WeixinSettingService
    {

        /// <summary>
        /// 配置底层服务
        /// </summary>
        protected static readonly ISettingService SettingService = EngineContext.Current.Resolve<ISettingService>();

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
        ///  微信帐号类型
        /// </summary>
        /// <returns></returns>
        public static string GetWeixinAccountType()
        {
            return SettingService.QueryValue<string>("weixin.account.type");
        }

        /// <summary>
        ///  获取微信支付商户id
        /// </summary>
        /// <returns></returns>
        public static string GetWeixinPayMchId()
        {
            return SettingService.QueryValue<string>("weixin.pay.mchid");
        }


        /// <summary>
        ///  商户支付密钥Key
        /// </summary>
        /// <returns></returns>
        public static string GetWeixinPayKey()
        {
            return SettingService.QueryValue<string>("weixin.pay.key");
        }

        /// <summary>
        ///  获取微信支付通知地址
        /// </summary>
        /// <returns></returns>
        public static string GetWeixinPayNotify()
        {
            return SettingService.QueryValue<string>("weixin.pay.notify");
        }

    }
}
