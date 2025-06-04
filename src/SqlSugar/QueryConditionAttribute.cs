// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// SqlSugar查询参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = true)]
public class QueryConditionAttribute : FromParameterAttribute
{
    /// <summary>
    /// 字段名
    /// </summary>
    public string? FieldName { get; set; }

    /// <summary>
    /// 条件
    /// </summary>
    public ConditionalType ConditionType { get; set; }

    /// <summary>
    /// 条件类型
    /// </summary>
    public WhereType WhereType { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public QueryConditionAttribute()
    {

    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="description">描述</param>
    /// <param name="displayName">参数名</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="conditionType"></param>
    /// <param name="whereType"></param>
    /// <param name="required"></param>
    public QueryConditionAttribute(string? description = null, string? displayName = null, string? fieldName = null, ConditionalType conditionType = ConditionalType.Like, WhereType whereType = WhereType.And, bool required = false)
    {
        Description = description;
        Name = displayName;
        FieldName = fieldName;
        ConditionType = conditionType;
        WhereType = whereType;
        Required = required;
    }
}
