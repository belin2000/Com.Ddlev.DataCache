using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <returns></returns>
        public static IDataCacheHelper GetCacheHelp(DataCacheType ct,int _dbRadis=-1)
        {
            var key = ct.ToString() + "db" + _dbRadis.ToString();
            lock (IClock)
            {
                if (dic.Count==0 || dic[key]==null)
                {
                    IDataCacheHelper _IDataCacheHelper = null;
                    switch (ct)
                    {
                        case DataCacheType.Redis:
                            _IDataCacheHelper = Redis.RedisPool.GetPool(_dbRadis);
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
