using System;

namespace SIE.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// 目标参数设置类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate)]
    public sealed class ArgsSettingAttribute : Attribute
    {
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="type"></param>
        public ArgsSettingAttribute(Type type)
        {
            this.ArgType = type;
        }

        /// <summary>
        /// 目标参数类型
        /// </summary>
        public Type ArgType { get; set; }
    }
}
