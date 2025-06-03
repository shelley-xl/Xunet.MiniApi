// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi;

/// <summary>
/// IServiceCollection扩展
/// </summary>
public static class IServiceCollectionExtension
{
    #region 服务是否已注册
    private static readonly ConcurrentDictionary<string, char> keyValuePairs = new();
    /// <summary>
    /// 服务是否已注册
    /// </summary>
    /// <param name="_"></param>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public static bool HasRegistered(this IServiceCollection _, string modelName)
        => !keyValuePairs.TryAdd(modelName.ToLower(), '1');
    #endregion

    #region 添加HttpContext访问对象

    /// <summary>
    /// 添加HttpContext访问对象
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetHttpContextAccessor(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetHttpContextAccessor))) return services;

        services.AddHttpContextAccessor();

        return services;
    }

    #endregion

    #region 添加Json配置

    /// <summary>
    /// 添加Json配置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetJsonOptions(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetJsonOptions))) return services;

        services.ConfigureHttpJsonOptions(options =>
        {
            // 不区分大小写
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            // 忽略循环引用
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            // 使用小驼峰命名
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // 不使用 Unicode 编码
            options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            // 使用缩进格式
            options.SerializerOptions.WriteIndented = true;
            // 自定义时间格式
            options.SerializerOptions.Converters.Add(new DateTimeJsonConverter());
        });

        return services;
    }

    #endregion

    #region 添加健康检查

    /// <summary>
    /// 添加健康检查
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetHealthChecks(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetHealthChecks))) return services;

        services.AddHealthChecks();

        return services;
    }

    #endregion

    #region 添加Sqlite数据存储

    /// <summary>
    /// 添加Sqlite数据存储
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetSqliteStorage(this IServiceCollection services, StorageOptions[]? options = null)
    {
        if (services.HasRegistered(nameof(AddXunetSqliteStorage))) return services;

        services.AddSqlSugarClient(options, DbType.Sqlite);

        return services;
    }

    #endregion

    #region 添加MySql数据存储

    /// <summary>
    /// 添加MySql数据存储
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetMySqlStorage(this IServiceCollection services, StorageOptions[]? options = null)
    {
        if (services.HasRegistered(nameof(AddXunetMySqlStorage))) return services;

        services.AddSqlSugarClient(options, DbType.MySql);

        return services;
    }

    #endregion

    #region 添加SqlServer数据存储

    /// <summary>
    /// 添加SqlServer数据存储
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetSqlServerStorage(this IServiceCollection services, StorageOptions[]? options = null)
    {
        if (services.HasRegistered(nameof(AddXunetSqlServerStorage))) return services;

        services.AddSqlSugarClient(options, DbType.SqlServer);

        return services;
    }

    #endregion

    #region 添加跨域

    /// <summary>
    /// 添加跨域
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetCors(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetCors))) return services;

        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var corsHosts = config.GetSection("CorsHosts").Get<string[]>() ?? [];
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy.Default, policy =>
            {
                policy
                .WithOrigins(corsHosts)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("X-Pagination");
            });
        });

        return services;
    }

    #endregion

    #region 添加限流中间件

    /// <summary>
    /// 添加限流中间件
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetRateLimiter(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetRateLimiter))) return services;

        services.AddRateLimiter(limiterOptions =>
        {
            #region 自定义限流策略 => 图形验证码限流器
            // 自定义限流策略 => 图形验证码限流器
            // 同一个IP限制10个请求每分钟
            limiterOptions.AddPolicy(policyName: RateLimiterPolicy.VeryCode, context =>
            {
                var requestIP = XunetHttpContext.ClientIPAddress;

                var requestPath = context.Request.Path;

                var requestMethod = context.Request.Method;

                var clientKey = $"{requestIP}.{requestPath}.{requestMethod}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: clientKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromSeconds(60),
                    QueueLimit = 0
                });
            });
            #endregion

            #region 自定义限流策略 => 短信验证码限流器
            // 自定义限流策略 => 短信验证码限流器
            // 同一个手机号限制1个请求每分钟
            limiterOptions.AddPolicy(policyName: RateLimiterPolicy.SmsCode, httpContext =>
            {
                var request = XunetHttpContext.Current?.Request!;
                request.EnableBuffering();
                var node = JsonNode.Parse(request.Body);
                var phoneNumber = node?["phoneNumber"]?.ToString();
                request.Body.Position = 0;
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: phoneNumber, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(60),
                    QueueLimit = 0
                });
            });
            #endregion

            #region 当请求被限流时，会触发回调 OnRejected
            // 当请求被限流时，会触发回调 OnRejected
            // 并发限流器无法获取到 RetryAfter，因为它不是时间段的限流，而是限制的并发数
            limiterOptions.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                // 可以重新设置响应状态码，会覆盖掉设置的 limiterOptions.RejectionStatusCode
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = XunetCode.TooManyRequests,
                    Message = "请求繁忙，请稍后再试"
                },
                cancellationToken);
            };
            #endregion
        });

        return services;
    }

    #endregion

    #region 添加Jwt认证

    /// <summary>
    /// 添加Jwt认证
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetJwtAuth(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetJwtAuth))) return services;

        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.Configure<JwtConfig>(config.GetSection("JwtConfig"));

        var jwtConfig = config.GetSection("JwtConfig").Get<JwtConfig>() ?? throw new ConfigurationException("未配置JwtConfig节点");

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters = JwtToken.CreateTokenValidationParameters(jwtConfig);
        });

        return services;
    }

    #endregion

    #region 添加授权处理器

    /// <summary>
    /// 添加授权处理器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetAuthorizationHandler(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetAuthorizationHandler))) return services;

        var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var handler = entryAssembly.GetTypes().Where(x => x.BaseType == typeof(AuthorizationHandler<PermissionRequirement>)).FirstOrDefault() ?? throw new InvalidOperationException("未找到AuthorizationHandler");

        services.AddScoped(typeof(IAuthorizationHandler), handler);

        services.AddAuthorizationBuilder().AddPolicy(AuthorizePolicy.Default, policy =>
        {
            policy.Requirements.Add(new PermissionRequirement());
        });

        return services;
    }

    #endregion

    #region 添加OpenIddict认证

    /// <summary>
    /// 添加OpenIddict认证
    /// </summary>
    /// <param name="services"></param>
    /// <param name="grantTypes"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetOpenIddict(this IServiceCollection services, string[]? grantTypes = null)
    {
        if (services.HasRegistered(nameof(AddXunetOpenIddict))) return services;

        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddOpenIddict()

        // 注册OpenIddict客户端
        .AddClient(options =>
        {
            // 仅允许的授权类型
            if (grantTypes == null || grantTypes.Length == 0)
            {
                grantTypes = config.GetSection("OpenIddictClientRegistration:GrantTypes").Get<string[]?>() ?? [];
            }
            foreach (var grantType in grantTypes)
            {
                options.AllowCustomFlow(grantType);
            }

            // Disable token storage, which is not necessary for non-interactive flows like
            // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
            options.DisableTokenStorage();

            // Register the System.Net.Http integration and use the identity of the current
            // assembly as a more specific user agent, which can be useful when dealing with
            // providers that use the user agent as a way to throttle requests (e.g Reddit).
            options
            .UseSystemNetHttp()
            .SetProductInformation(Assembly.GetEntryAssembly()!);

            // Add a client registration matching the client application definition in the server project.
            options.AddRegistration(new OpenIddictClientRegistration
            {
                Issuer = new Uri(config["OpenIddictClientRegistration:Issuer"]!, UriKind.Absolute),
                ClientId = config["OpenIddictClientRegistration:ClientId"],
                ClientSecret = config["OpenIddictClientRegistration:ClientSecret"],
            });
        })

        .AddValidation(options =>
        {
            options.SetIssuer(new Uri(config["OpenIddictClientRegistration:Issuer"]!, UriKind.Absolute));

            options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(config["OpenIddictClientRegistration:EncryptionKey"]!)));

            options.UseSystemNetHttp();

            options.UseAspNetCore();
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddHttpClient("identity-server", client =>
        {
            client.BaseAddress = new Uri(config["OpenIddictClientRegistration:Issuer"]!);
        });

        return services;
    }

    #endregion

    #region 添加Swagger

    /// <summary>
    /// 添加Swagger
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetSwagger(this IServiceCollection services, SwaggerOptions? options = null)
    {
        if (services.HasRegistered(nameof(AddXunetSwagger))) return services;

        var env = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
        if (env.IsProduction()) return services;

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(x =>
        {
            if (options == null || options.Endpoints == null || options.Endpoints.Length == 0)
            {
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                options = config.GetSection("SwaggerOptions").Get<SwaggerOptions?>();
                if (options == null || options.Endpoints == null || options.Endpoints.Length == 0)
                {
                    x.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Minimal API 接口服务",
                        Description = "Minimal API 接口服务",
                        Version = $"v{Assembly.GetEntryAssembly()?.GetName().Version?.ToString()}",
                    });
                }
            }
            foreach (var endpoint in options?.Endpoints ?? [])
            {
                x.SwaggerDoc(endpoint.Name, new OpenApiInfo
                {
                    Title = endpoint.Title,
                    Description = endpoint.Description,
                    Version = $"v{Assembly.GetEntryAssembly()?.GetName().Version?.ToString()}",
                });
            }
            var scheme = new OpenApiSecurityScheme()
            {
                Description = "Authorization header. Example: 'Bearer token'",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            };
            x.OrderActionsBy(x => x.GroupName);
            x.OperationFilter<OperationFilter>();
            x.AddSecurityDefinition("Authorization", scheme);
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [scheme] = []
            });
        });

        return services;
    }

    #endregion

    #region 添加自动验证

    /// <summary>
    /// 添加自动验证
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetFluentValidation(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetFluentValidation))) return services;

        services.AddFluentValidationAutoValidation(config =>
        {
            // 验证失败，停止验证其他项
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        });

        // 注册到所有引用的程序集
        var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(entryAssembly);
        foreach (var assembly in entryAssembly.GetReferencedAssemblies())
        {
            services.AddValidatorsFromAssembly(Assembly.Load(assembly));
        }

        return services;
    }

    #endregion

    #region 添加自定义事件处理器

    /// <summary>
    /// 添加自定义事件处理器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetEventHandler(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetEventHandler))) return services;

        // 从程序集获取所有继承IEventHandler的实体类型
        var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var implementationTypes = entryAssembly.GetTypes().Where(where).ToArray();
        foreach (var assembly in entryAssembly.GetReferencedAssemblies())
        {
            var types = Assembly.Load(assembly).GetTypes().Where(where).ToArray();
            implementationTypes = [.. implementationTypes, .. types];
        }

        foreach (var implementationType in implementationTypes)
        {
            var serviceTypes = implementationType.GetInterfaces().Except([typeof(IEventHandler)]).ToArray();
            foreach (var serviceType in serviceTypes)
            {
                services.AddSingleton(serviceType, implementationType);
            }
        }

        static bool where(Type x)
        {
            return x.GetInterfaces().Contains(typeof(IEventHandler)) && x.IsClass;
        }

        return services;
    }

    #endregion

    #region 添加缓存

    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetCache(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetCache))) return services;

        services.AddMemoryCache();

        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var redisConnectionString = config.GetConnectionString("RedisConnection");
        if (string.IsNullOrEmpty(redisConnectionString)) throw new ConfigurationException("未配置Redis连接字符串");

        services.AddSingleton<IDistributedCache>(new CSRedisCache(new CSRedisClient(redisConnectionString)));
        services.AddSingleton<IXunetCache, XunetCache>();

        return services;
    }

    #endregion

    #region 添加对象映射

    /// <summary>
    /// 添加对象映射
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetMapper(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetMapper))) return services;

        // 注册到所有引用的程序集
        var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        services.AddAutoMapper(entryAssembly);
        foreach (var assembly in entryAssembly.GetReferencedAssemblies())
        {
            services.AddAutoMapper(Assembly.Load(assembly));
        }
        services.AddScoped<IObjectMapper, ObjectMapper>();

        return services;
    }

    #endregion

    #region 添加微信客户端

    /// <summary>
    /// 添加微信客户端
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetWeixinClient(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetWeixinClient))) return services;

        services.AddHttpClient<IWeixinService, WeixinService>();

        return services;
    }

    #endregion
}
