using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.Ddlev.DataCache
{
    public class DataCacheHelper
    {
        static readonly object IClock = new object();
        static Dictionary<string, IDataCacheHelper> dic = new Dictionary<string, IDataCacheHelper>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="_dbRadis">只对Radis数据库有效</param>
        /// <param name="_conn">连接地址和端口，只对Radis数据库有效</param>
        /// <returns></returns>
        public static IDataCacheHelper GetCacheHelp(DataCacheType ct,int _dbRadis=-1,string conn=null)
        {
            var _conn = ct== DataCacheType.Redis?( conn ?? ConfigurationManager.AppSettings["RedisConfig"] ?? "127.0.0.1:6379"):"";
            var key = ct.ToString() + "db" + _conn;
            lock (IClock)
            {
                if (dic.Count==0 || dic[key]==null)
                {
                    IDataCacheHelper _IDataCacheHelper = null;
                    switch (ct)
                    {
                        case DataCacheType.Redis:
                            _IDataCacheHelper = Redis.RedisPool.GetPool(_conn, _dbRadis);
                            dic.Add(key, _IDataCacheHelper);
                            break;
                        case DataCacheType.Sysnet:
                            _IDataCacheHelper = Sysnet.SysnetPool.GetPool();
                            dic.Add(key, _IDataCacheHelper);
                            break;

                    }
                }
                return dic[key];
            }
        }
    }

    public enum DataCacheType
    {
        Sysnet=0,
        Redis=1
    }
}
