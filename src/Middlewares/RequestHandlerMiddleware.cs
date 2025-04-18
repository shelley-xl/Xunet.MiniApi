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
        if (activity != null)
        {
            context.Response.Headers["X-Request-Id"] = context.TraceIdentifier;
            context.Response.Headers["X-Trace-Id"] = activity.TraceId.ToHexString();
        }
        // 记录请求开始时间
        context.Items["StartTime"] = Stopwatch.StartNew();

        // 执行下一个中间件
        await next(context);

        // 请求后记录请求日志
        if (!context.RequestAborted.IsCancellationRequested)
        {
            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                // 200 记录请求日志
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                // 401 未授权
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.Unauthorized,
                    Message = "401 Unauthorized"
                });
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                // 403 未授权
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.Forbidden,
                    Message = "403 Forbidden"
                });
            }
            else if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                // 404 未找到
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.NotFound,
                    Message = "404 NotFound"
                });
            }
            else
            {
                // 其他
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.InvalidRequest,
                    Message = "Invalid Request"
                });
            }
        }
    }
}
