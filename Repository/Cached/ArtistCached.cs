using Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Data.ViewModels;

namespace Service.Cached
{
    public class ArtistCached : ICached<ArtistViewModel>
    {
        public readonly IBaseService<ArtistViewModel> _artistService;
        private const string CacheKey = "ArtistList";
        private readonly IMemoryCache _memoryCache;

        public ArtistCached(IBaseService<ArtistViewModel> service, IMemoryCache memoryCache)
        {
            _artistService = service;
            _memoryCache = memoryCache;
        }

        public List<string> GetAll()
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            if (_memoryCache.TryGetValue(CacheKey, out List<string> query))
                return query;

            query = _artistService.GetAll();

            _memoryCache.Set(CacheKey, query, cacheOptions);

            return query;
        }

        public string GetByName(string name)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromSeconds(10))
           .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            if (_memoryCache.TryGetValue(CacheKey, out string query))
                return query;

            query = _artistService.GetByName(name);

            _memoryCache.Set(CacheKey, query, cacheOptions);

            return query;
        }
    }
}
