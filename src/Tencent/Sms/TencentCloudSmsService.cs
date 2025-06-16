// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;

namespace Xunet.MiniApi.Tencent.Sms;

/// <summary>
/// 腾讯云短信服务
/// </summary>
internal class TencentCloudSmsService(IConfiguration config) : ITencentCloudSmsService
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

        // 实例化一个认证对象，入参需要传入腾讯云账户密钥对 SecretId，SecretKey。
        var credential = new Credential
        {
            SecretId = config["TencentCloudSettings:SecretId"],
            SecretKey = config["TencentCloudSettings:SecretKey"],
        };

        // 实例化一个客户端配置对象，可以指定超时时间等配置
        var clientProfile = new ClientProfile
        {
            // 默认用TC3-HMAC-SHA256进行签名
            SignMethod = ClientProfile.SIGN_TC3SHA256,
            // 实例化一个客户端配置对象，可以指定超时时间等配置
            HttpProfile = new HttpProfile
            {
                // 请求连接超时时间，单位为秒（默认60秒）
                Timeout = 60,
                // 指定接入地域域名（默认就近接入）
                Endpoint = "sms.tencentcloudapi.com",
            },
        };

        // 实例化一个短信客户端
        var client = new SmsClient(credential, "ap-guangzhou", clientProfile);

        // 实例化一个发送请求
        var sendRequest = new SendSmsRequest
        {
            // 短信应用ID
            SmsSdkAppId = config["TencentCloudSettings:Sms:AppId"],
            // 短信模版ID
            TemplateId = config["TencentCloudSettings:Sms:TemplateId"],
            // 短信签名内容
            SignName = config["TencentCloudSettings:Sms:SignName"],
            // 模板参数
            TemplateParamSet = [randomNumber],
            // 下发手机号码，采用 E.164 标准，+[国家或地区码][手机号]
            PhoneNumberSet = request.PhoneNumber,
        };

        // 发送短信验证码
        var response = await client.SendSms(sendRequest);

        // 获取发送状态
        var list = new List<SendSmsCodeDto>();
        foreach (var status in response.SendStatusSet)
        {
            list.Add(new SendSmsCodeDto
            {
                Success = status?.Code == "Ok",
                Message = status?.Message,
                PhoneNumber = status?.PhoneNumber,
            });
        }

        return list;
    }
}
