// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos.Requests;

/// <summary>
/// 创建小程序码请求（限制数量100,000）
/// </summary>
public class CreateQRCodeRequest
{
    /// <summary>
    /// 扫码进入的小程序页面路径，最大长度 1024 个字符，不能为空，scancode_time为系统保留参数，不允许配置；对于小游戏，可以只传入 query 部分，来实现传参效果，如：传入 "?foo=bar"，即可在 wx.getLaunchOptionsSync 接口中的 query 参数获取到 {foo:"bar"}。
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// 二维码的宽度，单位 px。默认值为430，最小 280px，最大 1280px
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// 默认值false；自动配置线条颜色，如果颜色依然是黑色，则说明不建议配置主色调
    /// </summary>
    [JsonPropertyName("auto_color")]
    public bool? AutoColor { get; set; }

    /// <summary>
    /// 默认值{"r":0,"g":0,"b":0} ；AutoColor 为 false 时生效，使用 rgb 设置颜色 例如 {"r":"xxx","g":"xxx","b":"xxx"} 十进制表示
    /// </summary>
    [JsonPropertyName("line_color")]
    public object? LineColor { get; set; }

    /// <summary>
    /// 默认值false；是否需要透明底色，为 true 时，生成透明底色的小程序码
    /// </summary>
    [JsonPropertyName("is_hyaline")]
    public bool? IsHyaline { get; set; }

    /// <summary>
    /// 要打开的小程序版本。正式版为 "release"，体验版为 "trial"，开发版为 "develop"。默认是正式版。
    /// </summary>
    [JsonPropertyName("env_version")]
    public string? EnvVersion { get; set; }

    /// <summary>
    /// 接口调用凭证
    /// </summary>
    [JsonIgnore]
    public string? AccessToken { get; set; }
}
