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
        string? requestBody = null;
        try
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            await next(context);
        }
        catch (Exception ex)
        {
            var db = context.RequestServices.GetService<ISqlSugarClient>();

            if (db != null)
            {
                var model = new ExceptionEntity
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    ExceptionType = ex.GetType().FullName,
                    RequestIP = XunetHttpContextAccessor.RequestIP,
                    RequestPath = context.Request.Path,
                    RequestMethod = context.Request.Method,
                    RequestQuery = context.Request.QueryString.Value,
                    RequestBody = requestBody,
                    UserAgent = context.Request.Headers.UserAgent,
                };

                db.Insertable(model).ExecuteCommand();
            }

            await context.Response.WriteAsJsonAsync(new OperateResultDto
            {
                Code = XunetCode.SystemException,
                Message = "系统异常，请联系管理员！"
            });
        }
    }
}
