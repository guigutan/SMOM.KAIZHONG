using SIE.Common;
using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 采集结果 For工艺路线设计
    /// </summary>
    public enum ResultTypeForDesign
    {
        /// <summary>
        /// 任意
        /// </summary>
        [Label("任意")]
        [Category("Common")]
        Any = ResultType.Pass | ResultType.Fail,

        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        [Category("Common")]
        Pass = ResultType.Pass,

        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        [Category("Common")]
        Fail = ResultType.Fail,

        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        [Category("Custom")]
        Custom = ResultType.Custom,

        /// <summary>
        /// 可选路径
        /// </summary>
        [Label("可选路径")]
        Optional = ResultType.Optional,
    }
}