using System;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 更新发运单数据
    /// </summary>
    [Serializable]
    public class UpdateSoReciveData
    {
        /// <summary>
        /// 备料单号
        /// </summary>
        public string BillNO { get; set; } 

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 明细行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo { get; set; }
    }
}
