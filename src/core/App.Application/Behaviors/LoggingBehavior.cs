using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace App.Application.Behaviors;

/// <summary>
/// PIPELINE BEHAVIOR FOR LOGGING REQUEST PERFORMANCE AND EXECUTION.
/// </summary>
public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("LoggingBehavior: HANDLING REQUEST {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();

            stopwatch.Stop();
            logger.LogInformation("LoggingBehavior: REQUEST {RequestName} COMPLETED IN {ElapsedMilliseconds}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex,
                "LoggingBehavior: REQUEST {RequestName} FAILED AFTER {ElapsedMilliseconds}ms. ERROR: {ErrorMessage}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                ex.Message);
            throw;
        }
    }
}
