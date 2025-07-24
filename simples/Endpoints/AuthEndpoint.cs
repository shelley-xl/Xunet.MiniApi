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
        var group = app.MapGroup("/api/auth").WithGroupName("test").WithTags("认证中心").AddEndpointFilter<AutoValidationFilter>();

        group.MapPost("/login", async (LoginRequest request, IAuthService service) => await service.LoginAsync(request)).WithSummary("登录");

        group.MapPost("/register", async (RegisterRequest request, IAuthService service) => await service.RegisterAsync(request)).WithSummary("注册");
    }
}
