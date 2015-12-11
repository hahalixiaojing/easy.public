using System;
using System.Linq;
using System.Web;
namespace Easy.Public.MvcSecurity
{
    public class AuthenticateModule : IHttpModule
    {
        #region IHttpModule 成员

        public void Dispose()
        {

        }
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);

        }
        private void context_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpApplication applicaiton = sender as HttpApplication;
            HttpRequest request = applicaiton.Context.Request;
            
            AuthenticateUserTicket ticket = AuthenticateHelper.AuthenticateTicket();
            if (ticket != null && !ticket.Expired)
            {
                if (request.AppRelativeCurrentExecutionFilePath.ToUpper() == AuthenticateHelper.LoginUrl.ToUpper())
                {//已经登录则不能再进入登录页面
                    applicaiton.Context.Response.Redirect(AuthenticateHelper.LoginRedirectPage);
                    return;
                }
                applicaiton.Context.User = new AuthenticateUser(ticket);
            }
        }
        #endregion
    }
}
