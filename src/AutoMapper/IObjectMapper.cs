// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.AutoMapper;

/// <summary>
/// 对象映射器
/// </summary>
public interface IObjectMapper
{
    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDestination Map<TDestination>(object? source);

    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDestination Map<TSource, TDestination>(TSource? source);

    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    TDestination Map<TSource, TDestination>(TSource? source, TDestination? destination);
}
