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
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetJsonOptions(this IServiceCollection services, Action<JsonOptions>? options = null)
    {
        if (services.HasRegistered(nameof(AddXunetJsonOptions))) return services;

        options ??= x =>
        {
            // 不区分大小写
            x.SerializerOptions.PropertyNameCaseInsensitive = true;
            // 忽略循环引用
            x.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            // 使用小驼峰命名
            x.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // 不使用 Unicode 编码
            x.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            // 使用缩进格式
            x.SerializerOptions.WriteIndented = true;
            // 自定义时间格式
            x.SerializerOptions.Converters.Add(new DateTimeJsonConverter());
        };

        services.ConfigureHttpJsonOptions(options);

        return services;
    }

    #endregion

    #region 添加健康检查

    /// <summary>
    /// 添加健康检查
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddXunetHealthChecks(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetHealthChecks))) return default!;

        return services.AddHealthChecks();
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

        var useCacheService = services.HasRegistered(nameof(AddXunetCache));

        services.AddSqlSugarClient(options, DbType.Sqlite, useCacheService);

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

        var useCacheService = services.HasRegistered(nameof(AddXunetCache));

        services.AddSqlSugarClient(options, DbType.MySql, useCacheService);

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

        var useCacheService = services.HasRegistered(nameof(AddXunetCache));

        services.AddSqlSugarClient(options, DbType.SqlServer, useCacheService);

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
            // 同一个IP限制20个请求每分钟
            limiterOptions.AddPolicy(policyName: RateLimiterPolicy.VeryCode, context =>
            {
                var partitionKey = XunetHttpContext.ClientIPAddress;

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 20,
                    Window = TimeSpan.FromSeconds(60),
                    QueueLimit = 0
                });
            });
            #endregion

            #region 自定义限流策略 => 短信验证码限流器
            // 自定义限流策略 => 短信验证码限流器
            // 同一个手机号限制1个请求每分钟
            limiterOptions.AddPolicy(policyName: RateLimiterPolicy.SmsCode, context =>
            {
                var requestIP = XunetHttpContext.ClientIPAddress;

                string? phoneNumber = null;
                if (context.Request.Method == HttpMethods.Post)
                {
                    context.Request.EnableBuffering();
                    var node = JsonNode.Parse(context.Request.Body);
                    phoneNumber = node?["phone"]?.ToString();
                    context.Request.Body.Position = 0;
                }
                else if (context.Request.Method == HttpMethods.Get)
                {
                    phoneNumber = context.Request.Query["phone"].FirstOrDefault();
                }
                phoneNumber ??= "0";

                var partitionKey = $"{requestIP}.{phoneNumber}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: partitionKey, _ => new FixedWindowRateLimiterOptions
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
    public static IServiceCollection AddXunetJwtBearer(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetJwtBearer))) return services;

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

        // 获取所有继承自 AuthorizationHandler<PermissionRequirement> 的类
        var types = MiniApiAssembly.GetAllReferencedAssemblies(x =>
        {
            return x.BaseType == typeof(AuthorizationHandler<PermissionRequirement>);
        });

        foreach (var handler in types)
        {
            services.AddScoped(typeof(IAuthorizationHandler), handler);
        }

        services.AddAuthorizationBuilder().AddPolicy(AuthorizePolicy.Default, policy =>
        {
            policy.Requirements.Add(new PermissionRequirement());
        });

        return services;
    }

    #endregion

    #region 添加OpenIddict客户端

    /// <summary>
    /// 添加OpenIddict客户端
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetOpenIddictClient(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetOpenIddictClient))) return services;

        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddOpenIddict()

        // 注册OpenIddict客户端
        .AddClient(options =>
        {
            // 允许的授权类型
            var grantTypes = config.GetSection("OpenIddictClient:GrantTypes").Get<string[]>() ?? [];
            foreach (var grantType in grantTypes)
            {
                switch (grantType)
                {
                    case OpenIddict.Abstractions.OpenIddictConstants.GrantTypes.ClientCredentials:
                        options.AllowClientCredentialsFlow();
                        break;
                    case OpenIddict.Abstractions.OpenIddictConstants.GrantTypes.Password:
                        options.AllowPasswordFlow();
                        break;
                    case OpenIddict.Abstractions.OpenIddictConstants.GrantTypes.AuthorizationCode:
                        options.AllowAuthorizationCodeFlow();
                        break;
                    case OpenIddict.Abstractions.OpenIddictConstants.GrantTypes.RefreshToken:
                        options.AllowRefreshTokenFlow();
                        break;
                    case OpenIddict.Abstractions.OpenIddictConstants.GrantTypes.DeviceCode:
                        options.AllowDeviceAuthorizationFlow();
                        break;
                    case OpenIddict.Abstractions.OpenIddictConstants.GrantTypes.Implicit:
                        options.AllowImplicitFlow();
                        break;
                    default:
                        options.AllowCustomFlow(grantType);
                        break;
                }
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

            // 是否使用客户端注册
            var useRegistration = config.GetSection("OpenIddictClient:UseRegistration").Get<bool?>() ?? false;
            if (useRegistration)
            {
                // 注册客户端
                options.AddRegistration(new OpenIddictClientRegistration
                {
                    ClientId = config["OpenIddictClient:ClientId"]!,
                    ClientSecret = config["OpenIddictClient:ClientSecret"]!,
                    Issuer = new Uri(config["OpenIddictClient:Issuer"]!),
                });
            }
        })

        .AddValidation(options =>
        {
            options.SetIssuer(new Uri(config["OpenIddictClient:Issuer"]!));

            options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(config["OpenIddictClient:EncryptionKey"]!)));

            options.UseSystemNetHttp();

            options.UseAspNetCore();
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddAuthorizationBuilder();

        services.AddHttpClient("identity-client", client =>
        {
            client.BaseAddress = new Uri(config["OpenIddictClient:Issuer"]!);
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

        // 验证失败，停止验证其他项
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        // 注册到所有引用的程序集
        var assemblies = MiniApiAssembly.GetAllReferencedAssembly(x =>
        {
            return x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>) && x.Assembly != typeof(AbstractValidator<>).Assembly;
        });

        services.AddValidatorsFromAssemblies(assemblies);

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

        // 获取所有继承自 IEventHandler 的类
        var types = MiniApiAssembly.GetAllReferencedAssemblies(x =>
        {
            return x.GetInterfaces().Contains(typeof(IEventHandler)) && x.IsClass;
        });

        foreach (var implementationType in types)
        {
            var serviceTypes = implementationType.GetInterfaces().Except([typeof(IEventHandler)]).ToArray();
            foreach (var serviceType in serviceTypes)
            {
                services.AddSingleton(serviceType, implementationType);
            }
        }

        return services;
    }

    #endregion

    #region 添加迷你服务

    /// <summary>
    /// 添加迷你服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetMiniService(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetMiniService))) return services;

        // 获取所有继承自 MiniService 的类
        var types = MiniApiAssembly.GetAllReferencedAssemblies(x =>
        {
            return x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(MiniService<>);
        });

        foreach (var implementationType in types)
        {
            var serviceTypes = implementationType.GetInterfaces().Except([typeof(IBaseRepository)]).ToArray();
            foreach (var serviceType in serviceTypes)
            {
                services.AddSingleton(serviceType, implementationType);
            }
        }

        return services;
    }

    #endregion

    #region 添加缓存

    /// <summary>
    /// 添加缓存(未配置RedisConnection节点时,默认内存缓存)
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetCache(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetCache))) return services;

        // 从配置文件读取Redis连接字符串
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var redisConnectionString = config.GetConnectionString("RedisConnection");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            CSRedisClient redisClient;
            var redisConnectionStringArray = redisConnectionString.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (redisConnectionStringArray.Length == 1)
            {
                // 单体模式
                redisClient = new CSRedisClient(redisConnectionString);
            }
            else
            {
                // 集群模式
                redisClient = new CSRedisClient(null, redisConnectionStringArray);
            }
            // 移除无效节点
            var invalidKeys = redisClient.Nodes.Where(x => !x.Value.IsAvailable).Select(x => x.Key).ToArray();
            foreach (var key in invalidKeys)
            {
                if (redisClient.Nodes.Remove(key, out RedisClientPool? pool))
                {
                    pool?.Dispose();
                }
            }
            // 检查是否包含可用节点
            if (redisClient.Nodes.Any(x => x.Value.IsAvailable == true))
            {
                services.AddSingleton<IDistributedCache>(new CSRedisCache(redisClient));
            }
        }

        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
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
        var assemblies = MiniApiAssembly.GetAllReferencedAssembly(x =>
        {
            return x.BaseType == typeof(Profile) && x.Assembly != typeof(Profile).Assembly;
        });

        services.AddAutoMapper(assemblies);
        services.AddScoped<IXunetMapper, XunetMapper>();

        return services;
    }

    #endregion

    #region 添加阿里云短信服务

    /// <summary>
    /// 添加阿里云短信服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetAlibabaCloudSmsService(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetAlibabaCloudSmsService))) return services;

        services.AddSingleton<IAliyunSmsService, AliyunSmsService>();

        return services;
    }

    #endregion

    #region 添加阿里云对象存储服务

    /// <summary>
    /// 添加阿里云对象存储服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetAlibabaCloudOssService(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetAlibabaCloudOssService))) return services;

        services.AddSingleton<IAliyunOssService, AliyunOssService>();

        return services;
    }

    #endregion

    #region 添加腾讯云短信服务

    /// <summary>
    /// 添加腾讯云短信服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetTencentCloudSmsService(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetTencentCloudSmsService))) return services;

        services.AddSingleton<ITencentCloudSmsService, TencentCloudSmsService>();

        return services;
    }

    #endregion

    #region 添加腾讯云对象存储服务

    /// <summary>
    /// 添加腾讯云对象存储服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetTencentCloudCosService(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetTencentCloudCosService))) return services;

        services.AddSingleton<ITencentCloudCosService, TencentCloudCosService>();

        return services;
    }

    #endregion

    #region 添加微信公众号服务

    /// <summary>
    /// 添加微信公众号服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetWeixinMpService(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetWeixinMpService))) return services;

        services.AddXunetCache();
        services.AddHttpClient<IWeixinMpService, WeixinMpService>();

        return services;
    }

    #endregion

    #region 添加小程序服务

    /// <summary>
    /// 添加小程序服务(微信/钉钉)
    /// </summary>
    /// <param name="services"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetMiniProgramService(this IServiceCollection services, MiniProgramProvider provider = MiniProgramProvider.WeChat)
    {
        if (services.HasRegistered(nameof(AddXunetMiniProgramService))) return services;

        services.AddXunetCache();

        switch (provider)
        {
            case MiniProgramProvider.WeChat:
                services.AddHttpClient<IMiniProgramService, MiniProgramService>();
                break;
            case MiniProgramProvider.DingTalk:
                services.AddHttpClient<IDingTalkService, DingTalkService>();
                break;
            default:
                throw new NotSupportedException($"参数'{nameof(provider)}'不支持'{provider}'");
        }

        return services;
    }

    #endregion

    #region 添加RabbitMQ客户端

    /// <summary>
    /// 添加RabbitMQ客户端
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetRabbitMQClient(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetRabbitMQClient))) return services;

        var provider = services.BuildServiceProvider();

        var section = provider.GetRequiredService<IConfiguration>().GetSection("RabbitMQ");
        if (!section.GetChildren().Any()) throw new ConfigurationException("Missing RabbitMQ configuration section");

        var factory = new ConnectionFactory
        {
            HostName = section.GetValue<string>("HostName") ?? throw new ConfigurationException("Missing RabbitMQ HostName configuration"),
            Port = section.GetValue<int?>("Port") ?? throw new ConfigurationException("Missing RabbitMQ Port configuration"),
            UserName = section.GetValue<string>("UserName") ?? throw new ConfigurationException("Missing RabbitMQ UserName configuration"),
            Password = section.GetValue<string>("Password") ?? throw new ConfigurationException("Missing RabbitMQ Password configuration"),
        };

        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();

        services.AddSingleton(connection);

        return services;
    }

    #endregion

    #region 添加图形验证码

    /// <summary>
    /// 添加图形验证码
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetCaptcha(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetCaptcha))) return services;

        services.AddXunetCache();
        services.AddSingleton<IXunetCaptcha, XunetCaptcha>();

        return services;
    }

    #endregion
}
