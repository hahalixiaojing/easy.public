using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web.Security;

namespace Easy.Public.MvcSecurity
{
    public class AuthenticateUser : IPrincipal 
    {
        private UserIdentity identity;
        private String userData;
       
        public AuthenticateUser(AuthenticateUserTicket ticket)
        {
            identity = new UserIdentity(!ticket.Expired, ticket.Name, ticket.Roles, ticket.MenuPrivType);
            this.userData = ticket.UserData;
        }

        public String UserData
        {
            get
            {
                return this.userData;
            }
        }

        #region IPrincipal 成员

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public Boolean IsInRole(String role)
        {
            return true;
        }

        #endregion
    }
}
