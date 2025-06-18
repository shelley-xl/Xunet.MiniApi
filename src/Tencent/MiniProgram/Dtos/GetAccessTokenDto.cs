// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos;

/// <summary>
/// 获取接口调用凭据返回
/// </summary>
public class GetAccessTokenDto : ErrorDto
{
    /// <summary>
    /// 获取到的凭证
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// 凭证有效时间，单位：秒
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }
}
