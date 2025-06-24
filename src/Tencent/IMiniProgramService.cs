// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent;

using MiniProgram.Dtos;
using MiniProgram.Dtos.Requests;

/// <summary>
/// 微信小程序服务
/// </summary>
public interface IMiniProgramService
{
    /// <summary>
    /// 获取接口调用凭据
    /// </summary>
    /// <param name="request">获取接口调用凭据请求</param>
    /// <returns></returns>
    Task<GetAccessTokenDto> GetAccessTokenAsync(GetAccessTokenRequest? request = null);

    /// <summary>
    /// 小程序登录
    /// </summary>
    /// <param name="request">小程序登录请求</param>
    /// <returns></returns>
    Task<WeixinLoginDto> WeixinLoginAsync(WeixinLoginRequest request);

    /// <summary>
    /// 创建小程序码（限制数量100,000）
    /// </summary>
    /// <param name="request">创建小程序码请求（限制数量100,000）</param>
    /// <returns></returns>
    Task<CreateQRCodeDto> CreateQRCodeAsync(CreateQRCodeRequest request);

    /// <summary>
    /// 创建小程序码（限制数量100,000且已发布）
    /// </summary>
    /// <param name="request">创建小程序码请求（限制数量100,000且已发布）</param>
    /// <returns></returns>
    Task<CreateQRCodeDto> CreateQRCodeAsync(CreateWxQRCodeRequest request);

    /// <summary>
    /// 创建小程序码（不限制）
    /// </summary>
    /// <param name="request">创建小程序码请求（不限制）</param>
    /// <returns></returns>
    Task<CreateQRCodeDto> CreateQRCodeAsync(CreateUnlimitedQRCodeRequest request);

    /// <summary>
    /// 获取手机号
    /// </summary>
    /// <param name="request">获取手机号请求</param>
    /// <returns></returns>
    Task<GetPhoneNumberDto> GetPhoneNumberAsync(GetPhoneNumberRequest request);

    /// <summary>
    /// 发送订阅消息
    /// </summary>
    /// <param name="request">发送订阅消息请求</param>
    /// <returns></returns>
    Task<SendSubscribeMessageDto> SendSubscribeMessageAsync(SendSubscribeMessageRequest request);
}
