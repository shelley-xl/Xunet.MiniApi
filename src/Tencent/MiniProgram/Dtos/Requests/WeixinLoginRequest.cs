// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos.Requests;

/// <summary>
/// 小程序登录请求
/// </summary>
public class WeixinLoginRequest
{
    /// <summary>
    /// 授权码
    /// </summary>
    public string? Code { get; set; }
}
