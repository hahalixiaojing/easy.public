using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.MvcSecurity
{
    public static class ConfigConst
    {
        //appSettings 配置节点key值列表
        public const String APP_SETTING_TICKET_EXPIRATION = "ticket_expiration";
        public const String APP_SETTING_COOKIE_DOMAIN = "cookied_omain";
        public const String APP_SETTING_COOKIE_PATH = "cookie_path";
        public const String APP_SETTING_LOGIN_URL = "login_url";
        public const String APP_SETTING_LOGIN_REDIRECT_PAGE = "login_redirect_page";
        public const String APP_SETTING_TICKET_NAME = "ticket_name";
        public const String APP_SETTING_AUTH_SER = "authorization_service";
        public const String APP_SETTING_SITE_KEY = "site_key";
    }
}
