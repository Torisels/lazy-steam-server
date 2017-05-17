using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace lazy_steam_server
{

    public partial class App : Form
    {
        public static App UiChanger;
        private bool _udpButtonStart;
        private bool _tcpButtonStart;
        public static int TcpPort = TcpServer.FreeTcpPort();

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr proccess);

        public App()
        {
            Console.WriteLine(TcpPort);
            InitializeComponent();
            UiChanger = this;
            SetText("Ip address is: " + GetLocalIpAddress());
            SetText("Host name is: " + Dns.GetHostName());
            SetText("Free TCP port is: " + TcpPort);
            checkBox2.Checked = Properties.Settings.Default.apply_enter;
            checkBox1.Checked = Properties.Settings.Default.trigger_scrapping;
//            SetText(ConnectionCodes.Code(ConnectionCodes.UDP_SERVER_REQUEST));

        }
        public static void SetText(string text)
        {

            if (UiChanger.textBoxLog.InvokeRequired)
            {
                Action<string> d = SetText;
                UiChanger.BeginInvoke(d, text);
            }
            else
            {
                UiChanger.textBoxLog.Text += text+"\n";
                UiChanger.textBoxLog.SelectionStart = UiChanger.textBoxLog.Text.Length;
                UiChanger.textBoxLog.ScrollToCaret();
            }
        }
        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void btnUdpStart_Click(object sender, EventArgs e)
        {
            if (!_udpButtonStart)
            {
                SetText("Udp Server starting...");
                UdpServer.StartUdp();
                SetText("Udp Server is running.");
                btnUdpStart.Text = "Stop UDP";
                _udpButtonStart = true;
            }
            else
            {
                SetText("Udp Server service is terminating...");
                UdpServer.StopUdp();
                SetText("Udp Server service has been terminated.");
                btnUdpStart.Text = "Start UDP";
                _udpButtonStart = false;
            }
        }

        private void btnTcpStart_Click(object sender, EventArgs e)
        {
            if (!_tcpButtonStart)
            {
                SetText("TCP Server starting...");
                TcpServer.SetupServer();
                SetText("TCP Server is running.");
                btnTcpStart.Text = "Stop TCP";
                _tcpButtonStart = true;
            }
            else
            {
                SetText("TCP Server service is terminating...");
                TcpServer.StopServer();
                SetText("TCP Server service has been terminated.");
                btnTcpStart.Text = "Start TCP";
                _tcpButtonStart = false;
            }
        }


        private static  void SendCodeToSteamWindow(string code)
        {
            Task.Run(async() =>
            {
                code = " " + code;
                var processes = Process.GetProcessesByName("steam");
                if (processes.Length != 0)
                {
                    var proc = processes[0];
                    var handle = proc.MainWindowHandle;
                    SetForegroundWindow(handle);
                    SendKeys.SendWait("^{BKSP}");
                    SendKeys.SendWait(code);
                    await Task.Delay(2);
                    if (UiChanger.checkBox2.Checked)
                        SendKeys.Send("~");
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendCodeToSteamWindow(steamTextBox.Text);
           // SetText("From textbox "+ steamTextBox.Text);
            string text = "User code " + steamTextBox.Text + " to login into " + "Janusz";
            ShowBaloonTip(text);
        }

        private static void ShowBaloonTip(string text)
        { 
            int duration = CheckIfSteamIsRunning() ? Properties.Settings.Default.steam_exists : Properties.Settings.Default.steam_not_exists;
            UiChanger.notifyIcon1.ShowBalloonTip(duration, "Code recieved!", text, ToolTipIcon.None);
        }

        public static async void   OnSteamDataRecieved(object f, EventArgs e)
        {
            string[] o = ((IEnumerable)f).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
            string text = "User code " + o[0] + " to login into " + o[1];
            SetText(text);
            ShowBaloonTip(text);
            await Task.Delay(1000);
            if(UiChanger.checkBox1.Checked)
            SendCodeToSteamWindow(o[0]);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsMenu frm = new SettingsMenu {StartPosition = FormStartPosition.CenterParent};
            frm.Show();
        }

        private static bool CheckIfSteamIsRunning()
        {
            var processes = Process.GetProcessesByName("steam");
            return processes.Length != 0;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.apply_enter = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.trigger_scrapping = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
