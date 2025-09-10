// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

// 别名: 与RabbitMQ冲突
using ConnectionConfig = SqlSugar.ConnectionConfig;

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// SqlSugar扩展
/// </summary>
public static class SugarExtension
{
    /// <summary>
    /// 转where条件
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static List<IConditionalModel> ToConditions(this object request)
    {
        var conditions = new List<IConditionalModel>();

        foreach (var attribute in request.GetType().GetCustomAttributes<QueryFilterAttribute>().ToList())
        {
            var collections = new ConditionalCollections
            {
                ConditionalList = []
            };
            collections.ConditionalList.Add(new KeyValuePair<WhereType, ConditionalModel>(attribute.WhereType, new ConditionalModel
            {
                FieldName = attribute.FieldName,
                ConditionalType = attribute.ConditionType,
                FieldValue = attribute.FieldValue,
            }));
            conditions.Add(collections);
        }

        var types = request.GetType().GetProperties().Where(x => x.GetCustomAttributes<QueryConditionAttribute>().Any());

        foreach (var type in types)
        {
            var value = type.GetValue(request)?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                var attributes = type.GetCustomAttributes<QueryConditionAttribute>().ToList();
                var collections = new ConditionalCollections
                {
                    ConditionalList = [],
                };
                foreach (var attribute in attributes)
                {
                    collections.ConditionalList.Add(new KeyValuePair<WhereType, ConditionalModel>(attribute.WhereType, new ConditionalModel
                    {
                        FieldName = attribute.FieldName,
                        ConditionalType = attribute.ConditionType,
                        FieldValue = value,
                    }));
                }
                conditions.Add(collections);
            }
        }

        return conditions;
    }

    internal static IServiceCollection AddSqlSugarClient(this IServiceCollection services, StorageOptions[]? options = null, DbType dbType = DbType.MySql, bool useCacheService = false)
    {
        var provider = services.BuildServiceProvider();

        if (options == null || options.Length == 0)
        {
            if (dbType == DbType.Sqlite)
            {
                var SQLITE_CONNECTIONSTRING = Environment.GetEnvironmentVariable("SQLITE_CONNECTIONSTRING");
                if (!string.IsNullOrEmpty(SQLITE_CONNECTIONSTRING))
                {
                    options = [new()
                    {
                        ConfigId = dbType.ToString(),
                        ConnectionString = SQLITE_CONNECTIONSTRING,
                    }];
                }
                else
                {
                    var dbVersion = "1.2.1.8";
                    var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MiniApiAssembly.AssemblyName, dbVersion);
                    var dataDir = Path.Combine(baseDir, "data");
                    var connectionString = $"Data Source={dataDir}\\{MiniApiAssembly.EntryAssemblyName}.db;";
                    options = [new()
                    {
                        ConfigId = dbType.ToString(),
                        ConnectionString = connectionString,
                    }];
                }
            }
            else
            {
                var config = provider.GetRequiredService<IConfiguration>();
                options = config.GetSection("StorageOptions").Get<StorageOptions[]?>()?
                .Select(x => new StorageOptions
                {
                    ConfigId = x.ConfigId,
                    ConnectionString = config.GetConnectionString(x.ConnectionString!),
                    SlaveConnectionString = x.SlaveConnectionString?.Select(y => new SlaveConnectionConfig
                    {
                        ConnectionString = config.GetConnectionString(y.ConnectionString!),
                        HitRate = y.HitRate,
                    })
                    .ToList()
                })
                .ToArray();
                if (options == null || options.Length == 0)
                {
                    var connectionString = config.GetConnectionString("DefaultConnection");
                    if (string.IsNullOrEmpty(connectionString)) throw new ConfigurationException("未配置默认连接字符串");
                    options = [new()
                    {
                        ConfigId = dbType.ToString(),
                        ConnectionString = connectionString,
                    }];
                }
            }
        }

        var configs = new List<ConnectionConfig>();

        foreach (var item in options)
        {
            configs.Add(new ConnectionConfig
            {
                ConfigId = item.ConfigId,
                DbType = dbType,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = item.ConnectionString,
                SlaveConnectionConfigs = item.SlaveConnectionString,
            });
        }

        var current = provider.GetService<ISqlSugarClient>();
        if (current != null)
        {
            var tenant = current.AsTenant();
            var tenantConfigs = tenant.GetCurrentConfigIds().Select(x => tenant.GetConnection(x).CurrentConnectionConfig);
            configs.InsertRange(0, tenantConfigs);

            // 移除当前ISqlSugarClient
            services.Remove(services.First(x => x.ServiceType == typeof(ISqlSugarClient)));
        }
        else
        {
            if (useCacheService) services.AddSingleton<ICacheService, SugarCacheService>();

            services.AddSugarDbContext();
        }

        // 添加ISqlSugarClient
        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            var watch = new Stopwatch();

            return new SqlSugarScope(configs, db =>
            {
                // 添加表过滤器 => 假删除
                db.QueryFilter.AddTableFilter<IDeleted>(x => x.IsDelete == false);
                db.Ado.CommandTimeOut = 60;
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // SQL执行之前事件
                    watch.Start();
                };
                db.Aop.OnLogExecuted = async (sql, pars) =>
                {
                    // SQL执行完成事件
                    watch.Stop();

                    try
                    {
                        await using var scope = XunetHttpContext.Current?.RequestServices.CreateAsyncScope();
                        if (scope == null) return;

                        foreach (var par in pars)
                        {
                            sql = sql.Replace(par.ParameterName, $"'{par.Value}'");
                        }

                        var eventHandler = scope.Value.ServiceProvider.GetService<ISqlLogEventHandler>();
                        if (eventHandler != null)
                        {
                            await eventHandler.InvokeAsync(sql, watch.ElapsedMilliseconds);
                        }

                        var connection = scope.Value.ServiceProvider.GetService<IConnection>();
                        if (connection == null) return;

                        // 发送到RabbitMQ消息队列
                        var queueName = "logs_sqls";
                        using var channel = await connection.CreateChannelAsync();
                        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                        var pairs = new Dictionary<string, object?>
                        {
                            { "Id", Guid.NewGuid().ToString() },
                            { "RequestId", XunetHttpContext.RequestId },
                            { "TraceId", XunetHttpContext.TraceId },
                            { "CommandText", sql },
                            { "Duration", watch.ElapsedMilliseconds },
                            { "IsFailed", 0 },
                            { "Message", "" },
                            { "Subject", (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name },
                            { "CreateId", XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject)},
                            { "CreateTime", DateTime.Now },
                        };
                        var message = pairs.SerializerObject();
                        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: Encoding.UTF8.GetBytes(message));
                    }
                    finally
                    {
                        watch.Reset();
                    }
                };
                db.Aop.OnError = async (exp) =>
                {
                    // 执行SQL错误事件
                    watch.Stop();

                    try
                    {
                        await using var scope = XunetHttpContext.Current?.RequestServices.CreateAsyncScope();
                        if (scope == null) return;

                        var sql = exp.Sql;
                        if (exp.Parametres is SugarParameter[] pars)
                        {
                            foreach (var par in pars)
                            {
                                sql = sql.Replace(par.ParameterName, $"'{par.Value}'");
                            }
                        }

                        var eventHandler = scope.Value.ServiceProvider.GetService<ISqlLogEventHandler>();
                        if (eventHandler != null)
                        {
                            await eventHandler.InvokeAsync(sql, watch.ElapsedMilliseconds, true, exp.Message);
                        }

                        var connection = scope.Value.ServiceProvider.GetService<IConnection>();
                        if (connection == null) return;

                        // 发送到RabbitMQ消息队列
                        var queueName = "logs_sqls";
                        using var channel = await connection.CreateChannelAsync();
                        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                        var pairs = new Dictionary<string, object?>
                        {
                            { "Id", Guid.NewGuid().ToString() },
                            { "RequestId", XunetHttpContext.RequestId },
                            { "TraceId", XunetHttpContext.TraceId },
                            { "CommandText", sql },
                            { "Duration", watch.ElapsedMilliseconds },
                            { "IsFailed", 1 },
                            { "Message", exp.Message },
                            { "Subject", (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name },
                            { "CreateId", XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject)},
                            { "CreateTime", DateTime.Now },
                        };
                        var message = pairs.SerializerObject();
                        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: Encoding.UTF8.GetBytes(message));
                    }
                    finally
                    {
                        watch.Reset();
                    }
                };
                db.Aop.DataExecuting = (oldValue, entityInfo) =>
                {
                    // 列级别事件：插入或更新的每个列都会进事件
                };
                db.Aop.DataExecuted = (value, entity) =>
                {
                    // 查询事件 （只有行级事件）
                };
                db.Aop.OnDiffLogEvent = it =>
                {
                    // 差异日志功能
                };
            });
        });

        return services;
    }

    internal static IServiceCollection AddSugarDbContext(this IServiceCollection services)
    {
        // 获取所有继承自 SugarDbContext 的类
        var types = MiniApiAssembly.GetAllReferencedAssemblies(x =>
        {
            return x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(SugarDbContext<>) && x != typeof(BaseRepository<>);
        });

        foreach (var type in types)
        {
            services.AddScoped(type);
        }

        return services;
    }
}
