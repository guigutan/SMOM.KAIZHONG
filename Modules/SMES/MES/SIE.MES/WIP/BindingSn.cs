using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 绑定条码
    /// </summary>
    [Serializable]
    public class BindingSn
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}