// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// 存储选项
/// </summary>
public class StorageOptions
{
    /// <summary>
    /// 数据库配置Id
    /// </summary>
    public object? ConfigId { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// 连接字符串（只读实例）
    /// </summary>
    public List<SlaveConnectionConfig>? SlaveConnectionString { get; set; }
}
