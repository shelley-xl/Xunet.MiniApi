// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Services;

/// <summary>
/// 认证服务类
/// </summary>
/// <param name="provider"></param>
/// <param name="jwtConfig"></param>
public class AuthService(IOptions<JwtConfig> jwtConfig) : MiniService<AppDbContext>, IAuthService
{
    /// <summary>
    /// 获取图形验证码
    /// </summary>
    /// <returns></returns>
    public async Task<IResult> GetVeryCodeAsync()
    {
        return XunetResults.Ok(await Captcha.GenerateAsync());
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request">登录请求</param>
    /// <returns></returns>
    public async Task<IResult> LoginAsync(LoginRequest request)
    {
        var accounts = await GetAsync<Accounts>(x => x.UserName == request.UserName);

        // 生成token
        var expires = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + jwtConfig.Value.Expire;
        var claims = new Claim[]
        {
            new("id", accounts.Id!),
            new("username", accounts.UserName!),
            new("expires", $"{expires}"),
        };
        var token = JwtToken.BuildJwtToken(claims, jwtConfig.Value);

        var dto = new LoginDto
        {
            Token = token,
            Expires = expires,
            Type = "Bearer",
        };

        return XunetResults.Ok(dto);
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="request">注册请求</param>
    /// <returns></returns>
    public async Task<IResult> RegisterAsync(RegisterRequest request)
    {
        var entity = Mapper.Map<Accounts>(request);

        entity.Password = entity.Password?.ToMD5Encrypt();

        await InsertAsync(entity);

        return XunetResults.Ok();
    }
}
