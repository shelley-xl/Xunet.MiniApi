﻿// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.MiniApis;

internal static partial class WebApplicationExtension
{
    internal static WebApplication UseUserMiniApi(this WebApplication app)
    {
        var group = app
            .MapGroup("/api/user")
            .WithGroupName("test")
            .WithTags("个人中心")
            .AddEndpointFilter<AutoValidationFilter>()
            .RequireAuthorization(AuthorizePolicy.Default);

        group.MapGet("/info", GetUserInfoAsync);

        return app;
    }

    [EndpointSummary("获取用户信息")]
    static async Task<IResult> GetUserInfoAsync()
    {
        await Task.CompletedTask;

        var dto = new UserDto
        {
            Id = XunetHttpContext.Current?.User.FindFirst("id")?.Value,
            UserName = XunetHttpContext.Current?.User.FindFirst("username")?.Value,
        };

        return XunetResults.Ok(dto);
    }
}
