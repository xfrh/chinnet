using System;
using System.Net;
using StackExchange.Redis;
using ManageSystem.Core.Configuration;

namespace ManageSystem.Core.Caching
{
    /// <summary>
    /// Redis 连接包装类
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        private readonly ManageSystemConfig _config;
        private readonly Lazy<string> _connectionString;

        private volatile ConnectionMultiplexer _connection;
        private readonly object _lock = new object();

        public RedisConnectionWrapper(ManageSystemConfig config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
        }

        private string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                if (_connection != null)
                {
                    //连接断开。处理连接…
                    _connection.Dispose();
                }

                //创建新实例的复述,连接
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }


        public IDatabase Database(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1); 
        }

        public IServer Server(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndpoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDb(int? db = null)
        {
            var endPoints = GetEndpoints();

            foreach (var endPoint in endPoints)
            {
                Server(endPoint).FlushDatabase(db ?? -1); 
            }
        }

        public ISubscriber GetSubscriber()
        {
            return this.GetConnection().GetSubscriber();
        }

        public ConnectionMultiplexer GetConnectionMultiplexer()
        {
            return this._connection;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

   
    }
}
