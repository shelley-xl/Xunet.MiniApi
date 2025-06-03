// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Weixin.Dtos;

/// <summary>
/// 网页授权返回
/// </summary>
public class GetWeixinTokenDto : WeixinErrorDto
{
    /// <summary>
    /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// access_token接口调用凭证超时时间，单位（秒）
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    /// <summary>
    /// 用户刷新access_token
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
    /// </summary>
    [JsonPropertyName("openid")]
    public string? OpenId { get; set; }

    /// <summary>
    /// 用户授权的作用域，使用逗号（,）分隔
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    /// <summary>
    /// 是否为快照页模式虚拟账号，只有当用户是快照页模式虚拟账号时返回，值为1
    /// </summary>
    [JsonPropertyName("is_snapshotuser")]
    public int? IsSnapshotUser { get; set; }

    /// <summary>
    /// 用户统一标识（针对一个微信开放平台账号下的应用，同一用户的 unionid 是唯一的），只有当scope为"snsapi_userinfo"时返回
    /// </summary>
    [JsonPropertyName("unionid")]
    public string? UnionId { get; set; }
}
