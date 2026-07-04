using System;

namespace SIE.Inventory.Onhands.Datas
{
    /// <summary>
    /// 保存数量
    /// </summary>
    [Serializable]
    public class InputData
    {
        /// <summary>
        /// 投入明细Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
