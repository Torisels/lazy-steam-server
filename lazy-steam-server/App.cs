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
                btnUdpStart.Text = "Stop TCP";
                _tcpButtonStart = true;
            }
            else
            {
                SetText("TCP Server service is terminating...");
                TcpServer.StopServer();
                SetText("TCP Server service has been terminated.");
                btnUdpStart.Text = "Start TCP";
                _tcpButtonStart = false;
            }
        }


        private static void SendCodeToSteamWindow(string code)
        {
            var processes = Process.GetProcessesByName("notepad");//change it!
            var proc = processes[0];
            var handle = proc.MainWindowHandle;
            SetForegroundWindow(handle);
            SendKeys.SendWait(code);
            SendKeys.Send("~");
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private static void ShowBaloonTip(string text)
        {
            UiChanger.notifyIcon1.ShowBalloonTip(1000, "Code recieved!", text, ToolTipIcon.None);
        }

        public static void OnSteamDataRecieved(object f, EventArgs e)
        {
            string[] o = ((IEnumerable)f).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
            string text = "Username: " + o[1] + "\n code:" + o[0];
            SetText(text);
            ShowBaloonTip(text);
        }
    }
}
