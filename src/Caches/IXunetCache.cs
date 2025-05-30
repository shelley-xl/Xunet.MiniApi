// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Caches;

/// <summary>
/// 缓存接口
/// </summary>
public interface IXunetCache
{
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    void SetCache(string key, object value);

    /// <summary>
    /// 设置缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    Task SetCacheAsync(string key, object value);

    /// <summary>
    /// 设置缓存
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    void SetCache(string key, object value, TimeSpan timeout);

    /// <summary>
    /// 设置缓存（异步）
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <returns></returns>
    Task SetCacheAsync(string key, object value, TimeSpan timeout);

    /// <summary>
    /// 设置缓存
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <param name="expireType">过期类型</param>  
    void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType);


    /// <summary>
    /// 设置缓存（异步）
    /// 注：默认过期类型为绝对过期
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">值</param>
    /// <param name="timeout">过期时间间隔</param>
    /// <param name="expireType">过期类型</param>  
    /// <returns></returns>
    Task SetCacheAsync(string key, object value, TimeSpan timeout, ExpireType expireType);

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    string GetCache(string key);

    /// <summary>
    /// 获取缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    Task<string> GetCacheAsync(string key);

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    T GetCache<T>(string key);

    /// <summary>
    /// 获取缓存（异步）
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    Task<T> GetCacheAsync<T>(string key);

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    void RemoveCache(string key);

    /// <summary>
    /// 清除缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    Task RemoveCacheAsync(string key);

    /// <summary>
    /// 刷新缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    void RefreshCache(string key);

    /// <summary>
    /// 刷新缓存（异步）
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    Task RefreshCacheAsync(string key);
}
