﻿// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Tencent.MiniProgram.Dtos.Requests;

/// <summary>
/// 发送订阅消息请求
/// </summary>
public class SendSubscribeMessageRequest
{
    /// <summary>
    /// 所需下发的订阅模板id
    /// </summary>
    [JsonPropertyName("template_id")]
    public string? TemplateId { get; set; }

    /// <summary>
    /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转
    /// </summary>
    [JsonPropertyName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// 接收者（用户）的 openid
    /// </summary>
    [JsonPropertyName("touser")]
    public string? ToUser { get; set; }

    /// <summary>
    /// 模板内容，格式形如{ "phrase3": { "value": "审核通过" }, "name1": { "value": "订阅" }, "date2": { "value": "2019-12-25 09:42" } }
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; set; }

    /// <summary>
    /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
    /// </summary>
    [JsonPropertyName("miniprogram_state")]
    public string? MiniProgramState { get; set; }

    /// <summary>
    /// 进入小程序查看”的语言类型，支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，默认为zh_CN
    /// </summary>
    [JsonPropertyName("lang")]
    public string? Lang { get; set; }

    /// <summary>
    /// 接口调用凭证
    /// </summary>
    [JsonIgnore]
    public string? AccessToken { get; set; }
}
