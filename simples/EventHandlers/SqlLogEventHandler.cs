// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.EventHandlers;

/// <summary>
/// 自定义Sql日志事件处理器
/// </summary>
public class SqlLogEventHandler : ISqlLogEventHandler
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="sql">sql</param>
    /// <param name="paras">参数</param>
    /// <param name="duration">执行耗时</param>
    /// <param name="fail">是否执行失败</param>
    /// <param name="error">错误消息</param>
    /// <returns></returns>
    public async Task InvokeAsync(string sql, List<KeyValuePair<string, object>> paras, long duration, bool? fail = false, string? error = null)
    {
        // 过滤掉非Api请求
        using var scope = XunetHttpContext.Current?.RequestServices.CreateAsyncScope();
        if (scope == null) return;

        // TODO: 记录日志

        await Task.CompletedTask;
    }
}
