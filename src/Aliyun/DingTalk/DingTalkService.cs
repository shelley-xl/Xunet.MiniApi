// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.DingTalk;

using DingTalk.Dtos;
using DingTalk.Dtos.Requests;

/// <summary>
/// 钉钉服务
/// </summary>
internal class DingTalkService : IDingTalkService
{
    #region 构造函数

    internal string? AppKey { get; private set; }
    internal string? AppSecret { get; private set; }

    readonly HttpClient _client;
    readonly IConfiguration _config;
    readonly IXunetCache _cache;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    /// <param name="cache"></param>
    public DingTalkService(HttpClient client, IConfiguration config, IXunetCache cache)
    {
        _client = client;
        _config = config;
        _cache = cache;

        _client.BaseAddress = new Uri("https://oapi.dingtalk.com");

        AppKey = _config["DingTalkSettings:AppKey"];
        AppSecret = _config["DingTalkSettings:AppSecret"];
    }

    #endregion

    #region 获取接口调用凭据（企业内部应用）

    /// <summary>
    /// 获取接口调用凭据（企业内部应用）
    /// </summary>
    /// <param name="request">获取接口调用凭据请求</param>
    /// <returns></returns>
    public async Task<GetAccessTokenDto> GetAccessTokenAsync(GetAccessTokenRequest? request = null)
    {
        if (request != null)
        {
            AppKey = request.AppKey;
            AppSecret = request.AppSecret;
        }

        // 缓存存在，直接返回
        var result = await _cache.GetCacheAsync<GetAccessTokenDto>($"{AppKey}_access_token");

        if (result != null) return result;

        var response = await _client.GetAsync($"/gettoken?appkey={AppKey}&appsecret={AppSecret}");

        result = await response.Content.ReadFromJsonAsync<GetAccessTokenDto>() ?? default!;

        // 缓存access_token
        if (result.ErrCode == 0 && result.ExpiresIn.HasValue)
        {
            await _cache.SetCacheAsync($"{AppKey}_access_token", result, TimeSpan.FromSeconds(result.ExpiresIn.Value - 300));
        }

        return result;
    }

    #endregion

    #region 小程序登录（企业内部应用）

    /// <summary>
    /// 小程序登录（企业内部应用）
    /// </summary>
    /// <param name="request">小程序登录请求</param>
    /// <returns></returns>
    public async Task<DingTalkLoginDto> DingTalkLoginAsync(DingTalkLoginRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/topapi/v2/user/getuserinfo?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<DingTalkLoginDto>() ?? default!;
    }

    #endregion

    #region 获取用户信息（企业内部应用）

    /// <summary>
    /// 获取用户信息（企业内部应用）
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    public async Task<GetUserInfoDto> GetUserInfoAsync(GetUserInfoRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/topapi/v2/user/get?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<GetUserInfoDto>() ?? default!;
    }

    #endregion
}
