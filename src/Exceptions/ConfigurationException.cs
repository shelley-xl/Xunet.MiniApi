// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Exceptions;

/// <summary>
/// 配置异常
/// </summary>
public class ConfigurationException : Exception
{
    /// <summary>
    /// 配置异常
    /// </summary>
    /// <param name="message"></param>
    public ConfigurationException(string message) : base(message) { }

    /// <summary>
    /// 配置异常
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
}
