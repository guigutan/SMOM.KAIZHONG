using Microsoft.Scripting.Utils;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Release;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.RoutingSettings;
using SIE.MES.WorkOrders.Configs;
using SIE.MES.WorkOrders.WorkOrderProcessBomGenerators;
using SIE.Resources.ProcessTechs;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders.Routings
{
    /// <summary>
    /// 工单工艺路线逻辑
    /// </summary>
    public class WoRoutingGenerator
    {
        /// <summary>
        /// 产品工艺路线
        /// </summary>
        private readonly EntityList<ProductRouting> productRoutings;

        /// <summary>
        /// 资源工艺路线
        /// </summary>
        private readonly EntityList<ResourceRouting> resourceRoutings;

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        private readonly EntityList<RoutingVersion> routingVersions;

        /// <summary>
        /// 制程工艺列表
        /// </summary>
        private readonly EntityList<ProcessTech> processTeches;

        /// <summary>
        /// 拼板工单
        /// </summary>
        private readonly EntityList<WorkOrder> panelWorkOrders;

        /// <summary>
        /// 工艺路线版本的布局
        /// </summary>
        private readonly EntityList<RoutingLayout> routingLayouts;

        /// <summary>
        /// 工单工序清单逻辑
        /// </summary>
        public WoRoutingProcessGenerator WoRoutingProcesssGenerator { get; }

        /// <summary>
        /// 工单工序BOM生成器
        /// </summary>
        private IWoProcessBomGenerator woProcessBomGenerator { get; }

        /// <summary>
        /// 是否使用工艺路线的工序 BOM 设置
        /// </summary>
        private readonly bool useRoutingBomConfig;

        /// <summary>
        /// 工单工艺路线逻辑
        /// </summary>
        /// <param name="_releasePlanDatas"></param>
        /// <param name="itemDataOwner"></param>
        public WoRoutingGenerator(IReadOnlyList<ReleasePlanData> _releasePlanDatas, ItemDataOwner itemDataOwner)
        {
            IEnumerable<ReleasePlanDetail> releasePlanDetails = _releasePlanDatas.SelectMany(x => x.Details);

            //产品工艺路线
            var productIds = releasePlanDetails.Select(x => x.ItemId).Distinct().ToList();
            productRoutings = RT.Service.Resolve<RoutingSettingController>().GetProductRoutings(productIds);
            var routingIds = productRoutings.Where(x => x.RoutingId.HasValue).Select(x => x.RoutingId.Value).ToList();

            //资源工艺路线
            var resourceIds = _releasePlanDatas.Select(x => x.WipResourceId).Distinct().ToList();
            resourceRoutings = RT.Service.Resolve<RoutingSettingController>().GetResourceRoutings(resourceIds);
            routingIds.AddRange(resourceRoutings.Where(x => x.RoutingId.HasValue).Select(x => x.RoutingId.Value));

            routingIds = routingIds.Distinct().ToList();

            //工艺路线默认版本
            routingVersions = RT.Service.Resolve<RoutingSettingController>().GetRoutingDefaultVersions(routingIds);
            //工艺路线版本Id列表
            var versionIds = routingVersions.Select(x => x.Id).ToList();

            //制程工艺
            var processTechIds = releasePlanDetails.Where(x => x.ProcessTechId.HasValue)
                .Select(x => x.ProcessTechId.Value).Distinct().ToList();
            processTeches = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechs(processTechIds);

            //拼板工单
            var panelWorkOrderNos = releasePlanDetails.Where(x => !x.PanelWorkOrderNo.IsNullOrEmpty())
                .Select(x => x.PanelWorkOrderNo).Distinct().ToList();
            panelWorkOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(panelWorkOrderNos);

            //拼板工单的工艺路线版本
            var panelWorkOrderVersionIds = panelWorkOrders.Where(x => x.VersionId.HasValue)
                .Select(x => x.VersionId.Value).Distinct().ToList();
            panelWorkOrderVersionIds = panelWorkOrderVersionIds.Except(versionIds).ToList();

            routingVersions.AddRange(RT.Service.Resolve<RoutingController>().GetRoutingVersions(panelWorkOrderVersionIds));
            versionIds.AddRange(panelWorkOrderVersionIds);

            //工艺路线版本的布局
            var layoutIds = routingVersions
                .Where(x => x.LayoutId.HasValue)
                .Select(x => x.LayoutId.Value).Distinct().ToList();
            routingLayouts = RT.Service.Resolve<RoutingController>().GetRoutingLayouts(layoutIds);

            //工单工序BOM生成器
            WorkOrderBomSourceType sourceType = WorkOrderBomSourceType.RoutingProcessBom;
            var workOrderConfig = ConfigService.GetConfig(new WorkOrderProcessBomSourceConfig(), typeof(WorkOrder));
            if (workOrderConfig != null)
            {
                sourceType = workOrderConfig.ProcessBomType;
            }

            // 如果配置工序bom来源工艺路线
            if (sourceType == WorkOrderBomSourceType.RoutingProcessBom)
            {
               var routingProcesses = AppRuntime.Service.Resolve<RoutingController>()
                .GetRoutingProcessList(versionIds);
                var routingProcessIds = routingProcesses.Select(x => x.Id).Distinct().ToList(); 
                

                var routingProcessBomConfigs = RT.Service.Resolve<RoutingController>()
                         .GetRoutingProcessBomConfigs(routingProcessIds);
                woProcessBomGenerator = new BomConfigGenerator(itemDataOwner, routingProcessBomConfigs);
                var itemIds = routingProcessBomConfigs.Select(m => m.ItemId).Distinct().ToList();
                itemDataOwner.GetItemsAndCache(itemIds);
                useRoutingBomConfig = true;
            }
            else
            {
                woProcessBomGenerator = new RoutingBomDetailGenerator(productIds, versionIds, itemDataOwner);

                //如果工单配置项【工单工序BOM配置项】的【工序bom来源】配置为【产品工序BOM】时，则不获取工艺路线的【工序BOM配置】资料
                //配置项对应 WorkOrderProcessBomSourceConfig()
                //配置在工单功能 typeof(WorkOrder)
                useRoutingBomConfig = false;
            }

            //工单工序清单生成器
            WoRoutingProcesssGenerator = new WoRoutingProcessGenerator(versionIds, useRoutingBomConfig);
        }

        /// <summary>
        /// 设置工单的工艺路线
        /// </summary>
        /// <param name="workOrders"></param>
        /// <param name="curReleasePlanDetail"></param>
        /// <param name="workOrder"></param>
        /// <param name="workOrderRoutingLayouts"></param>
        /// <exception cref="ValidationException"></exception>
        public void SetWorkOrderRouting(EntityList<WorkOrder> workOrders, ReleasePlanDetail curReleasePlanDetail,
            WorkOrder workOrder, EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrderRoutingLayouts == null)
            {
                throw new ArgumentNullException(nameof(workOrderRoutingLayouts));
            }

            var processTech = processTeches.FirstOrDefault(x => x.Id == curReleasePlanDetail.ProcessTechId);
            var routingVersion = AutoRoutingVersions(workOrder.Type, workOrder.PlanBeginDate, workOrder.ProductId,
                workOrder.ResourceId, processTech?.ProcessSegmentId);

            workOrder.ProcessSegmentId = processTech?.ProcessSegmentId;
            if (!curReleasePlanDetail.PanelWorkOrderNo.IsNullOrEmpty())
            {
                //找拼板工单
                var panelWorkOrder = workOrders.FirstOrDefault(p => p.No == curReleasePlanDetail.PanelWorkOrderNo);
                if (panelWorkOrder != null)
                {
                    workOrder.PanelWorkOrder = panelWorkOrder;
                    routingVersion = routingVersions.FirstOrDefault(x => x.Id == panelWorkOrder.VersionId);
                }
                else
                {
                    panelWorkOrder = panelWorkOrders.FirstOrDefault(p => p.No == curReleasePlanDetail.PanelWorkOrderNo);
                    if (panelWorkOrder != null)
                    {
                        routingVersion = routingVersions.FirstOrDefault(x => x.Id == panelWorkOrder.VersionId);
                        workOrder.PanelWorkOrder = panelWorkOrder;
                    }
                }
            }

            if (routingVersion != null)
            {
                workOrder.VersionId = routingVersion.Id;
                workOrder.RoutingId = routingVersion.RoutingId;
            }
            else
            {
                throw new ValidationException("工单的工艺路线版本不能为空!".L10N());
            }

            var routingLayout = routingLayouts.FirstOrDefault(x => x.Id == routingVersion.LayoutId) ?? throw new ValidationException("工单的工艺路线版本的工艺路线布局不能为空!".L10N());
            var layout = new WorkOrderRoutingLayout
            {
                //改成批量获取ID <code>layout.GenerateId();</code>

                Layout = routingLayout.Layout
            };
            workOrder.Layout = layout;
            workOrderRoutingLayouts.Add(layout);
        }

        /// <summary>
        /// 设置工单的工艺路线
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="workOrderRoutingLayouts">工单工艺路线</param>
        /// <param name="hasRouting">是否有工艺路线</param>
        /// <exception cref="ValidationException"></exception>
        public void SetWorkOrderRouting(WorkOrder workOrder, EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts,out bool hasRouting)
        {
            var routingVersion = AutoRoutingVersions(workOrder.Type, workOrder.PlanBeginDate, workOrder.ProductId,
                workOrder.ResourceId);

            if (routingVersion != null)
            {
                workOrder.VersionId = routingVersion.Id;
                workOrder.RoutingId = routingVersion.RoutingId;
                hasRouting = true;
            }
            else
            {
                hasRouting = false;
                return;
                // throw new ValidationException("工单未维护产品工艺路线或产线工艺路线！");
            }

            var routingLayout = routingLayouts.FirstOrDefault(x => x.Id == routingVersion.LayoutId) ?? throw new ValidationException("工单的工艺路线版本的工艺路线布局不能为空！".L10N());
            var layout = new WorkOrderRoutingLayout
            {
                //改成批量获取ID <code>layout.GenerateId();</code>

                Layout = routingLayout.Layout
            };
            workOrder.Layout = layout;
            workOrderRoutingLayouts.Add(layout);
        }

        /// <summary>
        /// 获取工艺路线版本（自动匹配优先取产品工艺路线）
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="productId">产品Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <returns>工艺路线版本列表</returns>
        private RoutingVersion AutoRoutingVersions(Core.WorkOrders.WorkOrderType type, DateTime startDate,
            double? productId = 0, double? resourceId = 0, double? processSegmentId = null)
        {
            RoutingVersion routingVersion = null;

            //优先取产品工艺路线
            if (productId.HasValue && productId > 0)
            {
                routingVersion = GetProductRoutingVersion(type, startDate, productId.Value, processSegmentId);
                if (routingVersion != null)
                {
                    return routingVersion;
                }
            }

            if (resourceId.HasValue && resourceId > 0)
            {
                return GetResourceRoutingVersion(type, startDate, resourceId.Value);
            }

            return routingVersion;
        }

        /// <summary>
        /// 获取产品工艺路线版本
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="productId">产品Id</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <returns>工艺路线版本列表</returns>
        private RoutingVersion GetProductRoutingVersion(Core.WorkOrders.WorkOrderType type, DateTime startDate,
            double productId, double? processSegmentId = null)
        {
            //processTechId 为null时也要取
            var productRouting = productRoutings
                .FirstOrDefault(p => p.StartDate <= startDate && p.EndDate >= startDate
                    && p.OrderType == type
                    && p.ProductId == productId
                    && p.ProcessSegmentId == processSegmentId);
            if (productRouting == null)
            {
                return null;
            }

            return routingVersions.FirstOrDefault(x => x.RoutingId == productRouting.RoutingId);
        }

        /// <summary>
        /// 获取资源工艺路线版本
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>工艺路线版本列表</returns>
        private RoutingVersion GetResourceRoutingVersion(Core.WorkOrders.WorkOrderType type, DateTime startDate,
            double resourceId)
        {
            var resourceRouting = resourceRoutings
                .FirstOrDefault(p => p.StartDate <= startDate && p.EndDate >= startDate && p.OrderType == type && p.ResourceId == resourceId);
            if (resourceRouting == null)
            {
                return null;
            }

            return routingVersions.FirstOrDefault(x => x.RoutingId == resourceRouting.RoutingId);
        }

        /// <summary>
        /// 生成工单工艺路线工序清单
        /// </summary>
        /// <param name="workOrder">工单</param> 
        public void GenerateRoutingProcesss(WorkOrder workOrder)
        {
            this.WoRoutingProcesssGenerator.GenerateRoutingProcesss(workOrder, setDisplayProperty: false);
        }

        /// <summary>
        /// 生成工序Bom（取工艺路线工序BOM配置 与 工单BOM 的交集）
        /// </summary>
        /// <param name="workOrder">工单</param>        
        public void GenerateProcessBoms(WorkOrder workOrder)
        {
            this.woProcessBomGenerator.GenerateProcessBoms(workOrder);
        }


        /// <summary>
        /// 工序列表
        /// </summary>
        /// <returns></returns>
        public EntityList<Process> GetProcesses()
        {
            if (WoRoutingProcesssGenerator != null)
            {
                return WoRoutingProcesssGenerator.Processs;
            }
            else
            {
                return new EntityList<Process>();
            }
        }
    }
}
