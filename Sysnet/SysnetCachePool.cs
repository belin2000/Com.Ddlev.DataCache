using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.Ddlev.DataCache.Sysnet
{
    public class SysnetCachePool : IDataCacheHelper
    {
        public dynamic Get(string key)
        {
            return S_Get(key);
        }

        public bool HasKey(string key)
        {
            return S_HasKey(key);
        }

        public void Remove(string key)
        {
            S_Remove(key);
        }

        public void Set(string key, dynamic value, int ss = -1)
        {
            S_Set(key, value);
        }

        public static dynamic S_Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        public static bool S_HasKey(string key)
        {
            if (HttpContext.Current.Cache[key] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void S_Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        public static void S_Set(string key, dynamic value, int ss = -1)
        {
            S_Remove(key);
            HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddSeconds(ss), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }
    }
}
