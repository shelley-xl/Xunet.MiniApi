// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Middlewares;

/// <summary>
/// 自定义异常中间件
/// </summary>
/// <param name="next">下一个中间件</param>
public class CustomExceptionMiddleware(RequestDelegate next)
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // 记录请求开始时间
        context.Items["StartTime"] = Stopwatch.StartNew();
        // 启用请求体缓冲以便多次读取
        context.Request.EnableBuffering();
        // 读取请求Body
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var eventHandler = context.RequestServices.GetService<IExceptionLogEventHandler>();
            eventHandler?.InvokeAsync(context, ex, string.IsNullOrEmpty(requestBody) ? null : requestBody);

            if (!context.Response.HasStarted)
            {
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
    }
}
