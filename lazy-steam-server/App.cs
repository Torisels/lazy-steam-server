﻿using System;
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

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr proccess);

        public App()
        {
            InitializeComponent();
            UiChanger = this;
            SetText("Ip address is: " + GetLocalIpAddress());
            SetText("Host name is: " + Dns.GetHostName());
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
                UdpServer.StartUdp();
                SetText("TCP Server is running.");
                btnUdpStart.Text = "Stop TCP";
                _tcpButtonStart = true;
            }
            else
            {
                SetText("TCP Server service is terminating...");
                UdpServer.StopUdp();
                SetText("TCP Server service has been terminated.");
                btnUdpStart.Text = "Start TCP";
                _tcpButtonStart = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            var processes = Process.GetProcessesByName("notepad");
            var proc = processes[0];
            var handle = proc.MainWindowHandle;
            SetForegroundWindow(handle);
            SendKeys.SendWait("ENTER");
            SendKeys.Send("~");
            SendKeys.SendWait("^v");
        }
    }
}
