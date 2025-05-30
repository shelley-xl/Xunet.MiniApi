// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Middlewares.IEventHandlers;

/// <summary>
/// 自定义请求日志事件处理器
/// </summary>
public interface IRequestLogEventHandler : IEventHandler
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <param name="body">请求body</param>
    /// <param name="duration">请求耗时</param>
    /// <returns></returns>
    Task InvokeAsync(HttpContext context, string? body, long duration);
}
