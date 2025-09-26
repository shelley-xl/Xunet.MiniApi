// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Endpoints;

/// <summary>
/// 账户管理
/// </summary>
internal static class AccountsEndpoint
{
    internal static void MapAccountsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/accounts", "test", "账户管理");

        group.MapGet<IAccountsService, PageRequest>("/page", "获取账户分页列表", (service, [AsParameters] request) =>
        {
            return service.GetPageListAsync(request);
        });
    }
}
