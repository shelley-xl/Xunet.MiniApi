// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SkiaSharp.Captcha;

/// <summary>
/// 图形验证码实现
/// </summary>
internal class XunetCaptcha(IXunetCache cache) : IXunetCaptcha
{
    /// <summary>
    /// 生成图形验证码
    /// </summary>
    /// <returns></returns>
    public async Task<CaptchaDto> GenerateAsync()
    {
        var captcha = new CaptchaHelper(4)
        {
            SetFontSize = 30
        };

        var bytes = captcha.GetVerifyCodeImage();

        var dto = new CaptchaDto
        {
            Uuid = Guid.NewGuid().ToString(),
            Base64 = $"data:image/jpg;base64,{Convert.ToBase64String(bytes)}",
        };

        var cacheKey = $"captcha_{dto.Uuid}";

        await cache.SetCacheAsync(cacheKey, captcha.SetVerifyCodeText, TimeSpan.FromSeconds(300));

        return dto;
    }

    /// <summary>
    /// 校验图形验证码
    /// </summary>
    /// <param name="uuid">图形验证码唯一标识</param>
    /// <param name="code">图形验证码</param>
    /// <returns></returns>
    public async Task<bool?> ValidateAsync(string? uuid, string? code)
    {
        var cacheKey = $"captcha_{uuid}";

        var cacheCode = await cache.GetCacheAsync<string>(cacheKey);

        if (cacheCode == null) return null;

        if (!cacheCode.Equals(code, StringComparison.OrdinalIgnoreCase)) return false;

        await cache.RemoveCacheAsync(cacheKey);

        return true;
    }
}
