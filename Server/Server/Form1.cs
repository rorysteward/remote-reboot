using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using System.Text.Json;

namespace Server
{
    public partial class Form1 : Form
    {
        private TcpListener server;
        private readonly ConcurrentDictionary<TcpClient, (NetworkStream Stream, string PCName, string IP)> clients = new();
        private CancellationTokenSource cts;
        private IPAddress server_ip;
        private int server_port;
        private bool server_status = false;
        private const string CONFIG_FILE = "config.xml";
        private bool include_this_computer;
        private int delay_execution = 5000;
        private double version = 0.1; 

        public Form1()
        {
            InitializeComponent();
        }

        private async Task StartServer()
        {
            if (server_status) return; // Prevent multiple starts

            try
            {
                server = new TcpListener(server_ip, server_port);
                server.Start();
                server_status = true;
                cts = new CancellationTokenSource();
                UpdateUI(true);

                while (!cts.Token.IsCancellationRequested)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    NetworkStream stream = client.GetStream();

                    // First message should contain PC Name
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string pcName = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                    clients[client] = (stream, pcName, clientIP);
                    UpdateConnectedClientsList();

                    _ = Task.Run(() => HandleClient(client, stream, cts.Token));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server Error: {ex.Message}");
                StopServer();
            }
        }

        private async Task HandleClient(TcpClient client, NetworkStream stream, CancellationToken token)
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (!token.IsCancellationRequested && client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0) break; // Client disconnected

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                    string response = HandleMessage(message);
                    byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length, token);
                }
            }
            catch
            {
                // Handle disconnection
            }
            finally
            {
                clients.TryRemove(client, out _);
                client.Close();
                UpdateConnectedClientsList();
            }
        }

        private string HandleMessage(string message)
        {
            switch (message)
            {
                case "0x10":
                    return "0x11"; // Respond with pong
                case "0x42":
                    return "Shutdown received!";
                default:
                    return "0x11";
            }
        }

        private void StopServer()
        {
            cts?.Cancel();
            foreach (var client in clients.Keys)
            {
                client.Close();
            }
            clients.Clear();
            UpdateConnectedClientsList();
            server?.Stop();
            server_status = false;
            UpdateUI(false);
        }
        private void UpdateConnectedClientsList()
        {
            this.Invoke((MethodInvoker)delegate
            {
                list_of_pc.Rows.Clear();
                foreach (var client in clients.Values)
                {
                    list_of_pc.Rows.Add(client.PCName, client.IP);
                }
            });
        }
        private void UpdateUI(bool isRunning)
        {
            this.Invoke((MethodInvoker)delegate
            {
                start_stop_button.Text = isRunning ? "Stop Server" : "Start Server";
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfig();
            this.Text += $" {this.version}";
            if (this.server_ip != null)
            {
                _ = StartServer();
            }
        }

        private void LoadConfig()
        {
            if (File.Exists(CONFIG_FILE))
            {
                try
                {
                    XDocument xml = XDocument.Load(CONFIG_FILE);
                    XElement config = xml.Element("config");
                    if (config != null)
                    {
                        string ip = config.Element("server_ip")?.Value;
                        string port = config.Element("server_port")?.Value;
                        string include_this_pc = config.Element("include_this_pc")?.Value;

                        if (IPAddress.TryParse(ip, out IPAddress server_ip) && int.TryParse(port, out int server_port) && bool.TryParse(include_this_pc, out bool this_pc))
                        {
                            this.server_ip = server_ip;
                            this.server_port = server_port;
                            this.include_this_computer = this_pc;
                            this.server_ip_text.Text = $"{this.server_ip}:{this.server_port}";
                            this.include_this_checkbox.Checked = this.include_this_computer;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load configuration: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool SaveConfig()
        {
            try
            {
                XDocument xml = new XDocument(
                    new XElement("config",
                        new XElement("server_ip", this.server_ip),
                        new XElement("server_port", this.server_port),
                        new XElement("include_this_pc", this.include_this_checkbox.Checked.ToString())
                    )
                );
                xml.Save(CONFIG_FILE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private async void start_stop_button_Click(object sender, EventArgs e)
        {
            if (!server_status)
            {
                string[] parts = server_ip_text.Text.Split(':');
                if (parts.Length != 2 || !IPAddress.TryParse(parts[0], out IPAddress ip) || !int.TryParse(parts[1], out int port))
                {
                    MessageBox.Show("Invalid format. Use 'IP:Port - 127.0.0.1:54300'", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.server_ip = ip;
                this.server_port = port;
                
                await StartServer();
            }
            else
            {
                StopServer();
            }
        }
        private void include_this_pc_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfig();
        }
        private async void shutdown_button_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("0x42");
            foreach (var client in clients.Values)
            {
                await client.Stream.WriteAsync(buffer, 0, buffer.Length);
            }
            if (this.include_this_computer)
            {
                Thread.Sleep(this.delay_execution); // delay
                Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
               {
                    CreateNoWindow = true,
                    UseShellExecute = false
               });
            }
        }
        private async void reset_button_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("0x10");
            foreach (var client in clients.Values)
            {
                await client.Stream.WriteAsync(buffer, 0, buffer.Length);
            }
            if (this.include_this_computer)
            {
               Thread.Sleep(this.delay_execution); // delay
               Process.Start(new ProcessStartInfo("shutdown", "/r /t 0")
               {
                   CreateNoWindow = true,
                   UseShellExecute = false
               });
            }
        }
        private void close_button_Click(object sender, EventArgs e)
        {
            StopServer();
            Application.Exit();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

    }
}