using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Win32;

namespace lazy_steam_server
{
    public partial class App : Form
    {
        private const int BallonTipToolStartUpDuration = 5000;
        public static App UiChanger;
        private bool _logsEnabled = Properties.Settings.Default.show_logs;
        public static int TcpPort = TcpServer.FreeTcpPort();
        public static string IpAdress = GetLocalIpAddress();
        public static string HostName = Dns.GetHostName();

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr proccess);


        public App()
        {
            WanService.GetExternalIpAdress(out string ip);
            var tsk = WanService.CheckRouterCapabilities();
            if (ip == String.Empty)
                ip = "not found";
            InitializeComponent();
            UiChanger = this;
            SetText("Ip address is: " + IpAdress);
            SetText("Host name is: " + HostName);
            SetText("Free TCP port is: " + TcpPort);
            SetText("Checking router capabilities...");
            SetText("External IP is: "+ ip);
            var success = tsk.Result;
            SetText(success ? "Your router configuration is correct" : "Your router configuration is incorrect");
            SetStartup();
            if(Properties.Settings.Default.run_at_startup)
                ShowBallonTipOnStartUp("Lazy steam server is running.");
            var tcpServer = new TcpServer(SetText1,TcpPort);
            tcpServer.Start();
            UdpServer.Start();
            SetText("Udp Started\nTcp Started");
            if (Properties.Settings.Default.unique_id == 0)
            {
                Properties.Settings.Default.unique_id = GenerateRandomId();
                Properties.Settings.Default.Save();
            }
        }
        public static void SetText(string text)
        {
            if (UiChanger.textBoxLog.InvokeRequired)
            {
                Action<string> method = SetText;
                UiChanger.BeginInvoke(method, text);
            }
            else
            {
                UiChanger.textBoxLog.Text += text + "\n";
                UiChanger.textBoxLog.SelectionStart = UiChanger.textBoxLog.Text.Length;
                UiChanger.textBoxLog.ScrollToCaret();
            }
        }
        public static string GetLocalIpAddress()
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
        public void SetText1(string text)
        {
            if (UiChanger.textBoxLog.InvokeRequired)
            {
                Action<string> method = SetText;
                UiChanger.BeginInvoke(method, text);
            }
            else
            {
                UiChanger.textBoxLog.Text += text + "\n";
                UiChanger.textBoxLog.SelectionStart = UiChanger.textBoxLog.Text.Length;
                UiChanger.textBoxLog.ScrollToCaret();
            }
        }


//        private void btnTcpStart_Click(object sender, EventArgs e)
//        {
//            if (!_tcpButtonStart)
//            {
//                SetText("TCP Server starting...");
//                TcpServer.Start();
//                SetText("TCP Server is running.");
//                btnTcpStart.Text = "Stop TCP";
//                _tcpButtonStart = true;
//            }
//            else
//            {
//                SetText("TCP Server service is terminating...");
//                TcpServer.Stop();
//                SetText("TCP Server service has been terminated.");
//                btnTcpStart.Text = "Start TCP";
//                _tcpButtonStart = false;
//            }
//        }

        private static void SendCodeToSteamWindow(string code)
        {
            Task.Run(async () =>
            {
                code = " " + code;
                var processes = Process.GetProcessesByName("steam");
                if (processes.Length != 0)
                {
                    var proc = processes[0];
                    var handle = proc.MainWindowHandle;
                    SetForegroundWindow(handle);
                    await Task.Delay(250);
                    SendKeys.SendWait("^{BKSP}");
                    SendKeys.SendWait(code);
                    await Task.Delay(10);
                    SendKeys.SendWait("~");
                    var process = Process.GetCurrentProcess();
                    var currentProcessHandle = process.MainWindowHandle;
                    SetForegroundWindow(currentProcessHandle);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendCodeToSteamWindow("CODE");
            string text = "Use code " + "some text"+ " to login into " + "Janusz";
            ShowBaloonTipDuringSteamSending(text);
        }

        private static void ShowBaloonTipDuringSteamSending(string text)
        { 
            int duration = CheckIfSteamIsRunning() ? Properties.Settings.Default.steam_exists : Properties.Settings.Default.steam_not_exists;
            UiChanger.notifyIcon1.ShowBalloonTip(duration, "Code recieved!", text, ToolTipIcon.None);
        }

        public static async void OnSteamDataRecieved(object f, EventArgs e)
        {
            string[] o = ((IEnumerable)f).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
            string text = "Use code " + o[0] + " to login into " + o[1];
            SetText(text);
            ShowBaloonTipDuringSteamSending(text);
            await Task.Delay(1000);
            if(Properties.Settings.Default.trigger_scrapping)
            SendCodeToSteamWindow(o[0]);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsMenu frm = new SettingsMenu {StartPosition = FormStartPosition.CenterParent};
            frm.ShowDialog(this);
        }

        private static bool CheckIfSteamIsRunning()
        {
            var processes = Process.GetProcessesByName("steam");
            return processes.Length != 0;
        }

        public static void SetStartup()
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                if (Properties.Settings.Default.run_at_startup)
                    rk.SetValue("lazy-steam-server", Application.ExecutablePath);
                else
                    rk.DeleteValue("lazy-steam-server", false);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                SetText(ex.StackTrace);
            }
        }
        private void ShowBallonTipOnStartUp(string text)
        {
            notifyIcon1.ShowBalloonTip(BallonTipToolStartUpDuration, "App started!", text, ToolTipIcon.None);
        }

        private void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000, "App minimized!", "Lazy steam helper has been minimized to the tray.", ToolTipIcon.None);
                Hide();
                e.Cancel = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(1);
        }


        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(1);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WindowState = FormWindowState.Normal;
                Show();
            }
            
        }
        private void toolStripMenuItemShow_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }
        private void logsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!_logsEnabled)
            {
                textBoxLog.Visible = true;
                _logsEnabled = true;
                Properties.Settings.Default.show_logs = true;
            }
            else
            {
                textBoxLog.Visible = false;
                _logsEnabled = false;
                Properties.Settings.Default.show_logs = false;
            }
           Properties.Settings.Default.Save();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Task.Run(() =>
            {

//                if (!WanService.GetExternalIpAdress(out string extIp))
//                {
//                    MessageBox.Show("Error when getting external IP!");
//                    //return;
//                }
//
//                if (!WanService.OpenNewPortForUpnp(out int extPort))
//                {
//                    MessageBox.Show("Error when opening new port for UnPnP!");
//                    //return;
//                }
//                MessageBox.Show(extIp + " port: " + extPort);

                WanService.DisplayallPorts();
            });
        }

        public static void SetCodeOnForm(string code)
        {
            Task.Run(() =>
            {
                var form = new CodeForm {StartPosition = FormStartPosition.CenterParent};
                form.SetLabelContent(code);
                form.ShowDialog();
            });
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
                ShowMe();
            }
            base.WndProc(ref m);
        }
        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            bool top = TopMost;
            TopMost = true;
            TopMost = top;
        }

        private int GenerateRandomId()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[4];
            rnd.NextBytes(b);
            return Math.Abs(BitConverter.ToInt32(b,0));
        }
    }
}
