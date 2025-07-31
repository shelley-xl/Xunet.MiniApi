// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.WeixinMp;

using WeixinMp.Dtos;
using WeixinMp.Dtos.Requests;

/// <summary>
/// 微信公众号服务实现
/// </summary>
internal class WeixinMpService : IWeixinMpService
{
    #region 构造函数

    internal string? AppId { get; private set; }
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
    public WeixinMpService(HttpClient client, IConfiguration config, IXunetCache cache)
    {
        _client = client;
        _config = config;
        _cache = cache;

        _client.BaseAddress = new Uri("https://api.weixin.qq.com");

        AppId = _config["WeixinMpSettings:AppId"];
        AppSecret = _config["WeixinMpSettings:AppSecret"];
    }

    #endregion

    #region 获取接口调用凭据

    /// <summary>
    /// 获取接口调用凭据
    /// </summary>
    /// <param name="request">获取接口调用凭据请求</param>
    /// <returns></returns>
    public async Task<GetAccessTokenDto> GetAccessTokenAsync(GetAccessTokenRequest? request = null)
    {
        if (request != null)
        {
            AppId = request.AppId;
            AppSecret = request.AppSecret;
        }

        // 缓存存在，直接返回
        var result = await _cache.GetCacheAsync<GetAccessTokenDto>($"{AppId}_access_token");

        if (result != null) return result;

        var response = await _client.GetAsync($"/cgi-bin/token?grant_type=client_credential&appid={AppId}&secret={AppSecret}");

        result = await response.Content.ReadFromJsonAsync<GetAccessTokenDto>() ?? default!;

        // 缓存access_token
        if (result.ErrCode == 0 && result.ExpiresIn.HasValue)
        {
            await _cache.SetCacheAsync($"{AppId}_access_token", result, TimeSpan.FromSeconds(result.ExpiresIn.Value - 300));
        }

        return result;
    }

    #endregion

    #region 网页授权

    /// <summary>
    /// 网页授权
    /// </summary>
    /// <param name="request">网页授权请求</param>
    /// <returns></returns>
    public async Task<WeixinLoginDto> WeixinLoginAsync(WeixinLoginRequest request)
    {
        var response = await _client.GetAsync($"/sns/oauth2/access_token?appid={AppId}&secret={AppSecret}&code={request.Code}&grant_type=authorization_code");

        return await response.Content.ReadFromJsonAsync<WeixinLoginDto>() ?? default!;
    }

    #endregion

    #region 获取用户信息

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    public async Task<GetUserInfoDto> GetUserInfoAsync(GetUserInfoRequest request)
    {
        var response = await _client.GetAsync($"/cgi-bin/user/info?access_token={request.AccessToken}&openid={request.OpenId}&lang={request.Lang}");

        return await response.Content.ReadFromJsonAsync<GetUserInfoDto>() ?? default!;
    }

    #endregion

    #region 发送模板消息

    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="request">发送模版消息请求</param>
    /// <returns></returns>
    public async Task<SendTemplateMessageDto> SendTemplateMessageAsync(SendTemplateMessageRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cgi-bin/message/template/send?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<SendTemplateMessageDto>() ?? default!;
    }

    #endregion
}
