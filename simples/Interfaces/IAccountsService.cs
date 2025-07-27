// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Interfaces;

/// <summary>
/// 账户服务接口
/// </summary>
public interface IAccountsService
{
    /// <summary>
    /// 获取账户分页列表
    /// </summary>
    /// <param name="request">获取账户分页列表请求</param>
    /// <returns></returns>
    Task<IResult> GetPageListAsync(PageRequest request);
}
