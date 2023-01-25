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
    public class SongCached : ICached<SongViewModel>
    {
        public readonly IBaseService<SongViewModel> _songService;
        private const string CacheKey = "SongList";
        private readonly IMemoryCache _memoryCache;

        public SongCached(IBaseService<SongViewModel> service,IMemoryCache memoryCache)
        {
            _songService = service;
            _memoryCache = memoryCache;
        }

        public List<string> GetAll()
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            if (_memoryCache.TryGetValue(CacheKey, out List<string> query))
                return query;

            query = _songService.GetAll();

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

            query = _songService.GetByName(name);

            _memoryCache.Set(CacheKey, query, cacheOptions);

            return query;
        }
    }
}
