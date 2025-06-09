// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
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
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    string BuildKey(string idKey, string? prefix = null)
    {
        return prefix == null ? $"{GetType().FullName}_{idKey}" : prefix.Trim() == "" ? idKey : $"{prefix}_{idKey}";
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="prefix">指定前缀</param>
    public void SetCache(string key, object value, string? prefix = null)
    {
        string cacheKey = BuildKey(key, prefix);
        _cache.SetString(cacheKey, JsonSerializer.Serialize(value));
    }

    /// <summary>
    /// 设置缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task SetCacheAsync(string key, object value, string? prefix = null)
    {
        string cacheKey = BuildKey(key, prefix);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(value));
    }

    /// <summary>
    /// 设置缓存
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <param name="prefix">指定前缀</param>
    public void SetCache(string key, object value, TimeSpan timeout, string? prefix = null)
    {
        string cacheKey = BuildKey(key, prefix);
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
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task SetCacheAsync(string key, object value, TimeSpan timeout, string? prefix = null)
    {
        string cacheKey = BuildKey(key, prefix);
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
    /// <param name="prefix">指定前缀</param>
    public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType, string? prefix = null)
    {
        string cacheKey = BuildKey(key, prefix);
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
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task SetCacheAsync(string key, object value, TimeSpan timeout, ExpireType expireType, string? prefix = null)
    {
        string cacheKey = BuildKey(key, prefix);
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
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public string GetCache(string key, string? prefix = null)
    {
        if (string.IsNullOrEmpty(key)) return "";
        string cacheKey = BuildKey(key, prefix);
        var cache = _cache.GetString(cacheKey)!;
        return cache;
    }

    /// <summary>
    /// 获取缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task<string> GetCacheAsync(string key, string? prefix = null)
    {
        if (string.IsNullOrEmpty(key)) return "";
        string cacheKey = BuildKey(key, prefix);
        var cache = await _cache.GetStringAsync(cacheKey);
        return cache!;
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="key">缓存Key</param>
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public T GetCache<T>(string key, string? prefix = null)
    {
        var cache = GetCache(key, prefix);
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
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task<T> GetCacheAsync<T>(string key, string? prefix = null)
    {
        var cache = await GetCacheAsync(key, prefix);
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
    /// <param name="prefix">指定前缀</param>
    public void RemoveCache(string key, string? prefix = null)
    {
        _cache.Remove(BuildKey(key, prefix));
    }

    /// <summary>
    /// 清除缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task RemoveCacheAsync(string key, string? prefix = null)
    {
        await _cache.RemoveAsync(BuildKey(key, prefix));
    }

    /// <summary>
    /// 刷新缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="prefix">指定前缀</param>
    public void RefreshCache(string key, string? prefix = null)
    {
        _cache.Refresh(BuildKey(key, prefix));
    }

    /// <summary>
    /// 刷新缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="prefix">指定前缀</param>
    /// <returns></returns>
    public async Task RefreshCacheAsync(string key, string? prefix = null)
    {
        await _cache.RefreshAsync(BuildKey(key, prefix));
    }
}
