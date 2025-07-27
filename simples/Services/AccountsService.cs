// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Services;

/// <summary>
/// 账户服务类
/// </summary>
public class AccountsService : MiniService<AppDbContext>, IAccountsService
{
    /// <summary>
    /// 获取账户分页列表
    /// </summary>
    /// <param name="request">获取账户分页列表请求</param>
    /// <returns></returns>
    public async Task<IResult> GetPageListAsync(PageRequest request)
    {
        RefAsync<int> totalNumber = 0;

        var page = request.Page.GetValueOrDefault();
        var size = request.Size.GetValueOrDefault();

        var list = await Db
            .Queryable<Accounts>()
            .Select<AccountsDto>()
            .ToPageListAsync(page, size, totalNumber);

        return XunetResults.Ok(list, request, totalNumber);
    }
}
