using System;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志关键字参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public class ApiLogKeyParamAttribute : Attribute
    {
        /// <summary>
        /// 关键字索引号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 关键字说明
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">关键字索引号</param>
        public ApiLogKeyParamAttribute(int index)
        {
            Index = index;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">关键字索引号</param>
        /// <param name="keyName">显示的名称</param>
        public ApiLogKeyParamAttribute(int index, string keyName)
        {
            Index = index;
            KeyName = keyName;
        }
    }
}
