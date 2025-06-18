// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos.Requests;

/// <summary>
/// 获取手机号请求
/// </summary>
public class GetPhoneNumberRequest
{
    /// <summary>
    /// 手机号获取凭证
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// 用户唯一标识
    /// </summary>
    [JsonPropertyName("openid")]
    public string? OpenId { get; set; }

    /// <summary>
    /// 接口调用凭证
    /// </summary>
    [JsonIgnore]
    public string? AccessToken { get; set; }
}
