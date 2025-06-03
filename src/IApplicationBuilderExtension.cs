// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi;

/// <summary>
/// IApplicationBuilder扩展
/// </summary>
public static class IApplicationBuilderExtension
{
    #region 使用HttpContext访问对象

    /// <summary>
    /// 使用HttpContext访问对象
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetHttpContextAccessor(this WebApplication app)
    {
        var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

        XunetHttpContext.Configure(httpContextAccessor);

        return app;
    }

    #endregion

    #region 使用健康检查

    /// <summary>
    /// 使用健康检查
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetHealthChecks(this WebApplication app)
    {
        app.UseHealthChecks("/health/check", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = (context, report) =>
            {
                return context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.Success,
                    Message = report.Status.ToString(),
                });
            }
        });

        return app;
    }

    #endregion

    #region 使用认证授权

    /// <summary>
    /// 使用认证授权
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    #endregion

    #region 使用Swagger

    /// <summary>
    /// 使用Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static WebApplication UseXunetSwagger(this WebApplication app, SwaggerOptions? options = null)
    {
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = string.Empty;
                x.DocumentTitle = options == null ? "Minimal API 接口服务" : options.DocumentTitle;
                x.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;
                x.DefaultModelsExpandDepth(-1);
                x.ShowExtensions();
                x.EnableValidator();
                if (options == null || options.Endpoints == null || options.Endpoints.Length == 0)
                {
                    options = app.Configuration.GetSection("SwaggerOptions").Get<SwaggerOptions?>();
                    if (options == null || options.Endpoints == null || options.Endpoints.Length == 0)
                    {
                        x.SwaggerEndpoint("/swagger/v1/swagger.json", "接口文档");
                    }
                }
                foreach (var endpoint in options?.Endpoints ?? [])
                {
                    x.SwaggerEndpoint($"/swagger/{endpoint.Name}/swagger.json", endpoint.EndpointName);
                }
            });
        }

        return app;
    }

    #endregion

    #region 使用数据存储

    /// <summary>
    /// 使用数据存储
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetStorage(this WebApplication app)
    {
        var db = app.Services.GetService<ISqlSugarClient>() ?? throw new InvalidOperationException("请先添加数据存储中间件");

        db.DbMaintenance.CreateDatabase();

        // 从程序集获取所有继承SugarEntity的实体类型
        var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var entityTypes = entryAssembly.GetTypes().Where(where).ToArray();
        foreach (var assembly in entryAssembly.GetReferencedAssemblies())
        {
            var types = Assembly.Load(assembly).GetTypes().Where(where).ToArray();
            entityTypes = [.. entityTypes, .. types];
        }

        db.CodeFirst.InitTables(entityTypes);

        static bool where(Type x)
        {
            return x.BaseType == typeof(SugarEntity);
        }

        return app;
    }

    #endregion

    #region 使用跨域

    /// <summary>
    /// 使用跨域
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetCors(this WebApplication app)
    {
        app.UseCors(CorsPolicy.Default);

        return app;
    }
    #endregion

    #region 使用自定义异常处理中间件

    /// <summary>
    /// 使用自定义异常处理中间件
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetCustomException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionMiddleware>(); ;
    }

    #endregion

    #region 使用自定义请求处理中间件

    /// <summary>
    /// 使用自定义请求处理中间件
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetRequestHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestHandlerMiddleware>();
    }

    #endregion
}
