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
        private static string _conn;
        public static RedisPool GetPool( string conn = null, int dbIndex = -1)
        {
            if (_RedisPool==null) {
                lock(redislock)
                {
                    if (_RedisPool == null || !_instance.IsConnected)
                    {
                        _RedisPool = new RedisPool(conn);
                    }
                }
            }
            _RedisPool._dbIndex = dbIndex;
            return _RedisPool;
        }
        public RedisPool(string conn=null)
        {
            try
            {
                _conn = conn ?? ConfigurationManager.AppSettings["RedisConfig"] ?? "127.0.0.1:6379";
                _instance = ConnectionMultiplexer.Connect(_conn);
            }
            catch
            {
                throw new Exception("数据源连接错误，请检查");
            }
        }

        public T Get<T>(string key)
        {
            return Get<T>(key, _dbIndex);
        }

        public T Get<T>(string key, int dbIndex)
        {
            dbIndex = dbIndex < -1 ? _dbIndex : dbIndex;
            if (!HasKey(key))
            {
                return default(T);
            }

            if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16) || typeof(T) == typeof(Int32) || typeof(T) == typeof(Int64) || typeof(T) == typeof(long) || typeof(T) == typeof(decimal) || typeof(T) == typeof(Decimal) || typeof(T) == typeof(float) || typeof(T) == typeof(Double) || typeof(T) == typeof(double) || typeof(T) == typeof(string) || typeof(T) == typeof(String) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(bool) || typeof(T) == typeof(Enum))
            {
                return (T)Convert.ChangeType(_instance.GetDatabase(dbIndex).StringGet(key), typeof(T));
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_instance.GetDatabase(dbIndex).StringGet(key).ToString());
            }
            
        }
        public void Remove(string key)
        {
            Remove(key, _dbIndex);
        }
        public void Remove(string key, int dbIndex)
        {
            dbIndex = dbIndex < -1 ? _dbIndex : dbIndex;
            _instance.GetDatabase(dbIndex).KeyDelete(key);
        }

        public void Clear()
        {
            Clear(_dbIndex);
        }
        public void Clear(int dbIndex)
        {
            dbIndex = dbIndex < -1 ? _dbIndex : dbIndex;
            _instance.GetServer(_conn).FlushDatabase(dbIndex);
        }
        public bool HasKey(string key)
        {
            return HasKey(key, _dbIndex);
        }
        public bool HasKey(string key, int dbIndex)
        {
            dbIndex = dbIndex < -1 ? _dbIndex : dbIndex;
            try
            {
                return _instance.GetDatabase(dbIndex).KeyExists(key);
            }
            catch
            {
                return false;
            }
        }
        public void Set<T>(string key, T value, int ss = 0)
        {
            Set(key, value, _dbIndex, ss);
        }
        public void Set<T>(string key, T value, int dbIndex,int ss=0 )
        {
            var nvale = "";
            dbIndex = dbIndex < -1 ? _dbIndex : dbIndex;
            //对数据进行json编码后存储
            if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16) || typeof(T) == typeof(Int32) || typeof(T) == typeof(Int64) || typeof(T) == typeof(long) || typeof(T) == typeof(decimal) || typeof(T) == typeof(Decimal) || typeof(T) == typeof(float) || typeof(T) == typeof(Double) || typeof(T) == typeof(double) || typeof(T) == typeof(string) || typeof(T) == typeof(String) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(bool) || typeof(T) == typeof(Enum))
            {
                nvale = value.ToString();
            }
            else
            {
                nvale = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
            
            if (ss <0)
            {
                _instance.GetDatabase(dbIndex).StringSet(key, nvale);
            }
            else
            {
                if (ss > 0)
                {
                    _instance.GetDatabase(dbIndex).StringSet(key, nvale, TimeSpan.FromSeconds(ss));
                }
            }
        }

        public dynamic Find(string pattern, int dbIndex = -99) {
            dbIndex = dbIndex < -1 ? _dbIndex : dbIndex;
            return _instance.GetServer(_conn).Keys(dbIndex, pattern, 999);
        }

    }
}
