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

       
    }
   
}
