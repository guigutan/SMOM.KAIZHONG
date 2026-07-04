using System;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志关键字属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ApiLogKeyPropertyAttribute : Attribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiLogKeyPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public ApiLogKeyPropertyAttribute()
        {           
        }
    }
}
