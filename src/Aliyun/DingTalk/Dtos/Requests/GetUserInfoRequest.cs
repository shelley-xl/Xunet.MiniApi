// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.DingTalk.Dtos.Requests;

/// <summary>
/// 获取用户信息请求
/// </summary>
public class GetUserInfoRequest
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [JsonPropertyName("userid")]
    public string? UserId { get; set; }

    /// <summary>
    /// 通讯录语言（zh_CN：中文，en_US：英文）
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// 接口调用凭证
    /// </summary>
    [JsonIgnore]
    public string? AccessToken { get; set; }
}
