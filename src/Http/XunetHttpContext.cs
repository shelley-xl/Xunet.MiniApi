// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Http;

/// <summary>
/// XunetHttpContext
/// </summary>
public static class XunetHttpContext
{
    #region 当前HttpContext

    /// <summary>
    /// 当前HttpContext
    /// </summary>
    public static HttpContext? Current => _accessor?.HttpContext;

    #endregion

    #region 客户端IP地址

    /// <summary>
    /// 客户端IP地址
    /// </summary>
    public static string ClientIPAddress
    {
        get
        {
            var requestIP = _accessor?.HttpContext?.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (string.IsNullOrEmpty(requestIP))
            {
                requestIP = _accessor?.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            if (string.IsNullOrEmpty(requestIP))
            {
                requestIP = _accessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();
            }
            if (string.IsNullOrEmpty(requestIP))
            {
                requestIP = "127.0.0.1";
            }
            return requestIP;
        }
    }

    #endregion

    #region 请求Uri地址

    /// <summary>
    /// 请求Uri地址
    /// </summary>
    public static string RequestUri
    {
        get
        {
            var request = _accessor?.HttpContext?.Request!;

            return $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
        }
    }

    #endregion

    #region 服务器IP地址

    /// <summary>
    /// 服务器IP地址
    /// </summary>
    public static string ServerIPAddress
    {
        get
        {
            var requestIP = _accessor?.HttpContext?.Connection.LocalIpAddress?.ToString();

            return string.IsNullOrEmpty(requestIP) ? "127.0.0.1" : requestIP;
        }
    }

    #endregion

    #region QueryString

    /// <summary>
    /// QueryString 
    /// </summary>
    public static string? QueryString
    {
        get
        {
            return _accessor?.HttpContext?.Request.QueryString.HasValue == true ? _accessor?.HttpContext?.Request.QueryString.Value : null;
        }
    }

    #endregion

    #region 接口描述

    /// <summary>
    /// 接口描述 
    /// </summary>
    public static string? Description
    {
        get
        {
            return _accessor?.HttpContext?.GetEndpoint()?.Metadata.GetMetadata<EndpointSummaryAttribute>()?.Summary;
        }
    }

    #endregion

    #region 请求内容长度

    /// <summary>
    /// 请求内容长度
    /// </summary>
    public static long ContentLength
    {
        get
        {
            return _accessor?.HttpContext?.Request.ContentLength ?? 0;
        }
    }

    #endregion

    #region 请求Id

    /// <summary>
    /// 请求Id
    /// </summary>
    public static string RequestId
    {
        get
        {
            return _accessor?.HttpContext?.TraceIdentifier!;
        }
    }

    #endregion

    #region 跟踪Id

    /// <summary>
    /// 跟踪Id
    /// </summary>
    public static string? TraceId
    {
        get
        {
            return _accessor?.HttpContext?.Features?.Get<IHttpActivityFeature>()?.Activity?.TraceId.ToHexString();
        }
    }

    #endregion

    #region UserAgent

    /// <summary>
    /// UserAgent 
    /// </summary>
    public static string? UserAgent
    {
        get
        {
            var userAgent = _accessor?.HttpContext?.Request.Headers.UserAgent.ToString();

            return string.IsNullOrEmpty(userAgent) ? null : userAgent;
        }
    }

    #endregion

    #region Referer

    /// <summary>
    /// Referer
    /// </summary>
    public static string? Referer
    {
        get
        {
            var referer = _accessor?.HttpContext?.Request.Headers.Referer.ToString();

            return string.IsNullOrEmpty(referer) ? null : referer;
        }
    }

    #endregion

    #region 获取服务

    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetService<T>()
    {
        if (_accessor?.HttpContext == null) return default;

        return _accessor.HttpContext.RequestServices.GetService<T>();
    }

    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRequiredService<T>() where T : notnull
    {
        if (_accessor?.HttpContext == null) return default!;

        return _accessor.HttpContext.RequestServices.GetRequiredService<T>();
    }

    #endregion

    #region 配置HttpContextAccessor

    /// <summary>
    /// 配置HttpContextAccessor
    /// </summary>
    /// <param name="accessor"></param>
    public static void Configure(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    static IHttpContextAccessor? _accessor;

    #endregion
}
