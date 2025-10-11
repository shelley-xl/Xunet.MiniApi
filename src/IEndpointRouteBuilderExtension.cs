// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi;

/// <summary>
/// IEndpointRouteBuilder扩展
/// </summary>
public static class IEndpointRouteBuilderExtension
{
    /// <summary>
    /// MapGroup
    /// </summary>
    /// <param name="endpoints"></param>
    /// <param name="prefix"></param>
    /// <param name="name"></param>
    /// <param name="auth"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static RouteGroupBuilder MapGroup(this IEndpointRouteBuilder endpoints, string prefix, string name, bool auth, params string[] tags)
    {
        var group = endpoints
            .MapGroup(prefix)
            .WithGroupName(name)
            .WithTags(tags)
            .AddEndpointFilter<AutoValidationFilter>();

        if (auth) group.RequireAuthorization(AuthorizePolicy.Default);

        return group;
    }

    /// <summary>
    /// MapGet
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapGet<TService>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, Task<IResult>> handler)
    {
        return endpoints
            .MapGet(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapGet
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapGet<TService, TRequest>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, TRequest, Task<IResult>> handler) where TRequest : class, new()
    {
        return endpoints
            .MapGet(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapPut
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapPut<TService>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, Task<IResult>> handler)
    {
        return endpoints
            .MapPut(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapPut
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapPut<TService, TRequest>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, TRequest, Task<IResult>> handler) where TRequest : class, new()
    {
        return endpoints
            .MapPut(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapPatch
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapPatch<TService>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, Task<IResult>> handler)
    {
        return endpoints
            .MapPatch(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapPatch
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapPatch<TService, TRequest>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, TRequest, Task<IResult>> handler) where TRequest : class, new()
    {
        return endpoints
            .MapPatch(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapPost
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapPost<TService>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, Task<IResult>> handler)
    {
        return endpoints
            .MapPost(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapPost
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapPost<TService, TRequest>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, TRequest, Task<IResult>> handler) where TRequest : class, new()
    {
        return endpoints
            .MapPost(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapDelete
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapDelete<TService>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, Task<IResult>> handler)
    {
        return endpoints
            .MapDelete(pattern, handler)
            .WithSummary(summary);
    }

    /// <summary>
    /// MapDelete
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="endpoints"></param>
    /// <param name="pattern"></param>
    /// <param name="summary"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapDelete<TService, TRequest>(this IEndpointRouteBuilder endpoints, string pattern, string summary, Func<TService, TRequest, Task<IResult>> handler) where TRequest : class, new()
    {
        return endpoints
            .MapDelete(pattern, handler)
            .WithSummary(summary);
    }
}
