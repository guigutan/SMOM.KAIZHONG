using System;
using System.Collections.Generic;

namespace SIE.xUnit.Core.Common
{
    /// <summary>
    /// IList扩展类
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// 随机获取数组中元素
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Random<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);
            return list[new Random().Next(list.Count)];
        }
    }
}
