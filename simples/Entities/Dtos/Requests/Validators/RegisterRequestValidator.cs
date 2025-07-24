// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Entities.Dtos.Requests.Validators;

/// <summary>
/// 注册请求验证器
/// </summary>
/// <param name="db"></param>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public RegisterRequestValidator(AppDbContext context)
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("密码不能为空");

        RuleFor(x => x)
            .MustAsync(async (x, token) =>
            {
                var entity = await context.Db.Queryable<Accounts>().FirstAsync(a => a.UserName == x.UserName, token);

                return entity == null;
            })
            .WithMessage("用户名被占用，请更换");
    }
}
