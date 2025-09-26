// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Interfaces;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// 获取图形验证码
    /// </summary>
    /// <returns></returns>
    Task<IResult> GetVeryCodeAsync();

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request">登录请求</param>
    /// <returns></returns>
    Task<IResult> LoginAsync(LoginRequest request);

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="request">注册请求</param>
    /// <returns></returns>
    Task<IResult> RegisterAsync(RegisterRequest request);
}
