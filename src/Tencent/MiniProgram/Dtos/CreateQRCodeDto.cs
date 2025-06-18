// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos;

/// <summary>
/// 创建小程序二维码返回
/// </summary>
public class CreateQRCodeDto : ErrorDto
{
    /// <summary>
    /// 图片 Buffer
    /// </summary>
    [JsonPropertyName("buffer")]
    public byte[]? Buffer { get; set; }
}
