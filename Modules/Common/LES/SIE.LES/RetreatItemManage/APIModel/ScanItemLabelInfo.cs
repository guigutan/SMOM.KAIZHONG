using SIE.LES.RetreatItemManage.MaterialReturns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.RetreatItemManage.APIModel
{
    /// <summary>
    /// 
    /// </summary>
   [Serializable]
    public class ScanItemLabelInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ReturnStates States { get; set; }

        /// <summary>
        /// 退料类型
        /// </summary>
        public ReturnTypes retireType { get; set; }

        /// <summary>
        /// 退料类型名称
        /// </summary>
        public string retireTypeLabel { get; set; }

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double LabelId { get; set; }

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
        public decimal Qty { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal BadQty { get; set; }

        /// <summary>
        /// 关联工单Id
        /// </summary>
        public double? ReWorkOrderId { get; set; }

        /// <summary>
        /// 关联工单号
        /// </summary>
        public string ReWorkOrderNo { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 所属批次
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 所属产线Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 来源工单
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 来源工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal WithdrawalQty { get; set; }

        /// <summary>
        /// 退料人Id
        /// </summary>
        public double WithdrawalById { get; set; }

        /// <summary>
        /// 退料操作人
        /// </summary>
        public string WithdrawalByName { get; set; }

        /// <summary>
        /// 退料时间
        /// </summary>
        public string WithdrawalDate { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 现有退料数量
        /// </summary>
        public decimal NowQty { get; set; }

        /// <summary>
        /// 前端输入退料数量
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 退料原因
        /// </summary>
        public string ReturnReason { get; set; }

        /// <summary>
        /// 退料描述
        /// </summary>
        public string ReturnReasonDesc { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 产线资源Id
        /// </summary>
        public double? WipId { get; set; }

        /// <summary>
        /// 产线资源名称
        /// </summary>
        public string WipName { get; set; }

        /// <summary>
        /// 操作人(提交人)
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// 操作时间(提交时间)
        /// </summary>
        public string SubTime { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public double unitPrecsion { get; set; }

        /// <summary>
        /// 进位
        /// </summary>
        public int carry { get; set; }

        /// <summary>
        /// 是否序列号管控
        /// </summary>
        public bool IsSerial {  get; set; }
    }
}
