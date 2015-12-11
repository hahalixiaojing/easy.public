using Easy.Public;
using Easy.Public.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace Easy.Public.MvcSecurity
{
    /// <summary>
    /// API访问授权验证基类
    /// </summary>
    public abstract class ApiAuthorize : FilterAttribute,IAuthorizationFilter
    {
        /// <summary>
        /// 获得当前API的资源ID
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionHashCode"></param>
        /// <returns></returns>
        protected abstract Int32 GetApiResourceId(String action, Int32 actionHashCode);
        /// <summary>
        /// 检查是否有权限访问该API，true表示可以，false不可以
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        protected abstract Boolean CheckPermission(String appKey, Int32 resourceId);
        /// <summary>
        /// 获得用户密钥
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        protected abstract String GetUserSecretKey(String appKey);
        /// <summary>
        /// API访问授权成功后，调用的方法，例如，设置 HttpContext.User属性
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appkey"></param>
        /// <param name="resourceId"></param>
        protected abstract void AuthorizeSuccess(HttpContextBase context, String appkey, Int32 resourceId);

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var app_key = StringHelper.ToString(request["appkey"], String.Empty);

      
            if (String.IsNullOrWhiteSpace(app_key))
            {
                filterContext.Result = new JsonResult() { Data = new { code = "415", message = "appkey error" } };
                return;
            }

            DateTime? timestamp = StringHelper.ToDateTime(request["timestamp"], null);
            if (timestamp == null || (DateTime.Now - timestamp.Value).TotalMinutes > 6)
            {
                filterContext.Result = filterContext.Result = new JsonResult() { Data = new { code = "415", message = "date error" } };

                return;
            }

            string actionPath = this.GetActionPath(filterContext);
            Int32 resourceId = this.GetApiResourceId(actionPath, actionPath.GetHashCode());
            if (resourceId == 0)
            {
                filterContext.Result = new JsonResult() { Data = new { code = "404", message = "resource not exists" } };
                return;
            }

            if (!this.CheckPermission(app_key, resourceId))
            {
                filterContext.Result = new JsonResult() { Data = new { code = "403", message = "permisson error" } };
                return;
            }

            String secretKey = this.GetUserSecretKey(app_key);
            if (String.IsNullOrWhiteSpace(secretKey))
            {
                filterContext.Result = new JsonResult() { Data = new { code = "415", message = "appkey error" } };
                return;
            }

          
            var usrSign = request["sign"];
            if (String.IsNullOrWhiteSpace(usrSign) || usrSign != this.SignData(request, secretKey))
            {
                filterContext.Result = filterContext.Result = new JsonResult() { Data = new { code = "416", message = "sign error" } };
                return;
            }
            this.AuthorizeSuccess(filterContext.HttpContext, app_key, resourceId);
        }

        protected virtual String SignData(HttpRequestBase request,string password)
        {
            var keys = request.Form.AllKeys;

            var dic = new SortedDictionary<String, String>();
            foreach (var key in keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }
                dic.Add(key, request.Form[key]);
            }

            var strBuilder = new StringBuilder();
            strBuilder.Append(password);
            foreach (var item in dic)
            {
                if (item.Key == "sign")
                {
                    continue;
                }
                strBuilder.AppendFormat("{0}{1}", item.Key, item.Value);
            }
            strBuilder.Append(password);

            var sign = MD5Helper.Encrypt(strBuilder.ToString()).ToUpper();

            return sign;
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
    }
}