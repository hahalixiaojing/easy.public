using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace Easy.Public.MvcSecurity
{
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            var user = filterContext.HttpContext.User as AuthenticateUser;
            if (user == null)
            {//票据无效
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new EmptyResult();
                }
                else
                {
                    filterContext.Result = new RedirectResult(AuthenticateHelper.LoginUrl);
                }
                return;
            }

            var identity = user.Identity as UserIdentity;
            if (identity == null)
            {//票据无效
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new EmptyResult();
                }
                else
                {
                    filterContext.Result = new RedirectResult(AuthenticateHelper.LoginUrl);
                }
                return;
            }

            if (identity.MenuPrivType == 1)
            {//所有功能权限
                return;
            }

            var actionPath = this.GetActionPath(filterContext);
            var alias = this.ResourceName(filterContext);
            var hashCode = this.GetActionPathHashCode(actionPath, alias);
            

            var authService = AuthorizationServiceFactory.CreateInstance();
            Int32 resourceId = authService.GetResourceId(actionPath, alias, hashCode);
            if (resourceId <= 0)
            {//登录就可以访问的资源
                return;
            }

            if (identity.MenuPrivType == 2)
            {//员工权限
                var userAuthResult = authService.UserPermission(Int32.Parse(identity.Name), resourceId);
                if (userAuthResult)
                {//用户有权限直接返回
                    return;
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new EmptyResult();
                }
            }
            else if (identity.MenuPrivType == 3)
            {
                if (identity.Roles.Length > 0)
                {
                    var roleAuthResult = authService.RolePermission(Int32.Parse(identity.Roles[0]), resourceId);
                    if (roleAuthResult)
                    {//用户角色有权限返回
                        return;
                    }
                    else
                    {
                        filterContext.HttpContext.Response.StatusCode = 403;
                        filterContext.Result = new EmptyResult();
                    }
                }
            }
        }

        private Int32 GetActionPathHashCode(String actionPath, String alias)
        {
            if (String.IsNullOrEmpty(alias))
            {
                return actionPath.ToUpperInvariant().GetHashCode();
            }
            return (actionPath + "_" + alias.ToUpperInvariant()).GetHashCode();
        }

        private string GetActionPath(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().FullName;

            var descriptor = filterContext.ActionDescriptor;
            string methodName = string.Empty;
            if (descriptor is ReflectedActionDescriptor)
            {
                var concreteDescriptor = descriptor as ReflectedActionDescriptor;
                methodName = concreteDescriptor.MethodInfo.Name;
            }
            else if (descriptor is ReflectedAsyncActionDescriptor)
            {
                var concreteDescriptor = descriptor as ReflectedAsyncActionDescriptor;
                methodName = concreteDescriptor.AsyncMethodInfo.Name;
            }
            return String.Concat(controllerName, ".", methodName);
        }

        private String ResourceName(AuthorizationContext filterContext)
        {
            object[] attribute = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ResourceName), false);

            if (attribute.Length > 0)
            {
                return (attribute[0] as ResourceName).Name;
            }
            return String.Empty;
        }
    }
}