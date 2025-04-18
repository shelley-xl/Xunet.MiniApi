namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// 逻辑删除接口
/// </summary>
public interface IDeleted
{
    /// <summary>
    /// 是否删除
    /// </summary>
    [SugarColumn(ColumnDescription = "是否删除")]
    public abstract bool? IsDelete { get; set; }
}
