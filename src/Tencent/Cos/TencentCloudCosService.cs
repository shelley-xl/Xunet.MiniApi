// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model.Bucket;
using COSXML.Model.Object;

namespace Xunet.MiniApi.Tencent.Cos;

/// <summary>
/// 腾讯云对象存储服务
/// </summary>
internal class TencentCloudCosService(IConfiguration config) : ITencentCloudCosService
{
    /// <summary>
    /// 存储任务的容器
    /// </summary>
    static readonly ConcurrentDictionary<string, List<UploadProgressDto>> _progresses = new();

    CosXmlServer CosXmlServer(string region)
    {
        var secretId = config["TencentCloudSettings:SecretId"];
        var secretKey = config["TencentCloudSettings:SecretKey"];

        CosXmlConfig cosXmlConfig = new CosXmlConfig.Builder()
            .IsHttps(true)
            .SetRegion(region)
            .SetDebugLog(true)
            .Build();

        QCloudCredentialProvider cosCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, 600);

        return new CosXmlServer(cosXmlConfig, cosCredentialProvider);
    }

    /// <summary>
    /// 创建任务
    /// </summary>
    /// <returns></returns>
    public string CreateTask()
    {
        var taskId = Guid.NewGuid().ToString("N");

        _progresses.TryAdd(taskId, []);

        // 自动清理缓存：设置2小时后自动移除进度
        JobManager.AddJob(() => _progresses.TryRemove(taskId, out _), (x) =>
        {
            x.WithName($"remove:progress:{taskId}");
            x.ToRunOnceIn(7200).Seconds();
        });

        return taskId;
    }

    /// <summary>
    /// 获取进度
    /// </summary>
    /// <param name="taskId">任务id</param>
    /// <returns></returns>
    public List<UploadProgressDto> GetProgress(string taskId)
    {
        if (_progresses.TryGetValue(taskId, out var progress))
        {
            return progress;
        }
        return [];
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="taskId">任务id</param>
    /// <param name="content">文件流</param>
    /// <param name="key">文件key</param>
    /// <param name="bucket">bucket</param>
    /// <param name="region">地区</param>
    public string PutObject(string taskId, Stream? content, string key, string bucket, string region)
    {
        if (!_progresses.TryGetValue(taskId, out var progress))
        {
            throw new ArgumentException($"任务{taskId}不存在");
        }

        var index = progress.Count;

        progress.Add(new UploadProgressDto());

        var request = new PutObjectRequest(bucket, key, content);

        request.SetCosProgressCallback((long completed, long total) =>
        {
            progress[index].BytesUploaded = completed;
            progress[index].TotalBytes = total;
        });

        var result = CosXmlServer(region).PutObject(request);

        if (!result.IsSuccessful())
        {
            throw new CosServerException(result.httpCode, result.httpMessage);
        }

        if (!key.StartsWith('/')) key = $"/{key}";

        return $"https://{bucket}.file.myqcloud.com{key}";
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="content">文件流</param>
    /// <param name="key">文件key</param>
    /// <param name="bucket">bucket</param>
    /// <param name="region">地区</param>
    public string PutObject(Stream? content, string key, string bucket, string region)
    {
        var request = new PutObjectRequest(bucket, key, content);

        var result = CosXmlServer(region).PutObject(request);

        if (!result.IsSuccessful())
        {
            throw new CosServerException(result.httpCode, result.httpMessage);
        }

        if (!key.StartsWith('/')) key = $"/{key}";

        return $"https://{bucket}.file.myqcloud.com{key}";
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="keys">文件key列表</param>
    /// <param name="bucket">bucket</param>
    /// <param name="region">地区</param>
    public void DeleteObject(IList<string> keys, string bucket, string region)
    {
        var request = new DeleteMultiObjectRequest(bucket);

        request.SetObjectKeys([.. keys]);

        var result = CosXmlServer(region).DeleteMultiObjects(request);

        if (!result.IsSuccessful())
        {
            throw new CosServerException(result.httpCode, result.httpMessage);
        }
    }

    /// <summary>
    /// 查找文件key列表
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <param name="bucket">bucket</param>
    /// <param name="region">地区</param>
    /// <returns></returns>
    public IList<string> GetKeys(string prefix, string bucket, string region)
    {
        var request = new GetBucketRequest(bucket);

        request.SetPrefix(prefix);

        var result = CosXmlServer(region).GetBucket(request);

        if (!result.IsSuccessful())
        {
            throw new CosServerException(result.httpCode, result.httpMessage);
        }

        return result.listBucket.contentsList.Select(x => x.key).ToList();
    }
}
