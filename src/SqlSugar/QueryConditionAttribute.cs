// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// SqlSugar查询参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class QueryConditionAttribute : FromQueryAttribute
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
    /// <param name="name"></param>
    /// <param name="fieldName"></param>
    /// <param name="conditionType"></param>
    /// <param name="whereType"></param>
    public QueryConditionAttribute(string name, string? fieldName, ConditionalType conditionType, WhereType whereType)
    {
        Name = name;
        FieldName = fieldName;
        ConditionType = conditionType;
        WhereType = whereType;
    }
}
