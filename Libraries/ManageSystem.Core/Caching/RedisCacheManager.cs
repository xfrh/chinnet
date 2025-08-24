using System;
using System.Text;
using Newtonsoft.Json;
using StackExchange.Redis;
using ManageSystem.Core.Configuration;

namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// Represents a manager for caching in Redis store (http://redis.io/).
    /// Mostly it'll be used when running in a web farm or Azure.
    /// But of course it can be also used on any server or environment
    /// </summary>
    public partial class RedisCacheManager : ICacheManager
    {
        #region Fields

        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;

        /// <summary>
        /// 缓存key名称的前缀，也就是将该值加到前缀里面
        /// </summary>
        private static string RedisKeyPrefix = "";

        #endregion

        #region Ctor

        public RedisCacheManager(ManageSystemConfig config, IRedisConnectionWrapper connectionWrapper)
        {
            if (String.IsNullOrEmpty(config.RedisCachingConnectionString))
                throw new Exception("Redis的链接字符串不能为空，请检查webconfig");

            this._connectionWrapper = connectionWrapper;
            RedisCacheManager.RedisKeyPrefix = config.RedisKeyPrefix;

            this._db = _connectionWrapper.Database();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取缓存的key名称，前面将加上项目标识
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetRedisKeyName(string key)
        {
            return RedisCacheManager.RedisKeyPrefix + key;
        }

        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 根据缓存的key获取一个缓存的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            var rValue = _db.StringGet(this.GetRedisKeyName(key));
            if (!rValue.HasValue)
                return default(T);
            var result = Deserialize<T>(rValue);

            return result;
        }

        /// <summary>
        /// 插入一个缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="data">缓存的数据</param>
        /// <param name="cacheTime">缓存时间(分钟)</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            _db.StringSet(this.GetRedisKeyName(key), entryBytes, expiresIn);
        }

        /// <summary>
        ///得到指示值与指定的键相关联的值是否缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return _db.KeyExists(this.GetRedisKeyName(key));
        }

        /// <summary>
        /// 根据缓存的key删除缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public virtual void Remove(string key)
        {
            _db.KeyDelete(this.GetRedisKeyName(key));
        }

        /// <summary>
        /// 根据模型删除缓存
        /// </summary>
        /// <param name="pattern">模型</param>
        public virtual void RemoveByPattern(string pattern)
        {
            foreach (var ep in _connectionWrapper.GetEndpoints())
            {
                var server = _connectionWrapper.Server(ep);
                var keys = server.Keys(pattern: "*" + pattern + "*");
                foreach (var key in keys)
                    _db.KeyDelete(key);
            }
        }

        /// <summary>
        /// 清除所有的缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var ep in _connectionWrapper.GetEndpoints())
            {

                var server = _connectionWrapper.Server(ep);
                var keys = server.Keys();
                foreach (var key in keys)
                    _db.KeyDelete(key);
            }
        }

        /// <summary>
        /// 分布式锁 验证锁是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true 表示已经存在了。 false 则表示没有</returns>
        public bool ExistLock(string key)
        {
            if (this.IsSet(key))
                return true;

            return false;
        }

        /// <summary>
        ///  分布式锁 添加一个分布式锁，如果锁存在则添加失败返回false，锁不存在则返回true
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="minute">缓存时间，分钟</param>
        /// <param name="content">缓存内容，空的话默认当前时间字符串</param>
        /// <returns></returns>
        public bool AddLock(string key, int minute, string content = "")
        {
            if (this.IsSet(key))
                return false;

            if (string.IsNullOrWhiteSpace(content))
                content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //上面处理完成了要释放缓存，如果没有释放则系统5分钟后自动释放
            this.Set(key, content, minute);
            return true;
        }

        /// <summary>
        /// 分布式锁 手动释放验证重复支付的缓存key
        /// </summary>
        ///<param name="key">缓存key</param>
        public void DeleteLock(string key)
        {
            this.Remove(key);
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion

    }
}
