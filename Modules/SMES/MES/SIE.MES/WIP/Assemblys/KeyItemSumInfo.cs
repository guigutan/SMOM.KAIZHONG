using System;

namespace SIE.MES.WIP.Assemblys
{
    /// <summary>
    /// 关键件使用信息
    /// </summary>
    [Serializable]
    public class KeyItemSumInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
