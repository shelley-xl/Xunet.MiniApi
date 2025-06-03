// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Weixin.Dtos;

/// <summary>
/// 发送模板消息返回
/// </summary>
public class SendTemplateMessageDto : WeixinErrorDto
{
    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonPropertyName("msgid")]
    public long? MsgId { get; set; }
}
