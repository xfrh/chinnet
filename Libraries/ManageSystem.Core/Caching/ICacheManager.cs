using System;

namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// 所有缓存的接口
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 根据缓存的key获取一个缓存的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 插入一个缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="data">缓存的数据</param>
        /// <param name="cacheTime">缓存时间(分钟)</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        ///得到指示值与指定的键相关联的值是否缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        bool IsSet(string key);

        /// <summary>
        /// 根据缓存的key删除缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        void Remove(string key);

        /// <summary>
        /// 根据模型删除缓存
        /// </summary>
        /// <param name="pattern">模型</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清除所有的缓存
        /// </summary>
        void Clear();


        /// <summary>
        /// 分布式锁 验证锁是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true 表示已经存在了。 false 则表示没有</returns>
        bool ExistLock(string key);

        /// <summary>
        ///  分布式锁 添加一个分布式锁，如果锁存在则添加失败返回false，锁不存在则返回true
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="minute">缓存时间，分钟</param>
        /// <param name="content">缓存内容，空的话默认当前时间字符串</param>
        /// <returns></returns>
        bool AddLock(string key, int minute, string content = "");

        /// <summary>
        ///  分布式锁 手动释放验证重复支付的缓存key
        /// </summary>
        ///<param name="key">缓存key</param>
        void DeleteLock(string key);
    }
}
