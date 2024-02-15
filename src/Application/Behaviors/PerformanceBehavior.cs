using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Behaviors;

internal sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private const int MaxElapsedMilliseconds = 1000;
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();
        
        timer.Start();

        var result = await next();
        
        timer.Stop();
        
        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > MaxElapsedMilliseconds)
        {
            logger.LogWarning(
                "Long running request {@RequestName} {@ElapsedMilliseconds} ms, {@DateTimeUtc}",
                typeof(TRequest).Name,
                elapsedMilliseconds,
                DateTime.UtcNow);
        }

        return result;
    }
}