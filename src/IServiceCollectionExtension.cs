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
    /// 添加Sqlite数据存储中间件
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetSqliteStorage(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetSqliteStorage))) return services;

        services.AddSingleton<ISqlSugarClient>(x =>
        {
            var dbVersion = "1.0.1.1";
            var dbName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Default.db";
            var dirName = Assembly.GetExecutingAssembly().GetName().Name ?? "Xunet.MiniApi";
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var baseDir = Path.Combine(appDataDir, dirName, dbVersion, "data");
            var connectionString = $@"Data Source={baseDir}\{dbName};";

            using var db = new SqlSugarScope(new ConnectionConfig
            {
                ConfigId = 1,
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = connectionString,
            });

            db.Ado.CommandTimeOut = 60;

            return db;
        });

        return services;
    }

    #endregion

    #region 添加MySql数据存储

    /// <summary>
    /// 添加MySql数据存储
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetMySqlStorage(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetMySqlStorage))) return services;

        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("请先配置数据库连接字符串");
            using var db = new SqlSugarScope(new ConnectionConfig
            {
                ConfigId = 2,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = connectionString,
            });

            db.Ado.CommandTimeOut = 60;

            return db;
        });

        return services;
    }

    #endregion

    #region 添加SqlServer数据存储

    /// <summary>
    /// 添加SqlServer数据存储
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetSqlServerStorage(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetSqlServerStorage))) return services;

        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("请先配置数据库连接字符串");
            using var db = new SqlSugarScope(new ConnectionConfig
            {
                ConfigId = 3,
                DbType = DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = connectionString,
            });

            db.Ado.CommandTimeOut = 60;

            return db;
        });

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
            #region 配置限流策略 => 固定窗口限流器
            // 配置限流策略 => 固定窗口限流器
            // 窗口时间长度为60s，在每个窗口时间范围内，最多允许100个请求被处理。
            limiterOptions.AddFixedWindowLimiter(policyName: RateLimiterPolicy.Fixed, fixedOptions =>
            {
                // 窗口阈值，即每个窗口时间范围内，最多允许的请求个数。这里指定为最多允许100个请求。该值必须 > 0
                fixedOptions.PermitLimit = 100;
                // 窗口大小，即时间长度。这里设置为 60 s。该值必须 > TimeSpan.Zero
                fixedOptions.Window = TimeSpan.FromSeconds(60);
                // 队列中最多允许请求排队数，设置为 0 时取消排队
                fixedOptions.QueueLimit = 0;
                // 排队请求的处理顺序。这里设置为优先处理先来的请求
                fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                // 指示开启新窗口时是否自动重置请求限制，该值默认为true。如果设置为false，则需要手动调用 SlidingWindowRateLimiter.TryReplenish来重置
                fixedOptions.AutoReplenishment = true;
            });
            #endregion

            #region 配置限流策略 => 滑动窗口限流器
            // 配置限流策略 => 滑动窗口限流器
            // 窗口时间长度为30s，在每个窗口时间范围内，最多允许100个请求，窗口段数为 3，每个段的时间间隔为 30s / 3 = 10s，即窗口每 10s 滑动一段。
            limiterOptions.AddSlidingWindowLimiter(policyName: RateLimiterPolicy.Sliding, slidingOptions =>
            {
                // 窗口阈值，即每个窗口时间范围内，最多允许的请求个数。这里指定为最多允许100个请求。该值必须 > 0
                slidingOptions.PermitLimit = 100;
                // 窗口大小，即时间长度。这里设置为 60 s。该值必须 > TimeSpan.Zero
                slidingOptions.Window = TimeSpan.FromSeconds(60);
                // 队列中最多允许请求排队数，设置为 0 时取消排队
                slidingOptions.QueueLimit = 0;
                // 排队请求的处理顺序。这里设置为优先处理先来的请求
                slidingOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                // 指示开启新窗口时是否自动重置请求限制，该值默认为true。如果设置为false，则需要手动调用 SlidingWindowRateLimiter.TryReplenish来重置
                slidingOptions.AutoReplenishment = true;
                // 每个窗口的段的个数，通过它可以计算出每个段滑动的时间间隔。这里设置段数为 3，时间间隔为 10s。该值必须 > 0
                slidingOptions.SegmentsPerWindow = 3;
            });
            #endregion

            #region 配置限流策略 => 令牌桶限流器
            // 配置限流策略 => 令牌桶限流器
            // 桶最多可以装 4 个令牌，每 10s 发放一次令牌，每次发放 2 个令牌，所以在一个发放周期内，最多可以处理 4 个请求，至少可以处理 2 个请求
            limiterOptions.AddTokenBucketLimiter(policyName: RateLimiterPolicy.TokenBucket, tokenBucketOptions =>
            {
                // 桶最多可以装的令牌数，发放的多余令牌会被丢弃。这里设置为最多装 4 个令牌。该值必须 > 0
                tokenBucketOptions.TokenLimit = 4;
                // 令牌发放周期，即多长时间发放一次令牌。这里设置为 10 s。该值必须 > TimeSpan.Zero
                tokenBucketOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                // 每个周期发放的令牌数，即每个周期向桶内放入的令牌数（若超过桶可装令牌数的最大值，则会被丢弃）。这里设置为 2 个。该值必须 > 0
                tokenBucketOptions.TokensPerPeriod = 2;
                // 队列中最多允许请求排队数，设置为 0 时取消排队
                tokenBucketOptions.QueueLimit = 0;
                // 排队请求的处理顺序。这里设置为优先处理先来的请求
                tokenBucketOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                // 指示当进入新的令牌发放周期时，是否自动发放令牌，该值默认为true。如果设置为false，则需要手动调用 TokenBucketRateLimiter.TryReplenish来发放
                tokenBucketOptions.AutoReplenishment = true;
            });
            #endregion

            #region 配置限流策略 => 并发限流器
            // 配置限流策略 => 并发限流器
            // 最多可以并发4个请求被处理。
            limiterOptions.AddConcurrencyLimiter(policyName: RateLimiterPolicy.Concurrency, concurrencyOptions =>
            {
                // 最多并发的请求数。该值必须 > 0
                concurrencyOptions.PermitLimit = 4;
                // 队列中最多允许请求排队数，设置为 0 时取消排队
                concurrencyOptions.QueueLimit = 0;
                // 排队请求的处理顺序。这里设置为优先处理先来的请求
                concurrencyOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            #endregion

            #region 自定义限流策略 = > 根据登录账号限流
            // 自定义限流策略 = > 根据登录账号限流
            limiterOptions.AddPolicy(policyName: RateLimiterPolicy.CustomByUserId, httpContext =>
            {
                // 匿名用户
                var userId = "anonymous user";

                // 判断是否已认证
                if (XunetHttpContextAccessor.Current?.User.Identity?.IsAuthenticated is true)
                {
                    userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                }

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromSeconds(60),
                    QueueLimit = 0
                });
            });
            #endregion

            #region 自定义限流策略 = > 根据IP限流
            // 自定义限流策略 = > 根据IP限流
            limiterOptions.AddPolicy(policyName: RateLimiterPolicy.CustomByIP, context =>
            {
                var requestIP = XunetHttpContextAccessor.RequestIP;

                var requestPath = context.Request.Path;

                var requestMethod = context.Request.Method;

                var clientKey = $"{requestIP}.{requestPath}.{requestMethod}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: clientKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
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

    #region 添加认证授权

    /// <summary>
    /// 添加认证授权
    /// </summary>
    /// <typeparam name="THandler">AuthorizationHandler</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetAuthentication<THandler>(this IServiceCollection services) where THandler : AuthorizationHandler<PermissionRequirement>
    {
        if (services.HasRegistered(nameof(AddXunetAuthentication))) return services;

        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.Configure<JwtConfig>(config.GetSection("JwtConfig"));

        var jwtConfig = config.GetSection("JwtConfig").Get<JwtConfig>() ?? throw new ConfigurationErrorsException("缺少配置节点：JwtConfig");

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

        services.AddScoped<IAuthorizationHandler, THandler>();

        services.AddAuthorizationBuilder().AddPolicy(AuthorizePolicy.Default, policy =>
        {
            policy.Requirements.Add(new PermissionRequirement());
        });

        return services;
    }

    #endregion

    #region 添加Swagger

    /// <summary>
    /// 添加Swagger
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetSwagger(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetSwagger))) return services;

        services.AddSwaggerGen(x =>
        {
            x.OrderActionsBy(x => x.GroupName);
            x.OperationFilter<OperationFilter>();
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"Minimal API 接口服务",
                Description = "Minimal API 接口服务",
                Version = $"v{Assembly.GetEntryAssembly()?.GetName().Version?.ToString()}",
            });
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
        foreach (var assembly in Assembly.GetEntryAssembly()?.GetReferencedAssemblies() ?? [])
        {
            services.AddValidatorsFromAssembly(Assembly.Load(assembly));
        }

        return services;
    }

    #endregion
}
