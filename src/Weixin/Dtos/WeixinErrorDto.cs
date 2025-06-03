// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Weixin.Dtos;

/// <summary>
/// 错误信息返回
/// </summary>
public class WeixinErrorDto
{
    /// <summary>
    /// 错误码
    /// </summary>
    [JsonPropertyName("errcode")]
    public string? ErrCode { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonPropertyName("errmsg")]
    public string? ErrMsg { get; set; }
}
