using System.Diagnostics;

namespace RequestLogger.API.Logging
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFileLogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, IFileLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            var timestamp = DateTimeOffset.Now;
            var stopwatch = Stopwatch.StartNew();
            string? errorMessage = null;

            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                context.Response.StatusCode = 499;
                errorMessage = "Клієнт перервав запит";
            }
            catch (Exception ex)
            {
                errorMessage = $"Необроблений виняток {ex.GetType().Name}: {ex.Message}";

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { title = "Внутрішня помилка сервера" });
                }
            }

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;

            await _logger.WriteAsync(new LogEntry(
                timestamp,
                context.Request.Method,
                context.Request.Path + context.Request.QueryString.Value,
                statusCode,
                Math.Round(stopwatch.Elapsed.TotalMilliseconds, 2),
                errorMessage ?? DefaultMessage(statusCode)));
        }

        private static string DefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                >= 500 => "Помилка сервера",
                >= 400 => "Помилка клієнта",
                >= 300 => "Перенаправлення",
                _ => "Запит виконано успішно"
            };
        }
    }

    public static class RequestLoggingExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
