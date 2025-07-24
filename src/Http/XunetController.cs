// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Http;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// 控制器基类
/// </summary>
[ApiController]
public class XunetController : ControllerBase
{
    /// <summary>
    /// 通用返回
    /// </summary>
    /// <returns></returns>
    [NonAction]
    protected virtual IActionResult XunetResult()
    {
        return Ok(new OperateResultDto());
    }

    /// <summary>
    /// 通用返回
    /// </summary>
    /// <typeparam name="T">类型参数</typeparam>
    /// <param name="value">值</param>
    /// <returns></returns>
    [NonAction]
    protected virtual IActionResult XunetResult<T>(T value) where T : OperateResultDto, new()
    {
        return Ok(value);
    }
}
