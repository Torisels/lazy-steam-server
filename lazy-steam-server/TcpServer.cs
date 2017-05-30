using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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
        public Dictionary<string, AesCypher> CyphersDictionary = new Dictionary<string, AesCypher>();

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

            var com = ConnectionCodes.RecieveCom(RecieveToString(socket)); // FIRST STEP
            if (com != ConnectionCodes.CLIENT_BEGIN_SETUP) 
                return;
               
            if (!CheckIfCodeIsValid(socket)) //SECOND STEP (RECIEVING security_code)
                return;

            var aesCrypt = new AesCypher();
            SendMessageFromString(ConnectionCodes.EncryptionKeyExchange(aesCrypt.AlgoKeyHexString),socket); //THIRD STEP SEND ENC CODE

            com = ConnectionCodes.RecieveCom(RecieveToString(socket)); //FOURTH STEP GET CLIENT ID
            if (com != ConnectionCodes.CLIENT_PORT_REQUEST)
                return;
            if (ConnectionCodes.ClientId(com) == string.Empty)
                return;
            CyphersDictionary.Add(ConnectionCodes.ClientId(com),aesCrypt); //ADD ID TO DICT

            var extIp = string.Empty;
            var extPort = 0;

            if (!WanService.GetExternalIpAdress(out extIp))
                return;
            if (!WanService.OpenNewPortForUpnp(out extPort))
                return;

            SendMessageFromString(ConnectionCodes.SetupPortResponse(extPort, extIp), socket);//FIFTH STEP SEND EXTERNAL PORT AND IP DATA

            if (ConnectionCodes.RecieveCom(RecieveToString(socket)) != ConnectionCodes.CLIENT_CONFIRM)//SIXTH STEP CONFIRMATION
                return;
            SendMessageFromString(ConnectionCodes.SendCom(ConnectionCodes.SERVER_CONFIRM), socket);

        }

        private string RecieveToString(Socket s)
        {
            try
            {
                var receivebuffer = new byte[1024];
                var recievedbytes = s.Receive(receivebuffer);
                var databuffer = new byte[recievedbytes];
                Array.Copy(receivebuffer, databuffer, recievedbytes);
                Array.Clear(receivebuffer, 0, receivebuffer.Length);
                var recstring = Encoding.ASCII.GetString(databuffer);
                Array.Clear(databuffer, 0, databuffer.Length);
                Console.WriteLine("Message received: " + recstring);
                _setTextInvoker("Message received: " + recstring);
                return recstring;
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Error when recieving message from socket: " + ex.Message);
            }
            return string.Empty;
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

        private string GenerateCode()
        {
            var rnd = new Random(73123123);
            return rnd.Next(1000,10000).ToString();
        }

        private bool CheckIfCodeIsValid(Socket socket)
        {
            var code = GenerateCode();             //generate code
            App.SetCodeOnForm(code);              //show code

            bool valid = false;
            int counter = 0;
            do
            {
                SendMessageFromString(ConnectionCodes.SendCom(ConnectionCodes.SERVER_CODE_REQUEST), socket);
                var str = RecieveToString(socket);
                var com = ConnectionCodes.RecieveCom(str);
                var codeR = ConnectionCodes.RecieveSecurityCode(str);

                if (com == ConnectionCodes.CLIENT_CODE_RESPONSE && codeR == code)
                    valid = true;
                else
                    counter++;


            } while (!valid && counter <= 25);

            return counter <= 25 && valid;
        }
        
    }
}
