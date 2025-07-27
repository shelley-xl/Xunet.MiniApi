// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.Oss.Dtos;

/// <summary>
/// 文件上传进度
/// </summary>
public class UploadProgressDto
{
    /// <summary>
    /// 已上传字节
    /// </summary>
    public long? BytesUploaded { get; set; } = 0;

    /// <summary>
    /// 总字节
    /// </summary>
    public long? TotalBytes { get; set; } = 0;

    /// <summary>
    /// 总进度
    /// </summary>
    public double? Percentage
    {
        get
        {
            if (BytesUploaded.HasValue && TotalBytes.HasValue && TotalBytes > 0)
            {
                return Math.Round((double)BytesUploaded.Value / TotalBytes.Value * 100, 2);
            }
            return 0;
        }
    }
}
