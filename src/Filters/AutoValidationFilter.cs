namespace Xunet.MiniApi.Filters;

/// <summary>
/// 模型验证过滤器
/// </summary>
public class AutoValidationFilter : IEndpointFilter
{
    /// <summary>
    /// 处理方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var serviceProvider = context.HttpContext.RequestServices;

        foreach (var argument in context.Arguments)
        {
            if (argument != null && argument is PageRequest request)
            {
                // 分页赋默认值
                request.Page ??= 1;
                request.Size ??= 20;
            }
            if (argument != null && serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(argument.GetType())) is IValidator validator)
            {
                var validationContext = new ValidationContext<object>(argument);

                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsValid)
                {
                    return Results.Ok(new OperateResultDto
                    {
                        Code = XunetCode.InvalidParameter,
                        Message = result.Errors.FirstOrDefault()?.ErrorMessage!
                    });
                }
            }
        }

        return await next(context);
    }
}
