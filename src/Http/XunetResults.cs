// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Http;

/// <summary>
/// 通用返回IResult
/// </summary>
public static partial class XunetResults
{
    /// <summary>
    /// 操作成功
    /// </summary>
    /// <returns></returns>
    public static IResult Ok()
    {
        return Results.Ok(new OperateResultDto());
    }

    /// <summary>
    /// 数据查询
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="data">数据</param>
    /// <returns></returns>
    public static IResult Ok<T>(T data)
    {
        return Results.Ok(new QueryResultDto<T>
        {
            Data = data,
        });
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="request">分页请求</param>
    /// <param name="data">数据</param>
    /// <param name="totalNumber">总记录数</param>
    /// <returns></returns>
    public static IResult Ok<T>(List<T> data, PageRequest request, RefAsync<int> totalNumber) where T : class, new()
    {
        return Results.Ok(new PageResultDto<T>
        {
            Data = new PageObjectDto<T>
            {
                PageNum = request.Page,
                PageSize = request.Size,
                RecordCount = totalNumber,
                Rows = data,
            },
        });
    }

    /// <summary>
    /// 操作错误
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns></returns>
    public static IResult Error(string? message)
    {
        return Results.Ok(new OperateResultDto
        {
            Code = XunetCode.Error,
            Message = message ?? "fail",
        });
    }

    /// <summary>
    /// 操作失败
    /// </summary>
    /// <returns></returns>
    public static IResult Fail()
    {
        return Results.Ok(new OperateResultDto
        {
            Code = XunetCode.Failure,
            Message = "fail",
        });
    }
}
