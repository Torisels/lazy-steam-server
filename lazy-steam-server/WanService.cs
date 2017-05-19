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

        public static string GetExternalIpAdress()
        {
            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            if (mappings != null)
            {
                if (mappings.Count != 0)
                {
                    foreach (IStaticPortMapping map in mappings)
                    {
                        return map.ExternalIPAddress;
                    }
                }
            }
            return "not found";
        }

        public static void AddPort()
        {
            var nat = UPnPNat;
            IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
            mappings.Add(199, "TCP", TcpServer.Port, "192.168.0.51", true, "Local Web Server");          
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
                }
                port = set.Max() + 1;
            }

            var UPnPNat1 = new UPnPNATClass();
            var newMappings = UPnPNat1.DynamicPortMappingCollection;
            var newList = new HashSet<int>();
            foreach (IStaticPortMapping map in newMappings)
                {
             newList.Add(map.ExternalPort);   
             }
            return newList.Contains(port);
        }
    }
   
}
