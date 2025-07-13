// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.EventHandlers;

/// <summary>
/// 自定义异常日志事件处理器
/// </summary>
public class ExceptionLogEventHandler : IExceptionLogEventHandler
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <param name="exception">异常信息</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, Exception exception)
    {
        // 忽略健康检查请求
        if (context.Request.Path == "/health/check") return;
        // 登录注册请求清空body
        // ...

        // TODO: 记录日志
        // 请求Id
        // var requestId = XunetHttpContext.RequestId;
        // 跟踪Id
        // var traceId = XunetHttpContext.TraceId;

        await Task.CompletedTask;
    }
}
