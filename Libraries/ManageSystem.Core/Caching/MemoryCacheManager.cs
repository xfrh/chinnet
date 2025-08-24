using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// 直接通过内存缓存
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// 根据缓存的key获取一个缓存的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 插入一个缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="data">缓存的数据</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new System.Runtime.Caching.CacheItem(key, data), policy);
        }

        /// <summary>
        ///得到指示值与指定的键相关联的值是否缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 根据缓存的key删除缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 根据模型删除缓存
        /// </summary>
        /// <param name="pattern">模型</param>
        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        }

        /// <summary>
        /// 清除所有的缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }

        public bool ExistLock(string key)
        {
            throw new NotImplementedException();
        }

        public bool AddLock(string key, int minute, string content = "")
        {
            throw new NotImplementedException();
        }

        public void DeleteLock(string key)
        {
            throw new NotImplementedException();
        }
    }
}