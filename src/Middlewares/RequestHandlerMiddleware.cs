// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Middlewares;

/// <summary>
/// 自定义请求处理中间件
/// </summary>
/// <param name="next">下一个中间件</param>
public class RequestHandlerMiddleware(RequestDelegate next)
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // 设置响应头
        var activity = context.Features?.Get<IHttpActivityFeature>()?.Activity;
        context.Response.Headers["X-Request-Id"] = context.TraceIdentifier;
        context.Response.Headers["X-Trace-Id"] = activity?.TraceId.ToHexString();
        // 启用请求体缓冲以便多次读取
        context.Request.EnableBuffering();
        // 读取请求Body
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        try
        {
            // 执行下一个中间件
            await next(context);

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status400BadRequest:
                    // 400 无效的请求
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetCode.BadRequest,
                        Message = "无效的请求"
                    });
                    break;
                case StatusCodes.Status401Unauthorized:
                    // 401 未授权
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetCode.Unauthorized,
                        Message = "未授权"
                    });
                    break;
                case StatusCodes.Status403Forbidden:
                    // 403 禁止访问
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetCode.Forbidden,
                        Message = "禁止访问"
                    });
                    break;
                case StatusCodes.Status404NotFound:
                    // 404 资源未找到
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetCode.NotFound,
                        Message = "资源未找到"
                    });
                    break;
                case StatusCodes.Status405MethodNotAllowed:
                    // 405 方法不允许
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetCode.MethodNotAllowed,
                        Message = "方法不允许"
                    });
                    break;
                default:
                    break;
            }
        }
        finally
        {
            // 记录请求日志
            var duration = 0L;
            if (context.Items.ContainsKey("Duration"))
            {
                duration = Convert.ToInt64(context.Items["Duration"]);
                context.Items.Remove("Duration");
                context.Items.Remove("StartTime");
            }

            var eventHandler = context.RequestServices.GetService<IRequestLogEventHandler>();
            eventHandler?.InvokeAsync(context, string.IsNullOrEmpty(requestBody) ? null : requestBody, duration);
        }
    }
}
