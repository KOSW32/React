using System.Net.Sockets;
using System.Text;

namespace GuessClient;

public class MainForm : Form
{
    private readonly TextBox _serverBox = new() { Text = "127.0.0.1" };
    private readonly TextBox _portBox = new() { Text = "9000" };
    private readonly Button _connectButton = new() { Text = "Підключитися" };
    private readonly TextBox _guessBox = new() { Enabled = false };
    private readonly Button _sendButton = new() { Text = "Надіслати", Enabled = false };
    private readonly TextBox _logBox = new()
    {
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Vertical
    };

    private UdpClient? _client;
    private CancellationTokenSource? _cts;

    public MainForm()
    {
        Text = "Хто перший вгадає (UDP-клієнт)";
        ClientSize = new Size(480, 400);
        MinimumSize = new Size(420, 340);
        StartPosition = FormStartPosition.CenterScreen;

        var serverLabel = new Label { Text = "Сервер:", Location = new Point(12, 16), AutoSize = true };
        _serverBox.Location = new Point(75, 12);
        _serverBox.Width = 140;

        var portLabel = new Label { Text = "Порт:", Location = new Point(228, 16), AutoSize = true };
        _portBox.Location = new Point(272, 12);
        _portBox.Width = 60;

        _connectButton.Location = new Point(345, 10);
        _connectButton.Size = new Size(120, 28);
        _connectButton.Click += OnConnect;

        var guessLabel = new Label { Text = "Ваша спроба (1–100):", Location = new Point(12, 56), AutoSize = true };
        _guessBox.Location = new Point(150, 52);
        _guessBox.Width = 70;

        _sendButton.Location = new Point(232, 50);
        _sendButton.Size = new Size(110, 28);
        _sendButton.Click += OnSend;

        _logBox.Location = new Point(12, 92);
        _logBox.Size = new Size(453, 296);
        _logBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        AcceptButton = _sendButton;

        Controls.AddRange(new Control[]
        {
            serverLabel, _serverBox, portLabel, _portBox, _connectButton,
            guessLabel, _guessBox, _sendButton, _logBox
        });

        FormClosing += (_, _) => Disconnect();
    }

    private void OnConnect(object? sender, EventArgs e)
    {
        if (!int.TryParse(_portBox.Text, out var port) || port < 1 || port > 65535)
        {
            MessageBox.Show("Вкажіть коректний порт (1–65535).", "Помилка");
            return;
        }

        try
        {
            _client = new UdpClient();
            _client.Connect(_serverBox.Text.Trim(), port);
            _cts = new CancellationTokenSource();

            AppendLog($"Підключено до {_serverBox.Text.Trim()}:{port}. Ваш локальний порт: {((System.Net.IPEndPoint)_client.Client.LocalEndPoint!).Port}");

            _connectButton.Enabled = false;
            _serverBox.Enabled = false;
            _portBox.Enabled = false;
            _guessBox.Enabled = true;
            _sendButton.Enabled = true;
            _guessBox.Focus();

            _ = ReceiveLoopAsync(_client, _cts.Token);
            SendText("JOIN");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Не вдалося підключитися");
            Disconnect();
            ResetUi();
        }
    }

    private void OnSend(object? sender, EventArgs e)
    {
        if (!int.TryParse(_guessBox.Text, out var guess) || guess < 1 || guess > 100)
        {
            MessageBox.Show("Введіть ціле число від 1 до 100.", "Некоректне значення");
            return;
        }

        AppendLog($"Ви: {guess}");
        SendText(guess.ToString());
        _guessBox.Clear();
        _guessBox.Focus();
    }

    private void SendText(string text)
    {
        if (_client is null)
        {
            return;
        }

        try
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            _client.Send(bytes, bytes.Length);
        }
        catch (Exception ex)
        {
            AppendLog($"Помилка надсилання: {ex.Message}");
        }
    }

    private async Task ReceiveLoopAsync(UdpClient client, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var result = await client.ReceiveAsync(token);
                var text = Encoding.UTF8.GetString(result.Buffer);

                AppendLog($"Сервер: {text}");

                if (text.Contains("переміг!"))
                {
                    AppendLog("Гру завершено.");
                    Disconnect();
                    _guessBox.Enabled = false;
                    _sendButton.Enabled = false;
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (SocketException)
            {
                AppendLog("Сервер недоступний.");
                Disconnect();
                ResetUi();
                break;
            }
        }
    }

    private void Disconnect()
    {
        _cts?.Cancel();
        _cts = null;
        _client?.Close();
        _client = null;
    }

    private void ResetUi()
    {
        _connectButton.Enabled = true;
        _serverBox.Enabled = true;
        _portBox.Enabled = true;
        _guessBox.Enabled = false;
        _sendButton.Enabled = false;
    }

    private void AppendLog(string line)
    {
        _logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {line}{Environment.NewLine}");
    }
}
