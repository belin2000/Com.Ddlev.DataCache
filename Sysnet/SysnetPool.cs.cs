using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.Ddlev.DataCache.Sysnet
{
    public class SysnetPool : IDataCacheHelper
    {
        

        private static SysnetPool _SysnetPool=null;
        static readonly object Sysnetlock = new object();
        public int cachedefaults = 0;
        public static SysnetPool GetPool( int _cachedefaults = 20)
        {
            if (_SysnetPool == null)
            {
                lock (Sysnetlock)
                {
                    _SysnetPool = new SysnetPool(_cachedefaults);
                }
            }
            return _SysnetPool;
        }
        public SysnetPool(int _cachedefaults=20)
        {
            cachedefaults = _cachedefaults;
        }
        public dynamic Get(string key)
        {
            return SysnetApplicationPool.S_Get(key)?? SysnetCachePool.S_Get(key);
        }

        public bool HasKey(string key)
        {
            return !SysnetApplicationPool.S_HasKey(key) ? SysnetCachePool.S_HasKey(key) : true;
        }

        public void Remove(string key)
        {
            SysnetCachePool.S_Remove(key);
            SysnetApplicationPool.S_Remove(key);

        }

        public void Set(string key, dynamic value, int ss = 0)
        {
            if (ss == 0) { ss=cachedefaults; }
            Remove(key);
            if (ss < 0) {
                SysnetApplicationPool.S_Set(key, value);
            }
            if (ss > 0) {
                SysnetCachePool.S_Set(key, value, ss);
            }
        }

        public void Clear()
        {
            SysnetApplicationPool.S_Clear();
            SysnetCachePool.S_Clear();
        }
    }
}
