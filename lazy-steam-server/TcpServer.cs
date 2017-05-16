﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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

        public static void SetupServer()
        {
            try
            {
                App.SetText("The open port is: " + Port);
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Unspecified);
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                ServerSocket.Listen(0);
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

        public static void StopServer()
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
            }
            else if (com == ConnectionCodes.TCP_SERVER_DATA)
            {
                var strings = ConnectionCodes.CodeAndNameFromRecievedString(text);
                var steamCode = strings[(int) ConnectionCodes.RecievedInfo.Code];
                var steamUserName = strings[(int) ConnectionCodes.RecievedInfo.UserName];
                SendMessageFromString(ConnectionCodes.Code(ConnectionCodes.TCP_SERVER_REQUEST_RESPONSE), socket);
                App.SetText("Code from steam is: " + steamCode);
                App.SetText("Username from steam is: " + steamUserName);
                OnDataRecieved(strings);
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
            Console.WriteLine("Message received: " + recstring);
            return recstring;
        }

        private static void SendMessageFromString(string s, Socket sock)
        {
            try
            {
                var sendingbyte = Encoding.ASCII.GetBytes(s);
                sock.Send(sendingbyte);
                Console.WriteLine("Message: " + s + " has been sent successfully");
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