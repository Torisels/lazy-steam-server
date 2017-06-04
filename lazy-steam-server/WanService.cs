using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NATUPNPLib;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;

namespace lazy_steam_server
{
    class WanService
    {
        private static UPnPNATClass _UPnPNat = null;

        private static UPnPNATClass UPnPNat
        {
            get
            {
                if (_UPnPNat == null)
                {
                    _UPnPNat = new UPnPNATClass();
                }
                return _UPnPNat;
            }
        }

        public static void DisplayallPorts()
        {
            try
            {
                var nat = UPnPNat;
                IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
                MessageBox.Show("Count: " + mappings.Count);
                foreach (IStaticPortMapping mapping in mappings)
                {
                    //Console.WriteLine(mapping.);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Your router doesn't support this.");
            }
        }

        public static bool GetExternalIpAdress(out string ip)
        {
            ip = string.Empty;
            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            if (mappings != null)
            {
                if (mappings.Count != 0)
                {
                    foreach (IStaticPortMapping map in mappings)
                    {
                        ip = map.ExternalIPAddress;
                        if (ip != String.Empty)
                            return true;
                    }
                }
                var success = AddAndRemovePortForCheckingExternalIpAdress(out ip);
                return success;
            }
            return false;
        }

        public static bool GetFreePort(out int port)
        {
            port = 0;
            var set = new HashSet<int>();
            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            if (mappings == null) return false;
            if (mappings.Count != 0)
            {
                foreach (IStaticPortMapping map in mappings)
                {
                    set.Add(map.ExternalPort);
                }
                port = FindNewPort(set);
            }
            else
                port = 1;

            return true;
        }

        private static bool AddAndRemovePortForCheckingExternalIpAdress(out string ip)
        {

            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            var freeLocalPort = TcpServer.FreeTcpPort();
            ip = string.Empty;

            if (!GetFreePort(out int extPort))
                return false;
            if (mappings != null)
            {
                mappings.Add(extPort, "TCP", freeLocalPort, App.GetLocalIpAddress(), true, "External IP Test");
                foreach (IStaticPortMapping map in mappings)
                {
                    ip = map.ExternalIPAddress;
                    if (ip != String.Empty)
                    {
                        mappings.Remove(extPort, "TCP");
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool OpenNewPortForUpnp(out int port)
        {
            port = 0;
            if (!GetFreePort(out port))
                return false;

            var upnp = UPnPNat;
            IStaticPortMappingCollection mappings = upnp.StaticPortMappingCollection;
            if (mappings == null)
                return false;

            mappings.Add(port, "TCP", App.TcpPort, App.GetLocalIpAddress(), true, "Lazy Steam Helper communication port");
            return true;

        }

        public static int FindNewPort(HashSet<int> ports)
        {
            int port;
            var rnd = new Random();
            do
            {
                port = rnd.Next(1, 60000);
            } while (ports.Contains(port));
            return port;
        }

        public static Task<bool> CheckRouterCapabilities()
        {
            return Task.Run(() =>
            {
                var nat = new UPnPNATClass();
                IStaticPortMappingCollection ports = nat.StaticPortMappingCollection;
                return ports != null;
            });
        }

        public static Task<Dictionary<int,string>> GetExternalIpAndPortForTcpConnectionTask()
        {
            return Task.Run(() =>
            {
                int port = -1;
                string extIp = string.Empty;
                var nat = new UPnPNATClass();
                IStaticPortMappingCollection portsCollection = nat.StaticPortMappingCollection;
                if (portsCollection == null) return new Dictionary<int, string> {{port, extIp}};
                if (portsCollection.Count == 0)
                {

                }
                else
                {
                    foreach (IStaticPortMapping portMapping in portsCollection)
                    {
                        var iphelper = portMapping.ExternalIPAddress;
                        if (iphelper != string.Empty)
                        {
                            //iphelper;
                            break;
                        }
                    }
                }
                return new Dictionary<int, string> { { port, extIp } };
            });
        }
}
   
}
