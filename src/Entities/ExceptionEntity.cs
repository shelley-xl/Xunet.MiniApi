namespace Xunet.MiniApi.Entities;

/// <summary>
/// 异常日志
/// </summary>
[SugarTable("exception", "异常日志")]
internal class ExceptionEntity : SugarEntity
{
    /// <summary>
    /// 消息
    /// </summary>
    [SugarColumn(ColumnDescription = "消息", ColumnDataType = "text")]
    public string? Message { get; set; }

    /// <summary>
    /// 堆栈信息
    /// </summary>
    [SugarColumn(ColumnDescription = "堆栈信息", ColumnDataType = "text")]
    public string? StackTrace { get; set; }

    /// <summary>
    /// 内部信息
    /// </summary>
    [SugarColumn(ColumnDescription = "内部信息", ColumnDataType = "text", IsNullable = true)]
    public string? InnerException { get; set; }

    /// <summary>
    /// 异常类型
    /// </summary>
    [SugarColumn(ColumnDescription = "异常类型", Length = 100)]
    public string? ExceptionType { get; set; }

    /// <summary>
    /// 请求IP
    /// </summary>
    [SugarColumn(ColumnDescription = "请求IP", Length = 50)]
    public string? RequestIP { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    [SugarColumn(ColumnDescription = "请求路径", Length = 100)]
    public string? RequestPath { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    [SugarColumn(ColumnDescription = "请求方法", Length = 10)]
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 请求参数QueryString
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数QueryString", ColumnDataType = "text", IsNullable = true)]
    public string? RequestQuery { get; set; }

    /// <summary>
    /// 请求参数Body
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数Body", ColumnDataType = "text", IsNullable = true)]
    public string? RequestBody { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    [SugarColumn(ColumnDescription = "浏览器信息", ColumnDataType = "text")]
    public string? UserAgent { get; set; }
}
