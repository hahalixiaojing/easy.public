using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Easy.Public.MvcSecurity
{
    public static class AuthorizationServiceFactory
    {
        static IAuthorizationService repository;
        static AuthorizationServiceFactory()
        {
            string classType = ConfigurationManager.AppSettings[ConfigConst.APP_SETTING_AUTH_SER];

            repository = Activator.CreateInstance(Type.GetType(classType)) as IAuthorizationService;
        }

        public static IAuthorizationService CreateInstance()
        {
            return repository;
        }
    }
}