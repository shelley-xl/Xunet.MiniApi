namespace Xunet.MiniApi.Http;

/// <summary>
/// XunetHttpContextAccessor
/// </summary>
public static class XunetHttpContextAccessor
{
    /// <summary>
    /// 当前HttpContext
    /// </summary>
    public static HttpContext? Current => _accessor?.HttpContext;

    /// <summary>
    /// 请求IP
    /// </summary>
    public static string RequestIP
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

    /// <summary>
    /// 配置HttpContextAccessor
    /// </summary>
    /// <param name="accessor"></param>
    public static void Configure(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    static IHttpContextAccessor? _accessor;
}
