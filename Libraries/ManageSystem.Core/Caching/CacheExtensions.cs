using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// 缓存扩展类
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 获取一个缓存的项，如果不存在缓存中，那么将加载到缓存中
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存接口</param>
        /// <param name="key">缓存的key</param>
        /// <param name="acquire">函数加载项如果没有在缓存中</param>
        /// <returns>获取缓存项</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// 获取一个缓存的项，如果不存在缓存中，那么将加载到缓存中
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存接口</param>
        /// <param name="key">缓存的key</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="acquire">函数加载项如果没有在缓存中</param>
        /// <returns>获取缓存项</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        /// <summary>
        /// 根据参数删除缓存
        /// </summary>
        /// <param name="cacheManager">缓存接口</param>
        /// <param name="pattern">参数</param>
        /// <param name="keys">缓存中的key</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }
    }
}
