using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.HttpContext
{
    /// <summary>
    ///  HttpContext Session 管理
    /// </summary>
    public interface ISessionManager: IDisposable
    {
        /// <summary>
        /// 根据session 名称获取一个值
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="sessionName">session 的名称</param>
        /// <returns></returns>
        T Get<T>(string sessionName);

        /// <summary>
        /// 添加一个Session
        /// </summary>
        /// <param name="sessionName">session 名称</param>
        /// <param name="data">数据</param>
        void Set(string sessionName, object data );

        /// <summary>
        /// 根据session 的名称检查是否存在
        /// </summary>
        /// <param name="sessionName">session 名称</param>
        /// <returns></returns>
        bool IsSet(string sessionName);

        /// <summary>
        /// 删除一个session
        /// </summary>
        ///<param name="sessionName">session 名称</param>
        void Remove(string sessionName);

    }
}
