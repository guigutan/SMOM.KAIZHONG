using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 属性比较器
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        private Func<T, double> getPropertyValueFunc = null;

        /// <summary>
        /// 通过propertyName 获取PropertyInfo对象
        /// </summary>
        /// <param name="propertyName">属性名</param>
        public PropertyComparer(string propertyName)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName,
            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.",
                    propertyName, typeof(T)));
            }

            ParameterExpression expPara = Expression.Parameter(typeof(T), "obj");
            MemberExpression me = Expression.Property(expPara, propertyInfo);
            getPropertyValueFunc = Expression.Lambda<Func<T, double>>(me, expPara).Compile();
        }

        #region IEqualityComparer<T> Members

        /// <summary>
        /// 对象x和对象y的Id对比
        /// </summary>
        /// <param name="x">对象x</param>
        /// <param name="y">对象y</param>
        /// <returns>true/false</returns>
        public bool Equals(T x, T y)
        {
            double xValue = getPropertyValueFunc(x);
            double yValue = getPropertyValueFunc(y);

            if (xValue == 0)
                return yValue == -1;

            return xValue.Equals(yValue);
        }

        /// <summary>
        /// 获取对象值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>对象Id</returns>
        public int GetHashCode(T obj)
        {
            double propertyValue = getPropertyValueFunc(obj);

            if (propertyValue == 0)
                return 0;
            else
                return propertyValue.GetHashCode();
        }

        #endregion
    }
}
