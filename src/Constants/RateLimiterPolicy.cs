// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Constants;

/// <summary>
/// 限流策略
/// </summary>
public static class RateLimiterPolicy
{
    /// <summary>
    /// 固定窗口限流器
    /// </summary>
    public const string Fixed = "fixed";

    /// <summary>
    /// 滑动窗口限流器
    /// </summary>
    public const string Sliding = "sliding";

    /// <summary>
    /// 令牌桶限流器
    /// </summary>
    public const string TokenBucket = "token_bucket";

    /// <summary>
    /// 并发限流器
    /// </summary>
    public const string Concurrency = "concurrency";

    /// <summary>
    /// 自定义图形验证码限流器
    /// </summary>
    public const string VeryCode = "very_code";

    /// <summary>
    /// 自定义短信验证码限流器
    /// </summary>

    public const string SmsCode = "sms_code";
}
