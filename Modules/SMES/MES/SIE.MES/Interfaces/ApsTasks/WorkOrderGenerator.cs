using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Routings;
using SIE.MES.WorkOrders.WorkOrderBomGenerators;
using SIE.MES.WorkOrders.WorkOrderJointByproductGenerators;
using SIE.MES.WorkOrders.WorkOrderPackageGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单下达创建工单逻辑
    /// </summary>
    public class WorkOrderGenerator
    {
        /// <summary>
        /// 工单数据缓存类
        /// </summary>
        private readonly WorkOrderDataOwner workOrderDataOwner = new WorkOrderDataOwner();

        /// <summary>
        /// 数据验证者
        /// </summary>
        private readonly TaskReleaseValidator taskReleaseValidator;

        /// <summary>
        /// 工单包装规则
        /// </summary>
        private readonly WoPackageGenerator woPackageGenerator;

        /// <summary>
        /// 工序包装关系生成器
        /// </summary>
        private readonly WoProcessPackingUnitGenerator woProcessPackingUnitGenerator;

        /// <summary>
        /// 工单验证器
        /// </summary>
        private readonly WorkOrderValidator workOrderValidator;

        /// <summary>
        /// 当前数据库的时间
        /// </summary>
        private readonly DateTime curDateTime;

        /// <summary>
        /// 工艺路线相关逻辑
        /// </summary>
        private readonly WoRoutingGenerator woRoutingGenerator;

        /// <summary>
        /// 工单BOM逻辑
        /// </summary>
        private readonly WorkBomUseSourceDataGenerator workOrderBomGenerator;

        /// <summary>
        /// 联副产品逻辑
        /// </summary>
        private readonly WoJointByproductGenerators woJointByproductGenerators;

        /// <summary>
        /// 物料数据拥有者
        /// </summary>
        private readonly ItemDataOwner itemDataOwner = new ItemDataOwner();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_releasePlanDatas">工单下达数据</param>
        /// <param name="_taskReleaseValidator">工单下达验证器</param>
        /// <param name="dateTime">时间</param>       
        public WorkOrderGenerator(IReadOnlyList<ReleasePlanData> _releasePlanDatas,
            TaskReleaseValidator _taskReleaseValidator,
            DateTime dateTime)
        {
            if (_taskReleaseValidator == null)
            {
                throw new ArgumentNullException(nameof(_taskReleaseValidator));
            }

            //工单下达数据验证逻辑
            this.taskReleaseValidator = _taskReleaseValidator;

            //工单BOM生成器（使用接口传过来的数据）
            this.workOrderBomGenerator = new WorkBomUseSourceDataGenerator(_releasePlanDatas, itemDataOwner);

            this.woJointByproductGenerators=new WoJointByproductGenerators (_releasePlanDatas, itemDataOwner);  

            //工单工艺路线生成器
            this.woRoutingGenerator = new WoRoutingGenerator(_releasePlanDatas, itemDataOwner);

            //工单包装规则生成器
            this.woPackageGenerator = new WoPackageGenerator(
                 _taskReleaseValidator.Products);

            this.woProcessPackingUnitGenerator = new WoProcessPackingUnitGenerator(this.woRoutingGenerator.GetProcesses());

            workOrderValidator = new WorkOrderValidator(_taskReleaseValidator, itemDataOwner);

            curDateTime = dateTime;
        }

        /// <summary>
        /// 根据APS下达信息创建MES工单
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanDetail">下达计划明细数据</param>
        /// <param name="curReleasePlanResult">下达结果</param>
        /// <param name="labelPrintTemplates">工单打印模板</param>
        /// <param name="workOrderRoutingLayouts">工单工艺路线布局</param>
        /// <param name="workOrders">已经生成的工单</param>
        /// <returns>创建的MES工单</returns>
        public WorkOrder ReleaseCreateWrorkOrder(ReleasePlanData curReleasePlanData, ReleasePlanDetail curReleasePlanDetail,
            ReleasePlanResult curReleasePlanResult,
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates,
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts, EntityList<WorkOrder> workOrders)
        {
            if (curReleasePlanData == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanData));
            }

            if (curReleasePlanDetail == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanDetail));
            }

            if (curReleasePlanResult == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanResult));
            }

            WorkOrder workOrder;
            try
            {
                //创建工单
                workOrder = CreateMesWorkOrder(curReleasePlanData, curReleasePlanDetail, labelPrintTemplates,
                    workOrderRoutingLayouts, workOrders);
            }
            catch (Exception exc)
            {
                workOrder = null;
                var message = "计划下达创建工单异常: {0} ".L10nFormat(exc.Message);

                var curDetailResult = TaskReleaseHelper.CreateReleaseDetailResult(curReleasePlanDetail.DetailId,
                    curReleasePlanDetail.ProcessTechOrderCode, message, string.Empty);

                curReleasePlanResult.Details.Add(curDetailResult);

                TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, string.Empty);
            }

            if (workOrder != null && curReleasePlanResult.IsSuccess)
            {
                //验证工单的合法性
                workOrderValidator.ValidateSavingWorkOrder(workOrder, curReleasePlanDetail, curReleasePlanResult);

                if (!curReleasePlanResult.IsSuccess)
                {
                    return null;
                }
            }

            workOrderDataOwner.CacheWorkOrder(curReleasePlanDetail.DetailId, workOrder);

            return workOrder;
        }

        /// <summary>
        /// 创建Mes工单
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanDetail">下达计划明细数据</param>
        /// <param name="labelPrintTemplates">工单打印模板设置</param>
        /// <param name="workOrderRoutingLayouts">工单工艺路线布局</param>
        /// <param name="workOrders">已经创建的工单</param>
        /// <returns>Mes工单</returns>
        private WorkOrder CreateMesWorkOrder(ReleasePlanData curReleasePlanData, ReleasePlanDetail curReleasePlanDetail,
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates,
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts, EntityList<WorkOrder> workOrders)
        {
            var workOrder = new WorkOrder();

            workOrder.Source = SourceType.External;
            workOrder.State = Core.WorkOrders.WorkOrderState.Release;
            workOrder.KitType = null;
            workOrder.Type = curReleasePlanDetail.WorkOrderType;
            workOrder.SaleOrderNo = curReleasePlanDetail.SaleOrderCode;
            workOrder.CustomerId = curReleasePlanDetail.CustomerId;
            workOrder.ProcessTechOrderCode = curReleasePlanDetail.ProcessTechOrderCode;
            workOrder.BeforeTechOrderCode = curReleasePlanDetail.BeforeProcessTechOrderCodes;
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = curDateTime;
            workOrder.SupplierId = curReleasePlanDetail.SupplierId;
            workOrder.ItemExtProp = curReleasePlanDetail.ItemExtProp;
            workOrder.ItemExtPropName = curReleasePlanDetail.ItemExtPropName;

            workOrder.LocalContext.SetExtendedProperty("IsExistBom", true);

            //使用接口传过来的数据
            workOrderBomGenerator.GenerateWorkOrderBom(curReleasePlanDetail, workOrder);
            //生成联副产品的数据
            woJointByproductGenerators.GenerateWorkOrderJointByproduct(curReleasePlanDetail, workOrder);

            //保存前会判断是否已经有工单号，如果没有工单号，则批量获取工单号，以提高性能
            workOrder.No = curReleasePlanDetail.WorkOrder;

            var wipResource = taskReleaseValidator.GetWipResource(curReleasePlanData.WipResourceId);
            if (wipResource != null)
            {
                workOrder.FactoryId = wipResource.FactoryId;
            }
            workOrder.WorkShopId = curReleasePlanData.WorkShopId;
            workOrder.ResourceId = curReleasePlanData.WipResourceId;
            workOrder.PlanNo = curReleasePlanData.PlanNo;
            workOrder.ProductId = curReleasePlanDetail.ItemId;
            workOrder.PanelQty = curReleasePlanDetail.PanelQty;
            workOrder.PlanQty = (decimal)curReleasePlanDetail.PlanAmount;
            workOrder.OrderQty = (decimal)curReleasePlanDetail.PlanAmount;
            workOrder.FinishQty = 0;
            workOrder.PlanBeginDate = curReleasePlanDetail.PlanStartTime;
            workOrder.PlanEndDate = curReleasePlanDetail.PlanEndTime;

            workOrder.IsCommonMode = curReleasePlanData.IsSameMode;
            workOrder.IsMainMaterial = curReleasePlanDetail.IsMainItem;
            workOrder.Proportion = curReleasePlanDetail.Proportion;

            workOrder.ProcessTechId = curReleasePlanDetail.ProcessTechId;//// processTech?.Id;
            workOrder.ProductionOrderCode = curReleasePlanDetail.ProductionOrderCode;

            //设置工艺路线(版本、工艺路线布局）
            woRoutingGenerator.SetWorkOrderRouting(workOrders, curReleasePlanDetail, workOrder, workOrderRoutingLayouts);

            workOrder.IsPanelWorkOrder = curReleasePlanDetail.IsPanelWorkOrder; //是否组合板工单

            //当前控制器扩展，其他工程继承自该控制器重写扩展，但IOC注入不成功，触发不到
            RT.Service.Resolve<TaskReleaseExtensionController>()
                .WorkOrderExtensionAssign(workOrder, curReleasePlanDetail);

            //生成工单工序清单
            woRoutingGenerator.GenerateRoutingProcesss(workOrder);

            //生成工单工序BOM
            woRoutingGenerator.GenerateProcessBoms(workOrder);

            //包装规则
            woPackageGenerator.GenerateWorkOrderPackageRule(workOrder);

            //工单工序与包装单位对应关系
            woProcessPackingUnitGenerator.GenerateWorkOrderProcessPackingUnit(workOrder);

            //工单打印模板
            labelPrintTemplates.Add(woPackageGenerator.GenerateProductLabelTemplate(workOrder));

            return workOrder;
        }

        private WorkOrder UpDateExistWorkOrder(WorkOrder existWo, ReleasePlanDetail curReleasePlanDetail)
        {
            
            if (curReleasePlanDetail.ItemId != existWo.ProductId)
            {
                throw new ValidationException("传入工单【{0}】产品Id【{1}】与已上传工单【{0}】产品Id【{2}】冲突，修改失败！".L10nFormat(curReleasePlanDetail.WorkOrder, curReleasePlanDetail.ItemId, existWo.No, existWo.ProductId));
            }
            // 发放可修改工单数量与计划时间以
            if (existWo.State == Core.WorkOrders.WorkOrderState.Release)
            {
                existWo.PlanQty = (decimal)curReleasePlanDetail.PlanAmount;
                existWo.OrderQty = (decimal)curReleasePlanDetail.PlanAmount;
                existWo.PlanBeginDate = curReleasePlanDetail.PlanStartTime;
                existWo.PlanBeginDate = curReleasePlanDetail.PlanEndTime;
            }
            // 生产中不可修改工单数量与计划时间
            else if(existWo.State == Core.WorkOrders.WorkOrderState.Producing && 
                (existWo.PlanQty != (decimal)curReleasePlanDetail.PlanAmount || existWo.OrderQty != (decimal)curReleasePlanDetail.PlanAmount ||
                existWo.PlanBeginDate != curReleasePlanDetail.PlanStartTime || existWo.PlanBeginDate != curReleasePlanDetail.PlanEndTime)) {
                throw new ValidationException("工单【{0}】状态为生产中，不可修改数量与日期！".L10nFormat(existWo.No));
            }
            else
            {
                throw new ValidationException("工单【{0}】状态不为发放、生产中，无法修改！".L10nFormat(existWo.No));
            }
            // 状态变更
            existWo.State = ErpWorkOrderStateMap(curReleasePlanDetail.WorkOrderState, existWo.State);
            //生成工单工单BOM
            UpDataWorkOrderBom(existWo, curReleasePlanDetail);
            return existWo;
        }

        /// <summary>
        /// 更新工单bom
        /// </summary>
        /// <param name="existWo"></param>
        /// <param name="curReleasePlanDetail"></param>
        private void UpDataWorkOrderBom(WorkOrder existWo, ReleasePlanDetail curReleasePlanDetail)
        {

            var upDataBom = curReleasePlanDetail.BomDetails[0] ?? null;
            if (upDataBom == null)
            {
                return;
            }
            var existBom = existWo.BomList.FirstOrDefault(p => p.ItemId == upDataBom.ItemId);
            if (existBom == null)
            {
                //生成工单工单BOM
                workOrderBomGenerator.GenerateWorkOrderBom(curReleasePlanDetail, existWo);
            }
            else
            {
                existBom.RequireQty = upDataBom.RequireQty;
                existBom.SingleQty = upDataBom.SingleQty;
            }
        }

        /// <summary>
        /// 创建Mes工单
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanDetail">下达计划明细数据</param>
        /// <param name="labelPrintTemplates">工单打印模板设置</param>
        /// <param name="workOrderRoutingLayouts">工单工艺路线布局</param>
        /// <param name="workOrders">已经创建的工单</param>
        /// <param name="hisWorkOrders">历史已经创建的工单</param>
        /// <returns>Mes工单</returns>
        private Tuple<WorkOrder, bool> EbsCreateMesWorkOrder(ReleasePlanData curReleasePlanData, ReleasePlanDetail curReleasePlanDetail,
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates,
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts, EntityList<WorkOrder> workOrders, EntityList<WorkOrder> hisWorkOrders)
        {
            WorkOrder workOrder;
            bool isNew;
            // 判断是否新增
            var existWo = hisWorkOrders.FirstOrDefault(p => p.No == curReleasePlanDetail.WorkOrder);
            existWo ??= workOrders.FirstOrDefault(p => p.No == curReleasePlanDetail.WorkOrder);
            if (existWo != null)
            {
                workOrder =  UpDateExistWorkOrder(existWo, curReleasePlanDetail);
                isNew = false;
            }
            else
            {
                workOrder = GenerateWorkOrder(curReleasePlanData, curReleasePlanDetail, labelPrintTemplates, workOrderRoutingLayouts, workOrders);
                isNew = true;
            }
            Tuple<WorkOrder, bool> tuple = new Tuple<WorkOrder, bool>(workOrder, isNew);
            return tuple;
        }

        private WorkOrder GenerateWorkOrder(ReleasePlanData curReleasePlanData, ReleasePlanDetail curReleasePlanDetail,
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates,
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts, EntityList<WorkOrder> workOrders)
        {
            var workOrder = new WorkOrder();

            workOrder.Source = SourceType.External;
            workOrder.State = ErpWorkOrderStateMap(curReleasePlanDetail.WorkOrderState, Core.WorkOrders.WorkOrderState.Release);
            workOrder.KitType = null;
            workOrder.Type = curReleasePlanDetail.WorkOrderType;
            workOrder.SaleOrderNo = curReleasePlanDetail.SaleOrderCode;
            workOrder.CustomerId = curReleasePlanDetail.CustomerId;
            workOrder.ProcessTechOrderCode = curReleasePlanDetail.ProcessTechOrderCode;
            workOrder.BeforeTechOrderCode = curReleasePlanDetail.BeforeProcessTechOrderCodes;
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = curDateTime;
            workOrder.SupplierId = curReleasePlanDetail.SupplierId;
            workOrder.ItemExtProp = curReleasePlanDetail.ItemExtProp;
            workOrder.ItemExtPropName = curReleasePlanDetail.ItemExtPropName;

            workOrder.LocalContext.SetExtendedProperty("IsExistBom", true);
            //使用接口传过来的数据
            workOrderBomGenerator.GenerateWorkOrderBom(curReleasePlanDetail, workOrder);
            //生成联副产品的数据
            woJointByproductGenerators.GenerateWorkOrderJointByproduct(curReleasePlanDetail, workOrder);

            //保存前会判断是否已经有工单号，如果没有工单号，则批量获取工单号，以提高性能
            workOrder.No = curReleasePlanDetail.WorkOrder;

            workOrder.FactoryId = curReleasePlanData.FactoryId;
            workOrder.WorkShopId = curReleasePlanData.WorkShopId;
            workOrder.ResourceId = curReleasePlanData.WipResourceId;
            workOrder.PlanNo = curReleasePlanData.PlanNo;
            workOrder.ProductId = curReleasePlanDetail.ItemId;
            workOrder.PanelQty = curReleasePlanDetail.PanelQty;
            workOrder.PlanQty = (decimal)curReleasePlanDetail.PlanAmount;
            workOrder.OrderQty = (decimal)curReleasePlanDetail.PlanAmount;
            workOrder.FinishQty = 0;
            workOrder.PlanBeginDate = curReleasePlanDetail.PlanStartTime;
            workOrder.PlanEndDate = curReleasePlanDetail.PlanEndTime;

            workOrder.IsCommonMode = curReleasePlanData.IsSameMode;
            workOrder.IsMainMaterial = curReleasePlanDetail.IsMainItem;
            workOrder.Proportion = curReleasePlanDetail.Proportion;

            workOrder.ProcessTechId = curReleasePlanDetail.ProcessTechId;//// processTech?.Id;
            workOrder.ProductionOrderCode = curReleasePlanDetail.ProductionOrderCode;

            //设置工艺路线(版本、工艺路线布局）
            woRoutingGenerator.SetWorkOrderRouting(workOrders, curReleasePlanDetail, workOrder, workOrderRoutingLayouts);

            workOrder.IsPanelWorkOrder = curReleasePlanDetail.IsPanelWorkOrder; //是否组合板工单

            //当前控制器扩展，其他工程继承自该控制器重写扩展，但IOC注入不成功，触发不到
            RT.Service.Resolve<TaskReleaseExtensionController>()
                .WorkOrderExtensionAssign(workOrder, curReleasePlanDetail);

            //生成工单工序清单
            woRoutingGenerator.GenerateRoutingProcesss(workOrder);

            //生成工单工序BOM
            woRoutingGenerator.GenerateProcessBoms(workOrder);

            //包装规则
            woPackageGenerator.GenerateWorkOrderPackageRule(workOrder);

            //工单工序与包装单位对应关系
            woProcessPackingUnitGenerator.GenerateWorkOrderProcessPackingUnit(workOrder);

            //工单打印模板
            labelPrintTemplates.Add(woPackageGenerator.GenerateProductLabelTemplate(workOrder));

            return workOrder;
        }

        /// <summary>
        /// 根据APS下达信息创建MES工单
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanDetail">下达计划明细数据</param>
        /// <param name="curReleasePlanResult">下达结果</param>
        /// <param name="labelPrintTemplates">工单打印模板</param>
        /// <param name="workOrderRoutingLayouts">工单工艺路线布局</param>
        /// <param name="workOrders">已经生成的工单</param>
        /// <param name="hisWorkOrders">历史生成工单</param>
        /// <returns>创建的MES工单</returns>
        public Tuple<WorkOrder, bool> EbsReleaseCreateWrorkOrder(ReleasePlanData curReleasePlanData, ReleasePlanDetail curReleasePlanDetail,
            ReleasePlanResult curReleasePlanResult,
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates,
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts, EntityList<WorkOrder> workOrders, EntityList<WorkOrder> hisWorkOrders)
        {
            if (curReleasePlanData == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanData));
            }

            if (curReleasePlanDetail == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanDetail));
            }

            if (curReleasePlanResult == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanResult));
            }

            WorkOrder workOrder = null;
            bool isNew = false;
            Tuple<WorkOrder, bool> tuple = new Tuple<WorkOrder, bool>(workOrder, isNew);
            try
            {
                //创建工单
                tuple = EbsCreateMesWorkOrder(curReleasePlanData, curReleasePlanDetail, labelPrintTemplates,
                    workOrderRoutingLayouts, workOrders, hisWorkOrders);
                workOrder = tuple.Item1;
                isNew = tuple.Item2;
            }
            catch (Exception exc)
            {
                workOrder = null;
                var message = "计划下达创建工单异常: {0} ".L10nFormat(exc.Message);

                var curDetailResult = TaskReleaseHelper.CreateReleaseDetailResult(curReleasePlanDetail.DetailId,
                    curReleasePlanDetail.ProcessTechOrderCode, message, string.Empty);

                curReleasePlanResult.Details.Add(curDetailResult);

                TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, string.Empty);
            }

            if (workOrder != null && isNew && curReleasePlanResult.IsSuccess)
            {
                //验证工单的合法性
                workOrderValidator.ValidateSavingWorkOrder(workOrder, curReleasePlanDetail, curReleasePlanResult);

                if (!curReleasePlanResult.IsSuccess)
                {
                    return null;
                }
            }

            workOrderDataOwner.CacheWorkOrder(curReleasePlanDetail.DetailId, workOrder);

            return tuple;
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="detailId">下达明细Id</param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(string detailId)
        {
            return workOrderDataOwner.GetWorkOrder(detailId);
        }

        /// <summary>
        /// Ebs工单状态转化
        /// </summary>
        /// <param name="ErpState"></param>
        /// <param name="woState"></param>
        /// <returns></returns>
        private SIE.Core.WorkOrders.WorkOrderState ErpWorkOrderStateMap(int ErpState, SIE.Core.WorkOrders.WorkOrderState woState)
        {
            bool flag = (woState == Core.WorkOrders.WorkOrderState.Release || woState == Core.WorkOrders.WorkOrderState.Producing);
            switch (ErpState)
            {
                case 1:
                    return SIE.Core.WorkOrders.WorkOrderState.CancelRelease;
                case 3:
                    if (flag && ErpState == 3)
                    {
                        return SIE.Core.WorkOrders.WorkOrderState.Release;
                    }
                    else
                    {
                        return woState;
                    }
                case 4:
                    return Core.WorkOrders.WorkOrderState.Finish;
                case 7:
                    return Core.WorkOrders.WorkOrderState.CancelRelease;
                case 12:
                    return Core.WorkOrders.WorkOrderState.Close;
                default:
                    return woState;
            }
        }
    }
}
