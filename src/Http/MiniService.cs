// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Http;

/// <summary>
/// 迷你服务
/// </summary>
/// <typeparam name="DbContext">数据库上下文</typeparam>
public class MiniService<DbContext> : BaseRepository<DbContext> where DbContext : SugarDbContext<DbContext>
{
    /// <summary>
    /// 配置文件对象
    /// </summary>
    protected virtual IConfiguration Configuration
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IConfiguration>();
        }
    }

    /// <summary>
    /// 缓存操作对象
    /// </summary>
    protected virtual IXunetCache Cache
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IXunetCache>();
        }
    }

    /// <summary>
    /// 实体映射对象
    /// </summary>
    protected virtual IXunetMapper Mapper
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IXunetMapper>();
        }
    }

    /// <summary>
    /// 认证中心客户端
    /// </summary>
    protected virtual OpenIddictClientService OpenIddictClient
    {
        get
        {
            return XunetHttpContext.GetRequiredService<OpenIddictClientService>();
        }
    }

    /// <summary>
    /// 微信公众号服务
    /// </summary>
    protected virtual IWeixinMpService WeixinMpService
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IWeixinMpService>();
        }
    }

    /// <summary>
    /// 微信小程序服务
    /// </summary>
    protected virtual IMiniProgramService MiniProgramService
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IMiniProgramService>();
        }
    }

    /// <summary>
    /// 腾讯云对象存储服务
    /// </summary>
    protected virtual ITencentCloudCosService TencentCloudCosService
    {
        get
        {
            return XunetHttpContext.GetRequiredService<ITencentCloudCosService>();
        }
    }

    /// <summary>
    /// 腾讯云短信服务
    /// </summary>
    protected virtual ITencentCloudSmsService TencentCloudSmsService
    {
        get
        {
            return XunetHttpContext.GetRequiredService<ITencentCloudSmsService>();
        }
    }

    /// <summary>
    /// 阿里云对象存储服务
    /// </summary>
    protected virtual IAliyunOssService AliyunOssService
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IAliyunOssService>();
        }
    }

    /// <summary>
    /// 阿里云短信服务
    /// </summary>
    protected virtual IAliyunSmsService AliyunSmsService
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IAliyunSmsService>();
        }
    }

    /// <summary>
    /// 图形验证码
    /// </summary>
    protected virtual IXunetCaptcha Captcha
    {
        get
        {
            return XunetHttpContext.GetRequiredService<IXunetCaptcha>();
        }
    }

    /// <summary>
    /// 当前登录用户Id
    /// </summary>
    protected virtual string? CurrentUserId
    {
        get
        {
            return XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject);
        }
    }
}
