using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Start] Handling {typeof(TRequest).Name} with content: {request}");

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        if (timer.Elapsed.Seconds > 3)
        {
            logger.LogInformation($"[Performance Warning] The request {typeof(TRequest).Name} took {timer.ElapsedMilliseconds}ms");
        }

        logger.LogInformation($"[End] Handled {typeof(TRequest).Name} with content: {response}");
        return response;
    }
}
