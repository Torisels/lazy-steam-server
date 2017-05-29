using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NATUPNPLib;
using System.Security.Cryptography;

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
                    return false;
                }
                else
                {
                    AddPort();
                }
            }
            return false;
        }

        public static void AddPort()
        {
            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            mappings?.Add(199, "TCP", TcpServer.SPort, "192.168.0.51", true, "Local Web Server");
        }

        public static bool GetFreePort(out int port)
        {
            port = 0;
            var set = new HashSet<int>();
            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            if (mappings != null)
            {
                if (mappings.Count != 0)
                {
                    foreach (IStaticPortMapping map in mappings)
                    {
                        set.Add(map.ExternalPort);
                    }
                    port = set.Max() + 1;
                }
                else
                    port = 1;

                return true;
            }
            return false;
        }

        private static string AddAndRemovePortForCheckingExternalIpAdress()
        {
            var nat = UPnPNat;
            var freeLocalPort = TcpServer.FreeTcpPort();
            string extIP = string.Empty;
            int extPort;
            GetFreePort(out extPort);
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            mappings?.Add(extPort, "TCP", freeLocalPort, "127.0.0.1", true, "External IP Test");
            foreach (IStaticPortMapping map in mappings)
            {
                extIP = map.ExternalIPAddress;
                break;
            }
            mappings.Remove(extPort, "TCP");
            return extIP;
        }
    }
   
}
