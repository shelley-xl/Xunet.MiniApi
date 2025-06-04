// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

internal class SugarXunetCache(IXunetCache cache) : ICacheService
{
    readonly IXunetCache _cache = cache;

    public void Add<V>(string key, V value)
    {
        _cache.SetCache(key, value!);
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        _cache.SetCache(key, value!, TimeSpan.FromSeconds(cacheDurationInSeconds));
    }

    public bool ContainsKey<V>(string key)
    {
        return _cache.GetCache<V>(key) != null;
    }

    public V Get<V>(string key)
    {
        return _cache.GetCache<V>(key);
    }

    public IEnumerable<string> GetAllKey<V>()
    {
        return _cache.GetCache<IEnumerable<string>>("*");
    }

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (ContainsKey<V>(cacheKey))
        {
            return Get<V>(cacheKey);
        }
        else
        {
            var result = create();
            Add(cacheKey, result, cacheDurationInSeconds);
            return result;
        }
    }

    public void Remove<V>(string key)
    {
        _cache.RemoveCache(key);
    }
}
