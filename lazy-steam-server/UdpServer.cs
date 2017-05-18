using System;
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
        public static void Start()
        {
            try
            {
                _udpServer = new UdpClient(8888, AddressFamily.InterNetwork);
                Task.Factory.StartNew(async () =>
                {
                    App.SetText("Waiting for broadcast...");
                    while (true)
                    {
                        UdpReceiveResult data = await _udpServer.ReceiveAsync();
                        if (Encoding.UTF8.GetString(data.Buffer) == ConnectionCodes.UDP_SERVER_REQUEST)
                        {
                            App.SetText("Broadcast recieved");
                            string toSend = ConnectionCodes.UdpResponseCode();
                            byte[] sendingBuffor = Encoding.ASCII.GetBytes(toSend);
                            await _udpServer.SendAsync(sendingBuffor, sendingBuffor.Length, data.RemoteEndPoint);
                            Console.WriteLine(toSend);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void Stop()
        {
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

