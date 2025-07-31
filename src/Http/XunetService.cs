// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Http;

/// <summary>
/// 服务基类
/// </summary>
public class XunetService
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

    /// <summary>
    /// 操作成功
    /// </summary>
    /// <returns></returns>
    protected virtual OperateResultDto XunetResult()
    {
        return new OperateResultDto();
    }

    /// <summary>
    /// 数据查询
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="data">数据</param>
    /// <returns></returns>
    protected virtual QueryResultDto<T> XunetResult<T>(T data)
    {
        return new QueryResultDto<T>
        {
            Data = data,
        };
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="request">分页请求</param>
    /// <param name="data">数据</param>
    /// <param name="totalNumber">总记录数</param>
    /// <returns></returns>
    protected virtual PageResultDto<T> XunetResult<T>(List<T> data, PageRequest request, RefAsync<int> totalNumber) where T : class, new()
    {
        return new PageResultDto<T>
        {
            Data = new PageObjectDto<T>
            {
                PageNum = request.Page,
                PageSize = request.Size,
                RecordCount = totalNumber,
                Rows = data,
            },
        };
    }

    /// <summary>
    /// 操作错误
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns></returns>
    protected virtual OperateResultDto ErrorResult(string? message)
    {
        return new OperateResultDto
        {
            Code = XunetCode.Error,
            Message = message ?? "fail",
        };
    }

    /// <summary>
    /// 操作失败
    /// </summary>
    /// <returns></returns>
    protected virtual OperateResultDto FailResult()
    {
        return new OperateResultDto
        {
            Code = XunetCode.Failure,
            Message = "fail",
        };
    }
}
