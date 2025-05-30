// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Caches;

/// <summary>
/// 缓存接口实现
/// </summary>
/// <param name="cache"></param>
public class XunetCache(IDistributedCache cache) : IXunetCache
{
    readonly IDistributedCache _cache = cache;

    /// <summary>
    /// 生成缓存Key
    /// </summary>
    /// <param name="idKey">idKey</param>
    /// <returns></returns>
    protected string BuildKey(string idKey)
    {
        return $"{GetType().FullName}_{idKey}";
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    public void SetCache(string key, object value)
    {
        string cacheKey = BuildKey(key);
        _cache.SetString(cacheKey, JsonSerializer.Serialize(value));
    }

    /// <summary>
    /// 设置缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public async Task SetCacheAsync(string key, object value)
    {
        string cacheKey = BuildKey(key);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(value));
    }

    /// <summary>
    /// 设置缓存
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    public void SetCache(string key, object value, TimeSpan timeout)
    {
        string cacheKey = BuildKey(key);
        _cache.SetString(cacheKey, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
        });
    }

    /// <summary>
    /// 设置缓存（异步）
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <returns></returns>
    public async Task SetCacheAsync(string key, object value, TimeSpan timeout)
    {
        string cacheKey = BuildKey(key);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
        });
    }

    /// <summary>
    /// 设置缓存
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <param name="expireType">过期类型</param> 
    public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType)
    {
        string cacheKey = BuildKey(key);
        if (expireType == ExpireType.Absolute)
        {
            _cache.SetString(cacheKey, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
            });
        }
        else
        {
            _cache.SetString(cacheKey, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeout
            });
        }
    }

    /// <summary>
    /// 设置缓存（异步）
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <param name="expireType">过期类型</param>  
    /// <returns></returns>
    public async Task SetCacheAsync(string key, object value, TimeSpan timeout, ExpireType expireType)
    {
        string cacheKey = BuildKey(key);
        if (expireType == ExpireType.Absolute)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
            });
        }
        else
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeout
            });
        }
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public string GetCache(string key)
    {
        if (string.IsNullOrEmpty(key)) return "";
        string cacheKey = BuildKey(key);
        var cache = _cache.GetString(cacheKey)!;
        return cache;
    }

    /// <summary>
    /// 获取缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public async Task<string> GetCacheAsync(string key)
    {
        if (string.IsNullOrEmpty(key)) return "";
        string cacheKey = BuildKey(key);
        var cache = await _cache.GetStringAsync(cacheKey);
        return cache!;
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public T GetCache<T>(string key)
    {
        var cache = GetCache(key);
        if (!string.IsNullOrEmpty(cache))
        {
            return JsonSerializer.Deserialize<T>(cache)!;
        }
        return default!;
    }

    /// <summary>
    /// 获取缓存（异步）
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public async Task<T> GetCacheAsync<T>(string key)
    {
        var cache = await GetCacheAsync(key);
        if (!string.IsNullOrEmpty(cache))
        {
            return JsonSerializer.Deserialize<T>(cache)!;
        }
        return default!;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    public void RemoveCache(string key)
    {
        _cache.Remove(BuildKey(key));
    }

    /// <summary>
    /// 清除缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public async Task RemoveCacheAsync(string key)
    {
        await _cache.RemoveAsync(BuildKey(key));
    }

    /// <summary>
    /// 刷新缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    public void RefreshCache(string key)
    {
        _cache.Refresh(BuildKey(key));
    }

    /// <summary>
    /// 刷新缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public async Task RefreshCacheAsync(string key)
    {
        await _cache.RefreshAsync(BuildKey(key));
    }
}
