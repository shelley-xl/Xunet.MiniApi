﻿// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.EventHandlers;

/// <summary>
/// 自定义请求日志事件处理器
/// </summary>
public class RequestLogEventHandler : IRequestLogEventHandler
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <param name="body">请求body</param>
    /// <param name="duration">请求耗时</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, long duration, string? body = null)
    {
        // TODO: 记录日志

        await Task.CompletedTask;
    }
}
