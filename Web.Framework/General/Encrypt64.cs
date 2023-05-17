using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web.Framework.General
{
    public class Encrypt64
    {
        private static byte[] key = { 123, 217, 19, 11, 24, 26, 76, 45, 114, 184, 27, 162, 37, 45, 213, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private static byte[] vector = { 146, 64, 191, 111, 23, 3, 111, 119, 121, 245, 221, 213, 79, 32, 114, 156 };

        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public Encrypt64()
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            string value = Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
            value = value.Replace("+", "|||");

            return value;
        }

        public string Decrypt(string encrypted)
        {
            encrypted = encrypted.Replace("|||", "+");
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}