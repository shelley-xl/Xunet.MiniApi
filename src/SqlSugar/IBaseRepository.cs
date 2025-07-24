// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// 仓储基础接口
/// </summary>
public interface IBaseRepository
{
    /// <summary>
    /// 根据主键获取实体
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="key">主键</param>
    /// <returns>单个实体</returns>
    Task<T> GetAsync<T>(object key) where T : class, new();

    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="where">条件</param>
    /// <param name="token">取消token</param>
    /// <returns>单个实体</returns>
    Task<T> GetAsync<T>(Expression<Func<T, bool>> where, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 根据主键数组获取实体列表
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="keys">主键数组</param>
    /// <param name="token">取消token</param>
    /// <returns>实体列表</returns>
    Task<List<T>> GetListAsync<T>(object[] keys, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 根据条件获取实体列表
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="where">条件</param>
    /// <param name="token">取消token</param>
    /// <returns>实体列表</returns>
    Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> where, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="expression">表达式</param>
    /// <returns>DateTime</returns>
    Task<DateTime> GetMaxAsync<T>(Expression<Func<T, DateTime?>> expression) where T : class, new();

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="expression">表达式</param>
    /// <returns>Int64</returns>
    Task<long> GetMaxAsync<T>(Expression<Func<T, long?>> expression) where T : class, new();

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="expression">表达式</param>
    /// <returns>Int32</returns>
    Task<int> GetMaxAsync<T>(Expression<Func<T, int?>> expression) where T : class, new();

    /// <summary>
    /// 获取最小值
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="expression">表达式</param>
    /// <returns>DateTime</returns>
    Task<DateTime> GetMinAsync<T>(Expression<Func<T, DateTime?>> expression) where T : class, new();

    /// <summary>
    /// 获取最小值
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="expression">表达式</param>
    /// <returns>Int64</returns>
    Task<long> GetMinAsync<T>(Expression<Func<T, long?>> expression) where T : class, new();

    /// <summary>
    /// 获取最小值
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="expression">表达式</param>
    /// <returns>Int32</returns>
    Task<int> GetMinAsync<T>(Expression<Func<T, int?>> expression) where T : class, new();

    /// <summary>
    /// 单条插入
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="entity">单个实体</param>
    /// <param name="token">取消token</param>
    /// <returns>插入行数</returns>
    Task<int> InsertAsync<T>(T entity, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量插入
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="token">取消token</param>
    /// <returns>插入行数</returns>
    Task<int> InsertAsync<T>(List<T> list, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量插入（自定义主键插入）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="columns">主键列</param>
    /// <param name="token">取消token</param>
    /// <returns>插入行数</returns>
    Task<int> InsertAsync<T>(List<T> list, Expression<Func<T, object>> columns, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量插入（传统分页插入）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="token">取消token</param>
    /// <returns>插入行数</returns>
    Task<int> InsertAsync<T>(List<T> list, int pageSize, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量插入（大数据插入）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <returns>插入行数</returns>
    Task<int> InsertBulkAsync<T>(List<T> list) where T : class, new();

    /// <summary>
    /// 批量插入（大数据分页插入）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns>插入行数</returns>
    Task<int> InsertBulkAsync<T>(List<T> list, int pageSize) where T : class, new();

    /// <summary>
    /// 单条更新
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="entity">单个实体</param>
    /// <param name="token">取消token</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateAsync<T>(T entity, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="token">取消token</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateAsync<T>(List<T> list, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量更新（自定义主键更新）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="columns">主键列</param>
    /// <param name="fields">要更新或忽略的字段</param>
    /// <param name="ignore">是否忽略指定字段：true忽略，false更新，为空时更新所有字段</param>
    /// <param name="token">取消token</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateAsync<T>(List<T> list, Expression<Func<T, object>> columns, Expression<Func<T, object>>? fields = null, bool? ignore = null, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量更新（传统分页更新）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="token">取消token</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateAsync<T>(List<T> list, int pageSize, CancellationToken token = default) where T : class, new();

    /// <summary>
    /// 批量更新（大数据更新）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateBulkAsync<T>(List<T> list) where T : class, new();

    /// <summary>
    /// 批量更新（大数据分页更新）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateBulkAsync<T>(List<T> list, int pageSize) where T : class, new();

    /// <summary>
    /// 批量保存（新增或更新）
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <param name="columns">主键列</param>
    /// <param name="ignore">忽略更新列</param>
    /// <returns>新增和更新行数</returns>
    Task<int> SaveAsync<T>(List<T> list, Expression<Func<T, object>> columns, Expression<Func<T, object>>? ignore = null) where T : class, new();

    /// <summary>
    /// 单条删除
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="key">主键</param>
    /// <returns>删除行数</returns>
    Task<int> DeleteAsync<T>(object key) where T : class, new();

    /// <summary>
    /// 单条删除
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="entity">单个实体</param>
    /// <returns>删除行数</returns>
    Task<int> DeleteAsync<T>(T entity) where T : class, new();

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="keys">主键数组</param>
    /// <returns>删除行数</returns>
    Task<int> DeleteAsync<T>(object[] keys) where T : class, new();

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="list">实体列表</param>
    /// <returns>删除行数</returns>
    Task<int> DeleteAsync<T>(List<T> list) where T : class, new();

    /// <summary>
    /// 使用事务（支持多库事务）
    /// </summary>
    /// <param name="action">主方法</param>
    /// <returns></returns>
    Task<bool> UseTranAsync(Func<Task> action);
}
