using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.XPath;

namespace Easy.Public.Doc
{
    public class XmlDoc
    {
        XPathNavigator navigator;

        public XmlDoc(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException();
            }

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(fileInfo.FullName);
            }

            XPathDocument doc = new XPathDocument(fileInfo.FullName);
            navigator =  doc.CreateNavigator();
        }

        public String ClassSummary(Type type)
        {
            string xmlName = ClassFullXmlName(type);

            XPathNavigator navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']", xmlName));

            if (navi != null)
            {
                return navi.Value;
            }
            return string.Empty;
        }
        public string MethodSummary(MethodInfo methodInfo)
        {
            string xmlName = MethodFullXmlName(methodInfo);
            XPathNavigator navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']/summary", xmlName));
            if (navi != null)
            {
                return navi.Value;
            }
            return string.Empty;
        }
        public IDictionary<String, MemberTypeInfo> MethodParameters(MethodInfo methodInfo)
        {
            string xmlName = MethodFullXmlName(methodInfo);
            XPathNavigator navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']", xmlName));

            var @params = new Dictionary<string, MemberTypeInfo>();
            if (navi != null)
            {
                ParameterInfo [] paramsInfos = methodInfo.GetParameters();
                XPathNodeIterator it = navi.SelectChildren("param", "");
                foreach (XPathNavigator p in it)
                {
                    string name = p.GetAttribute("name", "");
                    ParameterInfo info = paramsInfos.Single(m => m.Name == name);

                    var memberTypeInfo = new MemberTypeInfo(name, info.ParameterType, (p.Value ?? string.Empty).Trim());
                    @params.Add(name, memberTypeInfo);
                }
            }
            return @params;
        }
        /// <summary>
        /// 查询所有属性描述
        /// </summary>
        /// <param name="type"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        public IDictionary<String, MemberTypeInfo> AllPropertiesSummary(Type type, ICustomNameGetter getter = null)
        {
            var dic = new Dictionary<String, MemberTypeInfo>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in properties)
            {
                string xPath = this.PropertyFullXmlName(item);
                var navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']", xPath));

                if (navi != null)
                {
                    string name = item.Name;
                    if (getter != null)
                    {
                        name = getter.GetName(item);
                    }
                    var memberTypeInfo = new MemberTypeInfo(name, item.PropertyType, navi.Value.Trim());
                    dic.Add(name, memberTypeInfo);
                }
            }
            return dic;
        }


        /// <summary>
        /// 查询所有字段的描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IDictionary<String, MemberTypeInfo> AllFieldsSummary(Type type, ICustomNameGetter getter = null)
        {
            IDictionary<String, MemberTypeInfo> dic = new Dictionary<String, MemberTypeInfo>();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in fields)
            {
                string xPath = this.FieldFullXmlName(item);
                var navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']", xPath));

                if (navi != null)
                {
                    string name = item.Name;
                    if (getter != null)
                    {
                        name = getter.GetName(item);
                    }
                    var memberTypeInfo = new MemberTypeInfo(name, item.FieldType, navi.Value.Trim());
                    dic.Add(item.Name, memberTypeInfo);
                }
            }
            return dic;
        }
        public String MethodReturnSummary(MethodInfo methodInfo)
        {
            string xmlName = MethodFullXmlName(methodInfo);
            XPathNavigator navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']", xmlName));

            if (navi != null)
            {
                XPathNavigator returnNavi = navi.SelectSingleNode("returns");

                if (returnNavi != null)
                {
                    return returnNavi.Value;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获得自定义返回类型
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public String CustomReturnType(MethodInfo methodInfo)
        {
            string xmlName = MethodFullXmlName(methodInfo);
            XPathNavigator navi = navigator.SelectSingleNode(string.Format("doc/members/member[@name='{0}']", xmlName));

            if (navi != null)
            {
                XPathNavigator returnNavi = navi.SelectSingleNode("returns");

                if (returnNavi != null)
                {
                    return returnNavi.GetAttribute("type", "");
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查是不是可空类型
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Boolean IsNullableType(Type type)
        {
            if (type.Name == "Nullable`1")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查属性是不是数组
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Boolean IsArray(Type type)
        {
            return type.IsArray;
        }
        /// <summary>
        /// 获得可空类型的类型
        /// </summary>
        /// <param name="properInfo"></param>
        /// <returns></returns>
        public static Type GetNullableType(Type type)
        {
            if (!XmlDoc.IsNullableType(type))
            {
                throw new NotSupportedException("不是可空类型");
            }
            Type nullabelType = type.GetGenericArguments()[0];
            return nullabelType;
        }
        /// <summary>
        /// 获得属性数组的类型
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Type GetArrayType(Type type)
        {
            if (!type.IsArray)
            {
                throw new NotSupportedException("不是数组");
            }
            string arrayTypeString = type.FullName.Replace("[]", "");
            Type arrayType = Type.GetType(arrayTypeString) ?? Type.GetType(arrayTypeString + "," + type.Module.Assembly.FullName);
            return arrayType;
        }
        /// <summary>
        /// 是不为对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsObjectType(Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Object;
        }
        /// <summary>
        /// 是否元数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsMetaType(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        private String MethodFullXmlName(MethodInfo methodInfo)
        {
            string name = "M:" + methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            ParameterInfo[] paramsInfo = methodInfo.GetParameters();

            if (paramsInfo.Length > 0)
            {
                name += "(";
            }

            foreach (var param in paramsInfo)
            {
                if (param.ParameterType.IsGenericType)
                {
                    name += GetGenericTypeParams(param.ParameterType)+",";
                }
                else
                {
                    name += param.ParameterType.FullName + ",";
                }
            }
            name = name.Trim(',');

            if (paramsInfo.Length > 0)
            {
                name += ")";
            }
            return name;
        }

        private String ClassFullXmlName(Type type)
        {
            string name = type.FullName;
            return "T:" + name;
        }
        private String FieldFullXmlName(FieldInfo fieldInfo)
        {
            string name = "F:" + fieldInfo.DeclaringType.FullName + "." + fieldInfo.Name;
            return name;
        }
        private String PropertyFullXmlName(PropertyInfo propertyInfo)
        {
            string name = "P:" + propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name;
            return name;
        }
        private String GetGenericTypeParams(Type type)
        {
            Int32 index = type.FullName.LastIndexOf('`');

            string name = type.FullName.Substring(0, index);
            name += "{";
            name += type.GetGenericArguments()[0].FullName;
            name += "}";

            return name;
        }
    }
}
