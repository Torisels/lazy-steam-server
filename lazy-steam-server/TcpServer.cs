using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace lazy_steam_server
{
    internal class TcpServer
    {
        public static Socket ServerSocket;
        private static readonly List<Socket> ClientSockets = new List<Socket>();
        public static int Port;
        public static event EventHandler<EventArgs> DataRecieved;

        public static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint) l.LocalEndpoint).Port;
            l.Stop();
            Port = port;
            return port;
        }

        public static void Start()
        {
            try
            {
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                ServerSocket.Listen(5);
                ServerSocket.BeginAccept(AcceptCallback, null);
                DataRecieved += App.OnSteamDataRecieved;
            }
            catch (ObjectDisposedException)
            {
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
                foreach (var socket in ClientSockets)
                {
                    socket.Close();
                }
                ServerSocket.Close();
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.StackTrace);
                App.SetText(e.StackTrace);
            }

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket;
            try
            {
                socket = ServerSocket.EndAccept(ar);
                ServerSocket.BeginAccept(AcceptCallback, null);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            ClientSockets.Add(socket);

            string text = RecieveToString(socket);
            string com = ConnectionCodes.GetComFromJson(text);

            if (com == ConnectionCodes.TCP_SERVER_REQUEST)
            {
                SendMessageFromString(ConnectionCodes.Code(ConnectionCodes.TCP_SERVER_RESPONSE), socket);
                string text1 = RecieveToString(socket);
                string com1 = ConnectionCodes.GetComFromJson(text1);
                if (com1 == ConnectionCodes.TCP_SERVER_DATA)
                {
                    var strings = ConnectionCodes.CodeAndNameFromRecievedString(text1);
                    SendMessageFromString(ConnectionCodes.Code(ConnectionCodes.TCP_SERVER_REQUEST_RESPONSE), socket);
                    OnDataRecieved(strings);
                }
            }
        }

        private static string RecieveToString(Socket s)
        {
            byte[] receivebuffer = new byte[1024];
            int recievedbytes = s.Receive(receivebuffer);
            byte[] databuffer = new byte[recievedbytes];
            Array.Copy(receivebuffer, databuffer, recievedbytes);
            Array.Clear(receivebuffer, 0, receivebuffer.Length);
            string recstring = Encoding.ASCII.GetString(databuffer);
            Array.Clear(databuffer, 0, databuffer.Length);
            // ReSharper disable once LocalizableElement
            Console.WriteLine("Message received: " + recstring);
            App.SetText("Message received: " + recstring);
            return recstring;
        }

        private static void SendMessageFromString(string s, Socket sock)
        {
            try
            {
                var sendingbyte = Encoding.ASCII.GetBytes(s);
                sock.Send(sendingbyte);
                Console.WriteLine("Message: " + s + " has been sent successfully");
                App.SetText("Message: " + s + " has been sent successfully");
                Array.Clear(sendingbyte, 0, sendingbyte.Length);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected static void OnDataRecieved(string[] strings)
        {
            DataRecieved?.Invoke(strings, EventArgs.Empty);
        }
    }
}
