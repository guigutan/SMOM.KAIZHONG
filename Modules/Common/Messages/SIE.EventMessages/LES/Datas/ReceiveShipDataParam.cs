using System;
using System.Collections.Generic;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 接收发运单数据
    /// </summary>
    [Serializable]
    public class ReceiveShipDataParam
    {
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 发运单明细行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 发运单单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 接收物料单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 接收条码数据，按LPN收货一定是全部收，不需要回传标签，从数量去判断全收，部分接收，非序列号的传批次号
        /// </summary>
        public List<LabelData> ScanLabels { get; set; } = new List<LabelData>();

        /// <summary>
        /// 接收仓库编码
        /// </summary>
        public string ReceiveWarehouseCode { get; set; }

        /// <summary>
        /// 接收库位编码
        /// </summary>
        public string ReceiveLocationCode { get; set; }

    }

    /// <summary>
    /// 标签数据
    /// </summary>
    [Serializable]
    public class LabelData
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 批次对应数量
        /// </summary>
        public decimal Qty { get; set; }
    }

}
