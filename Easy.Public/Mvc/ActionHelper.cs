using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
namespace Easy.Public.Mvc
{
    public static class ActionHelper
    {
        public static ControllerDescription[] Get(Assembly ass)
        {

            var controllerDescList = new List<ControllerDescription>();

            IList<Type> controllerTypes = GetAllControllers(ass);

            controllerDescList.AddRange(GetAreaControllerType(controllerTypes));

            foreach (var controllerDesc in controllerDescList)
            {
                controllerDesc.ActionDescription = GetActionList(controllerDesc);
            }

            return controllerDescList.ToArray();

        }

        private static IList<Type> GetAllControllers(Assembly ass)
        {
            IList<Type> controllerTypes = new List<Type>();

            foreach (var type in ass.GetTypes().Where(t => typeof(IController).IsAssignableFrom(t) && !t.IsAbstract))
            {
                controllerTypes.Add(type);
            }
            return controllerTypes;
        }
        private static ControllerDescription[] GetAreaControllerType(IList<Type> controllerTypes)
        {
            var controllerDescList = new List<ControllerDescription>();

            foreach (Route route in RouteTable.Routes)
            {
                if (route.DataTokens == null)
                {
                    continue;
                }

                if (route.DataTokens.Keys.Contains("area"))
                {
                    String[] nameSpaces = route.DataTokens["NameSpaces"] as String[];

                    IList<Type> areaTypes = AreaTypes(controllerTypes, nameSpaces);

                    controllerDescList.AddRange(Convert(areaTypes, route.Url));
                }
                else
                {
                    controllerDescList.AddRange(Convert(controllerTypes, route.Url));
                }
            }

            return controllerDescList.ToArray();
        }
        private static IList<Type> AreaTypes(IList<Type> controllerTypes, string[] @namespace)
        {
            IList<Type> areaTypes = new List<Type>();

            foreach (string nameSpace in @namespace)
            {
                string subnamespace = nameSpace.Substring(0, nameSpace.LastIndexOf("."));

                IList<Type> areaControllerTypes = controllerTypes.Where(t => t.FullName.StartsWith(subnamespace)).ToList();

                foreach (var item in areaControllerTypes)
                {
                    areaTypes.Add(item);
                    controllerTypes.Remove(item);
                }
            }
            return areaTypes;
        }
        private static IList<ControllerDescription> Convert(IList<Type> controllerType,String url)
        {
            var controllerDescList = new List<ControllerDescription>();

            foreach (var type in controllerType)
            {
                var controllerDesc = new ControllerDescription()
                {
                    Name = type.Name.Replace("Controller", ""),
                    ControllerType = type,
                    UrlTemplate = url,
                    ClassPath = type.FullName,
                };
                controllerDescList.Add(controllerDesc);
            }
            return controllerDescList;
        }
        private static IList<ActionDescription> GetActionList(ControllerDescription controllerDesc)
        {
            var actionDescList = new List<ActionDescription>();

            var methods = controllerDesc.ControllerType.GetMethods().Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType));

            foreach (var method in methods)
            {
                var actionDesc = new ActionDescription()
                {
                    Name = method.Name,
                    Url = ReplaceUrl(controllerDesc.UrlTemplate, controllerDesc.Name, GetActionName(method)),
                    ActionPath = String.Concat(controllerDesc.ClassPath, ".", method.Name),
                    MethodInfo = method
                };
                
                actionDescList.Add(actionDesc);
            }
            return actionDescList;
        }
        private static String GetActionName(MemberInfo m)
        {
            var actionName = m.GetCustomAttributes(false).Where(t => t is ActionNameAttribute).Select(t => t as ActionNameAttribute).ToArray();

            if (actionName.Length > 0)
            {
                return actionName[0].Name;
            }
            return m.Name;
        }
        private static String ReplaceUrl(String urlTemplate, string controllerName, string actionName)
        {
            return urlTemplate.Replace("{controller}", controllerName.Replace("Controller", "")).Replace("{action}", actionName);
        }
    }
   
}