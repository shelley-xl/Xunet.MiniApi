// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent;

using Sms.Dtos;
using Sms.Dtos.Requests;

/// <summary>
/// 腾讯云短信服务
/// </summary>
public interface ITencentCloudSmsService
{
    /// <summary>
    /// 发送短信验证码
    /// </summary>
    /// <param name="request">发送短信验证码请求</param>
    /// <returns></returns>
    Task<List<SendSmsCodeDto>> SendSmsAsync(SendSmsCodeRequest request);
}
