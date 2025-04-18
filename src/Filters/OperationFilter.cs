namespace Xunet.MiniApi.Filters;

/// <summary>
/// Swagger自定义参数描述过滤器
/// </summary>
public class OperationFilter : IOperationFilter
{
    /// <summary>
    /// 自定义参数描述
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var item in context.ApiDescription.ParameterDescriptions)
        {
            if (item.ParameterInfo()?.GetCustomAttribute<FromParameterAttribute>() is FromParameterAttribute attribute)
            {
                var index = context.ApiDescription.ParameterDescriptions.IndexOf(item);
                operation.Parameters[index].Description = attribute.Description;
                operation.Parameters[index].Required = attribute.Required;
            }
        }
    }
}
