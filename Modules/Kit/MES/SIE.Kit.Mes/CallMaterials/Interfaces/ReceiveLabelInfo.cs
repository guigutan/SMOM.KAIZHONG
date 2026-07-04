using System;

namespace SIE.Kit.MES.CallMaterials.Interfaces
{
    /// <summary>
    /// 接收标签信息
    /// </summary>
    [Serializable]
    public class ReceiveLabelInfo
    {
        /// <summary>
        /// 标签号
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 叫料单ID
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 叫料单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 需求工位ID（配送工位ID）
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 需求工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 所属资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 所属资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public double ReceiveBy { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 0:验证成功
        /// 1:验证失败
        /// 2:叫料单号扫描
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
    }
}