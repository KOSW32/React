using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.Write("Адреса сервера [127.0.0.1]: ");
var host = Console.ReadLine();
if (string.IsNullOrWhiteSpace(host))
{
    host = "127.0.0.1";
}

Console.Write("Порт [5000]: ");
var portText = Console.ReadLine();
var port = int.TryParse(portText, out var parsedPort) ? parsedPort : 5000;

try
{
    using var client = new TcpClient();
    await client.ConnectAsync(host.Trim(), port);

    Console.WriteLine($"Підключено до {host.Trim()}:{port}");
    Console.WriteLine("Enter — отримати цитату, Q — вийти.");

    using var stream = client.GetStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };

    while (true)
    {
        var input = Console.ReadLine();

        if (input is null || input.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
        {
            await writer.WriteLineAsync("EXIT");
            break;
        }

        await writer.WriteLineAsync("QUOTE");
        var quote = await reader.ReadLineAsync();

        if (quote is null)
        {
            Console.WriteLine("Сервер закрив з'єднання.");
            break;
        }

        Console.WriteLine();
        Console.WriteLine($"  \"{quote}\"");
        Console.WriteLine();
    }
}
catch (SocketException ex)
{
    Console.WriteLine($"Не вдалося підключитися до сервера: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"З'єднання перервано: {ex.Message}");
}

Console.WriteLine("Клієнт завершив роботу.");
