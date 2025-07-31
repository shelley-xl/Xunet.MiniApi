// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SkiaSharp.Captcha.Dto;

/// <summary>
/// 图形验证码返回
/// </summary>
public class CaptchaDto
{
    /// <summary>
    /// 图形验证码唯一标识
    /// </summary>
    public string? Uuid { get; set; }

    /// <summary>
    /// 图形验证码base64
    /// </summary>
    public string? Base64 { get; set; }
}
