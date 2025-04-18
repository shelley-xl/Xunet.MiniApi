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

        XunetHttpContextAccessor.Configure(httpContextAccessor);

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
    /// <returns></returns>
    public static WebApplication UseXunetSwagger(this WebApplication app)
    {
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = string.Empty;
                x.DocumentTitle = "Minimal API 接口服务";
                x.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;
                x.DefaultModelsExpandDepth(-1);
                x.ShowExtensions();
                x.EnableValidator();
                x.SwaggerEndpoint($"/swagger/v1/swagger.json", "Minimal API 接口服务");
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

        var entityTypes = Array.Empty<Type>();

        // 从程序集获取所有继承SugarEntity的实体类型
        foreach (var assembly in Assembly.GetEntryAssembly()?.GetReferencedAssemblies() ?? [])
        {
            var types = Assembly.Load(assembly).GetTypes().Where(x => x.BaseType == typeof(SugarEntity)).ToArray();
            entityTypes = [.. entityTypes, .. types];
        }

        db.CodeFirst.InitTables(entityTypes);

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
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseXunetCustomException(this WebApplication app)
    {
        app.UseMiddleware<CustomExceptionMiddleware>();

        var builder = app.MapGet("/api/logs/exception/page", GetExceptionLogAsync);

        builder.AddEndpointFilter<AutoValidationFilter>();
        builder.RequireAuthorization(AuthorizePolicy.Default);

        builder.WithTags("系统日志").WithOpenApi(x => new(x)
        {
            Summary = "获取异常日志分页列表",
            Description = "获取异常日志分页列表",
        });

        static async Task<IResult> GetExceptionLogAsync([AsParameters] PageRequest request, HttpContext context)
        {
            var db = context.RequestServices.GetService<ISqlSugarClient>();

            if (db == null) return XunetResults.Error("请先添加数据存储中间件");

            RefAsync<int> totalNumber = 0;

            var page = request.Page.GetValueOrDefault();
            var size = request.Size.GetValueOrDefault();

            var list = await db
                .Queryable<ExceptionEntity>()
                .OrderByDescending(x => x.CreateTime)
                .ToPageListAsync(page, size, totalNumber);

            return XunetResults.Ok(list, request, totalNumber);
        }

        return app;
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

    #region 使用参数签名验证中间件

    /// <summary>
    /// 使用参数签名验证
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetSignValidator(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SignValidatorMiddleware>();
    }

    #endregion
}
