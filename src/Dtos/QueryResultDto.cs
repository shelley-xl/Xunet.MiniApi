namespace Xunet.MiniApi.Dtos;

/// <summary>
/// 数据查询响应
/// </summary>
/// <typeparam name="T">泛型对象</typeparam>
public class QueryResultDto<T> : OperateResultDto
{
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }
}
