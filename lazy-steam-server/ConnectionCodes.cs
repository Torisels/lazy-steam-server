﻿using System;
using System.Net;
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

        public const string UDP_SERVER_REQUEST = "LAZY_STEAM_HELPER_DISCOVERY_REQUEST";
        public const string UDP_SERVER_RESPONSE = "LAZY_STEAM_HELPER_DISCOVERY_RESPONSE";
        public const string TCP_SERVER_REQUEST = "LAZY_STEAM_HELPER_CONNECTION_REQUEST";
        public const string TCP_SERVER_RESPONSE = "LAZY_STEAM_HELPER_CONNECTION_RESPONSE";
        public const string TCP_SERVER_REQUEST_RESPONSE = "LAZY_STEAM_HELPER_RECEIVE_RESPONSE";
        public const string TCP_SERVER_DATA = "LAZY_STEAM_HELPER_DATA";

        public const string JSON_COM = "com";
        public const string JSON_CODE = "code";
        public const string JSON_USERNAME = "username";
        public const string JSON_SEC_CODE = "security_code";
        public const string JSON_ENC_KEY = "encryption_key";
        public const string JSON_EXT_PORT = "external_port";
        public const string JSON_EXT_HOST = "external_host";

        public enum RecievedInfo
        {
            Code,
            UserName
        }

        public static string SendCom(string code)
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
            JObject json = new JObject { [JSON_COM] = UDP_SERVER_RESPONSE, ["server_hostname"] = Dns.GetHostName(),["communication_port"] = App.TcpPort};
            return JsonConvert.SerializeObject(json,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }

        public static string RecieveCom(string s)
        {
            JObject o = JObject.Parse(s);
            return (string)o[JSON_COM];
        }

        public static string RecieveCode(string s)
        {
            JObject o = JObject.Parse(s);
            return (string)o[JSON_SEC_CODE];
        }
    }
}
