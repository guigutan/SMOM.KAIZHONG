using System;

namespace SIE.ControlChart.SpcWarningRull
{
    /// <summary>
    /// Spc判异规则简单信息
    /// </summary>
    [Serializable]
    public class SimpleSpcRull
    {
        /// <summary>
        /// 连接N个点
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// 规则说明
        /// </summary>
        public string RullDescription { get; set; }
    }
}
