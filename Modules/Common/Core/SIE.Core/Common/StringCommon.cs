using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringCommon
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        public const char SplitChar = ',';

        /// <summary>
        /// 分隔符
        /// </summary>
        public const string SplitStr = ",";

        /// <summary>
        /// 添加模糊查询符号【%】的位置
        /// </summary>
        public enum PadLikLocation
        {
            /// <summary>
            /// 左右
            /// </summary>
            ALL,
            /// <summary>
            /// 左
            /// </summary>
            LEFT,
            /// <summary>
            /// 右
            /// </summary>
            RIGHT
        }
        /// <summary>
        /// 给字符串添加模糊查询符号
        /// </summary>
        /// <param name="arg">字符串</param>
        /// <param name="pl">位置</param>
        /// <returns>拼接后的字符串</returns>
        public static string AddLike(this string arg, PadLikLocation pl = PadLikLocation.ALL)
        {
            if (arg.IsNullOrEmpty())
            {
                return string.Empty;
            }

            switch (pl)
            {
                case PadLikLocation.ALL:
                    return "%{0}%".FormatArgs(arg);
                case PadLikLocation.LEFT:

                    return "%{0}".FormatArgs(arg);

                case PadLikLocation.RIGHT:
                    return "{0}%".FormatArgs(arg);

                default:
                    return arg;
            }
        }

        /// <summary>
        /// 转换成ID数组
        /// </summary>
        /// <param name="strIdList"></param>
        /// <returns></returns>
        public static List<double> ToIdList(this string strIdList)
        {
            if (strIdList.IsNullOrEmpty())
                return new List<double>();
            var idArray = strIdList.Split(SplitChar);
            return Array.ConvertAll(idArray, p => Convert.ToDouble(p)).ToList();
        }
    }
}
