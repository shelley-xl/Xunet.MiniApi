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

        services.AddSingleton<IConfiguration>(configuration);

        services.AddXunetMiniProgramService(MiniProgramProvider.DingTalk);

        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact(DisplayName = "����С�������˵�¼�ӿڲ���")]
    public async Task LoginTest()
    {
        var DingTalkService = ServiceProvider.GetRequiredService<IDingTalkService>();

        Assert.NotNull(DingTalkService);

        //var token = await DingTalkService.GetAccessTokenAsync();

        //Assert.Equal(0, token.ErrCode);

        //var login = await DingTalkService.DingTalkLoginAsync(new DingTalkLoginRequest
        //{
        //    Code = "2f862ca8fc8f3eb0aefb2bd5b9650e53",
        //    AccessToken = token.AccessToken,
        //});

        //Assert.Equal(0, login.ErrCode);

        //var user = await DingTalkService.GetUserInfoAsync(new GetUserInfoRequest
        //{
        //    UserId = login.Result?.UserId,
        //    AccessToken = token.AccessToken,
        //});

        //Assert.Equal(0, user.ErrCode);

        await Task.CompletedTask;
    }
}
