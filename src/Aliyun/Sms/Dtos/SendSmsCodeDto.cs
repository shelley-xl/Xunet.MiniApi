// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.Sms.Dtos;

/// <summary>
/// 发送短信验证码返回
/// </summary>
public class SendSmsCodeDto
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool? Success { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? Message { get; set; }
}
