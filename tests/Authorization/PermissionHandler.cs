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
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            // 已认证，鉴权
            await Task.CompletedTask;
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
