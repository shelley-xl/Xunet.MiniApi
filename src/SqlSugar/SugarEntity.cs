namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// SqlSugar实体基类
/// </summary>
public abstract class SugarEntity : IDeleted
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnDescription = "主键", IsPrimaryKey = true, Length = 36)]
    public virtual string? Id { get; set; } = SnowFlakeSingle.Instance.NextId().ToString();

    /// <summary>
    /// 是否删除
    /// </summary>
    [SugarColumn(ColumnDescription = "是否删除")]
    public virtual bool? IsDelete { get; set; } = false;

    /// <summary>
    /// 创建人
    /// </summary>
    [SugarColumn(ColumnDescription = "创建人", IsNullable = true, Length = 36)]
    public virtual string? CreateId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
    public virtual DateTime? CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 修改人
    /// </summary>
    [SugarColumn(ColumnDescription = "修改人", IsNullable = true, Length = 36)]
    public virtual string? UpdateId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
    public virtual DateTime? UpdateTime { get; set; }
}
