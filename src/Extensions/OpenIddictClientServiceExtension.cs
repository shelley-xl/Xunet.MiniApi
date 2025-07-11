// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Extensions;

using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

/// <summary>
/// 认证中心客户端扩展类
/// </summary>
public static class OpenIddictClientServiceExtension
{
    static IConfiguration Configuration => XunetHttpContext.GetRequiredService<IConfiguration>();

    static string ClientId => Configuration["OpenIddictClient:ClientId"]!;

    static string ClientSecret => Configuration["OpenIddictClient:ClientSecret"]!;

    static JsonSerializerOptions JsonOptions
    {
        get
        {
            return new JsonSerializerOptions
            {
                // 不区分大小写
                PropertyNameCaseInsensitive = true,
                // 编码
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
        }
    }

    static HttpClient IdentityClient
    {
        get
        {
            var factory = XunetHttpContext.GetRequiredService<IHttpClientFactory>();

            var client = factory.CreateClient("identity-client");

            return client;
        }
    }

    static async Task<OpenIddictResponse?> RequestTokenAsync(List<KeyValuePair<string, string?>> parameters, IEnumerable<KeyValuePair<string, string?>>? pairs = null, CancellationToken cancellationToken = default)
    {
        if (pairs != null && pairs.Any())
        {
            parameters.AddRange(pairs);
        }

        var content = new FormUrlEncodedContent(parameters);

        var response = await IdentityClient.PostAsync("/connect/token", content, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<OpenIddictResponse>(JsonOptions, cancellationToken);

        return result;
    }

    /// <summary>
    /// 发送Get请求 
    /// </summary>
    /// <param name="_"></param>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<JsonNode?> RequestEndpointGetAsync(this OpenIddictClientService _, string path, CancellationToken cancellationToken = default)
    {
        var response = await IdentityClient.GetAsync(path, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<JsonNode?>(JsonOptions, cancellationToken);

        return result;
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="_"></param>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<JsonNode?> RequestEndpointPostAsync(this OpenIddictClientService _, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);

            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await IdentityClient.PostAsync(path, content, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<JsonNode?>(JsonOptions, cancellationToken);

        return result;
    }

    /// <summary>
    /// 发送Put请求
    /// </summary>
    /// <param name="_"></param>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<JsonNode?> RequestEndpointPutAsync(this OpenIddictClientService _, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);

            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await IdentityClient.PutAsync(path, content, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<JsonNode?>(JsonOptions, cancellationToken);

        return result;
    }

    /// <summary>
    /// 发送Patch请求
    /// </summary>
    /// <param name="_"></param>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<JsonNode?> RequestEndpointPatchAsync(this OpenIddictClientService _, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);

            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await IdentityClient.PatchAsync(path, content, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<JsonNode?>(JsonOptions, cancellationToken);

        return result;
    }

    /// <summary>
    /// 发送Delete请求 
    /// </summary>
    /// <param name="_"></param>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<JsonNode?> RequestEndpointDeleteAsync(this OpenIddictClientService _, string path, CancellationToken cancellationToken = default)
    {
        var response = await IdentityClient.DeleteAsync(path, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<JsonNode?>(JsonOptions, cancellationToken);

        return result;
    }

    /// <summary>
    /// 客户端凭证授权
    /// </summary>
    /// <param name="_"></param>
    /// <param name="pairs"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<OpenIddictResponse?> RequestClientCredentialsTokenAsync(this OpenIddictClientService _, IEnumerable<KeyValuePair<string, string?>>? pairs = null, CancellationToken cancellationToken = default)
    {
        var parameters = new List<KeyValuePair<string, string?>>
        {
            new("grant_type", GrantTypes.ClientCredentials),
            new("client_id", ClientId),
            new("client_secret", ClientSecret),
        };

        var result = await RequestTokenAsync(parameters, pairs, cancellationToken);

        return result;
    }

    /// <summary>
    /// 密码授权
    /// </summary>
    /// <param name="_"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="pairs"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<OpenIddictResponse?> RequestPasswordTokenAsync(this OpenIddictClientService _, string userName, string password, IEnumerable<KeyValuePair<string, string?>>? pairs = null, CancellationToken cancellationToken = default)
    {
        var parameters = new List<KeyValuePair<string, string?>>
        {
            new("grant_type", GrantTypes.Password),
            new("client_id", ClientId),
            new("client_secret", ClientSecret),
            new("username", userName),
            new("password", password),
        };

        var result = await RequestTokenAsync(parameters, pairs, cancellationToken);

        return result;
    }

    /// <summary>
    /// 自定义授权
    /// </summary>
    /// <param name="_"></param>
    /// <param name="grantType"></param>
    /// <param name="pairs"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<OpenIddictResponse?> RequestCustomTokenAsync(this OpenIddictClientService _, string grantType, IEnumerable<KeyValuePair<string, string?>>? pairs = null, CancellationToken cancellationToken = default)
    {
        var parameters = new List<KeyValuePair<string, string?>>
        {
            new("grant_type", grantType),
            new("client_id", ClientId),
            new("client_secret", ClientSecret),
        };

        var result = await RequestTokenAsync(parameters, pairs, cancellationToken);

        return result;
    }
}
