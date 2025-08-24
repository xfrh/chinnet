using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Domain.Configuration;
using ManageSystem.Core;

namespace ManageSystem.Services.Configuration
{

    /// <summary>
    /// 操作接口类 ，数据库表名：Setting 
    /// </summary>
    public partial interface ISettingService : IBaseService<Setting>
    {
        
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Setting> QueryPage(string title, string name,  int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 根据key更新数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="updateCache">是否更新缓存</param>
        bool UpdateValue(string key, string value, bool updateCache = true);

        /// <summary>
        /// 根据Key获取配置对象
        /// </summary>
        /// <typeparam name="key">配置的名称</typeparam>
        /// <typeparam name="cache">从缓存读取</typeparam>
        /// <returns>Setting </returns>
        Setting QueryEntity(string key ,bool cache=true);

        /// <summary>
        /// 根据Key获取配置的值，根据泛型反回值
        /// </summary>
        /// <typeparam name="T">配置中的值类型</typeparam>
        /// <param name="key">配置的名称</param>
        /// <returns>Setting value</returns>
        string QueryValue(string key);

        /// <summary>
        /// 根据Key获取对象
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>Setting value</returns>
        T QueryValue<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// 设置所有的配置到缓存中
        /// </summary>
        /// <returns></returns>
        List<Setting> SetAllSettingsCached();

        ///// <summary>
        ///// 获取缓存的中的所有配置
        ///// </summary>
        ///// <returns>Settings</returns>
        //List<Setting> GetAllSettingsCached();

        /// <summary>
        /// 确定设置是否存在
        /// </summary>
        /// <typeparam name="name">设置的名称</typeparam>
        bool Exists(string name);

 

    }
}
