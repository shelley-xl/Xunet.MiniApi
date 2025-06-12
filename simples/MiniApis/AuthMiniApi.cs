// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.MiniApis;

internal static partial class WebApplicationExtension
{
    internal static WebApplication UseAuthMiniApi(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithGroupName("test").WithTags("认证中心");

        group.MapPost("/login", LoginAsync);
        group.MapPost("/register", RegisterAsync);

        group.AddEndpointFilter<AutoValidationFilter>();

        return app;
    }

    [EndpointSummary("登录")]
    static async Task<IResult> LoginAsync(LoginRequest request, IOptions<JwtConfig> jwtConfig, ISqlSugarClient db)
    {
        var accounts = await db.Queryable<Accounts>().SingleAsync(x => x.UserName == request.UserName);

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

    [EndpointSummary("注册")]
    static async Task<IResult> RegisterAsync(RegisterRequest request, ISqlSugarClient db, IObjectMapper mapper)
    {
        var entity = mapper.Map<Accounts>(request);

        entity.Password = entity.Password?.ToMD5Encrypt();

        await db.Insertable(entity).ExecuteCommandAsync();

        return XunetResults.Ok();
    }
}
