using SIE.DataSync;
using System;
using System.Collections.Generic;
using System.Web;

namespace SIE.EventMessages.DataTrace.PushMsg
{
    /// <summary>
    /// 成品入库消息
    /// </summary>
    [Serializable]
    public class ProductInWarehouseMsg : DataSyncMsgBase
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 入库单据ID
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 入库条码ID集合
        /// </summary>
        public List<double> ToStorageBarcodeIds { get; set; }
    }
}
