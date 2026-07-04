using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages
{
    /// <summary>
    /// MES上料下料入库线边仓更新库存数据
    /// </summary>
    [Serializable]
    public class MesUpdateOnhandData
    {
        /// <summary>
        /// 标签号
        /// </summary>
        public List<MesLabelData> LabelDatas { get; set; } = new List<MesLabelData>();

        /// <summary>
        /// 操作类型 0-上料 1-下料 2-倒扣非工序BOM物料 3-接收 4-半成品入库线边仓,只有下料和接收是增加库位库存、其他都是减库存,
        /// </summary>
        public int OpType { get; set; }

        /// <summary>
        /// 是否维修下料
        /// </summary>
        public bool IsRepair { get; set; }

        /// <summary>
        /// 工单号,没有工单传备料单
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工单Id,没有工单传备料单Id
        /// </summary>
        public double? WoId { get; set; }

        /// <summary>
        /// 操作员工编号
        /// </summary>
        public string EmpCode { get; set; }

        /// <summary>
        /// 生产部门Id/车间Id
        /// </summary>
        public double? EnterpriseId { get; set; }

        /// <summary>
        /// 事务类型
        /// </summary>
        public string TranstypeName { get; set; }

        /// <summary>
        /// 包装规则明细（半成品入库线边仓必填）注意是物料的包装规则，工单有引用的
        /// </summary>
        public double? ItemPackRuleDetailId { get; set; }

        /// <summary>
        /// 入库的线边仓（半成品入库线边仓必填）
        /// </summary>
        public double? WarehouseId { get; set; }
    }

    /// <summary>
    /// MES条码数据
    /// </summary>
    [Serializable]
    public class MesLabelData
    {
        /// <summary>
        /// 条码/批次号/物料编码
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 是否不合格（下料才会有不合格的情况）
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 标签数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }


        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; } = "*";


        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 备料单明细ID
        /// </summary>
        public double StockDtlId { get; set; }
    }

    /// <summary>
    /// MES库存转移
    /// </summary>
    [Serializable]
    public class MesMoveUpdateOnhandData : MesUpdateOnhandData
    {
        /// <summary>
        /// 目标库位
        /// </summary>
        public double TargetStorageLocationId { get; set; }
    }

    /// <summary>
    /// 接收库存
    /// </summary>
    [Serializable]
    public class LesReciveInUpdateOnhandData
    {
        /// <summary>
        /// 收货仓库
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 收货库位
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 库存Id
        /// </summary>
        public double LotLpnonhandId { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty { get; set; }
        
        /// <summary>
        /// 单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 明细Id
        /// </summary>
        public double DtlId { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public List<string> Sns { get; set; }

    }
}
