using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace lazy_steam_server
{
    internal class ConnectionCodes
    {
        public const string UDP_SERVER_REQUEST = "LAZY_STEAM_APP_CONNECTION_REQUEST";
        public const string UDP_SERVER_RESPONSE = "LAZY_STEAM_APP_CONNECTION_RESPONSE";

        public static string Code(string code)
        {
            JObject json = new JObject {["code"] = code};
            return JsonConvert.SerializeObject(json,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }
    }
}
