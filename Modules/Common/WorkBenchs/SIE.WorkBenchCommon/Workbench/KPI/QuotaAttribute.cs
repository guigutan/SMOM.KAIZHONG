using System;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class QuotaAttribute : Attribute
    {
        /// <summary>
        /// 指标编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 指标名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QuotaAttribute()
        {
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        ///<param name="code">指标编码</param>
        ///<param name="name">指标名称</param>
        public QuotaAttribute(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
