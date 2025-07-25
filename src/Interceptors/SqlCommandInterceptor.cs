// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Interceptors;

using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

/// <summary>
/// SqlCommand拦截器
/// </summary>
public class SqlCommandInterceptor : DbCommandInterceptor
{
    Stopwatch? _stopwatch = null;

    /// <summary>
    /// ReaderExecutingAsync
    /// </summary>
    /// <param name="command"></param>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        _stopwatch ??= Stopwatch.StartNew();

        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    /// <summary>
    /// ReaderExecutedAsync
    /// </summary>
    /// <param name="command"></param>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        _stopwatch?.Stop();

        try
        {
            await using var scope = XunetHttpContext.Current?.RequestServices.CreateAsyncScope();
            if (scope == null) return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);

            var duration = _stopwatch?.ElapsedMilliseconds ?? 0;

            var sql = command.CommandText;

            foreach (DbParameter par in command.Parameters)
            {
                sql = sql.Replace(par.ParameterName, $"'{par.Value}'");
            }

            var eventHandler = scope.Value.ServiceProvider.GetService<ISqlLogEventHandler>();
            if (eventHandler != null)
            {
                await eventHandler.InvokeAsync(sql, duration);
            }

            var connection = scope.Value.ServiceProvider.GetService<IConnection>();
            if (connection == null) return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);

            var queueName = "logs_sqls";

            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            var pars = new Dictionary<string, object?>
            {
                { "Id", Guid.NewGuid().ToString() },
                { "RequestId", XunetHttpContext.RequestId },
                { "TraceId", XunetHttpContext.TraceId },
                { "CommandText", sql },
                { "Duration", duration },
                { "IsFailed", 0 },
                { "Message", "" },
                { "Subject", (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name },
                { "CreateId", XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject)},
                { "CreateTime", DateTime.Now },
            };

            var message = pars.SerializerObject();

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: Encoding.UTF8.GetBytes(message), cancellationToken: cancellationToken);
        }
        finally
        {
            _stopwatch = null;
        }

        return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    /// <summary>
    /// CommandFailedAsync
    /// </summary>
    /// <param name="command"></param>
    /// <param name="eventData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        _stopwatch?.Stop();

        try
        {
            await using var scope = XunetHttpContext.Current?.RequestServices.CreateAsyncScope();
            if (scope == null) return;

            var duration = _stopwatch?.ElapsedMilliseconds ?? 0;

            var sql = command.CommandText;

            foreach (DbParameter par in command.Parameters)
            {
                sql = sql.Replace(par.ParameterName, $"'{par.Value}'");
            }

            var eventHandler = scope.Value.ServiceProvider.GetService<ISqlLogEventHandler>();
            if (eventHandler != null)
            {
                await eventHandler.InvokeAsync(sql, duration, true, eventData?.Exception?.Message);
            }

            var connection = scope.Value.ServiceProvider.GetService<IConnection>();
            if (connection == null) return;

            var queueName = "logs_sqls";

            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            var pars = new Dictionary<string, object?>
            {
                { "Id", Guid.NewGuid().ToString() },
                { "RequestId", XunetHttpContext.RequestId },
                { "TraceId", XunetHttpContext.TraceId },
                { "CommandText", sql },
                { "Duration", duration },
                { "IsFailed", 1 },
                { "Message", eventData?.Exception?.Message },
                { "Subject", (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name },
                { "CreateId", XunetHttpContext.Current?.User.FindFirstValue(OpenIddict.Abstractions.OpenIddictConstants.Claims.Subject)},
                { "CreateTime", DateTime.Now },
            };

            var message = pars.SerializerObject();

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: Encoding.UTF8.GetBytes(message), cancellationToken: cancellationToken);
        }
        finally
        {
            _stopwatch = null;
        }
    }
}
