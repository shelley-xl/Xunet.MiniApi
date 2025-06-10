// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.WeixinMp.Dtos.Requests;

/// <summary>
/// 获取用户信息请求
/// </summary>
public class GetWeixinUserInfoRequest
{
    /// <summary>
    /// access_token
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// 用户的唯一标识
    /// </summary>
    public string? OpenId { get; set; }

    /// <summary>
    /// 非必填，语言（zh_CN 简体，zh_TW 繁体，en 英语）
    /// </summary>
    public string? Lang { get; set; } = "zh_CN";
}
