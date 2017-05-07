using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AppCache
{
    public class ApplicationCache
    {
        Object LOCK = new Object();

        private ApplicationCache() { }

        private static ApplicationCache instance = new ApplicationCache();

        /// <summary>
        /// 缓存管理实例
        /// </summary>
        public static ApplicationCache Instance
        {
            get
            {
                return instance;
            }
        }
        
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>缓存值</returns>
        public object GetValue(string key)
        {
            lock (LOCK)
            {
                return MemoryCache.Default.Get(key);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间--秒</param>
        /// <returns>设置结果</returns>
        public bool SetValue(string key, object value, int seconds)
        {
            lock (LOCK)
            {
                return MemoryCache.Default.Add(key, value, DateTimeOffset.UtcNow.AddSeconds(seconds));
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Delete(string key)
        {
            lock (LOCK)
            {
                if (MemoryCache.Default.Contains(key))
                {
                    MemoryCache.Default.Remove(key);
                }
            }
        }
    }
}
