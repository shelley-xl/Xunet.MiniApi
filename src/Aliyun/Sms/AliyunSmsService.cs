// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

using Aliyun.Credentials;
using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using AlibabaCloud.TeaUtil.Models;

namespace Xunet.MiniApi.Aliyun.Sms;

/// <summary>
/// 阿里云短信服务
/// </summary>
internal class AliyunSmsService(IConfiguration config) : IAliyunSmsService
{
    /// <summary>
    /// 发送短信验证码
    /// </summary>
    /// <param name="request">发送短信验证码请求</param>
    /// <returns></returns>
    public async Task<List<SendSmsCodeDto>> SendSmsAsync(SendSmsCodeRequest request)
    {
        // 生成6位随机验证码
        byte[] bytes = new byte[4];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(bytes);
        var number = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
        var randomNumber = (number % 1000000).ToString("D6");

        var credential = new Client();

        // Endpoint 请参考 https://api.aliyun.com/product/Dysmsapi
        var configs = new Config
        {
            AccessKeyId = config["AlibabaCloudSettings:AccessKeyId"],
            AccessKeySecret = config["AlibabaCloudSettings:AccessKeySecret"],
            Credential = credential,
            Endpoint = "dysmsapi.aliyuncs.com",
        };

        var client = new AlibabaCloud.SDK.Dysmsapi20170525.Client(configs);

        var sendSmsRequest = new SendSmsRequest
        {
            PhoneNumbers = string.Join(',', request.PhoneNumber),
            SignName = config["AlibabaCloudSettings:Sms:SignName"],
            TemplateCode = config["AlibabaCloudSettings:Sms:TemplateCode"],
            TemplateParam = JsonSerializer.Serialize(new { code = randomNumber }),
        };

        var runtime = new RuntimeOptions();

        var result = await client.SendSmsWithOptionsAsync(sendSmsRequest, runtime);

        var list = new List<SendSmsCodeDto>();
        foreach (var item in request.PhoneNumber)
        {
            list.Add(new SendSmsCodeDto
            {
                Success = result.StatusCode == 200,
                Message = result.StatusCode == 200 ? "ok" : "fail",
                PhoneNumber = item,
            });
        }

        return list;
    }
}
