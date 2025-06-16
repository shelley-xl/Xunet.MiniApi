// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

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
        if (useCacheService) services.AddSingleton<ICacheService, SugarCacheService>();

        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            if (options == null || options.Length == 0)
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
                    var connectionString = string.Empty;
                    if (dbType == DbType.Sqlite)
                    {
                        var dbVersion = "1.2.1.8";
                        var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MiniApiAssembly.AssemblyName, dbVersion);
                        var dataDir = Path.Combine(baseDir, "data");
                        connectionString = $"Data Source={dataDir}\\{MiniApiAssembly.EntryAssemblyName}.db;";
                    }
                    else
                    {
                        connectionString = config.GetConnectionString("DefaultConnection");
                        if (string.IsNullOrEmpty(connectionString)) throw new ConfigurationException("未配置默认连接字符串");
                    }
                    options =
                    [
                        new()
                        {
                            ConfigId = (int)dbType,
                            ConnectionString = connectionString,
                        }
                    ];
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

            var eventHandler = provider.GetRequiredService<ISqlLogEventHandler>();
            var watch = new Stopwatch();
            var paras = new List<KeyValuePair<string, object>>();

            return new SqlSugarScope(configs, db =>
            {
                // 添加表过滤器 => 假删除
                db.QueryFilter.AddTableFilter<IDeleted>(x => x.IsDelete == false);
                db.Ado.CommandTimeOut = 60;
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // SQL执行之前事件
                    foreach (var item in pars)
                    {
                        paras.Add(new KeyValuePair<string, object>(item.ParameterName, item.Value));
                    }

                    watch.Start();
                };
                db.Aop.OnLogExecuted = (sql, pars) =>
                {
                    // SQL执行完成事件
                    watch.Stop();

                    try
                    {
                        eventHandler?.InvokeAsync(sql, paras, watch.ElapsedMilliseconds);
                    }
                    finally
                    {
                        watch.Reset();
                        paras.Clear();
                    }
                };
                db.Aop.OnError = (exp) =>
                {
                    // 执行SQL错误事件
                    watch.Stop();

                    try
                    {
                        eventHandler?.InvokeAsync(exp.Sql, paras, watch.ElapsedMilliseconds, true, exp.Message);
                    }
                    finally
                    {
                        watch.Reset();
                        paras.Clear();
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
}
