using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.Ddlev.DataCache.Sysnet
{
    public class SysnetCachePool : IDataCacheHelper
    {
        public T Get<T>(string key)
        {
            return S_Get<T>(key);
        }

        public bool HasKey(string key)
        {
            return S_HasKey(key);
        }

        public void Remove(string key)
        {
            S_Remove(key);
        }


        public void Set<T>(string key, T value, int ss = -1)
        {
            S_Set(key, value,ss=-1);
        }

        public SysnetCachePool()
        {
        }

        public static T S_Get<T>(string key)
        {
            try
            {
                return (T)HttpRuntime.Cache.Get("Cac-" + key);
            }
            catch
            {
                return default(T);
            }
        }

        public static bool S_HasKey(string key)
        {
            if (HttpRuntime.Cache["Cac-" + key] == null)
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
            HttpRuntime.Cache.Remove("Cac-" + key);
        }

        public static void S_Set<T>(string key, T value, int ss = -1)
        {
            S_Remove(key);
            HttpRuntime.Cache.Insert("Cac-" + key, value, null, DateTime.Now.AddSeconds(ss), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }
        public void Clear()
        {
            S_Clear();
        }
        public static void S_Clear()
        {

            var CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                var itemkey = CacheEnum.Key.ToString();
                if (itemkey.Substring(0, 4) == "Cac-")
                {
                    HttpRuntime.Cache.Remove(itemkey);
                }
                continue;
            }
        }
    }
}
