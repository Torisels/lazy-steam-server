using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace lazy_steam_server
{
    internal class ConnectionCodes
    {
        public const string MAIN_COM = "LAZY_STEAM_SETUP_";
        public static readonly string CLIENT_BEGIN_SETUP = MAIN_COM + "BEGIN";
        public static readonly string SERVER_CODE_REQUEST = MAIN_COM + "CODE_REQUEST";
        public static readonly string CLIENT_CODE_RESPONSE = MAIN_COM + "CODE_RESPONSE"; //security_code 
        public static readonly string SERVER_KEY_EXCHANGE = MAIN_COM + "KEY_EXCHANGE"; //encryption_key
        public static readonly string CLIENT_PORT_REQUEST = MAIN_COM + "PORT_REQUEST";
        public static readonly string SERVER_PORT_RESPONSE = MAIN_COM + "PORT_RESPONSE"; //external_port + external_host
        public static readonly string CLIENT_CONFIRM = MAIN_COM + "COMPLETE";
        public static readonly string SERVER_CONFIRM = MAIN_COM + "COMPLETE_CONFIRM";

        public const string UDP_CLIENT_DISCOVERY_REQUEST = "LAZY_STEAM_DISCOVERY_REQUEST";
        public const string UDP_SERVER_DISCOVERY_RESPONSE = "LAZY_STEAM_DISCOVERY_RESPONSE";


        public const string JSON_COM = "com";
        public const string JSON_CODE = "code";
        public const string JSON_USERNAME = "username";
        public const string JSON_SEC_CODE = "security_code";
        public const string JSON_ENC_KEY = "encryption_key";
        public const string JSON_EXT_PORT = "external_port";
        public const string JSON_EXT_HOST = "external_host";
        public const string JSON_SERVER_ID = "server_id";
        public const string JSON_CLIENT_ID = "app_id";
        public const string JSON_TRIES = "tries";

        public enum RecievedInfo
        {
            Code,
            UserName
        }

        public static string SendCom(string code)
        {
            JObject json = new JObject {[JSON_COM] = code};
            return JsonConverting(json);
        }

        public static string[] CodeAndNameFromRecievedString(string recieved)
        {
            string[] array = new string[2];
            try
            {
                JObject o = JObject.Parse(recieved);
                array[0] = (string) o[JSON_CODE];
                array[1] = (string) o[JSON_USERNAME];

            }
            catch (Exception ex)
            {
                App.SetText(ex.StackTrace);
            }
                return array;
        }

        public static string UdpResponseCode()
        {
            JObject json = new JObject
            {
                [JSON_COM] = UDP_SERVER_DISCOVERY_RESPONSE, ["server_hostname"] = Dns.GetHostName(),["communication_port"] = App.TcpPort,
                [JSON_SERVER_ID] = Properties.Settings.Default.unique_id
            };
            return JsonConverting(json);
        }

        public static string RecieveCom(string s)
        {
            try
            {
                JObject o = JObject.Parse(s);
                return (string) o[JSON_COM];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                return string.Empty;
            }
        }

        public static string RecieveSecurityCode(string s)
        {
            try
            {
                JObject o = JObject.Parse(s);
                if (o != null)
                    return (string) o[JSON_SEC_CODE];
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        public static string EncryptionKeyExchange(string code)
        {
            JObject json = new JObject { [JSON_COM] = SERVER_KEY_EXCHANGE, [JSON_ENC_KEY] = code};
            return JsonConverting(json);
        }

        public static string ClientId(string s)
        {
            try
            {
                var o = JsonConvert.DeserializeObject<JObject>(s);
//                var o = JObject.Parse(s);
                if (o != null)
                    return (string)o[JSON_CLIENT_ID];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return string.Empty;
        }

        public static string SetupPortResponse(int extPort, string extIp)
        {
            JObject json = new JObject { [JSON_COM] = SERVER_PORT_RESPONSE, [JSON_EXT_PORT] = extPort, [JSON_EXT_HOST] = extIp};
            return JsonConverting(json);
        }

        public static string SendServerCodeRequest(int attempts)
        {
            JObject json = new JObject { [JSON_COM] = SERVER_CODE_REQUEST, [JSON_TRIES] = attempts};
            return JsonConverting(json);
        }

        public static string JsonConverting(JObject json)
        {
            return JsonConvert.SerializeObject(json,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }
    }
}
