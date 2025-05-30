// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.AutoMapper;

/// <summary>
/// 对象映射器
/// </summary>
/// <param name="mapper"></param>
public class ObjectMapper(IMapper mapper) : IObjectMapper
{
    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public TDestination Map<TDestination>(object? source)
    {
        if (source is null) return default!;

        return mapper.Map<TDestination>(source) ?? Activator.CreateInstance<TDestination>();
    }

    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public TDestination Map<TSource, TDestination>(TSource? source)
    {
        if (source is null) return default!;

        return mapper.Map<TSource, TDestination>(source) ?? Activator.CreateInstance<TDestination>();
    }

    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public TDestination Map<TSource, TDestination>(TSource? source, TDestination? destination)
    {
        if (source is null) return default!;

        return mapper.Map(source, destination) ?? Activator.CreateInstance<TDestination>();
    }
}
