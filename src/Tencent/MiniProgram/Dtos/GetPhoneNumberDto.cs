// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos;

/// <summary>
/// 获取手机号返回
/// </summary>
public class GetPhoneNumberDto : ErrorDto
{
    /// <summary>
    /// 用户手机号信息
    /// </summary>
    [JsonPropertyName("phone_info")]
    public PhoneInfoObject? PhoneInfo { get; set; }

    /// <summary>
    /// 用户手机号信息对象
    /// </summary>
    public class PhoneInfoObject
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        [JsonPropertyName("purePhoneNumber")]
        public string? PurePhoneNumber { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        /// <summary>
        /// 数据水印
        /// </summary>
        [JsonPropertyName("watermark")]
        public WaterMarkObject? WaterMark { get; set; }

        /// <summary>
        /// 数据水印对象
        /// </summary>
        public class WaterMarkObject
        {
            /// <summary>
            /// 时间戳
            /// </summary>
            [JsonPropertyName("timestamp")]
            public long? Timestamp { get; set; }

            /// <summary>
            /// 小程序appid
            /// </summary>
            [JsonPropertyName("appid")]
            public string? AppId { get; set; }
        }
    }
}
