using System;
using System.Xml.Linq;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// xml扩展类
    /// </summary>
    internal static class XElementExtension
    {
        /// <summary>
        /// 获取特性值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xelement">xml元素</param>
        /// <param name="name">特性名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        internal static T GetAttributeValue<T>(this XElement xelement, string name, T defaultValue)
        {
            var attribute = xelement.Attribute(XName.Get(name));
            if (attribute != null)
            {
                return attribute.Value.ConvertTo<T>(defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取bool特性值
        /// 由于泛型获取bool特性值无法获取到正确的，所以重写了一个方法单独获取bool特性值
        /// </summary>
        /// <param name="xelement">xml元素</param>
        /// <param name="name">特性名称</param>
        /// <returns>值</returns>
        internal static bool GetBoolAttributeValue(this XElement xelement, string name)
        {
            bool value = false;
            bool.TryParse(xelement.GetAttributeValue(name, string.Empty), out value);
            return value;
        }
    }
}
