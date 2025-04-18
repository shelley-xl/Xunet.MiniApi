namespace Xunet.MiniApi.Dtos.Requests;

/// <summary>
/// 分页查询请求基类
/// </summary>
public class PageRequest
{
    /// <summary>
    /// 页码
    /// </summary>
    [FromParameter("page", "页码")]
    public int? Page { get; set; }

    /// <summary>
    /// 页大小
    /// </summary>
    [FromParameter("size", "页大小")]
    public int? Size { get; set; }
}
