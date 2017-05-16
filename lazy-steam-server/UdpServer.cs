﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lazy_steam_server
{
    class UdpServer
    {
        private static UdpClient _udpServer;

        public static void StartUdp()
        {
            try
            {
                _udpServer = new UdpClient(8888, AddressFamily.InterNetwork);
                Task.Factory.StartNew(async () =>
                {
                    App.SetText("Waiting for broadcast...");
                    while (true)
                    {
                        var remoteEP = new IPEndPoint(IPAddress.Any, 8888);
                        UdpReceiveResult data = await _udpServer.ReceiveAsync();
                        if (Encoding.UTF8.GetString(data.Buffer) == "CSGO_DASHBOARD_DISCOVERY_REQUEST")
                        {
                            App.SetText("Broadcast recieved");
                            string to_send = ConnectionCodes.Code(ConnectionCodes.UDP_SERVER_RESPONSE);
                            byte[] sendingBuffor = Encoding.ASCII.GetBytes(to_send);
                            await _udpServer.SendAsync(sendingBuffor, sendingBuffor.Length, data.RemoteEndPoint);
                            sendingBuffor = null;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void StopUdp()
        {
            App.SetText("Udp listening is terminated");
            try
            {
                _udpServer?.Close();
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
