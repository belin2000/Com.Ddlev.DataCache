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
                return System.Web.HttpContext.Current.Application.Get(key);
            }
            catch
            {
                return null;
            }
        }
        public static bool S_HasKey(string key)
        {
            return System.Web.HttpContext.Current.Application.AllKeys.Contains(key);
        }

        public static void S_Remove(string key)
        {
            try
            {
                if (System.Web.HttpContext.Current.Application[key] != null)
                {
                    System.Web.HttpContext.Current.Application.Remove(key);
                }
            }
            catch
            { }
        }

        public static void S_Set(string key, dynamic value, int ss = -1)
        {

            S_Remove(key);
            System.Web.HttpContext.Current.Application.Add(key, value);
        }

        public void Clear()
        {
            S_Clear();
        }
        public static void S_Clear()
        {
            System.Web.HttpContext.Current.Application.Clear();
        }
    }
}
