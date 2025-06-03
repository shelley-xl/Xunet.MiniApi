// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Enums;

/// <summary>
/// 状态码枚举
/// </summary>
public enum XunetCode
{
    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success = 0,

    /// <summary>
    /// 失败
    /// </summary>
    [Description("失败")]
    Failure = 1,

    /// <summary>
    /// 错误
    /// </summary>
    [Description("错误")]
    Error = 2,

    /// <summary>
    /// 无效的参数
    /// </summary>
    [Description("无效的参数")]
    InvalidParameter = 3,

    /// <summary>
    /// 系统异常
    /// </summary>
    [Description("系统异常")]
    SystemException = 4,

    /// <summary>
    /// 签名错误
    /// </summary>
    [Description("签名错误")]
    SignError = 5,

    /// <summary>
    /// 请求繁忙
    /// </summary>
    [Description("请求繁忙")]
    TooManyRequests = 6,

    /// <summary>
    /// 无效的请求
    /// </summary>
    [Description("无效的请求")]
    BadRequest = 7,

    /// <summary>
    /// 未授权
    /// </summary>
    [Description("未授权")]
    Unauthorized = 8,

    /// <summary>
    /// 禁止访问
    /// </summary>
    [Description("禁止访问")]
    Forbidden = 9,

    /// <summary>
    /// 资源未找到
    /// </summary>
    [Description("资源未找到")]
    NotFound = 10,

    /// <summary>
    /// 方法不允许
    /// </summary>
    [Description("方法不允许")]
    MethodNotAllowed = 11,

    /// <summary>
    /// 请求错误
    /// </summary>
    [Description("请求错误")]
    RequestError = 12,
}
