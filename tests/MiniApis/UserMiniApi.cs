// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tests.MiniApis;

internal static partial class WebApplicationExtension
{
    internal static WebApplication UseUserMiniApi(this WebApplication app)
    {
        var group = app.MapGroup("/api/user").WithGroupName("test").WithTags("个人中心");

        group.MapGet("/info", GetUserInfoAsync);

        group.AddEndpointFilter<AutoValidationFilter>();
        group.RequireAuthorization(AuthorizePolicy.Default);

        return app;
    }

    [EndpointSummary("获取用户信息")]
    static async Task<IResult> GetUserInfoAsync()
    {
        var dto = new UserDto
        {
            Id = XunetHttpContext.Current?.User.FindFirst("id")?.Value,
            UserName = XunetHttpContext.Current?.User.FindFirst("username")?.Value,
        };

        return Results.Ok(await Task.FromResult(dto));
    }
}
