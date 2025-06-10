// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.Sms.Dtos.Requests;

/// <summary>
/// 发送短信验证码请求
/// </summary>
public class SendSmsCodeRequest
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string[] PhoneNumber { get; set; } = [];
}
