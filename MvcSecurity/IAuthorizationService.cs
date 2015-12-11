using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.MvcSecurity
{
    public interface IAuthorizationService
    {
        Int32 GetResourceId(String actionPath, String alias, Int32 hashCode);
        Boolean UserPermission(Int32 userId, Int32 resourceId);
        Boolean RolePermission(Int32 roleId, Int32 resourceId);
    }
}
