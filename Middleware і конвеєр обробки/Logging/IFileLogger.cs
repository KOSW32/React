namespace RequestLogger.API.Logging
{
    public interface IFileLogger
    {
        Task WriteAsync(LogEntry entry);
        Task<IReadOnlyList<LogEntry>> ReadLastAsync(int count);
    }
}
