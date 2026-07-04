using SIE.EventMessages.Release;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.Release.ApiModel
{

    /// <summary>
    /// 工单导入参数
    /// </summary>
    [Serializable]
    public class WorkerOrderImportParamas
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 生产资源编码
        /// </summary>
        public string WipResourceCode { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 模具ID（模具）
        /// </summary>
        public double? MouldId { get; set; }

        /// <summary>
        /// 模具编码
        /// </summary>
        public string MouldCode { get; set; }

        /// <summary>
        /// 模具条码ID（模具）
        /// </summary>
        public double? MouldBarId { get; set; }

        /// <summary>
        /// 摸具条码编码
        /// </summary>
        public string MouldBarCode { get; set; }
        /// <summary>
        /// 是否共模生产
        /// </summary>
        public bool IsSameMode { get; set; }
        /// <summary>
        /// 组合订单号
        /// </summary>
        public string CombinedOrderCode { get; set; }

        /// <summary>
        /// 组合下达的工单号
        /// </summary>
        public string CombinedWorkCode { get; set; }

        /// <summary>
        /// 外部库存组织Id
        /// </summary>
        public string ExterInvOrg {  get; set; }

        /// <summary>
        /// 无车间
        /// </summary>
        public bool WithOutEnterprise { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<WokerOrderImportParamasDetail> Details { get; } = new List<WokerOrderImportParamasDetail>();
    }
    /// <summary>
    /// 下达计划明细数据
    /// </summary>
    [Serializable]
    public class WokerOrderImportParamasDetail
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }

        /// <summary>
        /// 前工艺单编号（英文逗号分隔）
        /// </summary>
        public string BeforeProcessTechOrderCodes { get; set; }

        /// <summary>
        /// 生产订单编号
        /// </summary>
        public string ProductionOrderCode { get; set; }

        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string SaleOrderCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public double PlanAmount { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 是否主料（共模生产时要区分）
        /// </summary>
        public bool IsMainItem { get; set; }

        /// <summary>
        /// 与主物料投入数量比例
        /// </summary>
        public double Proportion { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public Core.WorkOrders.WorkOrderType WorkOrderType { get; set; }

        /// <summary>
        /// 外部工单状态
        /// </summary>
        public int WorkOrderState { get; set; }

        /// <summary>
        /// 拼版数
        /// </summary>
        public int PanelQty { get; set; }

        /// <summary>
        /// 制程工艺Id
        /// </summary>
        public double? ProcessTechId { get; set; }

        /// <summary>
        /// 制程工艺编码
        /// </summary>
        public string ProcessTechCode { get; set; }

        /// <summary>
        /// 工艺面（5正面、10背面）
        /// </summary>
        public int? ProcessSurface { get; set; }
        /// <summary>
        /// 是否外协工单
        /// </summary>
        public bool IsOutsource { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// 是否组合板工单
        /// </summary>
        public bool IsPanelWorkOrder { get; set; }
        /// <summary>
        /// 组合板工单号
        /// </summary>
        public string PanelWorkOrderNo { get; set; }
        /// <summary>
        /// BOM明细
        /// </summary>
        public List<WokerOrderImportParamasBomDetail> BomDetails { get; } = new List<WokerOrderImportParamasBomDetail>();

        /// <summary>
        /// 联副产品
        /// </summary>
        public List<JointByProduct> JointByProducts { get; } = new List<JointByProduct>();
    }

    /// <summary>
    /// Bom明细
    /// </summary>
    [Serializable]
    public class WokerOrderImportParamasBomDetail
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem { get; set; }

        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        public bool IsVritualItem { get; set; }

        /// <summary>
        /// 是否按单标识
        /// </summary>
        public bool IsByBill { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal? AttritionRate { get; set; }

        /// <summary>
        /// 点位
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 主料ID
        /// </summary>
        public double? MainItemId { get; set; }

        /// <summary>
        /// 主料编码
        /// </summary>
        public string MainItemCode { get; set; }

        /// <summary>
        /// 属性值明细
        /// </summary>
        public List<PropertyValue> PropertyValues { get; } = new List<PropertyValue>();

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get;
            set;
        }

        /// <summary>
        /// 组合分组
        /// </summary>
        public string CombinationGroup { get; set; }

        /// <summary>
        /// 欠料数量
        /// </summary>
        public decimal LackQty { get; set; }

    }
}
