using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Easy.Public.MvcSecurity
{
    public class UserIdentity : IIdentity
    {
        internal UserIdentity(Boolean isAuthenticated, String userName,String[] roles,Int32 menuPrivType)
        {
            this.IsAuthenticated = isAuthenticated;
            this.Name = userName;
            this.Roles = roles;
            this.MenuPrivType= menuPrivType;
        }
        #region IIdentity 成员
        public String AuthenticationType
        {
            get { return String.Empty; }
        }
        public Boolean IsAuthenticated
        {
            get;
            private set;
        }
        /// <summary>
        /// 功能权限类型 1所有权限2员工权限3角色权限
        /// </summary>
        public Int32 MenuPrivType
        {
            get;
            private set;

        }
        public String Name
        {
            get;
            private set;
        }
        public String[] Roles
        {
            get;
            private set;
        }
        #endregion
    }
}
