namespace Xunet.MiniApi.Dtos;

/// <summary>
/// 分页对象
/// </summary>
/// <typeparam name="T">泛型对象</typeparam>
public class PageObjectDto<T> where T : class, new()
{
    /// <summary>
    /// 页码
    /// </summary>
    public int? PageNum { get; set; }

    /// <summary>
    /// 页大小
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// 总记录数
    /// </summary>
    public int? RecordCount { get; set; }

    /// <summary>
    /// 数据集
    /// </summary>
    public List<T>? Rows { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int? TotalPage
    {
        get
        {
            return (RecordCount + PageSize - 1) / PageSize;
        }
    }
}
