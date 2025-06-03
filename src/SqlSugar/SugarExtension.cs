// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

internal static class SugarExtension
{
    internal static IServiceCollection AddSqlSugarClient(this IServiceCollection services, StorageOptions[]? options = null, DbType dbType = DbType.MySql)
    {
        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            var configs = new List<ConnectionConfig>();
            if (options == null || options.Length == 0)
            {
                if (dbType == DbType.Sqlite)
                {
                    var dbVersion = "1.0.1.2";
                    var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MiniApiAssembly.AssemblyName, dbVersion);
                    var dataDir = Path.Combine(baseDir, "data");
                    options =
                    [
                        new()
                        {
                            ConfigId = DbType.Sqlite,
                            ConnectionString = $"Data Source={dataDir}\\{MiniApiAssembly.EntryAssemblyName}.db;",
                        }
                    ];
                }
                else
                {
                    var config = provider.GetRequiredService<IConfiguration>();
                    var ops = config.GetSection("StorageOptions").Get<StorageOptions[]?>()?.Select(x => new StorageOptions
                    {
                        ConfigId = x.ConfigId,
                        ConnectionString = config.GetConnectionString(x.ConnectionString!)
                    });
                    options = ops?.ToArray();
                    if (options == null || options.Length == 0)
                    {
                        var connectionString = config.GetConnectionString("DefaultConnection");
                        if (string.IsNullOrEmpty(connectionString)) throw new ConfigurationException("未配置默认连接字符串");
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

            }

            foreach (var item in options)
            {
                configs.Add(new ConnectionConfig
                {
                    ConfigId = item.ConfigId,
                    DbType = dbType,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = item.ConnectionString,
                });
            }

            using var db = new SqlSugarScope(configs);

            db.Ado.CommandTimeOut = 60;

            return db;
        });

        return services;
    }
}
