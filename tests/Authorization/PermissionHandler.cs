namespace Xunet.MiniApi.Tests.Authorization;

/// <summary>
/// 权限处理
/// </summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// 权限处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        await Task.CompletedTask;
        context.Succeed(requirement);
        //if (context.User.Identity != null)
        //{
        //    if (context.User.Identity.IsAuthenticated && context.Resource is HttpContext httpContext)
        //    {
        //        await Task.CompletedTask;
        //        var result = true;
        //        if (result)
        //        {
        //            context.Succeed(requirement);
        //        }
        //        else
        //        {
        //            context.Fail();
        //        }
        //    }
        //    else
        //    {
        //        context.Fail();
        //    }
        //}
        //else
        //{
        //    context.Fail();
        //}
    }
}
