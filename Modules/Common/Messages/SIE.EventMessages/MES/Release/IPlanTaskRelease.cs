using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Release
{
    /// <summary>
    /// APS任务下达接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyPlanTaskRelease))]
    public interface IPlanTaskRelease
    {
        /// <summary>
        /// APS下达
        /// </summary>
        /// <param name="releasePlanDatas">下达计划任务数据列表</param>
        /// <returns>下达结果列表</returns>
        IReadOnlyList<ReleasePlanResult> TaskRelease(IReadOnlyList<ReleasePlanData> releasePlanDatas);

        /// <summary>
        ///  APS取消下达
        /// </summary>
        /// <param name="cancelReleasePlanDatas">取消下达计划任务数据</param>
        /// <returns>取消下达结果列表</returns>
        IReadOnlyList<ReleasePlanResult> TaskCancelRelease(IReadOnlyList<CancelReleasePlanData> cancelReleasePlanDatas);

        /// <summary>
        /// APS修改工单检查
        /// </summary>
        /// <param name="modifyWoDatas"></param>
        /// <returns></returns>
        IReadOnlyList<ModifyWoResult> CheckModifyWoRelease(IReadOnlyList<ModifyWoData> modifyWoDatas);

        /// <summary>
        /// APS修改工单时间
        /// </summary>
        /// <param name="modifyWoDatas"></param>
        /// <returns></returns>
        IReadOnlyList<ModifyWoResult> ModifyWoRelease(IReadOnlyList<ModifyWoData> modifyWoDatas);
    }

    /// <summary>
    /// 空方法
    /// </summary>
    public class EmptyPlanTaskRelease : IPlanTaskRelease
    {


        /// <summary>
        /// 取消下达
        /// </summary>
        /// <param name="cancelReleasePlanDatas"></param>
        /// <returns></returns>
        public IReadOnlyList<ReleasePlanResult> TaskCancelRelease(IReadOnlyList<CancelReleasePlanData> cancelReleasePlanDatas)
        {
            List<ReleasePlanResult> results = new List<ReleasePlanResult>();

            foreach (CancelReleasePlanData cancelReleasePlan in cancelReleasePlanDatas)
            {
                ReleasePlanResult cancelRelease = new ReleasePlanResult(cancelReleasePlan.PlanTaskId);
                cancelRelease.Message = "";
                cancelRelease.IsSuccess = true;

                ReleaseDetailResult releaseDetail = new ReleaseDetailResult();
                releaseDetail.DetailId = cancelReleasePlan.PlanTaskId;
                releaseDetail.Message = "";
                releaseDetail.WorkOrder = cancelReleasePlan.OrderCodes;
                cancelRelease.Details.Add(releaseDetail);

                results.Add(cancelRelease);
            }

            return results;
        }

        /// <summary>
        /// 计划下达
        /// </summary>
        /// <param name="releasePlanDatas"></param>
        /// <returns></returns>
        public IReadOnlyList<ReleasePlanResult> TaskRelease(IReadOnlyList<ReleasePlanData> releasePlanDatas)
        {
            List<ReleasePlanResult> results = new List<ReleasePlanResult>();

            foreach (ReleasePlanData releasePlan in releasePlanDatas)
            {
                ReleasePlanResult release = new ReleasePlanResult(releasePlan.PlanTaskId);                
                release.Message = "";
                release.IsSuccess = true;
                foreach (ReleasePlanDetail detail in releasePlan.Details)
                {
                    ReleaseDetailResult releaseDetail = new ReleaseDetailResult();
                    releaseDetail.DetailId = detail.DetailId;
                    releaseDetail.Message = "";
                    releaseDetail.ProcessTechOrderCode = detail.ProcessTechOrderCode;
                    releaseDetail.WorkOrder = releasePlan.PlanNo;
                    release.Details.Add(releaseDetail);
                }

                results.Add(release);
            }

            return results;
        }
        /// <summary>
        /// 修改工单校验
        /// </summary>
        /// <param name="modifyWoDatas"></param>
        /// <returns></returns>
        public IReadOnlyList<ModifyWoResult> CheckModifyWoRelease(IReadOnlyList<ModifyWoData> modifyWoDatas)
        {
            return new List<ModifyWoResult>();
        }
        /// <summary>
        /// 修改工单
        /// </summary>
        /// <param name="modifyWoDatas"></param>
        /// <returns></returns>
        public IReadOnlyList<ModifyWoResult> ModifyWoRelease(IReadOnlyList<ModifyWoData> modifyWoDatas)
        {
            var list = new List<ModifyWoResult>();
            foreach (var item in modifyWoDatas)
            {
                ModifyWoResult result = new ModifyWoResult();
                result.TaskPlanID = item.TaskPlanID;
                result.IsSuccess = "Y";
                result.WorkOrder = item.TaskPlanID;
                list.Add(result);
            }
            return list;
        }
    }

    /// <summary>
    /// 下达计划数据分组
    /// </summary>
    [Serializable]
    public class ReleasePlanDataGroup
    {
        /// <summary>
        /// 组合订单号
        /// </summary>
        public string CombinedOrderCode { get; set; }
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public List<string> OrderCodes { get; set; }

        /// <summary>
        /// 计划列表
        /// </summary>
        public List<ReleasePlanData> PlanDatas { get; } = new List<ReleasePlanData>();
    }

    /// <summary>
    /// 取消下达计划数据分组
    /// </summary>
    [Serializable]
    public class CancelReleasePlanDataGroup
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public List<string> OrderCodes { get; set; }

        /// <summary>
        /// 计划列表
        /// </summary>
        public List<CancelReleasePlanData> CancelReleasePlanDatas { get; } = new List<CancelReleasePlanData>();
    }

    /// <summary>
    /// 下达计划数据
    /// </summary>
    [Serializable]
    public class ReleasePlanData
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
        /// 工厂Id
        /// </summary>
        public double FactoryId {  get; set; }

        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 模具ID（模具）
        /// </summary>
        public double? MouldId { get; set; }

        /// <summary>
        /// 模具条码ID（模具）
        /// </summary>
        public double? MouldBarId { get; set; }

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
        /// 无车间
        /// </summary>
        public bool WithOutEnterprise { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<ReleasePlanDetail> Details { get; } = new List<ReleasePlanDetail>();
    }

    /// <summary>
    /// 下达计划明细数据
    /// </summary>
    [Serializable]
    public class ReleasePlanDetail
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
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }
        
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
        /// 工单状态
        /// </summary>
        public int WorkOrderState {  get; set; }

        /// <summary>
        /// 拼版数
        /// </summary>
        public int PanelQty { get; set; }

        /// <summary>
        /// 制程工艺Id
        /// </summary>
        public double? ProcessTechId { get; set; }

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
        public List<BomDetail> BomDetails { get; } = new List<BomDetail>();

        /// <summary>
        /// 联副产品
        /// </summary>
        public List<JointByProduct> JointByProducts { get; } = new List<JointByProduct>();
    }

    /// <summary>
    /// 联副产品数据
    /// </summary>
    [Serializable]
    public class JointByProduct
    {
        /// <summary>
        ///产出类型 0副产品 1联产品
        /// </summary>
        public int OutPutType { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        public double?ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

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
    }


    /// <summary>
    /// BOM明细数据
    /// </summary>
    [Serializable]
    public class BomDetail
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

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

    /// <summary>
    /// 属性值
    /// </summary>
    [Serializable]
    public class PropertyValue
    {
        /// <summary>
        /// 属性定义ID
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 取消下达信息数据
    /// </summary>
    [Serializable]
    public class CancelReleasePlanData
    {
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }
        /// <summary>
        /// 订单代码
        /// </summary>
        public string OrderCodes { get; set; }
    }

    /// <summary>
    /// 下达结果
    /// </summary>
    [Serializable]
    public class ReleasePlanResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReleasePlanResult(string planTaskId)
        {
            PlanTaskId = planTaskId;
            IsSuccess = true;
            Details = new List<ReleaseDetailResult>();
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId { get; set; }

        /// <summary>
        /// 每个计划任务的结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<ReleaseDetailResult> Details { get; }
    }

    /// <summary>
    /// 下达结果明细
    /// </summary>
    [Serializable]
    public class ReleaseDetailResult
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }

        /// <summary>
        /// 每个计划任务明细的结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 下达后的工单号
        /// </summary>
        public string WorkOrder { get; set; }
    }

    /// <summary>
    /// 修改工单数据
    /// </summary>
    [Serializable]
    public class ModifyWoData
    {
        /// <summary>
        /// 计划ID
        /// </summary>
        public string TaskPlanID { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 产线资源
        /// </summary>
        public double ResourceId { get; set; }

    }
    /// <summary>
    /// 结果返回的值
    /// </summary>
    [Serializable]
    public class ModifyWoResult
    {
        /// <summary>
        /// 计划ID
        /// </summary>
        public string TaskPlanID { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// 结果 Y/N
        /// </summary>
        public string IsSuccess { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

    }

}
