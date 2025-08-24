using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;

namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// Represents a manager for caching during an HTTP request (short term caching)
    /// </summary>
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }
        
        /// <summary>
        /// Creates a new instance of the NopRequestCache class
        /// </summary>
        protected virtual IDictionary GetItems()
        {
            if (_context != null)
                return _context.Items;

            return null;
        }

        /// <summary>
        /// 根据缓存的key获取一个缓存的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        /// 插入一个缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="data">缓存的数据</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        /// <summary>
        ///得到指示值与指定的键相关联的值是否缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;
            
            return (items[key] != null);
        }
        /// <summary>
        /// 根据缓存的key删除缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public virtual void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }

        /// <summary>
        /// 根据模型删除缓存
        /// </summary>
        /// <param name="pattern">模型</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }


        /// <summary>
        /// 清除所有的缓存
        /// </summary>
        public virtual void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Clear();
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
