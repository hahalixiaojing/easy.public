using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public
{
    /// <summary>
    /// 实体属性赋值帮助类
    /// </summary>
    public class EntityPropertyHelper<Model>
    {
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="M">实例对象类型</typeparam>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="expression"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetValue<T>(Expression<Func<Model, T>> expression, Model instance, T value)
        {
            PropertyInfo propertyInfo = GetPropertyName<T>(expression, instance);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(instance, value);
            }
        }

        private PropertyInfo GetPropertyName<T>(Expression<Func<Model, T>> expression, Model instance)
        {
            string text = expression.Body.ToString();
            string propertyName = text.Substring(text.IndexOf(".") + 1);

            return instance.GetType().GetProperty(propertyName);
        }
    }
}
