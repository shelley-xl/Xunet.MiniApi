// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos;

/// <summary>
/// 小程序登录返回
/// </summary>
public class WeixinLoginDto : ErrorDto
{
    /// <summary>
    /// 会话密钥
    /// </summary>
    [JsonPropertyName("session_key")]
    public string? SessionKey { get; set; }

    /// <summary>
    /// 用户唯一标识
    /// </summary>
    [JsonPropertyName("openid")]
    public string? OpenId { get; set; }

    /// <summary>
    /// 用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台帐号下会返回
    /// </summary>
    [JsonPropertyName("unionid")]
    public string? UnionId { get; set; }
}
