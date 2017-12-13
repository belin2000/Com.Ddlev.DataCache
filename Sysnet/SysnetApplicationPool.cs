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
            S_Set(key, value);
        }

        public static  dynamic S_Get(string key)
        {
            return HttpContext.Current.Application[key];
        }
        public static bool S_HasKey(string key)
        {
            return HttpContext.Current.Application.AllKeys.Contains(key);
        }

        public static void S_Remove(string key)
        {
            if (HttpContext.Current.Application[key] != null)
            {
                HttpContext.Current.Application.Remove(key);
            }
        }

        public static void S_Set(string key, dynamic value, int ss = -1)
        {
            S_Remove(key);
            HttpContext.Current.Application.Add(key, value);
        }

        public void Clear()
        {
            S_Clear();
        }
        public static void S_Clear()
        {
            HttpContext.Current.Application.Clear();
        }
    }
}
