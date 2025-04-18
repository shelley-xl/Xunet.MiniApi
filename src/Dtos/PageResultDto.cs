namespace Xunet.MiniApi.Dtos;

/// <summary>
/// 分页查询响应
/// </summary>
/// <typeparam name="T">泛型对象</typeparam>
public class PageResultDto<T> : OperateResultDto where T : class, new()
{
    /// <summary>
    /// 数据
    /// </summary>
    public PageObjectDto<T>? Data { get; set; }
}
