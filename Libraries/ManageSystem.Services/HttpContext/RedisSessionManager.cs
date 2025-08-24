using ManageSystem.Core.Caching;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ManageSystem.Services.HttpContext
{
    /// <summary>
    /// 
    /// 使用redis做session服务器，通过cookie存储redis的key
    /// 
    /// </summary>
    public class RedisSessionManager : ISessionManager
    {
        public readonly HttpContextBase HttpContext;
        public readonly ICacheManager CacheManager;

        /// <summary>
        /// 保存到cookie的session名称前缀
        /// </summary>
        private const string SessionNamePrefix = "session.";

        public RedisSessionManager(HttpContextBase _httpContext, ICacheManager _cacheManager)
        {
            this.HttpContext = _httpContext;
            this.CacheManager = _cacheManager;
        }

        /// <summary>
        /// 获取Cookie的名称
        /// </summary>
        /// <param name="sessionName">session 的名称</param>
        /// <returns></returns>
        private string GetCookieName(string sessionName)
        {
            return SessionNamePrefix + sessionName;
        }

        /// <summary>
        /// 获取redis的key名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetRedisKeyName(string name)
        {
            return SessionNamePrefix + name;
        }

        /// <summary>
        /// 添加一个Session
        /// </summary>
        /// <param name="sessionName">session 名称</param>
        /// <param name="data">数据</param>
        public void Set(string sessionName, object data)
        {
            if (data == null) return;

            string cookieName = this.GetCookieName(sessionName);
            string redisName = "";

            //添加Cookie记录sessionId
            string newSessionID = Guid.NewGuid().ToString();

            HttpCookie cookie = new HttpCookie(cookieName)
            {
                Value = newSessionID,
                Expires = DateTime.Now.Add(new TimeSpan(0, 24, 0))
            };

            this.HttpContext.Response.Cookies.Add(cookie);

            //保存到redis
            redisName = this.GetRedisKeyName(newSessionID);
            this.CacheManager.Set(redisName, data, 24);
        }

        /// <summary>
        /// 根据session 名称获取一个值
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="sessionName">session 的名称</param>
        /// <returns></returns>
        public T Get<T>(string sessionName)
        {
            try
            {
                //获取session的id
                HttpCookie cookie = this.HttpContext.Request.Cookies[this.GetCookieName(sessionName)];
                if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value)) return default(T);

                //读取Cookie获取session的id
                var value = this.CacheManager.Get<T>(this.GetRedisKeyName(cookie.Value));
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一个session
        /// </summary>
        ///<param name="sessionName">session 名称</param>
        public void Remove(string sessionName)
        {
            HttpCookie cookie = this.HttpContext.Request.Cookies[this.GetCookieName(sessionName)];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value)) return;

            //删除缓存的数据
            this.CacheManager.Remove(this.GetRedisKeyName(cookie.Value));

            //删除Cookie
            cookie.Expires = DateTime.Now.AddMinutes(-1);
            cookie.Value = "";
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

        }

        /// <summary>
        /// 根据session 的名称检查是否存在
        /// </summary>
        /// <param name="sessionName">session 名称</param>
        /// <returns></returns>
        public bool IsSet(string sessionName)
        {
            object value = this.Get<dynamic>(sessionName);

            return value == null;
        }

        public void Dispose()
        {

        }

    }
}
