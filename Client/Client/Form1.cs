using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
// Debug.WriteLine(data); // Dump var
namespace Client
{
    public partial class Form1 : Form
    {
        private string version = "0.01";
        private TcpClient tcp_client;
        private NetworkStream network_stream;
        private const string CONFIG_FILE = "config.xml";
        private string server_ip;
        private int server_port;
        private bool is_connected = false;
        private bool pong_response = false;
        private DateTime last_ping_sent;
        private System.Timers.Timer ping_timer;
        private BackgroundWorker worker;
        private bool reconnecting = false;
        private readonly int pong_timeout_period = 3; // 3 seconds
        private Int16 interval = 10000; // 10 seconds
        private string update_url = "https://";
        public Form1()
        {
            InitializeComponent();
        }
        private async void connect()
        {
            if (this.reconnecting) return; // Prevent multiple simultaneous reconnection attempts

            this.reconnecting = true;
            while (!this.is_connected) // Loop until connected
            {
                try
                {
                    Debug.WriteLine("Attempting to connect to server...");

                    if (this.tcp_client != null)
                    {
                        this.tcp_client.Close();
                        this.tcp_client = null;
                    }

                    string message = Environment.MachineName;
                    this.tcp_client = new TcpClient();
                    await this.tcp_client.ConnectAsync(this.server_ip, this.server_port);

                    if (this.tcp_client.Connected)
                    {
                        Debug.WriteLine("Connected to server. Sending init message.");
                        this.network_stream = tcp_client.GetStream();

                        // Init connection
                        Byte[] data = Encoding.ASCII.GetBytes(message);
                        await this.network_stream.WriteAsync(data, 0, data.Length);

                        // Update UI
                        this.Invoke((MethodInvoker)delegate
                        {
                            disconnected_status.Visible = false;
                            connected_status.Visible = true;
                            UpdateUI(true);
                        });

                        this.is_connected = true;
                        this.pong_response = false; // Important! Don't expect pong right away

                        // Start listening only if not already listening
                        if (worker == null || !worker.IsBusy)
                        {
                            listen();
                        }

                        // Only start ping timer if it's not already running
                        if (ping_timer == null || !ping_timer.Enabled)
                        {
                            keep_connection();
                        }

                        break; // Exit loop when connected successfully
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Connection error: {ex.Message}");

                    // Update UI on failure
                    this.Invoke((MethodInvoker)delegate
                    {
                        disconnected_status.Visible = true;
                        connected_status.Visible = false;
                    });

                    Debug.WriteLine("Retrying connection in 5 seconds...");
                    await Task.Delay(5000); // Wait before retrying
                }
            }

            this.reconnecting = false; // Ensure reconnect flag is reset
        }

        private void listen()
        {
            keep_connection();

            // Use the class-level worker
            if (this.worker == null || !this.worker.IsBusy)
            {
                this.worker = new BackgroundWorker();
                this.worker.DoWork += (sender, e) =>
                {
                    byte[] buffer = new byte[4096];
                    while (this.is_connected)
                    {
                        try
                        {
                            if (this.network_stream != null && this.network_stream.DataAvailable)
                            {
                                int bytes_read = this.network_stream.Read(buffer, 0, buffer.Length);
                                if (bytes_read > 0)
                                {
                                    string message = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes_read).Trim();
                                    this.worker.ReportProgress(0, message);
                                }
                            }
                            else
                            {
                                // Prevent high CPU usage
                                System.Threading.Thread.Sleep(50);
                            }
                        }
                        catch (Exception ex)
                        {
                            disconnect();
                            break;
                        }
                    }
                };
                this.worker.ProgressChanged += (sender, e) =>
                {
                    string message = e.UserState as string;
                    handle_message(message);
                };
                this.worker.WorkerReportsProgress = true;
                this.worker.RunWorkerAsync();
            }
        }
        private void handle_message(string message)
        {
            switch (message)
            {
                case "0x11":
                    this.pong_response = false;
                    break;
                case "0x10":
                    Process.Start(new ProcessStartInfo("shutdown", "/r /t 0")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                    break;
                case "0x42":
                    Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            disconnected_status.Visible = true;
            load_config();
            connect();
        }

        private void keep_connection()
        {
            // Only create a new timer if one doesn't already exist or is not enabled
            if (this.ping_timer == null || !this.ping_timer.Enabled)
            {
                // Use the class-level timer variable instead of creating a local one
                this.ping_timer = new System.Timers.Timer(this.interval);
                this.ping_timer.Elapsed += (sender, e) =>
                {
                    try
                    {
                        if (!this.is_connected)
                        {
                            // Try to reconnect if disconnected
                            connect();
                            return; // Skip ping attempt until connected
                        }

                        // Check network stream validity
                        if (this.is_connected && this.network_stream != null && this.network_stream.CanWrite)
                        {
                            // Awaiting for pong
                            if (this.pong_response)
                            {
                                if ((DateTime.Now - this.last_ping_sent).TotalSeconds > this.pong_timeout_period)
                                {
                                    this.pong_response = false;
                                    disconnect();
                                    return; // Skip ping attempt after disconnect
                                }
                            }

                            try
                            {
                                // Send ping message every $this.interval
                                string message = "0x10";
                                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                                this.network_stream.Write(data, 0, data.Length);
                                this.pong_response = true;
                                this.last_ping_sent = DateTime.Now;
                            }
                            catch (Exception ex)
                            {
                                disconnect();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Catch any unexpected exceptions
                        disconnect();
                    }
                };
                this.ping_timer.Start();
            }
        }
        private void disconnect()
        {
            this.is_connected = false;
            UpdateUI(false);
            // Update UI (using Invoke for thread safety)
            this.Invoke((MethodInvoker)delegate
            {
                disconnected_status.Visible = true;
                connected_status.Visible = false;
            });

            // Clean up resources properly
            if (this.network_stream != null)
            {
                this.network_stream.Close();
                this.network_stream = null; // Important!
            }

            if (this.tcp_client != null)
            {
                this.tcp_client.Close();
                this.tcp_client = null; // Important!
            }

            // Stop and dispose the timer if it exists
            if (this.ping_timer != null)
            {
                this.ping_timer.Stop();
                // Optionally dispose: this.ping_timer.Dispose();
            }

            // Reset ping-pong state
            this.pong_response = false;
            connect();
        }
        private void save_config()
        {
            try
            {
                var xml = new XDocument(
                    new XElement("config",
                        new XElement("server", this.server_ip),
                        new XElement("port", this.server_port),
                        new XElement("interval", this.interval),
                        new XElement("daemon", this.daemon_checkbox.Checked.ToString()),
                        new XElement("timeout_period", this.pong_timeout_period)
                    )
                );

                xml.Save(CONFIG_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save configuration: {ex.Message}", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void daemon_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            save_config();
        }
        private void load_config()
        {
            if (File.Exists(CONFIG_FILE))
            {
                try
                {
                    XDocument xml = XDocument.Load(CONFIG_FILE);
                    XElement config = xml.Element("config");

                    if (config != null)
                    {
                        this.server_ip = config.Element("server")?.Value;
                        string portStr = config.Element("port")?.Value;

                        if (!string.IsNullOrEmpty(this.server_ip) && int.TryParse(portStr, out int port))
                        {
                            this.server_port = port;
                            server_ip_text.Text = $"{this.server_ip}:{this.server_port}";
                        }
                        if (config.Element("daemon")?.Value == "True")
                        {
                            this.Visible = false;
                            this.ShowInTaskbar = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load configuration: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void close_btn_click(object sender, EventArgs e)
        {
            Close();
        }
        private void trust_click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(server_ip_text.Text))
            {
                MessageBox.Show("Please enter a server IP and port in the format '127.0.0.1:54300'", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] parts = server_ip_text.Text.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out int port))
            {
                MessageBox.Show("Invalid format. Use 'IP:Port - 127.0.0.1:54300'", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.server_ip = parts[0];
            this.server_port = port;
            connect();

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void save_config_button_Click(object sender, EventArgs e)
        {
            save_config();
        }
        private void UpdateUI(bool isRunning)
        {
            this.Invoke((MethodInvoker)delegate
            {
                trust.Text = isRunning ? "Disconnect" : "Connect";
            });
        }

        private void update()
        {

        }
    }
}
