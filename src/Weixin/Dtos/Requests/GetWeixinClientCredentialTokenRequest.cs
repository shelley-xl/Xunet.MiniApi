// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Weixin.Dtos.Requests;

/// <summary>
/// 获取微信客户端凭证
/// </summary>
public class GetWeixinClientCredentialTokenRequest
{
    /// <summary>
    /// AppId
    /// </summary>
    public string? AppId { get; set; }

    /// <summary>
    /// AppSecret
    /// </summary>
    public string? AppSecret { get; set; }
}
