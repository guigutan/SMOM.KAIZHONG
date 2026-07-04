using Newtonsoft.Json;
using SIE.Api;
using SIE.Common;
using SIE.Core.ApiModels;
using SIE.Core.Logs;
using SIE.Core.ProjectMaintains;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.Models;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.LoadItems;
using SIE.MES.WIP.Products;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders.Interfaces
{
    /// <summary>
    /// 工单查询接口实现
    /// </summary>
    public class WorkOrderQueryController : DomainController, IWorkOrderQuery
    {

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="queryInfo">工单查询信息</param>
        /// <returns>分页工单信息</returns>
        [ApiService("获取工单列表")]
        [return: ApiReturn("分页工单信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWorkOrdertInfos([ApiParameter("工单查询信息")] WorkOrderQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            EntityList<WorkOrder> workOrders;
            if (queryInfo.StateList != null && queryInfo.StateList.Count > 0)
            { workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(queryInfo.ResourceId, queryInfo.StateList, queryInfo.Keyword, pagingInfo); }
            else
            { workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(pagingInfo, queryInfo.Keyword, queryInfo.ResourceId); }
            var infos = new List<BaseDataInfo>();
            workOrders.ForEach(workOrder =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workOrder.Id,
                    Code = workOrder.No,
                    Name = workOrder.No,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = workOrders.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 根据车间和产线获取工单列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isFilterPause">是否过滤暂停工单</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<Core.WorkOrders.WorkOrder> GetWorkOrderList(double? workShopId, double? resourceId, PagingInfo pagingInfo, string keyword, bool isFilterPause = false)
        {
            //记录接口日志
            SaveGetWorkOrderListLog(workShopId, resourceId, pagingInfo, keyword);
            var workOrders = new EntityList<Core.WorkOrders.WorkOrder>();
            var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(workShopId, resourceId, pagingInfo, keyword, isFilterPause);
            workOrders.AddRange(wos);
            if (pagingInfo != null)
            {
                workOrders.SetTotalCount(wos.TotalCount);
            }
            return workOrders;
        }

        /// <summary>
        /// 备料单弹框查询工单
        /// </summary>
        /// <param name="no">工单号</param>
        /// <param name="factoryId">工厂</param>
        /// <param name="workshopId">车间</param>
        /// <param name="resourceId">资源</param>
        /// <param name="pcode">产品编码</param>
        /// <param name="pname">产品名称</param>
        /// <param name="exceptClose">排除关闭状态</param>
        /// <param name="exceptCancel">排除取消状态</param>
        /// <param name="planBeginStart">计划开始时间</param>
        /// <param name="planBeginEnd">计划开始时间</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual WorkOrderLesInfoWithCount GetWorkOrderList(string no, double? factoryId, double? workshopId, double? resourceId, string pcode, string pname, bool exceptClose = false, bool exceptCancel = false, DateTime? planBeginStart = null, DateTime? planBeginEnd = null, PagingInfo pagingInfo = null)
        {
            WorkOrderLesInfoWithCount info = new WorkOrderLesInfoWithCount();
            var query = Query<WorkOrder>().As("wo")
                .Where(wo => wo.No.Contains(no))
                .WhereIf(exceptClose, wo => wo.State != WorkOrderState.Close)
                .WhereIf(exceptCancel, wo => wo.State != WorkOrderState.CancelRelease)
                .WhereIf(planBeginStart.HasValue, p => p.PlanBeginDate >= planBeginStart)
                .WhereIf(planBeginEnd.HasValue, p => p.PlanBeginDate <= planBeginEnd)
                .OrderByDescending(wo => wo.CreateDate)
                .WhereIf(factoryId != null, wo => wo.FactoryId == factoryId)
                .WhereIf(workshopId != null, wo => wo.WorkShopId == workshopId)
                .WhereIf(resourceId != null, wo => wo.ResourceId == resourceId)
                .LeftJoin<Item>("i", (wo, i) => wo.ProductId == i.Id)
                .WhereIf<Item>(pcode.IsNotEmpty(), (wo, i) => i.Code.Contains(pcode))
                .WhereIf<Item>(pname.IsNotEmpty(), (wo, i) => i.Name.Contains(pname));

            var woList = query
                .LeftJoin<Enterprise>("f", (wo, f) => wo.FactoryId == f.Id)
                .LeftJoin<Enterprise>("w", (wo, w) => wo.WorkShopId == w.Id)
                .LeftJoin<WipResource>("wip", (wo, wip) => wo.ResourceId == wip.Id)
                .LeftJoin<ProjectMaintain>("pro", (wo, pro) => wo.ProjectMaintainId == pro.Id)
                .Select<Item, Enterprise, Enterprise, WipResource, ProjectMaintain>((wo, i, f, w, wip, pro) => new
                {
                    WorkOrderId = wo.Id,
                    WorkOrderNo = wo.No,
                    ProductId = i.Id,
                    ProductCode = i.Code,
                    ProductName = i.Name,
                    FactoryId = f.Id,
                    FactoryCode = f.Name,
                    WorkShopId = w.Id,
                    WorkShopCode = w.Code,
                    ResourceId = wip.Id,
                    ResourceCode = wip.Name,
                    PlanQty = wo.PlanQty,
                    FinishQty = wo.FinishQty,
                    Type = wo.Type,
                    State = wo.State,
                    PlanBeginDate = wo.PlanBeginDate,
                    PlanEndDate = wo.PlanEndDate,
                    ActuStartDate = wo.ActuStartDate,
                    ActuFinishDate = wo.ActuFinishDate,
                    SaleOrderNo = wo.SaleOrderNo,
                    CustomerOrderNo = wo.CustomerOrderNo,
                    ProjectMaintainId = wo.ProjectMaintainId,
                    ProjectMaintainCode = pro.Code,
                }).ToList<WorkOrderLesInfo>(pagingInfo).ToList();

            var totalCount = query.Count();
            info.WorkOrderInfos.AddRange(woList);
            info.TotalCount = totalCount;
            return info;
        }

        /// <summary>
        /// LES获取工单信息
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <returns></returns>
        public virtual Dictionary<double, WorkOrderInfo> GetWorkOrderList(List<double> woIds)
        {
            Dictionary<double, WorkOrderInfo> infoDic = new Dictionary<double, WorkOrderInfo>();
            List<WorkOrderInfo> list = new List<WorkOrderInfo>();
            woIds.SplitDataExecute(tempIds =>
            {
                var query = Query<WorkOrder>().As("wo")
                .Where(wo => tempIds.Contains(wo.Id))
                .OrderByDescending(wo => wo.CreateDate)
                .LeftJoin<Item>("i", (wo, i) => wo.ProductId == i.Id)
                .LeftJoin<Enterprise>("f", (wo, f) => wo.FactoryId == f.Id)
                .LeftJoin<Enterprise>("w", (wo, w) => wo.WorkShopId == w.Id)
                .LeftJoin<WipResource>("wip", (wo, wip) => wo.ResourceId == wip.Id)
                .LeftJoin<ProjectMaintain>("pro", (wo, pro) => wo.ProjectMaintainId == pro.Id)
                .Select<Item, Enterprise, Enterprise, WipResource, ProjectMaintain>((wo, i, f, w, wip, pro) => new
                {
                    WorkOrderId = wo.Id,
                    WorkOrderNo = wo.No,
                    ProductId = i.Id,
                    ProductCode = i.Code,
                    ProductName = i.Name,
                    FactoryId = f.Id,
                    FactoryCode = f.Code,
                    WorkShopId = w.Id,
                    WorkShopCode = w.Code,
                    ResourceId = wip.Id,
                    ResourceCode = wip.Code,
                    PlanQty = wo.PlanQty,
                    State = wo.State,
                    PlanBeginDate = wo.PlanBeginDate,
                    PlanEndDate = wo.PlanEndDate,
                    CreateTime = wo.CreateDate,
                    ProjectMaintainId = pro.Id,
                    ProjectMaintainCode = pro.Code,
                }).ToList<WorkOrderInfo>().ToList();
                list.AddRange(query);
            });
            foreach (var i in list)
            {
                if (!infoDic.ContainsKey(i.WorkOrderId) && (i.ResourceId != 0 || i.WorkShopId != 0))
                {
                    infoDic.Add(i.WorkOrderId, i);
                }
            }
            return infoDic;

        }

        /// <summary>
        /// 获取工单简要信息
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public virtual List<WorkOrderSimpleInfo> GetWorkOrderSimpleInfos(string no)
        {
            List<WorkOrderSimpleInfo> workOrderSimpleInfos = new List<WorkOrderSimpleInfo>();
            var query = Query<WorkOrder>().As("wo")
                .LeftJoin<ProjectMaintain>((wo, pm) => wo.ProjectMaintainId == pm.Id)
                .WhereIf(no.IsNotEmpty(), wo => wo.No.Contains(no))
                .Where(wo => wo.State != WorkOrderState.Close)
                .Select<ProjectMaintain>((wo, pm) => new
                {
                    Id = wo.Id,
                    WorkShopId = wo.WorkShopId,
                    ResourceId = wo.ResourceId,
                    No = wo.No,
                    ProjectId = wo.ProjectMaintainId,
                    ProjectNo = pm.Code,
                }).OrderByDescending(wo => wo.CreateDate).ToList<WorkOrderSimpleInfo>();
            workOrderSimpleInfos.AddRange(query);
            return workOrderSimpleInfos;
        }

        /// <summary>
        /// 保存根据车间和产线获取工单列表接口日志
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void SaveGetWorkOrderListLog(double? workShopId, double? resourceId, PagingInfo pagingInfo, string keyword)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var strPageInfo = JsonConvert.SerializeObject(pagingInfo);
                var inputValue = "车间Id:{0};产线Id:{1};分页:{2};关键字:{3}".L10nFormat(workShopId, resourceId, strPageInfo, keyword);
                var log = new InterfaceLog()
                {
                    Name = "IWorkOrderQuery",
                    Method = "GetWorkOrderList",
                    ControllerName = "WorkOrderQueryController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取工单资源、车间信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>工单信息</returns>
        public virtual WorkOrderInfo GetWorkOrderResource(double workOrderId)
        {
            SaveGetWorkOrderResourceLog(workOrderId);

            using (DataAuth.DataAuths.LoadAll())
            {
                var workOrder = RF.GetById<WorkOrder>(workOrderId);
                if (workOrder == null)
                {
                    throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
                }
                return new WorkOrderInfo()
                {
                    WorkOrderId = workOrderId,
                    WorkShopId = workOrder.WorkShopId ?? 0,
                    ResourceId = workOrder.ResourceId ?? 0,
                    ProcessSegmentId = workOrder.ProcessSegmentId ?? 0
                };
            }
        }

        /// <summary>
        /// 保存获取工单资源、车间信息接口日志
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        private void SaveGetWorkOrderResourceLog(double workOrderId)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "工单Id:{0};".L10nFormat(workOrderId);
                var log = new InterfaceLog()
                {
                    Name = "IWorkOrderQuery",
                    Method = "GetWorkOrderResource",
                    ControllerName = "WorkOrderQueryController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取工单工厂Id
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual double? GetWorkOrderFactoryId(double workOrderId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId);
            if (wo == null)
            {
                return null;
            }
            else
            {
                return wo.FactoryId;
            }
        }

        /// <summary>
        /// 获取工单工艺路线工段字典
        /// </summary>
        /// <param name="workIds"></param>
        /// <returns></returns>
        public virtual Dictionary<double, List<double>> GetDicWoProcessSegment(List<double> workIds)
        {
            var woList = workIds.SplitContains(tempIds =>
            {
                return Query<WorkOrderRoutingProcess>().Where(m => tempIds.Contains(m.WorkOrderId)).ToList();
            });
            return woList.Any() ? woList.GroupBy(w => w.WorkOrderId).ToDictionary(p => p.Key, p => p.Where(m => m.SegmentId.HasValue)
            .Select(m => m.SegmentId.Value).Distinct().ToList()) : new Dictionary<double, List<double>>();
        }

        /// <summary>
        /// 获取工单信息提供给LES备料计算使用
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceIds">工厂Id列表</param>
        /// <param name="workOrderIds">工单ID列表</param>
        /// <returns>工单信息列表</returns>
#if DEBUG
        [ApiService]
#endif
        public virtual List<WoInfoForLes> GetWoInfoForLes(double? workShopId, List<double?> resourceIds, List<double> workOrderIds)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                List<WoInfoForLes> woInfoForLesList = new List<WoInfoForLes>();

                EntityList<WorkOrder> workOrders;
                if (workOrderIds != null && workOrderIds.Any())
                {
                    workOrders = GetWorkOrders(workShopId, resourceIds, workOrderIds);
                }
                else
                {
                    workOrders = GetWorkOrders(workShopId, resourceIds);
                }

                var woIds = workOrders.Select(x => x.Id).Distinct().ToList();

                var woBoms = woIds.SplitContains(tempIds =>
                {
                    return Query<WorkOrderBom>()
                        .Where(x => tempIds.Contains(x.WorkOrderId))
                        .ToList(null, new EagerLoadOptions().LoadWith(WorkOrderBom.ItemProperty));
                });

                var woBomsDictionary = woBoms.GroupBy(x => x.WorkOrderId).ToDictionary(x => x.Key, v => v.ToList());

                var woIdNullables = woIds.Select(x => (double?)x).ToList();

                var itemLabelWorkOrders = woIdNullables.SplitContains(tempIds =>
                {
                    return Query<SIE.Packages.ItemLabels.ItemLabelWorkOrder>()
                        .Where(x => x.Qty > 0)
                        .Where(x => tempIds.Contains(x.WorkOrderId))
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });

                var itemLabelsDictionary = itemLabelWorkOrders
                    .GroupBy(x => x.WorkOrderId).ToDictionary(x => x.Key,
                        v => v.GroupBy(y => y.ItemId).ToDictionary(z => z.Key, zv => zv.ToList()));

                var loadItems = woIdNullables.SplitContains(tempIds =>
                {
                    return Query<LoadItem>()
                        .Where(x => x.Qty > 0)
                        .Where(x => tempIds.Contains(x.WorkOrderId))
                        .ToList();
                });

                var loadItemsDictionary = loadItems
                    .GroupBy(x => x.WorkOrderId.Value).ToDictionary(x => x.Key,
                        v => v.GroupBy(y => y.ItemId).ToDictionary(z => z.Key, zv => zv.ToList()));

                foreach (var workOrder in workOrders)
                {
                    var woInfo = new WoInfoForLes()
                    {
                        FactoryId = workOrder.FactoryId,
                        Id = workOrder.Id,
                        WorkOrderNo = workOrder.No,
                        PlanBeginDate = workOrder.PlanBeginDate,
                        ProductId = workOrder.ProductId,
                        ProductCode = workOrder.Product.Code,
                        ProductName = workOrder.Product.Name,
                        WorkShopId = workOrder.WorkShopId,
                        ResourceId = workOrder.ResourceId
                    };

                    if (woBomsDictionary.ContainsKey(workOrder.Id))
                    {
                        var boms = woBomsDictionary[workOrder.Id];

                        foreach (var bom in boms)
                        {
                            WoBomInfoForLes woBomInfoForLes = new WoBomInfoForLes
                            {
                                Id = bom.Id,
                                ItemId = bom.ItemId,
                                ItemCode = bom.Item.Code,
                                ItemName = bom.Item.Name,
                                ConsumeMode = (int)bom.Item.ConsumeMode,
                                RequestQty = bom.RequireQty,
                                SingleQty = bom.SingleQty,
                                ItemExtProp = bom.ItemExtProp,
                                ItemExtPropName = bom.ItemExtPropName,
                            };

                            ComputeOnhandQty(itemLabelsDictionary, loadItemsDictionary, workOrder, bom, woBomInfoForLes);

                            woInfo.WoBomInfos.Add(woBomInfoForLes);
                        }
                    }

                    woInfoForLesList.Add(woInfo);
                }

                return woInfoForLesList;
            }
        }

        /// <summary>
        /// 计算产线库存现有量
        /// </summary>
        /// <param name="itemLabelsDictionary">物料标签</param>
        /// <param name="loadItemsDictionary">上料记录</param>
        /// <param name="workOrder">工单</param>
        /// <param name="bom">工单BOM</param>
        /// <param name="woBomInfoForLes">工单BOM信息</param>
        private static void ComputeOnhandQty(Dictionary<double, Dictionary<double, List<ItemLabelWorkOrder>>> itemLabelsDictionary,
            Dictionary<double, Dictionary<double, List<LoadItem>>> loadItemsDictionary,
            WorkOrder workOrder, WorkOrderBom bom, WoBomInfoForLes woBomInfoForLes)
        {
            decimal itemLabelRemainQty = 0;
            decimal loadItemRemainQty = 0;

            if (itemLabelsDictionary.ContainsKey(workOrder.Id)
                && itemLabelsDictionary[workOrder.Id].ContainsKey(bom.ItemId))
            {
                var itemLabelsCurrent = itemLabelsDictionary[workOrder.Id][bom.ItemId];

                itemLabelRemainQty = itemLabelsCurrent
                    .Where(x => x.ItemExtProp == bom.ItemExtProp)
                    .Sum(x => x.Qty);
            }

            if (loadItemsDictionary.ContainsKey(workOrder.Id)
                && loadItemsDictionary[workOrder.Id].ContainsKey(bom.ItemId))
            {
                var loadItemsCurrent = loadItemsDictionary[workOrder.Id][bom.ItemId];

                loadItemRemainQty = loadItemsCurrent
                    .Where(x => x.ItemExtProp == bom.ItemExtProp)
                    .Sum(x => x.Qty);
            }

            woBomInfoForLes.OnhandQty = itemLabelRemainQty + loadItemRemainQty;
        }

        private EntityList<WorkOrder> GetWorkOrders(double? workShopId, List<double?> resourceIds)
        {
            EntityList<WorkOrder> workOrders;
            var query = Query<WorkOrder>()
                                .Where(x => (x.State == Core.WorkOrders.WorkOrderState.Producing
                                    || x.State == Core.WorkOrders.WorkOrderState.Release) && x.IsPause == YesNo.No);

            if (workShopId.HasValue)
            {
                query.Where(x => x.WorkShopId == workShopId.Value);
            }

            if (resourceIds != null && resourceIds.Any())
            {
                query.Where(x => resourceIds.Contains(x.ResourceId));
            }

            workOrders = query
               .ToList(null, new EagerLoadOptions().LoadWith(WorkOrder.ProductProperty));
            return workOrders;
        }

        private EntityList<WorkOrder> GetWorkOrders(double? workShopId, List<double?> resourceIds, List<double> workOrderIds)
        {
            return workOrderIds.SplitContains(tempIds =>
            {
                var query = Query<WorkOrder>()
                    .Where(x => (x.State == Core.WorkOrders.WorkOrderState.Producing
                        || x.State == Core.WorkOrders.WorkOrderState.Release) && x.IsPause == YesNo.No)
                    .Where(x => tempIds.Contains(x.Id));

                if (workShopId.HasValue)
                {
                    query.Where(x => x.WorkShopId == workShopId.Value);
                }

                if (resourceIds != null && resourceIds.Any())
                {
                    query.Where(x => resourceIds.Contains(x.ResourceId));
                }

                return query.ToList(null,
                    new EagerLoadOptions().LoadWith(WorkOrder.ProductProperty));
            });
        }

        /// <summary>
        /// 根据产线Ids获取在制工单Ids
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<WorkOrderInfo> GetWipWorkOrderIds(List<double?> resourceIds)
        {
            List<WorkOrderInfo> workOrderInfos = new List<WorkOrderInfo>();
            var woOrderList = resourceIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>().Where(p => p.State == WorkOrderState.Producing)
                .Exists<WipResourceWorkOrder>((x, y) => y.Where(p => p.WorkOrderId == x.Id && tempIds.Contains(p.ResourceId)))
                .ToList();
            });
            woOrderList.ForEach(wo =>
            {
                WorkOrderInfo workOrderInfo = new WorkOrderInfo
                {
                    WorkOrderId = wo.Id,
                    ResourceId = wo.ResourceId ?? 0,
                    ProductId = wo.ProductId,
                };
                workOrderInfos.Add(workOrderInfo);
            });
            return workOrderInfos;
        }

        /// <summary>
        /// 根据工单Ids获取工序bom
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<WoProcessBomInfo> GetWoProcessBomInfos(List<double> woOrderIds)
        {
            var woProcessBomList = woOrderIds.SplitContains(tempIds =>
            {
                return Query<WorkOrderProcessBom>().Where(p => tempIds.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            List<WoProcessBomInfo> woProcessBomInfos = new List<WoProcessBomInfo>();
            woProcessBomList.ForEach(bom =>
            {
                WoProcessBomInfo woProcessBomInfo = new WoProcessBomInfo
                {
                    WoOrderId = bom.WorkOrderId,
                    ItemId = bom.ItemId,
                    ItemName = bom.ItemName,
                    ItemExtPro = bom.ItemExtProp,
                    SingleQty = bom.SingleQty,
                };
                woProcessBomInfos.Add(woProcessBomInfo);
            });
            return woProcessBomInfos;
        }

        /// <summary>
        /// 根据工单Ids获取工单耗用单
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        public virtual List<WoOrderCostInfo> GetWoOrderCostInfos(List<double> woOrderIds)
        {
            var woCostList = woOrderIds.SplitContains(tempIds =>
            {
                return Query<WoCostItem>().Where(p => tempIds.Contains(p.WorkOrderId) && p.State == LoadItems.Enum.WoCostItemState.Submitted).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            List<WoOrderCostInfo> woOrderCostInfos = new List<WoOrderCostInfo>();
            woCostList.ForEach(woCost =>
            {
                WoOrderCostInfo woOrderCostInfo = new WoOrderCostInfo
                {
                    WoOrderId = woCost.WorkOrderId,
                    ItemId = woCost.ItemId,
                    ItemExtPro = woCost.ItemExtProp,
                    Qty = woCost.Qty,
                };
                woOrderCostInfos.Add(woOrderCostInfo);
            });
            return woOrderCostInfos;
        }

        /// <summary>
        /// 根据工单Ids获取单体关键件
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<WipProductKeyItem> GetSingleWipProductKeyItems(List<double> woOrderIds)
        {
            List<WipProductKeyItem> wipProductKeyItems = new List<WipProductKeyItem>();
            if (woOrderIds.Any())
            {
                woOrderIds.SplitDataExecute(tempIds =>
                {
                    var wipKeyList =
                    Query<WipProductProcessKeyItem>().Where(p => !p.IsUnbound).
                    Join<WipProductProcess>((x, y) => x.ProcessId == y.Id).
                    Join<WipProductProcess, WipProductVersion>((x, y) => x.VersionId == y.Id && tempIds.Contains(y.WorkOrderId)).
                    GroupBy<WipProductProcess, WipProductVersion>((k, p, v) => new
                    {
                        WoOrderId = v.WorkOrderId,
                        ItemId = k.ItemId,
                        ItemExtPro = k.ItemExtProp,
                    }).
                    Select<WipProductProcess, WipProductVersion>((k, p, v) => new
                    {
                        WoOrderId = v.WorkOrderId,
                        ItemId = k.ItemId,
                        ItemExtProp = k.ItemExtProp,
                        Qty = k.Qty.SUM(),
                    }).ToList<WipProductKeyItem>();
                    wipProductKeyItems.AddRange(wipKeyList);
                });
            }
            return wipProductKeyItems;
        }

        /// <summary>
        /// 根据工单Ids获取批次关键件
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        public virtual List<WipProductKeyItem> GetBatchWipProductKeyItems(List<double> woOrderIds)
        {
            List<WipProductKeyItem> wipProductKeyItems = new List<WipProductKeyItem>();
            if (woOrderIds.Any())
            {
                woOrderIds.SplitDataExecute(tempIds =>
                {
                    var wipKeyList =
                    Query<BatchWipProductProcessKeyItem>().
                    Join<BatchWipProductProcessDetail>((x, y) => x.DetailId == y.Id).
                    Join<BatchWipProductProcessDetail, BatchWipProductProcess>((x, y) => x.ProductProcessId == y.Id).
                    Join<BatchWipProductProcess, BatchWipProductVersion>((x, y) => x.VersionId == y.Id && tempIds.Contains(y.WorkOrderId)).
                    GroupBy<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        WoOrderId = v.WorkOrderId,
                        ItemId = k.ItemId,
                        ItemExtPro = k.ItemExtProp,
                    }).
                    Select<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        WoOrderId = v.WorkOrderId,
                        ItemId = k.ItemId,
                        ItemExtProp = k.ItemExtProp,
                        Qty = k.Qty.SUM(),
                    }).ToList<WipProductKeyItem>();
                    wipProductKeyItems.AddRange(wipKeyList);
                });
            }
            return wipProductKeyItems;
        }

        /// <summary>
        /// 根据工单号获取在制数量(单体+批次条码数)
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual IReadOnlyList<WoOrderTaskInfo> GetProductBarcodeCount(IReadOnlyList<string> planTaskIds)
        {
            if (!planTaskIds.Any())
            {
                return new List<WoOrderTaskInfo>();
            }
            var taskUnionList = planTaskIds.SplitContains(tempIds =>
            {
                return Query<TaskUnion>().Where(p => tempIds.Contains(p.PlanTaskId)).ToList();
            });
            var taskUnionIds = taskUnionList.Select(p => p.Id).ToList();
            // 计划任务明细
            var taskUnionDetailList = taskUnionIds.SplitContains(tempIds =>
            {
                return Query<TaskUnionDetail>().Where(p => tempIds.Contains(p.TaskUnionId)).ToList();
            });
            var woOrderIds = taskUnionDetailList.Select(p => p.WorkOrderId).ToList();
            // 工单
            var woOrderList = woOrderIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            // 单体条码
            var version = woOrderIds.SplitContains(tempIds =>
            {
                return Query<WipProductVersion>().Where(p => tempIds.Contains(p.WorkOrderId) && !p.IsFinish).ToList();
            });
            // 批次条码
            var batchVersion = woOrderIds.SplitContains(tempIds =>
            {
                return Query<BatchWipProductVersion>().Where(p => tempIds.Contains(p.WorkOrderId) && !p.IsFinish).ToList();
            });
            var woTaskInfoList = new List<WoOrderTaskInfo>();
            planTaskIds.ForEach(planId =>
            {
                var taskUnion = taskUnionList.First(p => p.PlanTaskId == planId);
                WoOrderTaskInfo woOrderTaskInfo = new WoOrderTaskInfo();
                woOrderTaskInfo.PlanTaskId = planId;
                woOrderTaskInfo.TaskDetailList = new List<TaskDetailInfo>();
                var detailList = taskUnionDetailList.Where(p => p.TaskUnionId == taskUnion.Id).ToList();

                foreach (var detail in detailList)
                {
                    WorkOrder workOrder = woOrderList.First(p => p.Id == detail.WorkOrderId);
                    var versionCount = version.Count(p => p.WorkOrderId == workOrder.Id);
                    var batchCount = batchVersion.Count(p => p.WorkOrderId == workOrder.Id);
                    TaskDetailInfo taskDetailInfo = new TaskDetailInfo
                    {
                        TaskDetailId = detail.DetailId,
                        WoId = detail.WorkOrderId,
                        WoNo = workOrder.No,
                        ProductionCount = versionCount + batchCount,
                        FinishedCount = workOrder.FinishQty,
                    };
                    woOrderTaskInfo.TaskDetailList.Add(taskDetailInfo);
                }
                woTaskInfoList.Add(woOrderTaskInfo);
            });

            return woTaskInfoList;
        }

        /// <summary>
        /// APS触发完工
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        public virtual IReadOnlyList<WoOrderTaskInfo> APSFinishWorkOrder(IReadOnlyList<string> planTaskIds)
        {
            if (!planTaskIds.Any())
            {
                return new List<WoOrderTaskInfo>();
            }
            var woTaskInfoList = GetProductBarcodeCount(planTaskIds);
            var saveWorkOrderIds = new List<double>();
            woTaskInfoList.ForEach(wo =>
            {
                var detailList = wo.TaskDetailList;
                foreach (var detail in detailList)
                {
                    if (detail.ProductionCount > 0)
                    {
                        detail.Success = false;
                        detail.Message = "存在在制数量时，不允许更新为已完工".L10N();
                    }
                    else
                    {
                        detail.Success = true;
                        detail.State = "完工".L10N();
                        detail.Message = "工单已完工".L10N();
                        saveWorkOrderIds.Add(detail.WoId);
                    }
                }
            });
            if (saveWorkOrderIds.Any())
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    saveWorkOrderIds.SplitDataExecute(tempIds =>
                    {
                        DB.Update<WorkOrder>().Set(p => p.State, WorkOrderState.Finish).Where(p => tempIds.Contains(p.Id)).Execute();
                    });
                    tran.Complete();
                }
            }
            return woTaskInfoList;
        }

        /// <summary>
        /// APS触发取消完工
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        public virtual IReadOnlyList<WoOrderTaskInfo> APSCancelFinishWoOrder(IReadOnlyList<string> planTaskIds)
        {
            if (!planTaskIds.Any())
            {
                return new List<WoOrderTaskInfo>();
            }
            var taskUnionList = planTaskIds.SplitContains(tempIds =>
            {
                return Query<TaskUnion>().Where(p => tempIds.Contains(p.PlanTaskId)).ToList();
            });
            var taskUnionIds = taskUnionList.Select(p => p.Id).ToList();
            // 计划任务明细
            var taskUnionDetailList = taskUnionIds.SplitContains(tempIds =>
            {
                return Query<TaskUnionDetail>().Where(p => tempIds.Contains(p.TaskUnionId)).ToList();
            });
            var woOrderIds = taskUnionDetailList.Select(p => p.WorkOrderId).ToList();
            // 工单
            var woOrderList = woOrderIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            var saveWorkOrderList = new EntityList<WorkOrder>();
            var woTaskInfoList = new List<WoOrderTaskInfo>();
            planTaskIds.ForEach(planId =>
            {
                var taskUnion = taskUnionList.First(p => p.PlanTaskId == planId);
                WoOrderTaskInfo woOrderTaskInfo = new WoOrderTaskInfo();
                woOrderTaskInfo.PlanTaskId = planId;
                var detailList = taskUnionDetailList.Where(p => p.TaskUnionId == taskUnion.Id).ToList();
                foreach (var detail in detailList)
                {
                    WorkOrder workOrder = woOrderList.First(p => p.Id == detail.WorkOrderId);
                    TaskDetailInfo taskDetailInfo = new TaskDetailInfo
                    {
                        TaskDetailId = detail.DetailId,
                        WoId = detail.WorkOrderId,
                        WoNo = workOrder.No,
                        Success = true,
                    };
                    if (workOrder.FinishQty > 0)
                    {
                        workOrder.State = WorkOrderState.Producing;
                        taskDetailInfo.State = WorkOrderState.Producing.ToLabel();
                        taskDetailInfo.Message = "工单状态变更为生产中".L10N();
                    }
                    else
                    {
                        workOrder.State = WorkOrderState.Release;
                        taskDetailInfo.State = WorkOrderState.Release.ToLabel();
                        taskDetailInfo.Message = "工单状态变更为发放".L10N();
                    }
                    saveWorkOrderList.Add(workOrder);
                    woOrderTaskInfo.TaskDetailList.Add(taskDetailInfo);
                }
                woTaskInfoList.Add(woOrderTaskInfo);
            });
            if (saveWorkOrderList.Any())
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(saveWorkOrderList);
                    tran.Complete();
                }
            }
            return woTaskInfoList;
        }

        /// <summary>
        /// 获取产线下工单bom(含节拍)
        /// </summary>
        /// <param name="resourceIds">工单产线Ids</param>
        /// <param name="itemIds">工单Bom物料Ids</param>
        public virtual List<WoBomPushPreInfo> GetWoBomPushPreInfos(List<double> resourceIds, List<double> itemIds)
        {
            List<WoBomPushPreInfo> woBomPushPreInfos = new List<WoBomPushPreInfo>();

            resourceIds.SplitDataExecute(tempRIds =>
            {
                itemIds.SplitDataExecute(tempIds =>
                {
                    var list = Query<WorkOrderBom>()
                    .Where(wb => tempIds.Contains(wb.ItemId))
                    .LeftJoin<WorkOrder>((wb, w) => wb.WorkOrderId == w.Id)
                    .Where<WorkOrder>((wb, w) => w.ResourceId != null && tempRIds.Contains((double)w.ResourceId)
                    && (w.State == WorkOrderState.Release || w.State == WorkOrderState.Producing)
                    && w.IsPause == YesNo.No)
                    .LeftJoin<Item>((wb, i) => wb.ItemId == i.Id && tempIds.Contains(i.Id))
                    .Where<Item>((wb, i) => i.ConsumeMode == ConsumeMode.Push)
                    .LeftJoin<Item, ProductModel>((i, pm) => i.ModelId == pm.Id)
                    .Select<WorkOrder, Item, ProductModel>((wb, w, i, pm) => new
                    {
                        Id = wb.Id,
                        WorkOrderId = w.Id,
                        WorkOrderNo = w.No,
                        ResourceId = w.ResourceId,
                        WorkShopId = w.WorkShopId,
                        FactoryId = w.FactoryId,
                        PlanBeginDate = w.PlanBeginDate,
                        ItemId = wb.ItemId,
                        ItemCode = i.Code,
                        ItemExtProp = wb.ItemExtProp,
                        ItemExtPropName = wb.ItemExtPropName,
                        BomNeedQty = wb.RequireQty,
                        SingleQty = wb.SingleQty,
                        LineNo = wb.LineNo,
                        ModelId = pm.Id,
                        ProductModelMeter = pm.WorkingHours,
                    }).ToList<WoBomPushPreInfo>();
                    woBomPushPreInfos.AddRange(list);
                });
            });
            // 查询产线产能
            var lineCapacity = resourceIds.SplitContains(tempIds =>
            {
                return Query<ProductModelLineCapacity>().Where(p => tempIds.Contains(p.ResourceId)).ToList();
            });
            var lineCapacityDic = lineCapacity.ToDictionary(p => p.ProductModelId, p => p.WorkingHours); // 同一机型下只能维护一个产线
            foreach (var bom in woBomPushPreInfos)
            {
                if (lineCapacityDic.TryGetValue(bom.ModelId, out var workingHours))
                {
                    bom.ResourceMeter = workingHours ?? 0;
                }
            }

            return woBomPushPreInfos;
        }

        /// <summary>
        /// 根据工单获取工艺路线(拼接)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual string GetWorkOrderLayoutByWoId(double Id)
        {
            var workOrder = RF.GetById<WorkOrder>(Id, new EagerLoadOptions().LoadWithViewProperty());
            List<string> strs = new List<string>();
            if (workOrder != null)
            {
                foreach (var routingProcess in workOrder.RoutingProcessList.OrderBy(p => p.Index))
                {
                    strs.Add(routingProcess.Process.Name);
                }
            }
            return string.Join(">", strs);
        }

        /// <summary>
        /// 根据工单获取工艺路线按照工序编码(拼接)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual string GetWorkOrderLayoutByWoCodeId(double Id)
        {
            var workOrder = RF.GetById<WorkOrder>(Id, new EagerLoadOptions().LoadWithViewProperty());
            List<string> strs = new List<string>();
            if (workOrder != null)
            {
                foreach (var routingProcess in workOrder.RoutingProcessList.OrderBy(p => p.Index))
                {
                    strs.Add(routingProcess.Process.Code);
                }
            }
            return string.Join(">", strs);
        }
    }
}