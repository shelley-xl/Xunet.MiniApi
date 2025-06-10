// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tests.Entities.Dtos.Requests.Validators;

/// <summary>
/// 登录请求验证器
/// </summary>
/// <param name="db"></param>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LoginRequestValidator(ISqlSugarClient db)
    {
        _db = db;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("密码不能为空");

        RuleFor(x => x)
            .MustAsync(async (x, token) =>
            {
                var entity = await _db.Queryable<Accounts>().FirstAsync(a => a.UserName == x.UserName, token);
                if (entity == null) return false;
                if (entity.Password != x.Password!.ToMD5Encrypt()) return false;

                return true;
            })
            .WithMessage("账号或密码错误");
    }
}
