// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// SqlSugar查询参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class QueryFilterAttribute : Attribute
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
    /// 字段值
    /// </summary>
    public string? FieldValue { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public QueryFilterAttribute()
    {

    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fieldName">字段名</param>
    /// <param name="fieldValue">字段值</param>
    /// <param name="conditionType"></param>
    /// <param name="whereType"></param>
    public QueryFilterAttribute(string? fieldName = null, string? fieldValue = null, ConditionalType conditionType = ConditionalType.Like, WhereType whereType = WhereType.And)
    {
        FieldName = fieldName;
        ConditionType = conditionType;
        WhereType = whereType;
        FieldValue = fieldValue;
    }
}
