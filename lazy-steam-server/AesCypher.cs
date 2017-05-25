using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace lazy_steam_server
{
    class AesCypher
    {
        private readonly byte[] _keyIv = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};//16
        private readonly RijndaelManaged _aes;
        public byte[] AlgoKey { get;}

        public AesCypher()
        {
            _aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                IV = _keyIv,
                Mode = CipherMode.CBC
            };
            _aes.GenerateIV();
            AlgoKey = _aes.Key;
        }
        public string Encrypt(string key, byte[] message)
        {
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                _aes.Key = StringToByteArray(key);

                using (var cs = new CryptoStream(ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(message, 0, message.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }

            return Encoding.ASCII.GetString(encryptedBytes);
        }
        public string Decrypt(string key, byte[] message)
        {
            byte[] decryptedBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                _aes.Key = StringToByteArray(key);
                using (var cs = new CryptoStream(ms, _aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(message, 0, message.Length);
                        cs.Close();
                    }
                decryptedBytes = ms.ToArray();
            }
            return Encoding.ASCII.GetString(decryptedBytes);
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public string HexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes);
        }
    }
}
