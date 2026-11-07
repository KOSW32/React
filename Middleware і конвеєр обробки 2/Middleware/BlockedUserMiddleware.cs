using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BlockedUserMiddleware.API.Middleware
{
    public class BlockedUserMiddleware
    {
        private const string UserIdHeader = "X-User-Id";
        private const string ProblemContentType = "application/problem+json";

        private static readonly HashSet<Guid> BlockedUserIds = new()
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("d4f6b1c2-8a3e-4c57-9b1d-5e2f7a9c0b13")
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<BlockedUserMiddleware> _logger;

        public BlockedUserMiddleware(RequestDelegate next, ILogger<BlockedUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(UserIdHeader, out var headerValues))
            {
                await _next(context);
                return;
            }

            if (headerValues.Count != 1 || !Guid.TryParse(headerValues[0], out var userId))
            {
                await WriteProblemAsync(context, new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid X-User-Id",
                    Detail = "Заголовок X-User-Id має містити коректний GUID.",
                    Instance = context.Request.Path
                });
                return;
            }

            if (BlockedUserIds.Contains(userId))
            {
                _logger.LogWarning("Заблокований користувач {UserId} намагався звернутися до {Path}", userId, context.Request.Path);

                await WriteProblemAsync(context, new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "User Blocked",
                    Detail = "Ваш акаунт заблоковано за порушення правил сервісу.",
                    Instance = context.Request.Path
                });
                return;
            }

            await _next(context);
        }

        private static async Task WriteProblemAsync(HttpContext context, ProblemDetails problem)
        {
            context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = ProblemContentType;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
        }
    }

    public static class BlockedUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseBlockedUser(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BlockedUserMiddleware>();
        }
    }
}
