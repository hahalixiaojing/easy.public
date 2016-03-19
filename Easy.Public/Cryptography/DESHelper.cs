using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Easy.Public.Security.Cryptography
{
    /// <summary>
    /// DESC加密算法帮助类
    /// </summary>
    public static class DESHelper
    {
        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <param name="key">加密提供程序 例如：System.Security.Cryptography.DESCryptoServiceProvider</param>
        /// <returns>返回经过十六进制编码的结果</returns>
        public static String Encrypt(String text, SymmetricAlgorithm key)
        {
            String encryptString = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    StreamWriter sw = new StreamWriter(encStream, Encoding.UTF8);
                    sw.Write(text);
                    sw.Close();
                    byte[] buffer = ms.ToArray();
                    encryptString = StringHelper.BytesToHex(buffer);
                }
                ms.Close();
            }
            return encryptString;
        }
        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <returns></returns>
        public static String Encrypt(String text)
        {
            return DESHelper.Encrypt(text, GetDefaultDESCryptoServiceProvider());
        }
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="safeText">基于base64编码的密文</param>
        /// <param name="key">加密提供程序 例如：System.Security.Cryptography.DESCryptoServiceProvider</param>
        /// <returns>返回解密后的文本</returns>
        public static String Decrypt(String safeText, SymmetricAlgorithm key)
        {
            Byte[] cypherText = StringHelper.HexToBytes(safeText);

            String decryptText = String.Empty;
            using (MemoryStream ms = new MemoryStream(cypherText))
            {
                using (CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    StreamReader sr = new StreamReader(encStream, Encoding.UTF8);
                    decryptText = sr.ReadLine();
                    sr.Close();
                    ms.Close();
                }
            }
            return decryptText;
        }
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="safeText"></param>
        /// <returns></returns>
        public static String Decrypt(String safeText)
        {
            return DESHelper.Decrypt(safeText, GetDefaultDESCryptoServiceProvider());
        }
        /// <summary>
        /// 获得默认的加密/解密提供者
        /// </summary>
        /// <returns></returns>
        private static DESCryptoServiceProvider GetDefaultDESCryptoServiceProvider()
        {
            const String KEY = "M&A!@HT%";
            const String IV = "Q^T&+G95";

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            provider.Key = ASCIIEncoding.ASCII.GetBytes(KEY);

            return provider;
        }
    }
}
