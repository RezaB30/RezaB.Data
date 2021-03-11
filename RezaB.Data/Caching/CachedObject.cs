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
        private CacheItemPolicy policy;

        public CachedObject(string cacheName, Func<T> onReload, CacheItemPolicy cachePolicy)
        {
            if (string.IsNullOrWhiteSpace(cacheName) || onReload == null || cachePolicy == null)
            {
                throw new ArgumentNullException("Non of 'cacheName', 'onReload' and 'cachePolicy' could not be null.");
            }
            CacheName = cacheName;
            onReloadDelegate = onReload;
            policy = cachePolicy;
        }
        
        public T Get()
        {
            var item = MemoryCache.Default.Get(CacheName) as T;
            return item ?? MemoryCache.Default.AddOrGetExisting(CacheName, onReloadDelegate(), policy) as T;
        }
    }
}
