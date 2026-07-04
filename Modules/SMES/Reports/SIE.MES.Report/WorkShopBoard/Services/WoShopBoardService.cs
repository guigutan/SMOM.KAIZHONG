using Microsoft.Scripting.Utils;
using SIE.Api;
using SIE.Core.Common.Service;
using SIE.Defects;
using SIE.Domain;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Report.WorkShopBoard.APIModels;
using SIE.MES.Statistics.Entities;
using SIE.MES.Statistics.Fpy;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Report.WorkShopBoard.Services
{
    /// <summary>
    /// 车间看板服务
    /// </summary>
    public class WoShopBoardService : DomainService
    {
        /// <summary>
        /// 获取车间下的产线工单信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual List<WoShopOrderInfo> GetWoShopOrderInfos(double? workShopId)
        {
            if (workShopId == null || workShopId == 0)
            {
                return new List<WoShopOrderInfo>();
            }
            List<WoShopOrderInfo> woShopOrderInfoList = CreateOrderInfoList(workShopId);

            return woShopOrderInfoList;
        }

        private List<WoShopOrderInfo> CreateOrderInfoList(double? workShopId)
        {
            var resourceIds = DB.Query<WipResource>().Where(p => p.WorkShopId == workShopId).Select(p => p.Id).ToList<double>();
            var wipResourceWorkOrderList = resourceIds.SplitContains(reId =>
            {
                return DB.Query<WipResourceWorkOrder>().Where(p => reId.Contains(p.ResourceId)).ToList();
            });
            var woIds = new List<double>();
            wipResourceWorkOrderList.ForEach(item =>
            {
                woIds.Add(item.WorkOrderId);
            });
            woIds = woIds.Distinct().ToList();
            // 资源+工单去重
            var workingOrderList = woIds.SplitContains(tempIds =>
            {
                return DB.Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            var fpyList = woIds.SplitContains(tempIds => { return DB.Query<ProductFpyStatistics>().Where(p => tempIds.Contains(p.WorkOrderId) && p.WorkShopId == workShopId).ToList(); });
            List<WoShopOrderInfo> woShopOrderInfoList = new List<WoShopOrderInfo>();
            workingOrderList.ForEach(wo =>
            {
                var fpy = fpyList.Where(p => p.WorkOrderId == wo.Id).ToList();
                WoShopOrderInfo woShopOrderInfo = new WoShopOrderInfo
                {
                    Situation = wo.State.ToLabel(),
                    ResourceName = wo.ResourceName,
                    WoNo = wo.No,
                    ProductCode = wo.ProductCode,
                    PlanQty = wo.PlanQty,
                    FinishQty = wo.FinishQty,
                    PassQty = fpy.Sum(p => p.PassQty),
                    InputQty = fpy.Sum(p => p.InputQty),
                    PlanRate = wo.FinishQty != 0 ? Math.Round(wo.FinishQty / wo.PlanQty, 3) * 100 : 0,
                };
                woShopOrderInfoList.Add(woShopOrderInfo);
            });
            return woShopOrderInfoList;
        }

        /// <summary>
        /// 获取车间下的工单产出信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual List<ProductOutput> GetProductOutputs(double? workShopId)
        {
            if (workShopId == null || workShopId == 0)
            {
                return new List<ProductOutput>();
            }
            List<ProductOutput> productOutputs = new List<ProductOutput>();
            List<ProductOutput> productOutputGroup = new List<ProductOutput>();

            var workOrderStatistics = DB.Query<WorkOrderStatistics>().Where(p => p.WorkShopId == workShopId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            workOrderStatistics.ForEach(item =>
            {
                ProductOutput productOutput = new ProductOutput
                {
                    Qty = item.QtyPass,
                    HourDate = item.BeginCollectTime.Hour,
                };
                productOutputGroup.Add(productOutput);
            });
            var group = productOutputGroup.GroupBy(p => p.HourDate).Select(p => new { HourData = p.Key, Qty = p.Sum(x => x.Qty) }).OrderBy(p => p.HourData).ToDictionary(p => p.HourData, p => p.Qty);
            group.ForEach(p =>
            {
                ProductOutput productOutput = new ProductOutput
                {
                    HourDate = p.Key,
                    Qty = p.Value,
                };
                productOutputs.Add(productOutput);
            });
            return productOutputs;
        }

        /// <summary>
        /// 获取一次通过率
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual PassRate GetPassRate(double? workShopId)
        {
            if (workShopId == null || workShopId == 0)
            {
                return new PassRate();
            }
            PassRate passRate = new PassRate();
            List<ProcessStatisticsEventInfo> list = new List<ProcessStatisticsEventInfo>();
            var entityList = DB.Query<ProcessFpyStatistics>()
                .Where(w => w.WorkShopId == workShopId)
                .ToList();

            foreach (var entity in entityList)
            {
                list.Add(new ProcessStatisticsEventInfo()
                {
                    ProcessId = entity.ProcessId,
                    ProcessName = entity.ProcessName,
                    InputQty = entity.InputQty,
                    PassQty = entity.PassQty,
                    FailedQty = entity.FailedQty
                });
            }

            if (list.Any())
            {
                passRate.InputQty = list.Sum(x => x.InputQty);
                passRate.PassQty = list.Sum(x => x.PassQty);
            }
            return passRate;
        }

        /// <summary>
        /// 缺陷简单信息类
        /// </summary>
        public class DefectSimpleClass
        {
            /// <summary>
            /// 缺陷代码
            /// </summary>
            public double DefectId { get; set; }

            /// <summary>
            /// 缺陷数量
            /// </summary>
            public decimal Qty { get; set; }
        }

        /// <summary>
        /// 获取缺陷统计
        /// </summary>
        /// <param name="workShopId"></param>
        public virtual List<DefectCount> GetDefectCount(double? workShopId)
        {
            if (workShopId == null || workShopId == 0)
            {
                return new List<DefectCount>();
            }
            var defectCount = new List<DefectCount>();
            var wipProductDefectList = DB.Query<WipProductDefect>()
                .Join<WipResource>((x, y) => x.ResourceId == y.Id && y.WorkShopId == workShopId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var batchWipProductDefectList = DB.Query<BatchWipProductDefectDetail>()
                .Join<BatchWipProductDefect>((x, y) => x.BatchWipProductDefectId == y.Id )
                .Join<BatchWipProductDefect, BatchWipProductVersion>((x, y) => x.VersionId == y.Id)
                .Join<BatchWipProductVersion, WorkOrder>((x, y) => x.WorkOrderId == y.Id && y.WorkShopId == workShopId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            
            //缺陷分组统计
            List<DefectSimpleClass> groupList = new List<DefectSimpleClass>();
            // 缺陷列表ids
            List<double> defectIds = new List<double>();

            // 单体
            var wipgroupList = wipProductDefectList.GroupBy(g => g.DefectId).Select(s => new DefectSimpleClass
            {
                DefectId = (double)s.Key,
                Qty = s.Count(),
            }).ToList();
            defectIds.AddRange(wipgroupList.Select(p => p.DefectId).Distinct().ToList());

            // 批次
            var batchwipgroupList = batchWipProductDefectList.GroupBy(g => g.DefectId).Select(s => new DefectSimpleClass
            {
                DefectId = s.Key,
                Qty = s.Count(),
            }).ToList();
            defectIds.AddRange(batchwipgroupList.Select(p => p.DefectId).Distinct().ToList());

            // 缺陷列表
            var defectList = defectIds.SplitContains(tempIds =>
            {
                return DB.Query<Defect>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            groupList.AddRange(wipgroupList);
            groupList.AddRange(batchwipgroupList);
            // 其他(缺陷统计)
            var otherNgQty = groupList.Sum(p => p.Qty);
            groupList = groupList.GroupBy(g => g.DefectId).Select(s => new DefectSimpleClass
            {
                DefectId = s.Key,
                Qty = s.Sum(p => p.Qty),
            }).ToList();
            if (!groupList.Any())
            {
                return new List<DefectCount>();
            }
            else if (groupList.Count == 1)
            {
                var defect = defectList.FirstOrDefault(p => p.Id == groupList[0].DefectId);
                defectCount.Add(new DefectCount { DefectCode = defect?.Code, DefectDesc = defect?.Description, Count = groupList[0].Qty });
            }
            else if (groupList.Count <= 6)
            {
                for (int i = 0; i < groupList.Count; i++)
                {
                    var defect = defectList.FirstOrDefault(p => p.Id == groupList[i].DefectId);
                    DefectCount defectCount1 = new DefectCount
                    {
                        DefectCode = defect?.Code,
                        DefectDesc = defect?.Description,
                        Count = groupList[i].Qty,
                    };
                    defectCount.Add(defectCount1);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    var defect = defectList.FirstOrDefault(p => p.Id == groupList[i].DefectId);
                    DefectCount defectCount1 = new DefectCount
                    {
                        DefectCode = defect?.Code,
                        DefectDesc = defect?.Description,
                        Count = groupList[i].Qty,
                    };
                    otherNgQty -= groupList[i].Qty;
                    defectCount.Add(defectCount1);
                }
                defectCount.Add(new DefectCount { DefectCode = "其他", DefectDesc = "其他", Count = otherNgQty });
            }
            return defectCount;
        }
    }
}
