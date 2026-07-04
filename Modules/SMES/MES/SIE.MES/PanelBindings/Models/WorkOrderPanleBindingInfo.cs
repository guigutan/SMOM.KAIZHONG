using System;

namespace SIE.MES.PanelBindings.Models
{
    /// <summary>
    /// 工单拼板绑定信息
    /// </summary>
    [Serializable]
    public class WorkOrderPanleBindingInfo
    {
        /// <summary>
        /// 未绑定拼板数（排除报废拼板）
        /// </summary>
        public int UnBindingPanelQty { get; set; }

        /// <summary>
        /// 拼板绑定SN数
        /// </summary>
        public int BindingSnQty { get; set; }

        /// <summary>
        /// 未绑定SN数量
        /// </summary>
        public int UnBindingSnQty { get; set; }

        /// <summary>
        /// 工单上线数量
        /// </summary>
        public decimal WorkOrderOnlineQty { get; set; }
    } 
}