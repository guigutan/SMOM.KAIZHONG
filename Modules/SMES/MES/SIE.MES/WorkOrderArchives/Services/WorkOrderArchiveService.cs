using SIE.Common;
using SIE.Core.Common.Service;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.BatchWIP.Products;
using SIE.MES.LoadItems;
using SIE.MES.LoadItems.Models;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrderArchives.Bases;
using SIE.MES.WorkOrderArchives.ChildProducts;
using SIE.MES.WorkOrderArchives.Daos;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Models;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrderArchives.Services
{
    /// <summary>
    /// 工单制作档案SERVICE层
    /// </summary>
    public partial class WorkOrderArchiveService : DomainService
    {
        /// <summary>
        /// 工单制造档案访问
        /// </summary>
        private readonly WorkOrderArchiveDao _workOrderArchiveDao;

        /// <summary>
        /// Service构造函数
        /// </summary>
        /// <param name="workOrderArchiveDao"></param>
        public WorkOrderArchiveService(WorkOrderArchiveDao workOrderArchiveDao)
        {
            _workOrderArchiveDao = workOrderArchiveDao;
        }

        /// <summary>
        /// 工单制造档案查询服务
        /// </summary>
        /// <param name="workOrderArchiveCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderArchive> QueryWorkOrderArchiveList(WorkOrderArchiveCriteria workOrderArchiveCriteria)
        {
            return _workOrderArchiveDao.QueryWorkOrderArchiveList(workOrderArchiveCriteria);
        }

        /// <summary>
        /// 工单制造档案工单产出查询服务
        /// </summary>
        /// <param name="workOrderArchive"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<WoOrderArchiveProduceViewModel> QueryWoOrderArchiveProduceList(WorkOrderArchive workOrderArchive, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            workOrderArchive = RF.GetById<WorkOrderArchive>(workOrderArchive.Id);

            EntityList<WoOrderArchiveProduceViewModel> WoOrderArchiveProduceList = new EntityList<WoOrderArchiveProduceViewModel>();
            if (workOrderArchive.RetrospectType == Core.Items.RetrospectType.Single)
            {
                //生产通用报表
                var produceTabs = DB.Query<WipProductVersion>().Where(p => p.WorkOrderId == workOrderArchive.Id).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                produceTabs.ForEach(tab =>
                {
                    var archiveProduce = new WoOrderArchiveProduceViewModel
                    {
                        WorkOrder = tab.WorkOrder,
                        BarCode = tab.Sn,
                        BatchNo = string.Empty,
                        Qty = 1,
                        DrawQty = 0,
                        IsOver = tab.IsFinish,
                        ProCode = tab.WorkOrder.Product.Code,
                        ProName = tab.WorkOrder.Product.Name,
                    };
                    WoOrderArchiveProduceList.Add(archiveProduce);
                });
                var WoOrderArchiveProduceSortList = new EntityList<WoOrderArchiveProduceViewModel>();
                WoOrderArchiveProduceSortList.AddRange(WoOrderArchiveProduceList.OrderBy(p => p.BarCode).ThenBy(p => p.BatchNo));
                WoOrderArchiveProduceSortList.SetTotalCount(produceTabs.TotalCount);
                return WoOrderArchiveProduceSortList;
            }
            else
            {
                //批次生产通用报表
                var batchProduceTabs = DB.Query<BatchWipProductVersion>().Where(p => p.WorkOrderId == workOrderArchive.Id).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                batchProduceTabs.ForEach(tab =>
                {
                    var archiveProduce = new WoOrderArchiveProduceViewModel
                    {
                        WorkOrder = tab.WorkOrder,
                        BarCode = string.Empty,
                        BatchNo = tab.BatchNo,
                        Qty = tab.Qty,
                        DrawQty = tab.ScrapQty,
                        IsOver = tab.IsFinish,
                        ProCode = tab.WorkOrder.Product.Code,
                        ProName = tab.WorkOrder.Product.Name,
                    };
                    WoOrderArchiveProduceList.Add(archiveProduce);
                });
                var WoOrderArchiveProduceSortList = new EntityList<WoOrderArchiveProduceViewModel>();
                WoOrderArchiveProduceSortList.AddRange(WoOrderArchiveProduceList.OrderBy(p => p.BarCode).ThenBy(p => p.BatchNo));
                WoOrderArchiveProduceSortList.SetTotalCount(batchProduceTabs.TotalCount);
                return WoOrderArchiveProduceSortList;
            }
        }

        /// <summary>
        /// 工单制造档案待用标签查询服务
        /// </summary>
        /// <param name="workOrderArchiveId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderArchiveItemlLabelViewModel> QueryWorkOrderArchiveItemlLabelList(double workOrderArchiveId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            EntityList<WorkOrderArchiveItemlLabelViewModel> workOrderArchiveItemlLabelList = new EntityList<WorkOrderArchiveItemlLabelViewModel>();
            var itemLabelWorkOrders = DB.Query<ItemLabelWorkOrder>().Where(p => p.WorkOrderId == workOrderArchiveId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            itemLabelWorkOrders.ForEach(item =>
            {
                WorkOrderArchiveItemlLabelViewModel workOrderArchiveItemlLabel = new WorkOrderArchiveItemlLabelViewModel
                {
                    Label = item.ItemLabel.Label,
                    BatchNo = item.ItemLabel.Lot,
                    ItemCode = item.ItemLabel.Item.Code,
                    ItemName = item.ItemLabel.Item.Name,
                    ItemExPro = item.ItemLabel.ItemExtPropName,
                    Warehouse = item.ItemLabel.Warehouse?.Code,
                    Storage = item.ItemLabel.StorageLocation?.Code,
                    Qty = item.Qty,
                    IsSerialNumber = item.ItemLabel.IsSerialNumber,
                };
                workOrderArchiveItemlLabelList.Add(workOrderArchiveItemlLabel);
            });
            var workOrderArchiveItemlLabelSortList = new EntityList<WorkOrderArchiveItemlLabelViewModel>();
            workOrderArchiveItemlLabelSortList.AddRange(workOrderArchiveItemlLabelList.OrderBy(p => p.Label).ThenBy(p => p.BatchNo));
            workOrderArchiveItemlLabelSortList.SetTotalCount(itemLabelWorkOrders.TotalCount);
            return workOrderArchiveItemlLabelSortList;
        }

        /// <summary>
        /// 工单制造档案报工记录查询服务
        /// </summary>
        /// <param name="workOrderArchiveId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WoOrderArchiveReportViewModel> QueryWoOrderArchiveReportList(double workOrderArchiveId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            EntityList<WoOrderArchiveReportViewModel> woOrderArchiveReportList = new EntityList<WoOrderArchiveReportViewModel>();
            var reportRecordSimpleList = RT.Service.Resolve<IWoArchive>().GetReportRecords(workOrderArchiveId, sortInfo, pagingInfo);
            reportRecordSimpleList.ForEach(record =>
            {
                WoOrderArchiveReportViewModel woOrderArchiveReport = new WoOrderArchiveReportViewModel
                {
                    TaskNo = record.DispatchTaskNo,
                    TaskState = record.DispatchTaskState,
                    DispatchQty = record.DispatchQty,
                    OkQty = record.OkQty,
                    NgQty = record.NgQty,
                    ReportQty = record.ReportQty,
                    Process = record.ProcessName,
                    Charger = record.PrincipalName,
                    //TaskTime = record.TaskTime,
                    RecordReportQty = record.RecordReportQty,
                    RecordOkQty = record.RecordOkQty,
                    RecordNgQty = record.RecordNgQty,
                    Hour = record.Hour,
                    Station = record.StationName,
                    BatchNo = record.BatchNo,
                    ReportTime = record.ReportTime,
                    Defects = record.Defects,
                    Remark = record.Remark,
                    SpecificationCode = record.SpecificationCode,
                    SpecificationName = record.SpecificationName,
                    IsVirtualPart = record.IsVirtualPart,
                    VirtualPartCode = record.VirtualPartCode,
                    VirtualPartName = record.VirtualPartName,
                    ReportMode = record.ReportMode,
                };
                woOrderArchiveReportList.Add(woOrderArchiveReport);
            });
            var woOrderArchiveReportSortList = new EntityList<WoOrderArchiveReportViewModel>();
            woOrderArchiveReportSortList.AddRange(woOrderArchiveReportList.OrderByDescending(p => p.TaskNo).OrderBy(p => p.BatchNo));
            woOrderArchiveReportSortList.SetTotalCount(reportRecordSimpleList.TotalCount);
            return woOrderArchiveReportSortList;

        }

        /// <summary>
        /// 工单制造档案生产采集查询服务
        /// </summary>
        /// <param name="workOrderArchiveId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WoOrderArchiveProcessViewModel> QueryWoOrderArchiveProcessList(double workOrderArchiveId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            EntityList<WoOrderArchiveProcessViewModel> woOrderArchiveProcessList = new EntityList<WoOrderArchiveProcessViewModel>();
            var workOrderRoutingProcessEntityList = DB.Query<WorkOrderRoutingProcess>()
                .Where(p => p.WorkOrderId == workOrderArchiveId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var workOrderRoutingProcesses = workOrderRoutingProcessEntityList
                .OrderBy(p => p.Index)
                .ToList();

            var workOrderRoutingProcessIds = workOrderRoutingProcessEntityList.Select(x => x.Id).Distinct().ToList();

            var routingProcessParameters = workOrderRoutingProcessIds.SplitContains(tempIds =>
            {
                return DB.Query<WorkOrderRoutingProcessParameter>()
                    .Where(x => tempIds.Contains(x.ProcessId))
                    .ToList();
            });

            //工序采集信息
            var processStatisticsList = RT.Service.Resolve<IProcessStatistics>().GetProcessStatisticsList(workOrderArchiveId);

            for (int i = 0; i < workOrderRoutingProcesses.Count; i++)
            {
                //当前工序
                var workOrderRoutingProcess = workOrderRoutingProcesses[i];

                //当前工序采集信息集合
                var processStatistic = processStatisticsList
                    .Where(p => p.ProcessId == workOrderRoutingProcess.ProcessId && p.ProcessIndex == workOrderRoutingProcess.Index).ToList();

                var qtyMove = processStatistic.Sum(p => p.InputQty);
                var qtyPass = processStatistic.Sum(p => p.PassQty);
                var qtyFailed = processStatistic.Sum(p => p.FailedQty);

                WoOrderArchiveProcessViewModel woOrderArchiveProcess = new WoOrderArchiveProcessViewModel
                {
                    Index = workOrderRoutingProcess.Index,
                    ProcessName = workOrderRoutingProcess.Name,
                    QtyMove = qtyMove,
                    QtyPass = qtyPass,
                    QtyFailed = qtyFailed
                };

                if ((workOrderRoutingProcess.Sign & Tech.Routings.RoutingProcessSign.Start) == Tech.Routings.RoutingProcessSign.Start)
                {
                    // 首工序堆积数0
                    woOrderArchiveProcess.QtyStacked = 0;
                }
                else
                {
                    // 计算堆积数
                    CalculateQtyStacked(workOrderRoutingProcesses, routingProcessParameters,
                        processStatisticsList, workOrderRoutingProcess, woOrderArchiveProcess);
                }

                woOrderArchiveProcessList.Add(woOrderArchiveProcess);
            }
            var woOrderArchiveProcessSortList = new EntityList<WoOrderArchiveProcessViewModel>();
            woOrderArchiveProcessSortList.AddRange(woOrderArchiveProcessList.OrderBy(p => p.Index).ThenBy(p => p.ProcessName));
            woOrderArchiveProcessSortList.SetTotalCount(workOrderRoutingProcessEntityList.TotalCount);
            return woOrderArchiveProcessSortList;
        }

        /// <summary>
        /// 计算堆积数
        /// </summary>
        /// <param name="workOrderRoutingProcesses"></param>
        /// <param name="routingProcessParameters"></param>
        /// <param name="processStatisticsList"></param>
        /// <param name="workOrderRoutingProcess"></param>
        /// <param name="woOrderArchiveProcess"></param>

        private static void CalculateQtyStacked(List<WorkOrderRoutingProcess> workOrderRoutingProcesses, EntityList<WorkOrderRoutingProcessParameter> routingProcessParameters, List<ProcessStatisticsEventInfo> processStatisticsList, WorkOrderRoutingProcess workOrderRoutingProcess, WoOrderArchiveProcessViewModel woOrderArchiveProcess)
        {
            //上工序的参数
            //所有采集结果为【成功】时，可以指向当前工序的工序参数
            var parametersOfPrev = routingProcessParameters
                .Where(x => x.NextProcessId == workOrderRoutingProcess.Id
                    && ((ResultType)x.ResultType & ResultType.Pass) == ResultType.Pass)
                .ToList();

            decimal qtyOfPrev = 0;
            decimal qtyOfNext = 0;
            Dictionary<double, bool> nextProcessIdsDictionary = new Dictionary<double, bool>();

            foreach (var parameterOfPrev in parametersOfPrev)
            {
                var routingProcessOfPrev = workOrderRoutingProcesses
                    .FirstOrDefault(x => x.Id == parameterOfPrev.ProcessId);

                //上工序过站数量
                qtyOfPrev += processStatisticsList.Where(x => x.ProcessId == routingProcessOfPrev.ProcessId
                    && x.ProcessIndex == routingProcessOfPrev.Index)
                  .Sum(x => x.InputQty);

                //前工序的所有采集结果为【成功】的后工序列表
                var parametersOfNext = routingProcessParameters
                    .Where(x => x.ProcessId == routingProcessOfPrev.Id
                        && ((ResultType)x.ResultType & ResultType.Pass) == ResultType.Pass)
                    .ToList();

                foreach (var nextProcessId in parametersOfNext
                    .Where(x=>x.NextProcessId.HasValue)
                    .Select(x=>x.NextProcessId.Value))
                {
                    if (  nextProcessIdsDictionary.ContainsKey(nextProcessId))
                    {
                        continue;
                    }

                    nextProcessIdsDictionary.Add(nextProcessId, true);
                    
                    var routingProcessOfNext = workOrderRoutingProcesses
                        .FirstOrDefault(x => x.Id == nextProcessId);

                    //上工序的所有后工序的过站数量
                    qtyOfNext += processStatisticsList.Where(x => x.ProcessId == routingProcessOfNext.ProcessId
                        && x.ProcessIndex == routingProcessOfNext.Index)
                      .Sum(x => x.InputQty);
                }
            }


            //堆积数计算
            woOrderArchiveProcess.QtyStacked = qtyOfPrev - qtyOfNext;
        }

        /// <summary>
        /// 工单制造档案物料耗用查询服务
        /// </summary>
        /// <param name="workOrderArchive"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WoOrderArchiveItemCostViewModel> QueryWoOrderArchiveItemCostList(WorkOrderArchive workOrderArchive, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            workOrderArchive = RF.GetById<WorkOrderArchive>(workOrderArchive.Id);

            EntityList<WoOrderArchiveItemCostViewModel> woOrderArchiveItemCostList = new EntityList<WoOrderArchiveItemCostViewModel>();
            // 工单bom
            var bomList = DB.Query<WorkOrderBom>().Where(p => p.WorkOrderId == workOrderArchive.Id).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            // 工序bom
            var processbomList = DB.Query<WorkOrderProcessBom>().Where(p => workOrderArchive.Id == p.WorkOrderId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            // 物料耗用
            var deductItems = DB.Query<WoCostItem>().Where(p => p.WorkOrderId == workOrderArchive.Id  && p.State == LoadItems.Enum.WoCostItemState.Submitted).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            // 单体关键件清单
            var singleKeyItemList = DB.Query<WipProductProcessKeyItem>().Where(p => !p.IsUnbound)
                .Join<WipProductProcess>((x, y) => x.ProcessId == y.Id)
                .Join<WipProductProcess, WipProductVersion>((x, y) => x.VersionId == y.Id && y.WorkOrderId == workOrderArchive.Id)
                .GroupBy<WipProductProcess, WipProductVersion>((k, p, v) => new
                {
                    k.ItemId,
                    k.ItemExtProp,
                    p.ProcessId,
                })
                .Select<WipProductProcess, WipProductVersion>((k, p, v) => new
                {
                    ItemId = k.ItemId,
                    ItemExtProp = k.ItemExtProp,
                    ProcessId = p.ProcessId,
                    Qty = k.Qty.SUM(),
                }).ToList<WipProductKeyItem>().ToList();

            // 批次关键件清单
            var batchKeyItemList = DB.Query<BatchWipProductProcessKeyItem>()
                    .Join<BatchWipProductProcessDetail>((x, y) => x.DetailId == y.Id)
                    .Join<BatchWipProductProcessDetail, BatchWipProductProcess>((x, y) => x.ProductProcessId == y.Id)
                    .Join<BatchWipProductProcess, BatchWipProductVersion>((x, y) => x.VersionId == y.Id && y.WorkOrderId == workOrderArchive.Id)
                    .GroupBy<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        k.ItemId,
                        k.ItemExtProp,
                        k.ProcessId,
                    })
                    .Select<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        ItemId = k.ItemId,
                        ItemExtProp = k.ItemExtProp,
                        ProcessId = k.ProcessId,
                        Qty = k.Qty.SUM(),
                    }).ToList<WipProductKeyItem>().ToList();

            bomList.ForEach(bom =>
            {
                WoOrderArchiveItemCostViewModel woOrderArchiveItemCost = new WoOrderArchiveItemCostViewModel();
                //当前工单bom的工序bom列表
                var processbom = processbomList.Where(p =>  p.ItemId == bom.ItemId && p.ItemExtProp == bom.ItemExtProp).ToList();
                
                //没有工序bom,按工单bom展示
                if (processbom.Count <= 0)
                {
                    //总消耗量
                    decimal TotalQty = CalculateTotalQty(workOrderArchive.RetrospectType, bom.ItemId, bom.ItemExtProp, false, null, deductItems, singleKeyItemList, batchKeyItemList);
                    woOrderArchiveItemCost.ProcessName = string.Empty;
                    woOrderArchiveItemCost.WorkStep = string.Empty;
                    woOrderArchiveItemCost.ItemCode = bom.ItemCode;
                    woOrderArchiveItemCost.ItemName = bom.ItemName;
                    woOrderArchiveItemCost.ItemExPro = bom.ItemExtPropName;
                    woOrderArchiveItemCost.SingleQty = bom.SingleQty;
                    woOrderArchiveItemCost.RequireQty = bom.RequireQty;
                    woOrderArchiveItemCost.TotalQty = TotalQty;
                    woOrderArchiveItemCostList.Add(woOrderArchiveItemCost);
                }
                //有工序bom，按工序+工步+物料为同一行展示
                else
                {

                    var processDistincts = processbom.GroupBy(p => new { p.ItemId, p.ProcessId, p.WorkStepId, p.WorkOrderId })
                        .Select(p => new WorkOrderProcessBom
                        {
                            ProcessId = p.Key.ProcessId,
                            WorkStepId = p.Key.WorkStepId,
                            ItemId = p.Key.ItemId,
                            WorkOrderId = p.Key.WorkOrderId,
                            ItemExtPropName = p.Select(q => q.ItemExtPropName).First(),
                            SingleQty = p.Sum(q => q.SingleQty),
                            Weight = p.Sum(q => q.SingleQty),
                        }).ToList();
                    processDistincts.ForEach(processDistinct =>
                    {
                        //总消耗量
                        decimal TotalQty = CalculateTotalQty(workOrderArchive.RetrospectType, bom.ItemId, bom.ItemExtProp, true, processDistinct.ProcessId, deductItems, singleKeyItemList, batchKeyItemList);
                        woOrderArchiveItemCost.ProcessName = processDistinct.Process?.Name;
                        woOrderArchiveItemCost.WorkStep = processDistinct.WorkStep?.Name;
                        woOrderArchiveItemCost.ItemCode = processDistinct.Item.Code;
                        woOrderArchiveItemCost.ItemName = processDistinct.Item.Name;
                        woOrderArchiveItemCost.ItemExPro = processDistinct.ItemExtPropName;
                        woOrderArchiveItemCost.SingleQty = processDistinct.SingleQty;
                        woOrderArchiveItemCost.RequireQty = processDistinct.WorkOrder.PlanQty * processDistinct.SingleQty;
                        woOrderArchiveItemCost.TotalQty = TotalQty;
                    });
                    woOrderArchiveItemCostList.Add(woOrderArchiveItemCost);
                }
            });
            var woOrderArchiveItemCostSortList = new EntityList<WoOrderArchiveItemCostViewModel>();
            woOrderArchiveItemCostSortList.AddRange(woOrderArchiveItemCostList.OrderByDescending(p => p.ProcessName).OrderBy(p => p.ItemName));
            woOrderArchiveItemCostSortList.SetTotalCount(bomList.TotalCount);
            return woOrderArchiveItemCostSortList;
        }

        /// <summary>
        /// 计算总耗用量
        /// </summary>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtpro">物料拓展属性</param>
        /// <param name="hasProcess">是否有工序bom</param>
        /// <param name="processId">工序id</param>
        /// <param name="deductItemList">物料耗用</param>
        /// <param name="singleKeyItemList">单体关键件清单</param>
        /// <param name="batchKeyItemList">批次关键件清单</param>
        /// <returns></returns>
        private decimal CalculateTotalQty(RetrospectType retrospectType, double itemId, string itemExtpro,
            bool hasProcess, double? processId, EntityList<WoCostItem> deductItemList, List<WipProductKeyItem> singleKeyItemList, List<WipProductKeyItem> batchKeyItemList)
        {
            //无工序bom，取扣料数量
            if (!hasProcess)
            {
                var deductItems = deductItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtpro).ToList();
                return deductItems.Sum(p => p.Qty);
            }
            //有工序bom，取工单工序下的关键件清单之和(20231023取消工序过滤，考虑到维修换料)
            else
            {
                if (retrospectType == RetrospectType.Single)
                {
                    return singleKeyItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtpro /*&& p.ProcessId == processId*/).Sum(p => p.Qty);
                }
                else
                {
                    return batchKeyItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtpro /*&& p.ProcessId == processId*/).Sum(p => p.Qty);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrderArchiveId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderArchiveItemShortViewModel> QueryWorkOrderArchiveItemShortList(double workOrderArchiveId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            // 取数
            EntityList<WorkOrderArchiveItemShortViewModel> itemShortList = new EntityList<WorkOrderArchiveItemShortViewModel>();
            // 排序
            EntityList<WorkOrderArchiveItemShortViewModel> sortedItemShortList = new EntityList<WorkOrderArchiveItemShortViewModel>();
            // 原始数据
            CalculateParameterInfo calculateParameterInfo = new CalculateParameterInfo(workOrderArchiveId, pagingInfo);

            if (calculateParameterInfo.BomPullItemIds.Any())
            {
                PullItemHandle(calculateParameterInfo, itemShortList);
            }
            if (calculateParameterInfo.BomPushItemIds.Any())
            {
                PushItemHandle(calculateParameterInfo, itemShortList);
            }

            sortedItemShortList.AddRange(itemShortList.OrderByDescending(p => p.ShortQty));
            sortedItemShortList.SetTotalCount(calculateParameterInfo.BomList.TotalCount);
            return sortedItemShortList;
        }

        /// <summary>
        /// 拉式物料缺料处理
        /// </summary>
        /// <param name="calculateParameterInfo">原始数据集</param>
        /// <param name="itemShortList">展示数据</param>
        private void PullItemHandle(CalculateParameterInfo calculateParameterInfo, EntityList<WorkOrderArchiveItemShortViewModel> itemShortList)
        {
            #region new
            PullMaterial pullMaterial = new PullMaterial(calculateParameterInfo);
            var pullbom = calculateParameterInfo.BomList.Where(p => calculateParameterInfo.BomPullItemIds.Contains(p.ItemId)).ToList();
            foreach (var bom in pullbom)
            {
                WorkOrderArchiveItemShortViewModel workOrderArchiveItemShortViewModel = pullMaterial.ReturnViewModel(bom);
                itemShortList.Add(workOrderArchiveItemShortViewModel);
            }

            #endregion

        }

        /// <summary>
        /// 推式物料缺料处理
        /// </summary>
        /// <param name="calculateParameterInfo"></param>
        /// <param name="itemShortList"></param>
        private void PushItemHandle(CalculateParameterInfo calculateParameterInfo, EntityList<WorkOrderArchiveItemShortViewModel> itemShortList)
        {
            #region new
            PushMaterial pushMaterial1 = new PushMaterial(calculateParameterInfo);
            var pushbom = calculateParameterInfo.BomList.Where(p => calculateParameterInfo.BomPushItemIds.Contains(p.ItemId)).ToList();
            foreach (var bom in pushbom)
            {
                WorkOrderArchiveItemShortViewModel workOrderArchiveItemShortViewModel = pushMaterial1.ReturnViewModel(bom);
                itemShortList.Add(workOrderArchiveItemShortViewModel);
            }
            #endregion

        }

        /// <summary>
        /// 计算相同线边仓工单剩余需求数量
        /// </summary>
        /// <param name="productRuleList"></param>
        /// <param name="sameResourceWo"></param>
        /// <param name="samWoBomList"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="singleKeyItemList"></param>
        /// <param name="batchKeyItemList"></param>
        /// <param name="woCostList"></param>
        /// <returns></returns>
        private decimal CalculateSameWoNeedQty(EntityList<ItemBatchRule> productRuleList, List<WorkOrderBaseData> sameResourceWo, EntityList<WorkOrderBom> samWoBomList, double itemId, string itemExtProp,
            List<WipProductKeyItem> singleKeyItemList, List<WipProductKeyItem> batchKeyItemList, List<WoCostItemBaseData> woCostList)
        {
            decimal sameNeedQty = 0;
            var itemWoBomList = samWoBomList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).ToList();
            foreach(var p in itemWoBomList)
            {
                var wo = sameResourceWo.FirstOrDefault(q => q.Id == p.WorkOrderId);
                if (wo == null)
                {
                    continue;
                }
                var retrospectType = productRuleList.FirstOrDefault(x => x.ItemId == wo.ProductId).RetrospectType;
                var woNeedQty = p.SingleQty * wo.PlanQty;
                var woCostQty = CalculateHasCostQty(retrospectType, p.ItemId, p.ItemExtProp, singleKeyItemList.Where(p => p.WoOrderId == wo.Id).ToList(), batchKeyItemList.Where(p => p.WoOrderId == wo.Id).ToList(), woCostList.Where(p => p.WorkOrderId == wo.Id).ToList());
                sameNeedQty += woNeedQty - woCostQty;
            }
            return sameNeedQty;
        }

        /// <summary>
        /// 计算工单已耗用数量
        /// </summary>
        /// <param name="retrospectType"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="singleKeyItemList"></param>
        /// <param name="batchKeyItemList"></param>
        /// <param name="woCostList"></param>
        /// <returns></returns>
        private decimal CalculateHasCostQty(RetrospectType retrospectType, double itemId, string itemExtProp, List<WipProductKeyItem> singleKeyItemList, List<WipProductKeyItem> batchKeyItemList, List<WoCostItemBaseData> woCostList)
        {
            var deductItems = woCostList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp).ToList();
            decimal countOne = deductItems.Sum(p => p.Qty);
            decimal countTwo = 0;
            
            if (retrospectType == RetrospectType.Single)
            {
                countTwo = singleKeyItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp ).Sum(p => p.Qty);
            }
            else
            {
                countTwo = batchKeyItemList.Where(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp ).Sum(p => p.Qty);
            }
            return countOne + countTwo;
        }
    }
}
