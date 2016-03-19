using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Easy.Public.Security.Cryptography
{
    /// <summary>
    /// SHA256 哈希函数 加密帮助类
    /// </summary>
    public static class SHA256Helper
    {
        private const String KEY = "AA@#9FF++&&T?!!~`LEEBMW";

        /// <summary>
        /// 将文本进行加密码签名
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="text">需要签名的文本</param>
        /// <returns>返回基于base64的签名文本</returns>
        public static String Signature(String key, String text)
        {
            Byte[] byteKey = Encoding.UTF8.GetBytes(key);
            Byte[] byteText = Encoding.UTF8.GetBytes(text);

            HMACSHA256 hmacsha256 = new HMACSHA256(byteKey);
            Byte[] signatureBytes = hmacsha256.ComputeHash(byteText);

            return StringHelper.BytesToHex(signatureBytes);
        }
        /// <summary>
        /// 将文本进行加密签名，使用默认密钥
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <returns>返回基于base64的签名文本</returns>
        public static String Signature(String text)
        {
            return SHA256Helper.Signature(KEY, text);
        }
    }
}
