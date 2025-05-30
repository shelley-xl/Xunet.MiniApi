// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Swagger;

/// <summary>
/// SwaggerOptions
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// 文档标题
    /// </summary>
    public string? DocumentTitle { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public SwaggerEndpoints[]? Endpoints { get; set; }
}
