using System;
using System.Configuration;
using System.Web;
using Easy.Public.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using Easy.Public;

namespace Easy.Public.MvcSecurity
{
    public static class AuthenticateHelper
    {
        /// <summary>
        /// Cookie域,可选配置
        /// </summary>
        public static String CookieDomain
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_COOKIE_DOMAIN];
            }
        }
        /// <summary>
        /// cookie签名密钥
        /// </summary>
        public static String SiteKey
        {
            get
            {
                return StringHelper.ToString(ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_SITE_KEY], "DB87A73F4AEFB1442FB392A88B55AF94");
            }
        }
        /// <summary>
        /// 票名称，可选配置
        /// </summary>
        public static String CookieName
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_TICKET_NAME] ?? "default_tickect";
            }
        }
        /// <summary>
        /// Cookie Path，可选配置
        /// </summary>
        public static String CookiePath
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_COOKIE_PATH] ?? "/";
            }
        }
        /// <summary>
        /// 票据过有效期（分钟），可选配置
        /// </summary>
        public static Int32 TicketExpiration
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_TICKET_EXPIRATION] ?? "30");
            }
        }
        /// <summary>
        /// 登录URL
        /// </summary>
        public static String LoginUrl
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_LOGIN_URL];
            }
        }
        /// <summary>
        /// 登录成功后需要跳转到的页面
        /// </summary>
        public static String LoginRedirectPage
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_LOGIN_REDIRECT_PAGE];
            }
        }
        /// <summary>
        /// 设置票据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="menuPrivType">权限类型1所有权限2员工权限3角色权限</param>
        /// <param name="userData"></param>
        
        public static AuthenticateUserTicket SetTicket(String userName, String[] roles,Int32 menuPrivType, String userData)
        {
            AuthenticateUserTicket userTicket = new AuthenticateUserTicket(userName, roles,menuPrivType, userData, DateTime.Now, DateTime.Now.AddMinutes(TicketExpiration));
            AuthenticateHelper.SetTicket(userTicket);
            HttpContext.Current.User = new AuthenticateUser(userTicket);
            return userTicket;
        }

        /// <summary>
        /// 登录用户信息
        /// </summary>
        public static void RememberLoginInfo(String key, String[] values, String split)
        {
            String value = String.Join(split, values);

            String encryptText = DESHelper.Encrypt(value, GetLoginUserInfoDESCryptoServiceProvider());
            HttpCookie cookie = new HttpCookie(key);
            HttpContext.Current.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            cookie.HttpOnly = true;
            cookie.Value = encryptText;
            if (!String.IsNullOrEmpty(CookieDomain))
            {
                cookie.Domain = CookieDomain;
            }
            cookie.Path = CookiePath;
            cookie.Expires = DateTime.Now.AddMonths(1);//保存一个月
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static String GetLoginUserInfo(String key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null && cookie.Value != null)
            {
                String userInfo = DESHelper.Decrypt(cookie.Value, GetLoginUserInfoDESCryptoServiceProvider());
                return userInfo;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 销毁票据
        /// </summary>
        public static void DestroyTicket()
        {
            HttpCookie cookie = new HttpCookie(CookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            if (!String.IsNullOrEmpty(CookieDomain))
            {
                cookie.Domain = CookieDomain;
            }
            cookie.Path = CookiePath;
            cookie.Value = null;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 获得票据信息
        /// </summary>
        /// <param name="userTicket"></param>
        /// <returns></returns>
        public static AuthenticateUserTicket AuthenticateTicket()
        {
            String ticketString = GetTicket();
            if (String.IsNullOrEmpty(ticketString))
            {
                return null;
            }
            AuthenticateUserTicket userTicket = Decrypt(DESHelper.Decrypt(ticketString));
            if (userTicket == null)
            {
                DestroyTicket();
                return null;
            }
            if (userTicket.Expired)
            {
                DestroyTicket();
                return null;
            }
            RenewTicket(userTicket);
            return userTicket;
        }
        private static String Encrypt(AuthenticateUserTicket ticket)
        {
            if (ticket != null)
            {
                String base64Ticket = AuthenticateHelper.SerializeUserTicket(ticket);
                String encryptText = DESHelper.Encrypt(base64Ticket);
                String signatureText = SHA256Helper.Signature(encryptText,SiteKey);
                String encryptAndSignatureText = encryptText + "|" + signatureText;

                return DESHelper.Encrypt(encryptAndSignatureText);
            }
            return string.Empty;
        }
        private static AuthenticateUserTicket Decrypt(String encryptedTicket)
        {
            if (String.IsNullOrEmpty(encryptedTicket))
            {
                return null;
            }
            String[] ticketArray = encryptedTicket.Split('|');
            if (ticketArray.Length != 2)
            {
                return null;
            }
            String newSignatureText = SHA256Helper.Signature(ticketArray[0],SiteKey);
            String oldSignatureText = ticketArray[1];

            if (newSignatureText != oldSignatureText)
            {
                return null;
            }
            String base64Ticket = DESHelper.Decrypt(ticketArray[0]);
            return AuthenticateHelper.DeserializeUserTicket(base64Ticket);
        }
        /// <summary>
        /// 序列化用户票据,返回Base64编码
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private static String SerializeUserTicket(AuthenticateUserTicket ticket)
        {
            Byte[] ticketBytes = ObjectSerializationHelper.Serialize<AuthenticateUserTicket>(ticket);
            return Convert.ToBase64String(ticketBytes);
        }
        private static DESCryptoServiceProvider GetLoginUserInfoDESCryptoServiceProvider()
        {
            const String KEY = "1&A!@HT%";
            const String IV = "1^T&+G95";

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            provider.Key = ASCIIEncoding.ASCII.GetBytes(KEY);
            return provider;
        }
        /// <summary>
        /// 从base64返序化到时用户票据对象
        /// </summary>
        /// <param name="base64Ticket"></param>
        /// <returns></returns>
        private static AuthenticateUserTicket DeserializeUserTicket(String base64Ticket)
        {
            Byte[] ticketBytes = Convert.FromBase64String(base64Ticket);
            return ObjectSerializationHelper.Deserialize<AuthenticateUserTicket>(ticketBytes);
        }
        private static void SetTicket(AuthenticateUserTicket userTicket)
        {
            HttpCookie cookie = new HttpCookie(CookieName);
            HttpContext.Current.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            cookie.HttpOnly = true;
            cookie.Value = Encrypt(userTicket);
            if (!String.IsNullOrEmpty(CookieDomain))
            {
                cookie.Domain = CookieDomain;
            }
            cookie.Path = CookiePath;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 获取票据
        /// </summary>
        /// <returns></returns>
        private static String GetTicket()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null && cookie.Value != null)
            {
                return cookie.Value.ToString();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 更新票据
        /// </summary>
        /// <param name="userTicket"></param>
        /// <returns></returns>
        private static void RenewTicket(AuthenticateUserTicket userTicket)
        {
            userTicket.Expiration = DateTime.Now.AddMinutes(TicketExpiration);
            AuthenticateHelper.SetTicket(userTicket);
        }
    }
}
