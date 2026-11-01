using System.Net;
using System.Net.Sockets;
using System.Text;

const int Port = 5000;
const string LogFile = "server.log";

var quotes = new[]
{
    "Борітеся — поборете! — Тарас Шевченко",
    "Учітеся, брати мої, думайте, читайте. — Тарас Шевченко",
    "Contra spem spero! (Без надії сподіваюсь) — Леся Українка",
    "Лиш боротись — значить жить. — Іван Франко",
    "Світ ловив мене, та не спіймав. — Григорій Сковорода",
    "Подорож у тисячу миль починається з першого кроку. — Лао-цзи",
    "Знання — сила. — Френсіс Бекон",
    "Хто не ризикує, той не п'є шампанського. — народна мудрість"
};

var logLock = new object();

Console.OutputEncoding = Encoding.UTF8;

void Log(string message)
{
    var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

    lock (logLock)
    {
        Console.WriteLine(line);
        File.AppendAllText(LogFile, line + Environment.NewLine, Encoding.UTF8);
    }
}

async Task HandleClientAsync(TcpClient client)
{
    var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "невідомий клієнт";

    Log($"Підключився клієнт {endpoint}");

    try
    {
        using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };

        while (true)
        {
            var request = await reader.ReadLineAsync();

            if (request is null)
            {
                break;
            }

            request = request.Trim().ToUpperInvariant();

            if (request == "QUOTE")
            {
                var quote = quotes[Random.Shared.Next(quotes.Length)];
                await writer.WriteLineAsync(quote);
                Log($"Клієнт {endpoint}: запит цитати. Надіслано: \"{quote}\"");
            }
            else if (request == "EXIT")
            {
                break;
            }
            else
            {
                await writer.WriteLineAsync("ERROR: невідома команда");
                Log($"Клієнт {endpoint}: невідома команда \"{request}\"");
            }
        }
    }
    catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException)
    {
        Log($"Клієнт {endpoint}: з'єднання перервано ({ex.Message})");
    }
    finally
    {
        client.Close();
        Log($"Клієнт {endpoint} відключився");
    }
}

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();

Log($"Сервер цитат запущено на порту {Port}. Лог пишеться у файл {LogFile}");

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    _ = Task.Run(() => HandleClientAsync(client));
}
