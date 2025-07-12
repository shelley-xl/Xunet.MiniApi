// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Middlewares;

/// <summary>
/// 参数签名中间件
/// </summary>
/// <param name="next">下一个中间件</param>
public class SignValidationMiddleware(RequestDelegate next)
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var config = context.RequestServices.GetRequiredService<IConfiguration>();
        var secretKey = config["SignSettings:SecretKey"] ?? throw new ConfigurationException("未配置参数签名SecretKey");
        var parameters = new Dictionary<string, string?>();
        // 收集body参数
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        var bodyDic = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);
        foreach (var (key, value) in bodyDic ?? [])
        {
            parameters.Add(key, value.ToString());
        }
        // 收集查询参数
        foreach (var (key, value) in context.Request.Query)
        {
            parameters.Add(key, value.ToString());
        }
        // 收集表单参数
        if (context.Request.HasFormContentType)
        {
            foreach (var (key, value) in context.Request.Form)
            {
                parameters.Add(key, value.ToString());
            }
        }
        // 获取客户端签名
        if (!parameters.TryGetValue("sign", out var clientSign) || string.IsNullOrEmpty(clientSign))
        {
            await SignErrorResult(context);
            return;
        }
        // 移除签名参数
        parameters.Remove("sign");
        // 验证时间戳
        if (!parameters.TryGetValue("timestamp", out var timestampStr) || !long.TryParse(timestampStr, out var timestamp) || !ValidateTimestamp(timestamp))
        {
            await SignErrorResult(context);
            return;
        }
        // 获取nonce
        if (!parameters.TryGetValue("nonce", out var nonce) || string.IsNullOrEmpty(nonce))
        {
            await SignErrorResult(context);
            return;
        }
        // 验证nonce
        var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
        if (cache.TryGetValue(nonce, out _))
        {
            await SignErrorResult(context);
            return;
        }
        // 排序并编码参数
        var sortedParams = parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => Uri.EscapeDataString(p.Value?.ToString() ?? ""));
        // 拼接参数
        var signString = string.Join("&", sortedParams.Select(p => $"{p.Key}={p.Value}"));
        // 计算签名
        var serverSign = ComputeSignature(signString, secretKey);
        if (!SecureCompare(serverSign, clientSign))
        {
            await SignErrorResult(context);
            return;
        }
        // nonce缓存
        cache.Set(nonce, true, TimeSpan.FromMinutes(5));
        await next(context);
    }
    // 返回参数签名错误
    static async Task SignErrorResult(HttpContext context)
    {
        await context.Response.WriteAsJsonAsync(new OperateResultDto
        {
            Code = XunetCode.SignError,
            Message = "参数签名错误！"
        });
    }
    // 生成HMAC签名
    static string ComputeSignature(string signString, string secretKey)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signString));
        return Convert.ToHexString(hashBytes).ToLower();
    }
    // 安全比较：使用恒定时间比较方法，防止时间攻击。
    static bool SecureCompare(string a, string b)
    {
        if (a.Length != b.Length) return false;
        int result = 0;
        for (int i = 0; i < a.Length; i++)
            result |= a[i] ^ b[i];
        return result == 0;
    }
    // 防重放攻击：检查请求中的时间戳是否在允许的时间窗口内（如±5分钟）。
    static bool ValidateTimestamp(long clientTimestamp, int toleranceMinutes = 5)
    {
        var serverTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var difference = Math.Abs(serverTime - clientTimestamp);
        return difference <= toleranceMinutes * 60;
    }
}
