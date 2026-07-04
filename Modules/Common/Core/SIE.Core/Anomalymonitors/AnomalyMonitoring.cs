using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.AnomalyMonitors
{
    /// <summary>
    /// 异常监控特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AnomalyMonitoringAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="label"></param>
        public AnomalyMonitoringAttribute(string label)
        {
            Label = label;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Label { get; private set; }
    }
}
