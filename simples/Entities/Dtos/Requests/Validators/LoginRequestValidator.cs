// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Entities.Dtos.Requests.Validators;

/// <summary>
/// 登录请求验证器
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LoginRequestValidator(AppDbContext context, IXunetCaptcha captcha)
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("密码不能为空");

        RuleFor(x => x.VeryUuid)
            .NotEmpty().WithMessage("图形验证码唯一标识不能为空");

        RuleFor(x => x.VeryCode)
            .NotEmpty().WithMessage("图形验证码不能为空");

        RuleFor(x => x)
            .CustomAsync(async (x, y, token) =>
            {
                var ok = await captcha.ValidateAsync(x.VeryUuid, x.VeryCode);

                if (!ok.HasValue) y.AddFailure("图形验证码已过期");

                else if (!ok.Value) y.AddFailure("图形验证码错误");
            });

        RuleFor(x => x)
            .MustAsync(async (x, token) =>
            {
                var entity = await context.Db.Queryable<Accounts>().FirstAsync(a => a.UserName == x.UserName, token);

                return entity != null && entity.Password == x.Password?.ToMD5Encrypt();
            })
            .WithMessage("账号或密码错误");
    }
}
