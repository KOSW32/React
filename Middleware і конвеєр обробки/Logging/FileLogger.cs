using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RequestLogger.API.Logging
{
    public class FileLogger : IFileLogger
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly string _directory;

        public FileLogger(IConfiguration configuration, IHostEnvironment environment)
        {
            var configured = configuration["FileLogger:Directory"] ?? "logs";

            _directory = Path.IsPathRooted(configured)
                ? configured
                : Path.Combine(environment.ContentRootPath, configured);
        }

        public async Task WriteAsync(LogEntry entry)
        {
            var line = JsonSerializer.Serialize(entry, JsonOptions) + Environment.NewLine;

            await _lock.WaitAsync();

            try
            {
                Directory.CreateDirectory(_directory);
                await File.AppendAllTextAsync(GetPathFor(entry.Timestamp), line, Encoding.UTF8);
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine($"Не вдалося записати лог: {ex.Message}");
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<IReadOnlyList<LogEntry>> ReadLastAsync(int count)
        {
            var path = GetPathFor(DateTimeOffset.Now);

            if (!File.Exists(path))
            {
                return new List<LogEntry>();
            }

            string content;

            await using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            var entries = new List<LogEntry>();

            foreach (var line in content.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                try
                {
                    var entry = JsonSerializer.Deserialize<LogEntry>(line, JsonOptions);

                    if (entry is not null)
                    {
                        entries.Add(entry);
                    }
                }
                catch (JsonException)
                {
                }
            }

            return entries.TakeLast(count).Reverse().ToList();
        }

        private string GetPathFor(DateTimeOffset timestamp)
        {
            return Path.Combine(_directory, $"requests-{timestamp:yyyy-MM-dd}.log");
        }
    }
}
