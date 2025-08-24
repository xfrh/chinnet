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
    /// 默认的session处理方式
    /// 
    /// </summary>
    public class DefaultSessionManager : ISessionManager
    {
        public readonly HttpContextBase HttpContext;

        public DefaultSessionManager(HttpContextBase httpContext)
        {
            this.HttpContext = httpContext;
        }

        /// <summary>
        /// 根据session 名称获取一个值
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="sessionName">session 的名称</param>
        /// <returns></returns>
        public T Get<T>(string sessionName)
        {
            object value = this.HttpContext.Session[sessionName];
            if (value == null) return default(T);

            return (T)value;
        }

        /// <summary>
        /// 根据session 的名称检查是否存在
        /// </summary>
        /// <param name="sessionName">session 名称</param>
        /// <returns></returns>
        public bool IsSet(string sessionName)
        {
            object value = this.HttpContext.Session[sessionName];

            return value == null;
        }

        /// <summary>
        /// 删除一个session
        /// </summary>
        ///<param name="sessionName">session 名称</param>
        public void Remove(string sessionName)
        {
            this.HttpContext.Session[sessionName] =null;
            this.HttpContext.Session.Remove(sessionName);
        }

        /// <summary>
        /// 添加一个Session
        /// </summary>
        /// <param name="sessionName">session 名称</param>
        /// <param name="data">数据</param>
        public void Set(string sessionName, object data )
        {
            if (data == null) return;

            this.HttpContext.Session[sessionName] = data;
        }

        public void Dispose()
        {

        }

    }
}
