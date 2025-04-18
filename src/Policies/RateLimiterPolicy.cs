namespace Xunet.MiniApi.Policies;

/// <summary>
/// 限流策略
/// </summary>
public static class RateLimiterPolicy
{
    /// <summary>
    /// 固定窗口限流器
    /// </summary>
    public static readonly string Fixed = "fixed";

    /// <summary>
    /// 滑动窗口限流器
    /// </summary>
    public static readonly string Sliding = "sliding";

    /// <summary>
    /// 令牌桶限流器
    /// </summary>
    public static readonly string TokenBucket = "token_bucket";

    /// <summary>
    /// 并发限流器
    /// </summary>
    public static readonly string Concurrency = "concurrency";

    /// <summary>
    /// 自定义：根据登录账号限流
    /// </summary>
    public static readonly string CustomByUserId = "by_userid";

    /// <summary>
    /// 自定义：根据IP限流
    /// </summary>
    public static readonly string CustomByIP = "by_ip";
}
