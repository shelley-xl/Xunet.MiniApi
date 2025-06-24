// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos.Requests;

/// <summary>
/// 获取接口调用凭据请求
/// </summary>
public class GetAccessTokenRequest
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
