using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 图表常量
    /// </summary>
    [Serializable]
    public class ChartCalConst
    {
        /// <summary>
        /// Spc计算公式常量A
        /// </summary>
        public double?[] A { get; set; }
        /// <summary>
        /// Spc计算公式常量A2
        /// </summary>
        public double?[] A2 { get; set; }
        /// <summary>
        /// Spc计算公式常量A3
        /// </summary>
        public double?[] A3 { get; set; }
        /// <summary>
        /// Spc计算公式常量B3
        /// </summary>
        public double?[] B3 { get; set; }
        /// <summary>
        /// Spc计算公式常量B4
        /// </summary>
        public double?[] B4 { get; set; }
        /// <summary>
        /// Spc计算公式常量B5
        /// </summary>
        public double?[] B5 { get; set; }
        /// <summary>
        /// Spc计算公式常量B6
        /// </summary>
        public double?[] B6 { get; set; }
        /// <summary>
        /// Spc计算公式常量C4
        /// </summary>
        public double?[] C4 { get; set; }
        /// <summary>
        /// Spc计算公式常量D1
        /// </summary>
        public double?[] D1 { get; set; }
        /// <summary>
        /// Spc计算公式常量D2
        /// </summary>
        public double?[] D2 { get; set; }
        /// <summary>
        /// Spc计算公式常量D3
        /// </summary>
        public double?[] D3 { get; set; }
        /// <summary>
        /// Spc计算公式常量D4-适用于柯西分布
        /// </summary>
        public double?[] D4 { get; set; }
        /// <summary>
        /// Spc计算公式常量d2
        /// </summary>
        public double?[] D2Nd { get; set; }
        /// <summary>
        /// Spc计算公式常量d3
        /// </summary>
        public double?[] D3Nd { get; set; }
        /// <summary>
        /// Spc计算公式常量D4-适用于正太分布
        /// </summary>
        public double?[] D4Nd { get; set; }
        /// <summary>
        /// Spc计算公式常量E2
        /// </summary>
        public double?[] E2 { get; set; }
        /// <summary>
        /// 中值图
        /// </summary>
        public double?[] MeA2 { get; set; }
    }
}
