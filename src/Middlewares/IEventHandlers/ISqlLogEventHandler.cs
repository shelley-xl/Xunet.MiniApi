// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Middlewares.IEventHandlers;

/// <summary>
/// 自定义Sql日志事件处理器
/// </summary>
public interface ISqlLogEventHandler : IEventHandler
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="sql">sql</param>
    /// <param name="duration">执行耗时</param>
    /// <param name="fail">是否执行失败</param>
    /// <param name="error">错误消息</param>
    /// <returns></returns>
    Task InvokeAsync(string sql, long duration, bool? fail = false, string? error = null);
}
