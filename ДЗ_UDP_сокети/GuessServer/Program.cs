using System.Net;
using System.Net.Sockets;
using System.Text;

const int Port = 9000;
const int Min = 1;
const int Max = 100;

Console.OutputEncoding = Encoding.UTF8;

var secret = Random.Shared.Next(Min, Max + 1);
var players = new HashSet<IPEndPoint>();

using var server = new UdpClient(Port);

if (OperatingSystem.IsWindows())
{
    server.Client.IOControl(-1744830452, new byte[] { 0 }, null);
}

Console.WriteLine($"Сервер запущено на UDP-порту {Port}.");
Console.WriteLine($"Загадане число: {secret} (видно лише на сервері)");
Console.WriteLine("Очікування гравців...");

void Send(IPEndPoint target, string text)
{
    var bytes = Encoding.UTF8.GetBytes(text);
    server.Send(bytes, bytes.Length, target);
}

var remote = new IPEndPoint(IPAddress.Any, 0);
var finished = false;

while (!finished)
{
    byte[] data;

    try
    {
        data = server.Receive(ref remote);
    }
    catch (SocketException)
    {
        continue;
    }

    var sender = new IPEndPoint(remote.Address, remote.Port);
    var message = Encoding.UTF8.GetString(data).Trim();

    players.Add(sender);
    Console.WriteLine($"{sender} -> {message}");

    if (message.Equals("JOIN", StringComparison.OrdinalIgnoreCase))
    {
        Send(sender, $"Ви в грі! Вгадайте число від {Min} до {Max}.");
        continue;
    }

    if (!int.TryParse(message, out var guess) || guess < Min || guess > Max)
    {
        Send(sender, $"Некоректне значення. Введіть ціле число від {Min} до {Max}.");
        continue;
    }

    if (guess < secret)
    {
        Send(sender, "Занадто мало");
    }
    else if (guess > secret)
    {
        Send(sender, "Занадто багато");
    }
    else
    {
        var winnerMessage = $"Гравець із {sender} переміг!";

        foreach (var player in players)
        {
            Send(player, winnerMessage);
        }

        Console.WriteLine(winnerMessage);
        finished = true;
    }
}

Console.WriteLine("Гру завершено. Сервер зупиняється.");
