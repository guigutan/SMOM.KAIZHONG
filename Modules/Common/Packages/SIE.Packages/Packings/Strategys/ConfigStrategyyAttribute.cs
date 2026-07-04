using System;

namespace SIE.Packages.Packings.Strategys
{
    /// <summary>
    /// 策略类型(算法特征)
    /// </summary>
    [Flags]
    public enum ScanStrategyMode
    {
        /// <summary>
        /// 逐个扫描
        /// </summary>
        ScanSingle = 1,

        /// <summary>
        /// 扫一个加入多个
        /// </summary>
        ScanOneJoinToMany = 2,

        /// <summary>
        /// 可选
        /// </summary>
        CustomAble = 4,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 8
    }
}