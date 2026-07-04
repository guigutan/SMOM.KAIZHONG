using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// Distinct扩展方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="C"></typeparam>
    public class DistinctCommon<T, C> : IEqualityComparer<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<T, C> _getField;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getfield"></param>
        public DistinctCommon(Func<T, C> getfield)
        {
            this._getField = getfield;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return EqualityComparer<C>.Default.Equals(_getField(x), _getField(y));
        }

        /// <summary>
        /// 获取hash code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return EqualityComparer<C>.Default.GetHashCode(this._getField(obj));
        }
    }

    /// <summary>
    /// 去重扩展帮助类
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 自定义Distinct扩展方法
        /// </summary>
        /// <typeparam name="T">要去重的对象类</typeparam>
        /// <typeparam name="C">自定义去重的字段类型</typeparam>
        /// <param name="source">要去重的对象</param>
        /// <param name="getfield">获取自定义去重字段的委托</param>
        /// <returns></returns>
        public static IEnumerable<T> MyDistinct<T, C>(this IEnumerable<T> source, Func<T, C> getfield)
        {
            return source.Distinct(new DistinctCommon<T, C>(getfield));
        }
    }
}
