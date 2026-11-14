using System.Diagnostics;

namespace LearnApp.Api.Infrastructure;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    private const string HeaderName = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context) 
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault() ?? Guid.NewGuid().ToString("N");
        context.Response.Headers[HeaderName] = correlationId;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        } finally 
        {
            stopwatch.Stop();
            logger.LogInformation("{Method} {Path} -> {StatusCode} in {ElapsedMs} ms (correlation {CorrelationId})",
                context.Request.Method, context.Request.Path, context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds, correlationId);
        }

    }
}
