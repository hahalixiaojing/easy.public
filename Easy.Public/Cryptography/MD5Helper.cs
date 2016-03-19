using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Easy.Public.Security.Cryptography
{
    public class MD5Helper
    {
        public static String Encrypt(String text)
        {
            Byte[] data = Encoding.UTF8.GetBytes(text.ToCharArray());
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] result = md5.ComputeHash(data);
            return StringHelper.BytesToHex(result);
        }

        public static byte[] Encrypt2(String text)
        {
            Byte[] data = Encoding.UTF8.GetBytes(text.ToCharArray());
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] result = md5.ComputeHash(data);
            return result;
        }
        
    }
}
