using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Ddlev.DataCache
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataCacheHelper
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <param name="ss">过期时间(-1表示永不过期，0表示马上过期，大于0表示缓存的秒数)</param>
        void Set(string key, dynamic value, int ss = -1);
        /// <summary>
        /// 获取键值对应的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        dynamic Get(string key) ;
        /// <summary>
        /// 移除一个键值（它对应的值也被移除）
        /// </summary>
        /// <param name="key">键值</param>
        void Remove(string key);
        /// <summary>
        /// 判断是否有这个键值的存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        bool HasKey(string key);
    }
}
