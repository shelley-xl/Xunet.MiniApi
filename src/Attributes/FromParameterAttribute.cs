namespace Xunet.MiniApi.Attributes;

/// <summary>
/// 查询参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class FromParameterAttribute : FromQueryAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public FromParameterAttribute()
    {

    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="description">描述</param>
    /// <param name="required">是否必填</param>
    public FromParameterAttribute(string name, string description, bool required = false)
    {
        Name = name;
        Description = description;
        Required = required;
    }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 是否必填
    /// </summary>
    public bool Required { get; set; }
}
