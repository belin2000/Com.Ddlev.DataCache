using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;

namespace Com.Ddlev.DataCache.Redis
{
    public class RedisPool : IDataCacheHelper
    {
        static readonly object redislock = new object();
        private static ConnectionMultiplexer _instance;
        private static RedisPool _RedisPool;
        int _dbIndex = -1;
        
        public static RedisPool GetPool(int dbIndex=-1)
        {
            if (_RedisPool==null) {
                lock(redislock)
                {
                    _RedisPool = new RedisPool(dbIndex);
                }
            }
            return _RedisPool;
        }
        public RedisPool(int dbIndex=-1)
        {
            try
            {
                _dbIndex = dbIndex;
                _instance = ConnectionMultiplexer.Connect(_conn);
            }
            catch
            {
                throw new Exception("数据源连接错误，请检查");
            }
        }

        private static string _conn = ConfigurationManager.AppSettings["RedisConfig"] ?? "127.0.0.1:6379";

        public dynamic Get(string key)
        {
            if (!HasKey(key))
            {
                return null;
            }
            return _instance.GetDatabase(_dbIndex).StringGet(key);
        }
        public void Remove(string key)
        {
            _instance.GetDatabase(_dbIndex).KeyDelete(key);
        }

        public void Clear()
        {
        }
        public bool HasKey(string key)
        {
            return _instance.GetDatabase(_dbIndex).KeyExists(key);
        }

        public void Set(string key, dynamic value,int ss=0)
        {
            if (ss <0)
            {
                _instance.GetDatabase(_dbIndex).StringSet(key, value);
            }
            else
            {
                if (ss > 0)
                {
                    _instance.GetDatabase(_dbIndex).StringSet(key, value, TimeSpan.FromSeconds(ss));
                }
            }
        }

    }
}
