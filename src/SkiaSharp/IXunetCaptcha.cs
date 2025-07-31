// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SkiaSharp;

/// <summary>
/// 图形验证码接口
/// </summary>
public interface IXunetCaptcha
{
    /// <summary>
    /// 生成图形验证码
    /// </summary>
    /// <returns></returns>
    Task<CaptchaDto> GenerateAsync();

    /// <summary>
    /// 校验图形验证码
    /// </summary>
    /// <param name="uuid">图形验证码唯一标识</param>
    /// <param name="code">图形验证码</param>
    /// <returns></returns>
    Task<bool?> ValidateAsync(string? uuid, string? code);
}
