using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.Common
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtentions
    {
        /// <summary>
        /// 下一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T? Next<T>(this T v) where T : struct
        {
            T[] Arr = (T[])Enum.GetValues(v.GetType());
            int j = Array.IndexOf<T>(Arr, v) + 1;
            if (Arr.Length == j)
                return null;
            else
                return Arr[j];
        }

        /// <summary>
        /// 前一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T? Previous<T>(this T v) where T : struct
        {
            T[] Arr = (T[])Enum.GetValues(v.GetType());
            int j = Array.IndexOf<T>(Arr, v) - 1;
            if (j < 0)
                return null;
            else
                return Arr[j];
        }
    }
}
