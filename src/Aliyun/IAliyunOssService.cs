// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun;

/// <summary>
/// 阿里云对象存储服务
/// </summary>
public interface IAliyunOssService
{
    /// <summary>
    /// 创建任务
    /// </summary>
    /// <returns></returns>
    string CreateTask();

    /// <summary>
    /// 获取进度
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    List<UploadProgressDto> GetProgress(string taskId);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="taskId">任务id</param>
    /// <param name="content">文件流</param>
    /// <param name="key">文件key</param>
    /// <param name="bucket">bucket</param>
    /// <param name="area">地区</param>
    /// <returns>文件地址</returns>
    string PutObject(string taskId, Stream? content, string key, string bucket, string area);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="content">文件流</param>
    /// <param name="key">文件key</param>
    /// <param name="bucket">bucket</param>
    /// <param name="area">地区</param>
    /// <returns>文件地址</returns>
    string PutObject(Stream? content, string key, string bucket, string area);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="keys">文件key列表</param>
    /// <param name="bucket">bucket</param>
    /// <param name="area">地区</param>
    void DeleteObject(IList<string> keys, string bucket, string area);

    /// <summary>
    /// 查找文件key列表
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <param name="bucket">bucket</param>
    /// <param name="area">地区</param>
    /// <returns></returns>
    IList<string> GetKeys(string prefix, string bucket, string area);
}
