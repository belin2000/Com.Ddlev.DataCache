using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.Ddlev.DataCache.Sysnet
{
    public class SysnetApplicationPool : IDataCacheHelper
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
            S_Set(key, value,ss=-1);
        }
        public SysnetApplicationPool()
        {
        }

        public static  dynamic S_Get(string key)
        {
            
            try
            {
                return HttpRuntime.Cache.Get("APLT-"+key);
            }
            catch
            {
                return null;
            }
        }
        public static bool S_HasKey(string key)
        {
            return !S_Get(key) == null;
        }

        public static void S_Remove(string key)
        {
            try
            {
                if (S_HasKey(key))
                {
                    HttpRuntime.Cache.Remove("APLT-" + key);
                }
            }
            catch
            { }
        }

        public static void S_Set(string key, dynamic value, int ss = -1)
        {

            HttpRuntime.Cache.Insert("APLT-" + key,value);
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
                if (itemkey.Substring(0, 5) == "APLT-")
                {
                    HttpRuntime.Cache.Remove(itemkey);
                }
                continue;
            }
        }
    }
}
