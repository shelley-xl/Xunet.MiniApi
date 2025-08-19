// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.DingTalk.Dtos;

/// <summary>
/// 小程序登录返回
/// </summary>
public class DingTalkLoginDto : ErrorDto
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [JsonPropertyName("result")]
    public ResultObject? Result { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public class ResultObject
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonPropertyName("userid")]
        public string? UserId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        [JsonPropertyName("device_id")]
        public string? DeviceId { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        [JsonPropertyName("sys")]
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// 级别：1主管理员，2子管理员，100老板，0其他（如普通员工）
        /// </summary>
        [JsonPropertyName("sys_level")]
        public int? Level { get; set; }

        /// <summary>
        /// 用户关联的UnionId
        /// </summary>
        [JsonPropertyName("associated_unionid")]
        public string? AssociatedUnionid { get; set; }

        /// <summary>
        /// 用户UnionId
        /// </summary>
        [JsonPropertyName("unionid")]
        public string? Unionid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
