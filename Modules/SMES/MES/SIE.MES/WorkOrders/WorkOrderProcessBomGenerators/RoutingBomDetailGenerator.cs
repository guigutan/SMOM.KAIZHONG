using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Release;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.Routings.RoutingBoms;
using SIE.MES.WorkOrders.Configs;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders.WorkOrderProcessBomGenerators
{
    /// <summary>
    /// 工单工序BOM生成器（使用产品工序BOM）
    /// </summary>
    public class RoutingBomDetailGenerator : IWoProcessBomGenerator
    {
        /// <summary>
        /// 工序BOM主表
        /// </summary>
        private readonly EntityList<RoutingBom> routingBoms;

        /// <summary>
        /// 工序BOM明细
        /// </summary>
        private readonly EntityList<RoutingBomDetail> routingBomDetails;
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 工步列表
        /// </summary>
        private readonly EntityList<WorkStep> workSteps;

        /// <summary>
        /// 是否参考工序BOM
        /// </summary>
        private readonly bool referenceWoBom;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productIds">产品ID列表</param>
        /// <param name="versionIds">工艺路线版本ID列表</param>
        /// <param name="_itemDataOwner">物料数据拥有者</param>
        public RoutingBomDetailGenerator(List<double> productIds, List<double> versionIds,
            ItemDataOwner _itemDataOwner)
        {
            if (_itemDataOwner == null)
            {
                throw new ArgumentNullException(nameof(_itemDataOwner));
            }

            routingBoms = RT.Service.Resolve<RoutingBomController>().GetRoutingBoms(productIds, versionIds);

            var routingBomIds = routingBoms.Select(x => x.Id).ToList();

            routingBomDetails = RT.Service.Resolve<RoutingBomController>()
              .GetRoutingBomDetails(routingBomIds);

            //获取物料并缓存
            itemDataOwner = _itemDataOwner;
            var itemIds = routingBomDetails.Select(x => x.MaterialId).ToList();

            itemIds.AddRange(routingBomDetails.Where(x => x.MainMaterialId.HasValue)
                .Select(x => x.MainMaterialId.Value));

            itemIds = itemIds.Distinct().ToList();

            itemDataOwner.GetItemsAndCache(itemIds);

            //工步列表
            var workStepIds = routingBomDetails.Where(x => x.WorkStepId.HasValue)
                .Select(x => x.WorkStepId.Value).Distinct().ToList();
            workSteps = RT.Service.Resolve<ProcessController>().GetWorkStepsByIds(workStepIds);

            //工序 BOM 管理 是否考虑工单BOM 配置项
            referenceWoBom = true;
            var referenceWoBomConfigValue = ConfigService.GetConfig(new ReferenceWoBomConfig(), typeof(WorkOrder));
            if (referenceWoBomConfigValue != null)
            {
                referenceWoBom = referenceWoBomConfigValue.ReferenceWoBom;
            }
        }

        /// <summary>
        /// 生成工单工序BOM
        /// </summary>
        /// <param name="workOrder"></param>        
        /// <exception cref="ArgumentNullException"></exception>
        public void GenerateProcessBoms(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            workOrder.ProcessBomList.Clear();

            if (!workOrder.VersionId.HasValue)
            {
                return;
            }

            // 获取当前工艺路线版本的工序bom
            List<RoutingBomDetail> routingBomDetailsCrt = GetRoutingBomDetailForWorkOrder(workOrder);

            foreach (var workOrderRoutingProcess in workOrder.RoutingProcessList)
            {
                if (!workOrderRoutingProcess.ProcessId.HasValue)
                {
                    continue;
                }

                int seq = 1;

                var rtBomDetailsOfRoutingProcess = routingBomDetailsCrt
                    .Where(x => x.RoutingProcessId == workOrderRoutingProcess.RoutingProcessId)
                    .ToList();

                foreach (var bom in rtBomDetailsOfRoutingProcess)
                {
                    if (referenceWoBom && !workOrder.BomList
                        .Any(x => x.ItemId == bom.MaterialId && x.ItemExtProp == bom.ItemExtProp))
                    {
                        //如果配置要参考工单BOM，且工单BOM不包含这个物料，则跳过
                        continue;
                    }

                    var workOrderProcessBom = new WorkOrderProcessBom
                    {
                        UnitId = bom.MaterialUnitId,
                        ItemUnitName = bom.MaterialUnit,
                        SingleQty = bom.Amount,
                        Weight = bom.Amount,
                        Seq = seq++,
                        ItemId = bom.MaterialId,
                        //此时未生成ID
                        RoutingProcess = workOrderRoutingProcess,
                        //此时未生成ID
                        WorkOrder = workOrder,
                        ProcessId = workOrderRoutingProcess.ProcessId,
                        ItemExtProp = bom.ItemExtProp,
                        ItemExtPropName = bom.ItemExtPropName,
                    };

                    if (bom.WorkStepId.HasValue)
                    {
                        var step = workSteps.FirstOrDefault(x => x.Id == bom.WorkStepId.Value);
                        if (step != null)
                        {
                            workOrderProcessBom.WorkStepId = step.Id;
                            workOrderProcessBom.WorkStep = step;
                        }
                    }

                    seq = ProcessAlternative(workOrder, routingBomDetailsCrt, workOrderRoutingProcess, seq, bom, workOrderProcessBom);

                    workOrderProcessBom.ExtValues["ItemId_Display"] = bom.MaterialCode;
                    workOrderProcessBom.ItemName = bom.MaterialName;
                    workOrderProcessBom.ItemSpecificationModel = bom.SpecificationModel;

                    //改成保存前批量获取Id
                    //<code>workOrderProcessBom.GenerateId();</code> 

                    workOrder.ProcessBomList.Add(workOrderProcessBom);
                }
            }
        }

        /// <summary>
        /// 处理替代料
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="routingBomDetailsCrt"></param>
        /// <param name="workOrderRoutingProcess"></param>
        /// <param name="seq"></param>
        /// <param name="bom"></param>
        /// <param name="workOrderProcessBom"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private int ProcessAlternative(WorkOrder workOrder, List<RoutingBomDetail> routingBomDetailsCrt,
            WorkOrderRoutingProcess workOrderRoutingProcess, int seq, RoutingBomDetail bom, WorkOrderProcessBom workOrderProcessBom)
        {
            if (referenceWoBom)
            {
                //找对应非替代料且物料ID相同的工单BOM
                var workOrderBom = workOrder.BomList.FirstOrDefault(x => x.ItemId == bom.MaterialId
                        && x.ItemExtProp == bom.ItemExtProp
                        && !x.IsAlternative);

                if (workOrderBom != null)
                {
                    workOrderProcessBom.AlterGroup = workOrderBom.AlterGroup;
                    workOrderProcessBom.Alter = workOrderBom.Alter;
                    workOrderProcessBom.IsAlternative = false;

                    //工单BOM的替代料
                    var alternativeList = workOrder.BomList
                        .Where(x => x.Alter == workOrderBom.Alter && x.IsAlternative);

                    foreach (var woBom in alternativeList)
                    {
                        var item = itemDataOwner.GetItem(woBom.ItemId);

                        if (item == null)
                        {
                            throw new ValidationException("物料【{0}】找不到".L10nFormat(woBom.ItemId));
                        }

                        var alternativeProcessBom = new WorkOrderProcessBom
                        {
                            UnitId = item.UnitId,
                            ItemUnitName = woBom.UnitName,
                            //用量相同
                            SingleQty = bom.Amount,
                            Weight = bom.Amount,
                            Seq = seq++,
                            ItemId = woBom.ItemId,
                            //此时未生成ID
                            RoutingProcess = workOrderRoutingProcess,
                            //此时未生成ID
                            WorkOrder = workOrder,
                            ProcessId = workOrderRoutingProcess.ProcessId,
                            ItemExtProp = woBom.ItemExtProp,
                            ItemExtPropName = woBom.ItemExtPropName,
                            IsAlternative = true,
                            AlterGroup = woBom.AlterGroup,
                            Alter = woBom.Alter,
                        };

                        workOrderProcessBom.ExtValues["ItemId_Display"] = item.Code;
                        workOrderProcessBom.ItemName = item.Code;
                        workOrderProcessBom.ItemSpecificationModel = item.SpecificationModel;

                        workOrder.ProcessBomList.Add(alternativeProcessBom);
                    }
                }
            }
            else
            {
                if (!bom.MainMaterialId.HasValue || bom.MainMaterialId == bom.MaterialId)
                {
                    //主料且有替代料
                    if (routingBomDetailsCrt.Any(x => x.MainMaterialId == bom.MaterialId))
                    {
                        var item = itemDataOwner.GetItem(bom.MaterialId);

                        if (item == null)
                        {
                            throw new ValidationException("物料【{0}】找不到".L10nFormat(bom.MaterialId));
                        }

                        workOrderProcessBom.Alter = item.Code;
                        workOrderProcessBom.IsAlternative = false;
                    }
                }
                else
                {
                    //替代组为主料编码
                    var mainItem = itemDataOwner.GetItem(bom.MainMaterialId.Value);

                    if (mainItem == null)
                    {
                        throw new ValidationException("物料【{0}】找不到".L10nFormat(bom.MainMaterialId.Value));
                    }

                    workOrderProcessBom.Alter = mainItem.Code;
                    workOrderProcessBom.IsAlternative = true;
                }
            }

            return seq;
        }

        private List<RoutingBomDetail> GetRoutingBomDetailForWorkOrder(WorkOrder workOrder)
        {
            var fitRoutingBomIds = routingBoms.Where(x => x.ProductId == workOrder.ProductId
                && x.RoutingVersionId == workOrder.VersionId.Value
                && x.ProcessSegmentId == workOrder.ProcessSegmentId && x.ProjectMaintainId == workOrder.ProjectMaintainId)
                .Select(x => x.Id)
                .Distinct()
                .ToList();

            List<RoutingBomDetail> routingBomDetailsCrt;
            if (referenceWoBom)
            {
                //参考工单BOM，则替代料来源为工单BOM,不取工序BOM管理的替代料
                routingBomDetailsCrt = routingBomDetails
                    .Where(x => fitRoutingBomIds.Contains(x.RoutingBomId) && !x.MainMaterialId.HasValue)
                    .ToList();
            }
            else
            {
                routingBomDetailsCrt = routingBomDetails
                    .Where(x => fitRoutingBomIds.Contains(x.RoutingBomId))
                    .ToList();
            }

            return routingBomDetailsCrt;
        }
    }
}
