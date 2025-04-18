namespace Xunet.MiniApi.Dtos;

/// <summary>
/// 数据操作响应
/// </summary>
public class OperateResultDto
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public OperateResultDto()
    {
        Code = XunetCode.Success;
        Message = "ok";
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        RequestId = XunetHttpContextAccessor.Current?.TraceIdentifier;
        TraceId = XunetHttpContextAccessor.Current?.Features.Get<IHttpActivityFeature>()?.Activity.TraceId.ToHexString();
        Instance = XunetHttpContextAccessor.Current?.Request.Path;
        if (XunetHttpContextAccessor.Current?.Items["StartTime"] is Stopwatch stopwatch)
        {
            stopwatch.Stop();
            Duration = $"{stopwatch.ElapsedMilliseconds}ms";
        }
        else
        {
            Duration = "0ms";
        }
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public XunetCode? Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long? Timestamp { get; private set; }

    /// <summary>
    /// 请求Id
    /// </summary>
    public string? RequestId { get; private set; }

    /// <summary>
    /// 跟踪Id
    /// </summary>
    public string? TraceId { get; private set; }

    /// <summary>
    /// 请求实例
    /// </summary>
    public string? Instance { get; private set; }

    /// <summary>
    /// 响应时长
    /// </summary>
    public string? Duration { get; private set; }
}
