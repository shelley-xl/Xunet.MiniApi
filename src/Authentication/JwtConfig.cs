﻿// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Authentication;

/// <summary>
/// Jwt配置
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// 是否校验颁发者
    /// </summary>
    public bool ValidateIssuer { get; set; } = default!;

    /// <summary>
    /// 颁发者
    /// </summary>
    public string ValidIssuer { get; set; } = string.Empty;

    /// <summary>
    /// 是否校验签名
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; } = default!;

    /// <summary>
    /// 签名
    /// </summary>
    public string SymmetricSecurityKey { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string IssuerSigningKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否校验受众
    /// </summary>
    public bool ValidateAudience { get; set; } = default!;

    /// <summary>
    /// Accessoken受众
    /// </summary>
    public string ValidAudience { get; set; } = string.Empty;

    /// <summary>
    /// RefreshToken受众
    /// </summary>
    public string RefreshTokenAudience { get; set; } = string.Empty;

    /// <summary>
    /// 校验Lifetime
    /// </summary>
    public bool ValidateLifetime { get; set; } = default!;

    /// <summary>
    ///  校验是否有Expire字段
    /// </summary>
    public bool RequireExpirationTime { get; set; }

    /// <summary>
    /// 时间歪斜，单位秒
    /// </summary>
    public int ClockSkew { get; set; } = default;

    /// <summary>
    /// AccessToken过期时间，单位分钟
    /// </summary>
    public int Expire { get; set; } = default;

    /// <summary>
    /// RefreshToken过期时间，单位分钟
    /// </summary>
    public int RefreshTokenExpire { get; set; } = default;
}
