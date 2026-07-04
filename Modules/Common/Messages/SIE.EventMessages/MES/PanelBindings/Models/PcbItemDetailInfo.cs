using System;

namespace SIE.EventMessages.MES.PanelBindings.Models
{
    /// <summary>
    /// Pcb物料明细信息
    /// </summary>
    [Serializable]
    public class PcbItemDetailInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
    }
}
