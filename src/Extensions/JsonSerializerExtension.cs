// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Extensions;

#region Json扩展
/// <summary>
/// Json扩展
/// </summary>
public static class JsonSerializerExtension
{
    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="namingPolicy">命名策略</param>
    /// <returns></returns>
    public static string SerializerObject<TValue>(this TValue value, JsonNamingPolicy? namingPolicy = null)
    {
        return JsonSerializer.Serialize(value, JsonOptions(namingPolicy));
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="namingPolicy">命名策略</param>
    /// <returns></returns>
    public static TValue? DeserializeObject<TValue>(this string? value, JsonNamingPolicy? namingPolicy = null)
    {
        return value == null ? default : JsonSerializer.Deserialize<TValue>(value, JsonOptions(namingPolicy));
    }
    /// <summary>
    /// JsonOptions
    /// </summary>
    /// <param name="namingPolicy">命名策略</param>
    /// <returns></returns>
    static JsonSerializerOptions JsonOptions(JsonNamingPolicy? namingPolicy = null)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        options.Converters.Add(new DateTimeJsonConverter());

        return options;
    }
}
#endregion
