namespace RequestLogger.API.Logging
{
    public record LogEntry(
        DateTimeOffset Timestamp,
        string Method,
        string Path,
        int StatusCode,
        double DurationMs,
        string Message);
}
