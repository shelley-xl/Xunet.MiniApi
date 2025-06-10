// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent;

/// <summary>
/// 微信公众号服务
/// </summary>
public interface IWeixinMpService
{
    /// <summary>
    /// 网页授权
    /// </summary>
    /// <param name="request">网页授权请求</param>
    /// <returns></returns>
    Task<GetWeixinTokenDto> GetWeixinTokenAsync(GetWeixinTokenRequest request);

    /// <summary>
    /// 获取用户信息（scope为snsapi_userinfo）
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    Task<GetWeixinSnsUserInfoDto> GetWeixinUserInfoAsync(GetWeixinSnsUserInfoRequest request);

    /// <summary>
    /// 获取微信客户端凭证
    /// </summary>
    /// <returns></returns>
    Task<GetWeixinClientCredentialTokenDto> GetWeixinClientCredentialTokenAsync();

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    Task<GetWeixinUserInfoDto> GetWeixinUserInfoAsync(GetWeixinUserInfoRequest request);

    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="request">发送模版消息请求</param>
    /// <returns></returns>
    Task<SendTemplateMessageDto> SendTemplateMessageAsync(SendTemplateMessageRequest request);
}
