using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SkyRoute.API.Exceptions
{
    /// <summary>
    /// Maps application exceptions to RFC 7807 ProblemDetails responses.
    /// Registered via UseExceptionHandler / AddExceptionHandler in Program.cs.
    /// </summary>
    internal sealed class SkyRouteExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<SkyRouteExceptionHandler> _logger;

        public SkyRouteExceptionHandler(ILogger<SkyRouteExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                ArgumentNullException or
                ArgumentException or
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad Request"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            _logger.LogError(
                exception,
                "Unhandled exception [{Type}] → HTTP {StatusCode}: {Message}",
                exception.GetType().Name, statusCode, exception.Message);

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            problem.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }
    }
}
