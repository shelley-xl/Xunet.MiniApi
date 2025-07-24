// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// 数据库上下文
/// </summary>
public class SugarDbContext<DbContext> where DbContext : SugarDbContext<DbContext>
{
    /// <summary>
    /// 数据库操作对象（主对象）
    /// </summary>
    protected virtual ISqlSugarClient MasterDb
    {
        get
        {
            return XunetHttpContext.GetRequiredService<ISqlSugarClient>();
        }
    }

    /// <summary>
    /// 数据库操作对象
    /// </summary>
    public virtual ISqlSugarClient Db
    {
        get
        {
            return MasterDb.AsTenant().GetConnectionScopeWithAttr<DbContext>();
        }
    }
}
