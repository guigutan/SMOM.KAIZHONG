using SIE.ObjectModel;
using System;

namespace SIE.Packages
{
    /// <summary>
    /// 包装载体
    /// </summary>    
    public enum ContainerType
    {
        /// <summary>
        /// 主单位
        /// </summary>
        [Label("主单位")]
        Unit=0,

        /// <summary>
        /// 内包装
        /// </summary>
        [Label("内包装")]
        Package=1,

        /// <summary>
        /// 箱
        /// </summary>
        [Label("箱")]
        Carton=2,

        /// <summary>
        /// 栈板
        /// </summary>
        [Label("栈板")]
        Pallet=3,
    }
}