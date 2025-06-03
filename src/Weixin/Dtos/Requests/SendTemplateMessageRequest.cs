// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Weixin.Dtos.Requests;

/// <summary>
/// 发送模板消息请求
/// </summary>
public class SendTemplateMessageRequest
{
    /// <summary>
    /// 接收者openid
    /// </summary>
    [JsonPropertyName("touser")]
    public string? ToUser { get; set; }

    /// <summary>
    /// 模板ID
    /// </summary>
    [JsonPropertyName("template_id")]
    public string? TemplateId { get; set; }

    /// <summary>
    /// 模板跳转链接（海外帐号没有跳转能力）
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// 模板跳小程序所需数据，不需跳小程序可不用传该数据
    /// </summary>
    [JsonPropertyName("miniprogram")]
    public Dictionary<string, string>? MiniProgram { get; set; }

    /// <summary>
    /// 模板数据
    /// </summary>
    [JsonPropertyName("data")]
    public Dictionary<string, string>? Data { get; set; }

    /// <summary>
    /// 防重入id。对于同一个openid + client_msg_id, 只发送一条消息,10分钟有效,超过10分钟不保证效果。若无防重入需求，可不填
    /// </summary>
    [JsonPropertyName("client_msg_id")]
    public string? ClientMsgId { get; set; }

    /// <summary>
    /// access_token
    /// </summary>
    [JsonIgnore]
    public string? AccessToken { get; set; }
}
