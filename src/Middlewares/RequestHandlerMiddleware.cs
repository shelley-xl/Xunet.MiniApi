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
        // 定义body
        string? body = null;

        try
        {
            // 预处理，并返回body
            body = await PrepareHandlerAsync(context);

            // 执行下一个中间件
            await next(context);

            // 状态码处理
            await StatusCodeHandlerAsync(context);
        }
        catch (Exception ex)
        {
            // 异常处理
            await ExceptionHandlerAsync(context, ex);
        }
        finally
        {
            // 请求处理
            await RequestHandlerAsync(context, body);
        }
    }

    // 预处理
    static async Task<string?> PrepareHandlerAsync(HttpContext context)
    {
        var requestEventHandler = context.RequestServices.GetService<IRequestLogEventHandler>();

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

            // 检测敏感词
            var sensitiveKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "password", "pwd", "passwd", "pass", "token", "secret", "sign", "signature", "client_id", "client_secret",
            };
            if (body != null && sensitiveKeywords.Any(x => body.Contains(x, StringComparison.OrdinalIgnoreCase)))
            {
                body = null;
            }

            return body;
        }

        return null;
    }

    // 处理状态码
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

    // 处理请求
    static async Task RequestHandlerAsync(HttpContext context, string? body = null)
    {
        // 过滤掉未找到端点的请求
        if (context.GetEndpoint() == null) return;
        // 过滤掉OPTIONS检查请求
        if (context.Request.Method == "OPTIONS") return;
        // 过滤掉健康检查请求
        if (context.Request.Path == "/health/check") return;

        // 获取请求耗时
        var duration = 0L;
        if (context.Items.TryGetValue("Duration", out var durationObj) && long.TryParse(durationObj?.ToString(), out duration))
        {
            context.Items.Remove("Duration");
            context.Items.Remove("StartTime");
        }

        try
        {
            var requestEventHandler = context.RequestServices.GetService<IRequestLogEventHandler>();

            if (requestEventHandler != null)
            {
                await requestEventHandler.InvokeAsync(context, duration, body);
            }

            var connection = context.RequestServices.GetService<IConnection>();

            if (connection != null)
            {
                // 发送到RabbitMQ消息队列
                var queueName = "logs_requests";
                using var channel = await connection.CreateChannelAsync();
                await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                var pars = new Dictionary<string, object?>
                {
                    { "Id", Guid.NewGuid().ToString() },
                    { "RequestId", XunetHttpContext.RequestId },
                    { "TraceId", XunetHttpContext.TraceId },
                    { "StatusCode", context.Response.StatusCode },
                    { "ServerIP", XunetHttpContext.ServerIPAddress },
                    { "ClientIP", XunetHttpContext.ClientIPAddress },
                    { "Path", context.Request.Path.Value },
                    { "Method", context.Request.Method },
                    { "ContentLength", XunetHttpContext.ContentLength },
                    { "Duration", duration },
                    { "RequestQuery", XunetHttpContext.QueryString },
                    { "RequestBody", body },
                    { "Referrer", XunetHttpContext.Referer },
                    { "UserAgent", XunetHttpContext.UserAgent },
                    { "Description", XunetHttpContext.Description },
                    { "Subject", (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name },
                    { "CreateId", XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject)},
                    { "CreateTime", DateTime.Now },
                };
                var message = pars.SerializerObject();
                await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: Encoding.UTF8.GetBytes(message));
            }
        }
        catch (Exception ex)
        {
            await ExceptionHandlerAsync(context, ex);
        }
    }

    // 处理异常
    static async Task ExceptionHandlerAsync(HttpContext context, Exception ex)
    {
        // 过滤掉未找到端点的请求
        if (context.GetEndpoint() == null) return;

        var exceptionEventHandler = context.RequestServices.GetService<IExceptionLogEventHandler>();

        if (exceptionEventHandler != null)
        {
            await exceptionEventHandler.InvokeAsync(context, ex);
        }

        var connection = context.RequestServices.GetService<IConnection>();

        if (connection != null)
        {
            // 发送到RabbitMQ消息队列
            var queueName = "logs_exceptions";
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var pars = new Dictionary<string, object?>
            {
                { "Id", Guid.NewGuid().ToString() },
                { "RequestId", XunetHttpContext.RequestId },
                { "TraceId", XunetHttpContext.TraceId },
                { "Message", ex.Message },
                { "StackTrace", ex.StackTrace },
                { "InnerException", ex.InnerException },
                { "ExceptionType", ex.GetType().FullName },
                { "Subject", (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name },
                { "CreateId", XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject)},
                { "CreateTime", DateTime.Now },
            };
            var message = pars.SerializerObject();
            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: Encoding.UTF8.GetBytes(message));
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
