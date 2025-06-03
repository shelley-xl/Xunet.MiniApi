// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Weixin;

/// <summary>
/// 微信服务实现
/// </summary>
public class WeixinService : IWeixinService
{
    readonly HttpClient _client;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="client"></param>
    public WeixinService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://api.weixin.qq.com");
    }

    /// <summary>
    /// 网页授权
    /// </summary>
    /// <param name="request">网页授权请求</param>
    /// <returns></returns>
    public async Task<GetWeixinTokenDto> GetWeixinTokenAsync(GetWeixinTokenRequest request)
    {
        var response = await _client.GetAsync($"/sns/oauth2/access_token?appid={request.AppId}&secret={request.AppSecret}&code={request.Code}&grant_type=authorization_code");

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<GetWeixinTokenDto>();

        return dto!;
    }

    /// <summary>
    /// 获取用户信息（scope为snsapi_userinfo）
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    public async Task<GetWeixinSnsUserInfoDto> GetWeixinUserInfoAsync(GetWeixinSnsUserInfoRequest request)
    {
        var response = await _client.GetAsync($"/sns/userinfo?access_token={request.AccessToken}&openid={request.OpenId}&lang={request.Lang}");

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<GetWeixinSnsUserInfoDto>();

        return dto!;
    }

    /// <summary>
    /// 获取微信客户端凭证
    /// </summary>
    /// <param name="request">获取微信客户端凭证请求</param>
    /// <returns></returns>
    public async Task<GetWeixinClientCredentialTokenDto> GetWeixinClientCredentialTokenAsync(GetWeixinClientCredentialTokenRequest request)
    {
        var response = await _client.GetAsync($"/cgi-bin/token?grant_type=client_credential&appid={request.AppId}&secret={request.AppSecret}");

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<GetWeixinClientCredentialTokenDto>();

        return dto!;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="request">获取用户信息请求</param>
    /// <returns></returns>
    public async Task<GetWeixinUserInfoDto> GetWeixinUserInfoAsync(GetWeixinUserInfoRequest request)
    {
        var response = await _client.GetAsync($"/cgi-bin/user/info?access_token={request.AccessToken}&openid={request.OpenId}&lang={request.Lang}");

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<GetWeixinUserInfoDto>();

        return dto!;
    }

    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="request">发送模版消息请求</param>
    /// <returns></returns>
    public async Task<SendTemplateMessageDto> SendTemplateMessageAsync(SendTemplateMessageRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/cgi-bin/message/template/send?access_token={request.AccessToken}", content);

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<SendTemplateMessageDto>();

        return dto!;
    }
}
