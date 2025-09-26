// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) ���� ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tests;

/// <summary>
/// ��Ԫ����
/// </summary>
public class DingTalkUnitTest
{
    IServiceProvider ServiceProvider { get; }

    public DingTalkUnitTest()
    {
        IServiceCollection services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile($"appsettings.json", true, true)
            .Build();

        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddXunetMiniProgramService(MiniProgramProvider.DingTalk);

        ServiceProvider = services.BuildServiceProvider();
    }

    [Theory(DisplayName = "����С�������˵�¼�ӿڲ���")]
    [InlineData("6aafa7fd094e3671a5777ab46e61f30b")]
    public async Task LoginTest(string code)
    {
        var DingTalkService = ServiceProvider.GetRequiredService<IDingTalkService>();

        Assert.NotNull(DingTalkService);

        var token = await DingTalkService.GetAccessTokenAsync();

        Assert.Equal(0, token.ErrCode);

        var login = await DingTalkService.DingTalkLoginAsync(new DingTalkLoginRequest
        {
            Code = code,
            AccessToken = token.AccessToken,
        });

        Assert.Equal(0, login.ErrCode);

        var user = await DingTalkService.GetUserInfoAsync(new GetUserInfoRequest
        {
            UserId = login.Result?.UserId,
            AccessToken = token.AccessToken,
        });

        Assert.Equal(0, user.ErrCode);

        await Task.CompletedTask;
    }
}
