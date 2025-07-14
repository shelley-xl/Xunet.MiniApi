// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
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
        var requestEventHandler = context.RequestServices.GetService<IRequestLogEventHandler>();
        var exceptionEventHandler = context.RequestServices.GetService<IExceptionLogEventHandler>();
        var body = string.Empty;

        try
        {
            // 预处理，并返回body
            body = await PrepareHandlerAsync(requestEventHandler, context);

            // 执行下一个中间件
            await next(context);

            // 状态码处理
            await StatusCodeHandlerAsync(context);
        }
        catch (Exception ex)
        {
            // 异常处理
            await ExceptionHandlerAsync(exceptionEventHandler, context, ex);
        }
        finally
        {
            // 请求处理
            await RequestHandlerAsync(requestEventHandler, exceptionEventHandler, context, body);
        }
    }

    static async Task<string?> PrepareHandlerAsync(IRequestLogEventHandler? requestEventHandler, HttpContext context)
    {
        // 记录请求开始时间
        context.Items["StartTime"] = Stopwatch.StartNew();
        // 设置响应头
        var activity = context.Features?.Get<IHttpActivityFeature>()?.Activity;
        context.Response.Headers["X-Request-Id"] = context.TraceIdentifier;
        context.Response.Headers["X-Trace-Id"] = activity?.TraceId.ToHexString();

        // 读取请求Body
        if (requestEventHandler != null)
        {
            // 如果是文件上传请求则跳过body读取
            if (context.Request.ContentType?.StartsWith("multipart/form-data") == true || context.Request.ContentType?.StartsWith("application/octet-stream") == true)
            {
                return null;
            }

            // 启用请求体缓冲以便多次读取
            context.Request.EnableBuffering();
            // 读取请求Body
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            body = string.IsNullOrWhiteSpace(body) ? null : body;
            context.Request.Body.Position = 0;

            return body;
        }

        return null;
    }

    static async Task StatusCodeHandlerAsync(HttpContext context)
    {
        if (context.Response.HasStarted) return;

        switch (context.Response.StatusCode)
        {
            case StatusCodes.Status400BadRequest:
                // 400 请求无效
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.BadRequest,
                    Message = "请求无效"
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
        }
    }

    static async Task RequestHandlerAsync(IRequestLogEventHandler? requestEventHandler, IExceptionLogEventHandler? exceptionEventHandler, HttpContext context, string? body = null)
    {
        if (requestEventHandler == null) return;

        // 记录请求日志
        var duration = 0L;
        if (context.Items.TryGetValue("Duration", out var durationObj) && long.TryParse(durationObj?.ToString(), out duration))
        {
            context.Items.Remove("Duration");
            context.Items.Remove("StartTime");
        }

        try
        {
            await requestEventHandler.InvokeAsync(context, duration, body);
        }
        catch (Exception ex)
        {
            await ExceptionHandlerAsync(exceptionEventHandler, context, ex);
        }
    }

    static async Task ExceptionHandlerAsync(IExceptionLogEventHandler? exceptionEventHandler, HttpContext context, Exception ex)
    {
        if (exceptionEventHandler != null)
        {
            await exceptionEventHandler.InvokeAsync(context, ex);
        }

        if (context.Response.HasStarted) return;

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            // 开发环境输出详细异常信息
            await context.Response.WriteAsJsonAsync(new OperateResultDto
            {
                Code = XunetCode.SystemException,
                Message = ex.ToString(),
            });
        }
        else
        {
            // 非开发环境输出友好提示信息
            await context.Response.WriteAsJsonAsync(new OperateResultDto
            {
                Code = XunetCode.SystemException,
                Message = "系统异常，请联系管理员！",
            });
        }

    }
}
