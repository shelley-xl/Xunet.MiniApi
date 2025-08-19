// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun;

using DingTalk.Dtos;
using DingTalk.Dtos.Requests;

/// <summary>
/// 钉钉服务
/// </summary>
public interface IDingTalkService
{
    /// <summary>
    /// 获取接口调用凭据（企业内部应用）
    /// </summary>
    /// <param name="request">获取接口调用凭据请求</param>
    /// <returns></returns>
    Task<GetAccessTokenDto> GetAccessTokenAsync(GetAccessTokenRequest? request = null);

    /// <summary>
    /// 小程序登录（企业内部应用）
    /// </summary>
    /// <param name="request">小程序登录请求</param>
    /// <returns></returns>
    Task<DingTalkLoginDto> DingTalkLoginAsync(DingTalkLoginRequest request);

    /// <summary>
    /// 获取用户信息（企业内部应用）
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    Task<GetUserInfoDto> GetUserInfoAsync(GetUserInfoRequest request);
}
