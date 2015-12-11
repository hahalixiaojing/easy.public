using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Easy.Public.Security.Cryptography
{
    public static class RSAHelper
    {
        public const String publickey = "BgIAAACkAABSU0ExAAQAAAEAAQCfGBy4N5e+DnKm5kUMLMpxlnpr3oZE8yZWlCdk+PyiFJjVtXlB7jWM3NjEJBQf1fovS8o41ExJJhhquCpF/XU56fz1rwN3w8Qe+JFmZ5VnsdFgQpTOjkBxRwkJqu0467hCNl8Lnt26q23rAZbX0luoQIkBqbJSagI49W5pH2h5qQ==";
        private const String privatekey = "BwIAAACkAABSU0EyAAQAAAEAAQCfGBy4N5e+DnKm5kUMLMpxlnpr3oZE8yZWlCdk+PyiFJjVtXlB7jWM3NjEJBQf1fovS8o41ExJJhhquCpF/XU56fz1rwN3w8Qe+JFmZ5VnsdFgQpTOjkBxRwkJqu0467hCNl8Lnt26q23rAZbX0luoQIkBqbJSagI49W5pH2h5qQEwMFgANc5gBvHiskcPobiOO0ufDshT1RhbEBX4ozqPcxYGcn34xtdlsjistKfj6NC+wpiJfRMSN4VXPA2e+tOfSK7Esl0Ro3vnbwJeiGrhHWcJx8Mcz80fWtLurBf7jVdVAtYo9BrQDexmICZ4jstyrX3qAj5G0aGhj1QfH6vMAUDACoFNW4CBnx+UEs//lK0Hxin/WtGiRIpAJhbxiSejrO6OlQlwRnDB8RSpKO4QMtuEKc68kpXS1KFoN9FKt7kJqE9eWVkNnu2/UuUfUv033tFMtWrkZu3lWl+kVB9CHCokRq1SgjwY317/FgVe6Rds3FdOLhD+RtRdiZhXzqG/j1p/5OfMz+WPIhnVlPnaD5iANdtE4uGNOMAx1jogLtS08PRceaTFH5nwA3V18kAXKhCmRfs32eKZDtFXXaJkAaB29cKz2ojWOuIruWIaT2xEMuVfjOef7IU5pRp6uc+PhKp8fgIVVLKyDUeASZff0K3LAbgtbA9ask992wvIcYIx50Wh60SM+wnRbsG16mcxQ1nPc/qHo41Zz42U1mWJ4oEjYmt+ovhOxwpgcYabr0CWGJe83a9UiL0jLW4/TWk=";

        /// <summary>
        /// 加密码文本
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <param name="csp">加密参数</param>
        /// <param name="dwkeySize"></param>
        /// <param name="key">基于base64编码密钥</param>
        /// <returns></returns>
        public static String Encrypt(String text, CspParameters csp, Int32 dwkeySize, String key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwkeySize, csp);
            rsa.ImportCspBlob(Convert.FromBase64String(key));

            byte[] encrtype_bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(text), false);

            return StringHelper.BytesToHex(encrtype_bytes);
        }
        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String Encrypt(String text)
        {
            CspParameters cspParameters = new CspParameters();
            cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

            return RSAHelper.Encrypt(text, cspParameters, 1024, publickey);
        }
        /// <summary>
        /// 角密文本
        /// </summary>
        /// <param name="encryptText">要解密的密文</param>
        /// <param name="csp">解密参数</param>
        /// <param name="dwkeySize">默认传入1024</param>
        /// <param name="key">基于base64编码密钥</param>
        /// <returns></returns>
        public static String Decrypt(String encryptText, CspParameters csp, Int32 dwkeySize, String key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwkeySize, csp);
            rsa.ImportCspBlob(Convert.FromBase64String(key));

            Byte[] cipherbytes = rsa.Decrypt(Convert.FromBase64String(encryptText), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns></returns>
        public static String Decrypt(String encryptText)
        {
            CspParameters cspParameters = new CspParameters();
            cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

            return RSAHelper.Decrypt(encryptText, cspParameters, 1024, privatekey);
        }
        /// <summary>
        /// 对数据进行签名
        /// </summary>
        /// <param name="text">要签名的文本</param>
        /// <param name="csp">签名参数</param>
        /// <param name="dwkeySize">默认传入1024</param>
        /// <param name="key">基于base64编码密钥</param>
        /// <returns></returns>
        public static String SignData(String text, CspParameters csp, Int32 dwkeySize, String key)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            Byte[] byte_data = asciiEncoding.GetBytes(text);

            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(1024, csp);
            RSAalg.ImportCspBlob(Convert.FromBase64String(key));

            Byte[] signedData = RSAalg.SignData(byte_data, new SHA1CryptoServiceProvider());
            return StringHelper.BytesToHex(signedData);
        }
        /// <summary>
        /// 对数据进行签名
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String SignData(String text)
        {
            CspParameters csp = new CspParameters();
            csp.Flags = CspProviderFlags.UseMachineKeyStore;

            return RSAHelper.SignData(text, csp, 1024, privatekey);
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="verifyData">要验证字符串明文</param>
        /// <param name="signedData">要验证字符串的签名</param>
        /// <param name="csp">默认传入1024</param>
        /// <param name="dwkeySize"></param>
        /// <param name="key">基于base64编码密钥</param>
        /// <returns></returns>
        public static Boolean VerifySign(String verifyData, String signedData, CspParameters csp, Int32 dwkeySize, String key)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();

            byte[] byteSignedData = Convert.FromBase64String(signedData);
            byte[] byteVerifyDta = asciiEncoding.GetBytes(verifyData);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, csp);
            rsa.ImportCspBlob(Convert.FromBase64String(key));

            return rsa.VerifyData(byteVerifyDta, new SHA1CryptoServiceProvider(), byteSignedData);
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="verifyData">要验证字符串明文</param>
        /// <param name="signedData">要验证字符串的签名</param>
        /// <returns></returns>
        public static Boolean VerifySign(String verifyData, String signedData)
        {
            CspParameters csp = new CspParameters();
            csp.Flags = CspProviderFlags.UseMachineKeyStore;

            return RSAHelper.VerifySign(verifyData, signedData, csp, 1024, publickey);
        }
        /// <summary>
        /// 产生新的公钥和密钥
        /// </summary>
        /// <param name="dwkeySize">默认传1024</param>
        /// <returns></returns>
        public static RSAKey NewRSAKey(Int32 dwkeySize)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwkeySize);
            
            String publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
            String privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));

            RSAKey rsaKey = new RSAKey(publickey, privatekey);

            return rsaKey;
        }

        
    }

    public class RSAKey
    {
        public RSAKey(String publicKey, String privateKey)
        {
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
        }
        public String PublicKey { get; private set; }
        public String PrivateKey { get; private set; }
    }
}
