using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lazy_steam_server
{
    class WanService
    {
        private static NATUPNPLib.UPnPNATClass _UPnPNat = null;
        private static NATUPNPLib.UPnPNATClass upnpnat = new NATUPNPLib.UPnPNATClass();
       

        private static NATUPNPLib.UPnPNATClass UPnPNat
        {
            get
            {
                if (_UPnPNat == null)
                {
                    _UPnPNat = new NATUPNPLib.UPnPNATClass();
                }
                return _UPnPNat;
            }
        }

        public static void DisplayallPorts()
        {
            try
            {
                var nat = UPnPNat;
                NATUPNPLib.IStaticPortMappingCollection mappings = nat.StaticPortMappingCollection;
                MessageBox.Show("Count: " + mappings.Count.ToString());
                foreach (var mapping in mappings)
                {
                    Console.WriteLine(mapping.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
   
}
