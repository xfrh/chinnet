namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// Represents a NopNullCache (caches nothing)
    /// </summary>
    public partial class NopNullCache : ICacheManager
    {
        /// <summary>
        /// 根据缓存的key获取一个缓存的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        /// 插入一个缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="data">缓存的数据</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        /// <summary>
        /// <summary>
        ///得到指示值与指定的键相关联的值是否缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <summary>
        /// 根据缓存的key删除缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public virtual void Remove(string key)
        {
        }

        /// <summary>
        /// 根据模型删除缓存
        /// </summary>
        /// <param name="pattern">模型</param>
        public virtual void RemoveByPattern(string pattern)
        {
        }


        /// <summary>
        /// 清除所有的缓存
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }

        public bool ExistLock(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool AddLock(string key, int minute, string content = "")
        {
            throw new System.NotImplementedException();
        }

        public void DeleteLock(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}