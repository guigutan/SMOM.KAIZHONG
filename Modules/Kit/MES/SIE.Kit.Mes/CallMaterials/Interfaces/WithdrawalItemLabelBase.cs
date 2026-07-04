using System;

namespace SIE.Kit.MES.CallMaterials.Interfaces
{
    /// <summary>
    /// 物料标签退料信息基类
    /// </summary>
    [Serializable]
    public class WithdrawalItemLabelBase
    {
        /// <summary>
        /// 物料标签号
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 物料Id
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
        /// 物料单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal WithdrawalQty { get; set; }

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double ReceiveWarehouseId { get; set; }

        /// <summary>
        /// 接收仓库名称
        /// </summary>
        public string ReceiveWarehouseName { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 上料工位Id
        /// </summary>
        public double LoadStationId { get; set; }

        /// <summary>
        /// 上料工位名称
        /// </summary>
        public string LoadStationName { get; set; }

        /// <summary>
        /// 所属资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 所属资源名称
        /// </summary>
        public string ResourceName { get; set; }
    }
}
