using SIE.LES.StockOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.MaterialReceptions.APIModels
{
    /// <summary>
    /// 前后端交互对象
    /// </summary>
  [Serializable]
   public class MaterialReceptionInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public string Index { get; set; }


        /// <summary>
        /// 标签号
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

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
        /// 工厂ID
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 备料单ID
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 备料单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 备料单明细ID
        /// </summary>
        public double BillDtlId { get; set; }

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
       /// 待接收数
       /// </summary>
        public decimal StayQty { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public decimal ShipQty { get; set; }

        /// <summary>
        /// 所属资源ID
        /// </summary>
        public double?ResourceId { get; set; }

        /// <summary>
        /// 所属资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public double ReceiveBy { get; set; }

       

        /// <summary>
        /// 接收仓库
        /// </summary>
        public string ReceiveWarehouseName { get; set; }

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? ReceiveWarehouseId { get; set; }

        /// <summary>
        /// 接收库位
        /// </summary>
        public string ReceiveStorageLocationName { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double? ReceiveStorageLocationId { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 明细行号
        /// </summary>
        public string SoLineNo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 基本分类
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        ///0：非序列号扫描  1:序列号扫描，
        /// </summary>
        public int ScanType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public StockState StockState { get; set; }

        /// <summary>
        /// 状态·
        /// </summary>
        public string StockStateDisplay { get; set; }

        /// <summary>
        /// 接收明细状态
        /// </summary>
        public ReceiveState DetailState { get; set; }

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料拓展属性(显示)
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 是否手动启用
        /// </summary>
        public bool IsManualRec { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime { get; set; }

        /// <summary>
        /// 接收行号
        /// </summary>
        public string ReceiveRowNumber { get; set; }

        /// <summary>
        /// 是否序列号管控
        /// </summary>
        public bool IsSerialNumberControl { get; set; }
    }
}
