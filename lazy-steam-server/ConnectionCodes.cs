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
        public const string UDP_SERVER_REQUEST = "LAZY_STEAM_HELPER_DISCOVERY_REQUEST";
        public const string UDP_SERVER_RESPONSE = "LAZY_STEAM_HELPER_DISCOVERY_RESPONSE";
        public const string TCP_SERVER_REQUEST = "LAZY_STEAM_HELPER_CONNECTION_REQUEST";
        public const string TCP_SERVER_RESPONSE = "LAZY_STEAM_HELPER_CONNECTION_RESPONSE";
        public const string TCP_SERVER_REQUEST_RESPONSE = "LAZY_STEAM_HELPER_RECEIVE_RESPONSE";
        public const string TCP_SERVER_DATA = "LAZY_STEAM_HELPER_DATA";
        public const string JSON_COM = "com";

        public enum RecievedInfo
        {
            Code,
            UserName
        }

        public static string Code(string code)
        {
            JObject json = new JObject {[JSON_COM] = code};
            return JsonConvert.SerializeObject(json,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }

        public static string[] CodeAndNameFromRecievedString(string recieved)
        {
            string[] array = new string[2];
            try
            {
                JObject o = JObject.Parse(recieved);
                array[0] = (string) o["code"];
                array[1] = (string) o["username"];

            }
            catch (Exception ex)
            {
                App.SetText(ex.StackTrace);
            }
                return array;
        }

        public static string UdpResponseCode()
        {
            JObject json = new JObject { [JSON_COM] = UDP_SERVER_RESPONSE, ["server_hostname"] =  };
            return JsonConvert.SerializeObject(json,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }
    }
}
