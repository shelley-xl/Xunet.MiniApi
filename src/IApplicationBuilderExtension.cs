// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
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

    #region 使用核心服务

    /// <summary>
    /// 使用核心服务
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetCore(this WebApplication app)
    {
        app.UseXunetRequestHandler();
        app.UseXunetHttpContextAccessor();
        app.UseXunetSwagger();
        app.UseXunetCors();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

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
                return context.Response.WriteAsJsonAsync(new QueryResultDto<HealthResultDto>
                {
                    Data = new HealthResultDto
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthResultDto.ChecksResult
                        {
                            Name = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description,
                        }),
                    }
                });
            }
        });

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
        if (app.Environment.IsProduction()) return app;

        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
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
            x.RoutePrefix = string.Empty;
            x.DocumentTitle = options == null ? "Minimal API 接口服务" : options.DocumentTitle;
            x.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;
            x.DefaultModelsExpandDepth(-1);
            x.ShowExtensions();
            x.EnableValidator();
        });

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
        var db = app.Services.GetService<ISqlSugarClient>() ?? throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddXunet(xxx)Storage' in the application startup code.");

        // 获取所有继承自 SugarEntity 的类
        var types = MiniApiAssembly.GetAllReferencedAssemblies(x =>
        {
            return x.BaseType == typeof(SugarEntity) && x.GetCustomAttribute<IgnoreSugarEntity>() == null;
        });

        if (types.Length == 0) return app;

        db.DbMaintenance.CreateDatabase();
        db.CodeFirst.InitTables(types);

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

    #region 使用自定义请求处理中间件

    /// <summary>
    /// 使用自定义请求处理中间件(包含异常处理)
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetRequestHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestHandlerMiddleware>();
    }

    #endregion

    #region 使用参数签名中间件

    /// <summary>
    /// 使用参数签名中间件
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetSignValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SignValidationMiddleware>();
    }

    #endregion
}
