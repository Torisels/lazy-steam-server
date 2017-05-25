using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lazy_steam_server
{
    internal class TcpServer
    {
        public Socket ServerSocket;
        private readonly List<Socket> ClientSockets = new List<Socket>();
        public int Port;
        public event EventHandler<EventArgs> DataRecieved;
        public HashSet<string> CodesSet;
        public delegate void TextInvokerDelegate(string s);

        public static int SPort;

        private readonly TextInvokerDelegate _setTextInvoker;


        public TcpServer(TextInvokerDelegate setTextInvoker, int port)
        {
            _setTextInvoker = setTextInvoker;
            Port = port;
            SPort = port;
        }

        public static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint) l.LocalEndpoint).Port;
            l.Stop();         
            return port;
        }

        public void Start()
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

        public void Stop()
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

        public void AcceptCallback(IAsyncResult ar)
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
            string com = ConnectionCodes.RecieveCom(text);

            if (com == ConnectionCodes.TCP_SERVER_REQUEST)
            {
                SendMessageFromString(ConnectionCodes.SendCom(ConnectionCodes.TCP_SERVER_RESPONSE), socket);
                string text1 = RecieveToString(socket);
                string com1 = ConnectionCodes.RecieveCom(text1);
                if (com1 == ConnectionCodes.TCP_SERVER_DATA)
                {
                    var strings = ConnectionCodes.CodeAndNameFromRecievedString(text1);
                    SendMessageFromString(ConnectionCodes.SendCom(ConnectionCodes.TCP_SERVER_REQUEST_RESPONSE), socket);
                    OnDataRecieved(strings);

                }
            }
        }

        private string RecieveToString(Socket s)
        {
            var receivebuffer = new byte[1024];
            var recievedbytes = s.Receive(receivebuffer);
            var databuffer = new byte[recievedbytes];
            Array.Copy(receivebuffer, databuffer, recievedbytes);
            Array.Clear(receivebuffer, 0, receivebuffer.Length);
            var recstring = Encoding.ASCII.GetString(databuffer);
            Array.Clear(databuffer, 0, databuffer.Length);
            // ReSharper disable once LocalizableElement
            Console.WriteLine("Message received: " + recstring);
            _setTextInvoker("Message received: " + recstring);
            return recstring;
        }

        private void SendMessageFromString(string s, Socket sock)
        {
            try
            {
                var sendingbyte = Encoding.ASCII.GetBytes(s);
                sock.Send(sendingbyte);
                Console.WriteLine("Message: " + s + " has been sent successfully");
                _setTextInvoker("Message: " + s + " has been sent successfully");
                Array.Clear(sendingbyte, 0, sendingbyte.Length);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected void OnDataRecieved(string[] strings)
        {
            DataRecieved?.Invoke(strings, EventArgs.Empty);
        }
    }
}
