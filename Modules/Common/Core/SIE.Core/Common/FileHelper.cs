using System;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        protected FileHelper() { }

        /// <summary>
        /// 路径分隔符
        /// </summary>
        public const char DIR_SEPARATOR = '/';

        #region 拼接路径
        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombinePath(params string[] paths)
        {
            string[] processedPaths = paths
                .Select(path =>
                {
                    if (path == null) return path;

                    var temp = path.Replace('\\', DIR_SEPARATOR);
                    temp = temp.Trim(DIR_SEPARATOR);
                    return temp;
                })
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .ToArray();
            return processedPaths.Concat(DIR_SEPARATOR.ToString());
        }
        #endregion
    }
}
