// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram;

using Dtos;
using Dtos.Requests;

/// <summary>
/// 微信小程序服务实现
/// </summary>
internal class MiniProgramService : IMiniProgramService
{
    #region 构造函数

    internal string? AppId { get; private set; }
    internal string? AppSecret { get; private set; }

    readonly HttpClient _client;
    readonly IConfiguration _config;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    public MiniProgramService(HttpClient client, IConfiguration config)
    {
        _client = client;
        _config = config;

        _client.BaseAddress = new Uri("https://api.weixin.qq.com");

        AppId = _config["MiniProgramSettings:AppId"];
        AppSecret = _config["MiniProgramSettings:AppSecret"];
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
        GetAccessTokenDto? result;

        if (request != null)
        {
            AppId = request.AppId;
            AppSecret = request.AppSecret;
        }

        // 查找缓存服务，缓存存在，直接返回
        var cache = XunetHttpContext.GetService<IXunetCache>();
        if (cache != null)
        {
            result = await cache.GetCacheAsync<GetAccessTokenDto>($"{AppId}_access_token");
            if (result != null) return result;
        }

        var response = await _client.GetAsync($"/cgi-bin/token?grant_type=client_credential&appid={AppId}&secret={AppSecret}");

        result = await response.Content.ReadFromJsonAsync<GetAccessTokenDto>() ?? default!;

        // 如果使用缓存，缓存access_token
        if (cache != null && result.ExpiresIn.HasValue)
        {
            await cache.SetCacheAsync($"{AppId}_access_token", result, TimeSpan.FromSeconds(result.ExpiresIn.Value - 300));
        }

        return result;
    }

    #endregion

    #region 小程序登录

    /// <summary>
    /// 小程序登录
    /// </summary>
    /// <param name="request">小程序登录请求</param>
    /// <returns></returns>
    public async Task<WeixinLoginDto> WeixinLoginAsync(WeixinLoginRequest request)
    {
        var response = await _client.GetAsync($"/sns/jscode2session?appid={AppId}&secret={AppSecret}&js_code={request.Code}&grant_type=authorization_code");

        return await response.Content.ReadFromJsonAsync<WeixinLoginDto>() ?? default!;
    }

    #endregion

    #region 创建小程序码（限制数量100,000）

    /// <summary>
    /// 创建小程序码（限制数量100,000）
    /// </summary>
    /// <param name="request">创建小程序码请求（限制数量100,000）</param>
    /// <returns></returns>
    public async Task<CreateQRCodeDto> CreateQRCodeAsync(CreateQRCodeRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/wxa/getwxacode?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<CreateQRCodeDto>() ?? default!;
    }

    #endregion

    #region 创建小程序码（限制数量100,000且已发布）

    /// <summary>
    /// 创建小程序码（限制数量100,000且已发布）
    /// </summary>
    /// <param name="request">创建小程序码请求（限制数量100,000且已发布）</param>
    /// <returns></returns>
    public async Task<CreateQRCodeDto> CreateQRCodeAsync(CreateWxQRCodeRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cgi-bin/wxaapp/createwxaqrcode?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<CreateQRCodeDto>() ?? default!;
    }

    #endregion

    #region 创建小程序码（不限制）

    /// <summary>
    /// 创建小程序码（不限制）
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<CreateQRCodeDto> CreateQRCodeAsync(CreateUnlimitedQRCodeRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/wxa/getwxacodeunlimit?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<CreateQRCodeDto>() ?? default!;
    }

    #endregion

    #region 获取手机号

    /// <summary>
    /// 获取手机号
    /// </summary>
    /// <param name="request">获取手机号请求</param>
    /// <returns></returns>
    public async Task<GetPhoneNumberDto> GetPhoneNumberAsync(GetPhoneNumberRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/wxa/business/getuserphonenumber?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<GetPhoneNumberDto>() ?? default!;
    }

    #endregion

    #region 发送订阅消息

    /// <summary>
    /// 发送订阅消息
    /// </summary>
    /// <param name="request">发送订阅消息请求</param>
    /// <returns></returns>
    public async Task<SendSubscribeMessageDto> SendSubscribeMessageAsync(SendSubscribeMessageRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cgi-bin/message/subscribe/send?access_token={request.AccessToken}", content);

        return await response.Content.ReadFromJsonAsync<SendSubscribeMessageDto>() ?? default!;
    }

    #endregion
}
