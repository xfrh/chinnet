using System;
using System.Linq.Expressions;
using System.Reflection;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Infrastructure;

namespace ManageSystem.Services.Configuration
{
    public static class SettingExtensions
    {

   

        /// <summary>
        /// Get setting key (stored into database)
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>Key</returns>
        public static string GetSettingKey<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            var key = typeof(T).Name + "." + propInfo.Name;
            return key;
        }


        /// <summary>
        /// 刷新微信端的缓存
        /// </summary>
        /// <param name="service"></param>
        public static void RefreshWebCache(this ISettingService service ,string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            //采用redis 来缓存所以，所有的端都是读同一个地方的数据，所以不需要调用接口刷新数据
            return; 
           
            //微信端的url
            string weixinUrl = service.QueryValue<string>("weixin.refresh.cache.url");
            if (!string.IsNullOrWhiteSpace(weixinUrl))
            {
                //更新微信端全部缓存
                HttpHelper.HttpPost(weixinUrl, "key=" + key);
            }

            //web端的url
            string webUrl = service.QueryValue<string>("web.refresh.cache.url");
            if (!string.IsNullOrWhiteSpace(webUrl))
            {
                //更新微信端全部缓存
                HttpHelper.HttpPost(webUrl, "key=" + key);
            }

        }
    }
}
