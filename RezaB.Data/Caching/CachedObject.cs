using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Data.Caching
{
    public class CachedObject<T> where T: class
    {
        private string CacheName { get; set; }
        private Func<T> onReloadDelegate;
        private TimeSpan CacheDuration { get; set; }

        public CachedObject(string cacheName, Func<T> onReload, TimeSpan cacheDuration)
        {
            if (string.IsNullOrWhiteSpace(cacheName) || onReload == null || cacheDuration == null)
            {
                throw new ArgumentNullException("Non of 'cacheName', 'onReload' and 'cacheDuration' could not be null.");
            }
            CacheName = cacheName;
            onReloadDelegate = onReload;
            CacheDuration = cacheDuration;
        }
        
        public T Get()
        {
            var item = MemoryCache.Default.Get(CacheName) as T ?? onReloadDelegate();
            MemoryCache.Default.AddOrGetExisting(CacheName, item, DateTimeOffset.Now.Add(CacheDuration));
            return item;
        }
    }
}
