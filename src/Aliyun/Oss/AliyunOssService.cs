// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace Xunet.MiniApi.Aliyun.Oss;

/// <summary>
/// 阿里云对象存储服务
/// </summary>
internal class AliyunOssService(IConfiguration config) : IAliyunOssService
{
    /// <summary>
    /// 存储任务的容器
    /// </summary>
    static readonly ConcurrentDictionary<string, List<UploadProgressDto>> _progresses = new();

    string? AccessKeyId => config["AlibabaCloudSettings:AccessKeyId"];

    string? AccessKeySecret => config["AlibabaCloudSettings:AccessKeySecret"];

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
    /// <returns>文件地址</returns>
    public string PutObject(string taskId, Stream? content, string key, string bucket, string region)
    {
        if (!_progresses.TryGetValue(taskId, out var progress))
        {
            throw new ArgumentException($"任务{taskId}不存在");
        }

        var index = progress.Count;

        progress.Add(new UploadProgressDto());

        var request = new PutObjectRequest(bucket, key, content);

        request.StreamTransferProgress += (object? sender, StreamTransferProgressArgs args) =>
        {
            progress[index].BytesUploaded = args.TransferredBytes;
            progress[index].TotalBytes = args.TotalBytes;
        };

        var endpoint = $"oss-cn-{region}.aliyuncs.com";
        var client = new OssClient(endpoint, AccessKeyId, AccessKeySecret);

        var result = client.PutObject(request);

        if (result.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new OssException("上传文件失败");
        }

        if (!key.StartsWith('/')) key = $"/{key}";

        return $"https://{bucket}.{endpoint}{key}";
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="content">文件流</param>
    /// <param name="key">文件key</param>
    /// <param name="bucket">bucket</param>
    /// <param name="region">地区</param>
    /// <returns>文件地址</returns>
    public string PutObject(Stream? content, string key, string bucket, string region)
    {
        var request = new PutObjectRequest(bucket, key, content);
        
        var endpoint = $"oss-cn-{region}.aliyuncs.com";
        var client = new OssClient(endpoint, AccessKeyId, AccessKeySecret);

        var result = client.PutObject(request);

        if (result.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new OssException("上传文件失败");
        }

        if (!key.StartsWith('/')) key = $"/{key}";

        return $"https://{bucket}.{endpoint}{key}";
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="keys">文件key列表</param>
    /// <param name="bucket">bucket</param>
    /// <param name="region">地区</param>
    public void DeleteObject(IList<string> keys, string bucket, string region)
    {
        var request = new DeleteObjectsRequest(bucket, keys);

        var endpoint = $"oss-cn-{region}.aliyuncs.com";
        var client = new OssClient(endpoint, AccessKeyId, AccessKeySecret);
        
        var result = client.DeleteObjects(request);

        if (result.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new OssException("删除文件失败");
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
        var endpoint = $"oss-cn-{region}.aliyuncs.com";
        var client = new OssClient(endpoint, AccessKeyId, AccessKeySecret);

        var result = client.ListObjects(bucket, prefix);

        if (result.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new OssException("查找文件失败");
        }

        return result.ObjectSummaries.Select(x => x.Key).ToList();
    }
}
