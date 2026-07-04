using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 数据处理帮助类
    /// </summary>
    public static class DataProcessEx
    {
        /// <summary>
        /// 数据拆分执行（1000执行一次）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempDataList"></param>
        /// <param name="action"></param>
        /// <param name="splitSize"></param>
        public static void SplitDataExecute<T>(List<T> tempDataList, Action<List<T>> action, int splitSize = 1000)
        {
            if (tempDataList.Count > splitSize)
            {
                double pageCount = Math.Ceiling((double)tempDataList.Count / splitSize);
                for (int i = 0; i < pageCount; i++)
                {
                    var ids = tempDataList.Skip(i * splitSize).Take(splitSize).ToList();
                    if (action != null)
                    {
                        action.Invoke(ids);
                    }
                }
            }
            else if (action != null)
            {
                action.Invoke(tempDataList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="codes"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<T> SplitContains<T, Y>(IEnumerable<Y> codes, Func<IEnumerable<Y>, List<T>> func)
        {
            codes = codes.Distinct();
            var results = new List<T>();
            while (true)
            {
                if (codes.Count() <= 1000)
                {
                    results.AddRange(func.Invoke(codes));
                    break;
                }
                var firstCodes = codes.Take(1000);
                results.AddRange(func.Invoke(firstCodes));
                codes = codes.Skip(1000);
            }
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="codes"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IList<T> SplitContains<T, Y>(IEnumerable<Y> codes, Func<IEnumerable<Y>, IList<T>> func)
        {
            codes = codes.Distinct();
            var results = new List<T>();
            while (true)
            {
                if (codes.Count() <= 1000)
                {
                    results.AddRange(func.Invoke(codes));
                    break;
                }
                var firstCodes = codes.Take(1000);
                results.AddRange(func.Invoke(firstCodes));
                codes = codes.Skip(1000);
            }
            return results;
        }
    }
}
