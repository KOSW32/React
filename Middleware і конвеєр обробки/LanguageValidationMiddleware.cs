using Microsoft.AspNetCore.Mvc;

namespace RequestLogger.API.Middleware;

public sealed class LanguageValidationMiddleware
{
    private const string UnsupportedLanguageDetail =
        "Обрана мова не підтримується сервісом. Будь ласка, використовуйте uk-UA або en-US.";

    private readonly RequestDelegate _next;

    public LanguageValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();

        if (ContainsRussianLanguage(acceptLanguage))
        {
            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status406NotAcceptable,
                Title = "Unsupported Language",
                Detail = UnsupportedLanguageDetail,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(
                problemDetails,
                contentType: "application/problem+json",
                cancellationToken: context.RequestAborted);
            return;
        }

        context.Items["Language"] = string.IsNullOrWhiteSpace(acceptLanguage)
            ? "uk-UA"
            : GetFirstLanguage(acceptLanguage);

        await _next(context);
    }

    private static bool ContainsRussianLanguage(string acceptLanguage)
    {
        return acceptLanguage
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(value => value.Split(';', 2)[0].Trim())
            .Any(language =>
                string.Equals(language, "ru", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(language, "ru-RU", StringComparison.OrdinalIgnoreCase));
    }

    private static string GetFirstLanguage(string acceptLanguage)
    {
        return acceptLanguage
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0]
            .Split(';', 2)[0]
            .Trim();
    }
}
