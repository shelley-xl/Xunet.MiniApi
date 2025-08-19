// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.DingTalk.Dtos.Requests;

/// <summary>
/// 小程序登录请求
/// </summary>
public class DingTalkLoginRequest
{
    /// <summary>
    /// 授权码
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// 接口调用凭证
    /// </summary>
    [JsonIgnore]
    public string? AccessToken { get; set; }
}
