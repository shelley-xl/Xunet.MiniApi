// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tests.Entities.Dtos;

/// <summary>
/// 登录信息
/// </summary>
public class LoginDto
{
    /// <summary>
    /// 访问token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public long? Expires { get; set; }

    /// <summary>
    /// token类型
    /// </summary>
    public string? Type { get; set; }
}
