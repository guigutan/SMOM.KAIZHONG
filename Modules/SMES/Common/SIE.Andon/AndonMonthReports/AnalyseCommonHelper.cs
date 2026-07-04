using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonMonthReports
{
    /// <summary>
    /// 分析通用帮助类
    /// </summary>
    public static class AnalyseCommonHelper
    {

        /// <summary>
        /// 获取指定属性的值
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertieName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static decimal GetStatisticProperties<T>(T t, string propertieName, System.Reflection.PropertyInfo[] properties=null) where T : class
        {
            const decimal tStr = 0m;
            if (t == null)
            {
                return tStr;
            }

            if (properties==null)
                properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                var name = item.Name;
                if (name == propertieName.Replace("Property", ""))
                {
                    object value = item.GetValue(t, null);
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        if (value.ToString().IsNullOrEmpty()) return 0m;
                        return  Convert.ToDecimal(value);
                    }
                }
            }
            return tStr;
        }

        /// <summary>
        /// 获取类属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static System.Reflection.PropertyInfo[] GetPropertyInfos<T>(T t)
        {
            return t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        }

        /// <summary>
        /// 设置属性值-dcimal类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertieName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetStatisticProperties<T>(T t, string propertieName, decimal value) where T : class
        {
            if (t == null)
            {
                return;
            }

            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                var name = item.Name;
                if (name == propertieName.Replace("Property", ""))
                    item.SetValue(t, value);
            }
        }

        /// <summary>
        /// 设置属性值-字符串类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertieName"></param>
        /// <param name="value"></param>
        public static void SetStatisticProperties<T>(T t, string propertieName, string value) where T : class
        {
            if (t == null)
            {
                return;
            }

            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                var name = item.Name;
                if (name == propertieName.Replace("Property", ""))
                    item.SetValue(t, value);
            }
        }
    }
}
