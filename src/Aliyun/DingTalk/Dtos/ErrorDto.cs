// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.DingTalk.Dtos;

/// <summary>
/// 错误信息返回
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// 请求Id
    /// </summary>
    [JsonPropertyName("request_id")]
    public string? RequestId { get; set; }

    /// <summary>
    /// 错误码
    /// </summary>
    [JsonPropertyName("errcode")]
    public int? ErrCode { get; set; } = 0;

    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonPropertyName("errmsg")]
    public string? ErrMsg { get; set; } = "ok";
}
