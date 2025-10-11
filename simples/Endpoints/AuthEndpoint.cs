// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Endpoints;

/// <summary>
/// 认证中心
/// </summary>
internal static class AuthEndpoint
{
    internal static void MapAuthEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth", "test", false, "认证中心");

        group.MapGet<IAuthService>("/code", "获取图形验证码", (service) =>
        {
            return service.GetVeryCodeAsync();
        });

        group.MapPost<IAuthService, LoginRequest>("/login", "登录", (service, request) =>
        {
            return service.LoginAsync(request);
        });

        group.MapPost<IAuthService, RegisterRequest>("/register", "注册", (service, request) =>
        {
            return service.RegisterAsync(request);
        });
    }
}
