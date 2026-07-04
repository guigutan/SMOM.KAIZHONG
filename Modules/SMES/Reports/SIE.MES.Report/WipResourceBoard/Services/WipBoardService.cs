using SIE.Api;
using SIE.Core.Common.Service;
using SIE.Core.Items;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Report.WipProducts;
using SIE.MES.Report.WipResourceBoard.APIModels;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WorkOrderArchives;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static IronPython.Modules._ast;
using static QRCoder.PayloadGenerator;

namespace SIE.MES.Report.WipResourceBoard.Services
{
    /// <summary>
    /// 产线看板服务
    /// </summary>
    public class WipBoardService : DomainService
    {
        /// <summary>
        /// 根据产线获取工单任务信息
        /// </summary>
        /// <param name="wipId">产线Id</param>
        /// <returns></returns>
        public virtual List<WoOrderTaskInfo> GetWipWorkOrderList(double? wipId)
        {
            if (wipId == null || wipId == 0)
            {
                return new List<WoOrderTaskInfo>();
            }
            // 班次取不到取当天23:59:59
            var timeNow = GetShiftTime(wipId);
            // 计划开始时间小于timeNow且状态为发放、生产中、发放暂停、生产中暂停的工单
            var workList = DB.Query<WorkOrder>().Where(p => p.ResourceId == wipId && p.PlanBeginDate <= timeNow && (p.State == Core.WorkOrders.WorkOrderState.Release || p.State == Core.WorkOrders.WorkOrderState.Producing)).OrderBy(p => p.PlanBeginDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var woOrderTaskInfoList = new List<WoOrderTaskInfo>();
            workList.ForEach(work =>
            {
                WoOrderTaskInfo woOrderTask = new WoOrderTaskInfo
                {
                    WoId = work.Id,
                    State = work.State,
                    IsStop = work.IsPause == YesNo.Yes,
                    WoNo = work.No,
                    ProductName = work.ProductName,
                    PlanQty = work.PlanQty,
                    FinishQty = work.FinishQty,
                    ScrapQty = work.ScrapQty,
                    PlanBeginDate = work.PlanBeginDate.ToString("yyyy-MM-dd HH:mm"),
                    PlanEndDate = work.PlanEndDate.ToString("yyyy-MM-dd HH:mm"),
                };
                woOrderTaskInfoList.Add(woOrderTask);
            });
            return woOrderTaskInfoList;
        }

        /// <summary>
        /// 计算当前产线的班次结束日期
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        private DateTime GetShiftTime(double? wipId)
        {
            // 班次取不到取当天23:59:59
            var timeNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var wipResourceMove = RF.GetById<WipResourceMove>(wipId);
            double? shiftTypeId = null;
            Shift shift = null;
            EntityList<Shift> shiftList = null;
            if (wipResourceMove != null)
            {
                shiftTypeId = RT.Service.Resolve<WipResourceController>().GetWipResourceShiftTypeId(wipResourceMove, timeNow);
                shiftList = RT.Service.Resolve<ShiftTypeController>().GetShifts(shiftTypeId ?? 0);
                shift = RT.Service.Resolve<ShiftTypeController>().GetShift(shiftList, timeNow);
            }
            if (shift != null)
            {
                if (shift.IsOverDay)
                {
                    timeNow = timeNow.AddDays(1);
                }
                timeNow = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, shift.EndTime.Hour, shift.EndTime.Minute, shift.EndTime.Second);

            }
            return timeNow;
        }

        /// <summary>
        /// 根据产线获取在制工单
        /// </summary>
        /// <param name="wipId">wipId</param>
        /// <returns></returns>
        public virtual WipWorkOrder GetWipWorkOrder(double? wipId)
        {
            if (wipId == null || wipId == 0)
            {
                return new WipWorkOrder();
            }
            var workingOrder = DB.Query<WorkOrder>()
                .Exists<WipResourceWorkOrder>((x, y) => y.Where(p => p.WorkOrderId == x.Id && p.ResourceId == wipId)).OrderByDescending(p => p.UpdateDate).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            if (workingOrder == null)
            {
                return new WipWorkOrder();
            }
            WipWorkOrder wipWorkOrder = new WipWorkOrder
            {
                WoId = workingOrder.Id,
                No = workingOrder.No,
                Type = workingOrder.Type.ToLabel(),
                ProCode = workingOrder.ProductCode,
                ProName = workingOrder.ProductName,
                ProModel = workingOrder.ProductModelName,
                Customer = workingOrder.Customer?.Name,
                PlanBeginDate = workingOrder.PlanBeginDate.ToString(),
                PlanEndDate = workingOrder.PlanEndDate.ToString(),
                PlanQty = workingOrder.PlanQty,
                FinishQty = workingOrder.FinishQty,
            };
            return wipWorkOrder;
        }

        /// <summary>
        /// 获取在制工单一次通过率
        /// </summary>
        /// <param name="woId">在制工单Id</param>
        /// <returns></returns>
        public virtual WipWorkOrderPass GetWipProducePassRate(double? woId)
        {
            if (woId == null || woId == 0)
            {
                return new WipWorkOrderPass();
            }
            var passRate = new WipWorkOrderPass();
            // 工序采集信息
            //var routingProcessList = DB.Query<WorkOrderRoutingProcess>().Where(w => w.WorkOrderId == woId)
            //    .OrderBy(o => o.Index).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //var routingProcess = routingProcessList.FirstOrDefault(w => w.IsPassRate);//取设置了直通率取值的
            //if (routingProcess != null)
            //{
            //    var processFpyList = RT.Service.Resolve<IProcessStatistics>().GetProcessStatisticsList(woId.Value).Where(w => w.ProcessId == routingProcess.ProcessId).ToList();
            //    if (processFpyList.Any())
            //    {
            //        passRate.InputQty = processFpyList.Sum(s => s.InputQty);
            //        passRate.PassQty = processFpyList.Sum(s => s.PassQty);
            //        passRate.FailedQty = processFpyList.Sum(s => s.FailedQty);
            //    }
            //}
            var workOrder = RF.GetById<WorkOrder>(woId);
            var productId = workOrder.ProductId;
            var retrospectType = DB.Query<ItemBatchRule>().Where(p => p.ItemId == productId).FirstOrDefault()?.RetrospectType;
            if (retrospectType == null)
            {
                return new WipWorkOrderPass();
            }
            else if (retrospectType == RetrospectType.Single) // 单体
            {
                var wipVersion = DB.Query<WipProductVersion>().Where(p => p.WorkOrderId == woId && p.IsFinish).ToList();
                var wipVersionIds = wipVersion.Select(p => p.Id).ToList();
                var defectList = wipVersionIds.SplitContains(tempIds =>
                {
                    return DB.Query<WipProductDefect>().Where(p => tempIds.Contains(p.VersionId)).ToList();
                });
                var badWipVersionCount = defectList.Select(p => p.VersionId).Distinct().Count();
                passRate.FailedQty = badWipVersionCount;
                passRate.PassQty = wipVersionIds.Count - badWipVersionCount;
                return passRate;
            }
            else // 批次
            {
                var batchWipVersion = DB.Query<BatchWipProductVersion>().Where(p => p.WorkOrderId == woId && p.IsFinish).ToList();
                var batchWipVersionIds = batchWipVersion.Select(p => p.Id).ToList();
                var batchdefectList = batchWipVersionIds.SplitContains(tempIds =>
                {
                    return DB.Query<BatchWipProductDefect>().Where(p => tempIds.Contains(p.VersionId)).ToList();
                });
                var badWipVersionCount = batchdefectList.Select(p => p.VersionId).Distinct().Count();
                passRate.FailedQty = badWipVersionCount;
                passRate.PassQty = batchWipVersion.Sum(p => p.FinishQty) - badWipVersionCount;
                return passRate;
            }
        }

        /// <summary>
        /// 获取时段生产效率
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <param name="wipId">产线Id</param>
        /// <returns></returns>
        public virtual WipProductEfficiency GetWipEfficiency(List<double> woIds, double? wipId)
        {
            if (wipId == null || wipId == 0)
            {
                return new WipProductEfficiency();
            }
            if (!woIds.Any())
            {
                return new WipProductEfficiency();
            }
            var tiemRangeIn24 = new List<string>();
            List<string> timeRange = GetTimeRange(wipId, tiemRangeIn24);
            if (!timeRange.Any())
            {
                return new WipProductEfficiency();
            }
            List<decimal> targetProduct = new List<decimal>();
            List<decimal> actualProduct = new List<decimal>();
            List<decimal> efficiency = new List<decimal>();
            Dictionary<string, decimal> timeRangeValue = new Dictionary<string, decimal>();

            timeRange.ForEach(i =>
            {
                timeRangeValue.Add(i, 0);
            });
            CalculateWipProduct(wipId, woIds, timeRangeValue, targetProduct, actualProduct, efficiency);
            WipProductEfficiency wipProductEfficiency = new WipProductEfficiency
            {
                TimeRange = tiemRangeIn24,
                TargetProduct = targetProduct,
                ActualProduct = actualProduct,
                Efficiency = efficiency,
            };
            return wipProductEfficiency;
        }

        /// <summary>
        /// 统计生产通用报表
        /// </summary>
        private void CalculateWipProduct(double? wipId, List<double> woIds, Dictionary<string, decimal> timeRangeValue, List<decimal> targetProduct, List<decimal> actualProduct, List<decimal> efficiency)
        {
            var nowTime = DateTime.Now;
            var queryTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0, DateTimeKind.Utc);
            // 工单
            var workOrderList = woIds.SplitContains(tempIds =>
            {
                return DB.Query<WorkOrder>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            // 产品
            var productIds = workOrderList.Select(p => p.ProductId).ToList();
            var productList = productIds.SplitContains(tempIds =>
            {
                return DB.Query<Items.Item>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            var proModelIds = productList.Select(p => p.ModelId).ToList();
            var proModelList = proModelIds.SplitContains(tempIds =>
            {
                return DB.Query<ProductModel>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            // 节拍
            var meterList = proModelIds.SplitContains(tempIds =>
            {
                return DB.Query<ProductModelLineCapacity>().Where(p => tempIds.Contains(p.ProductModelId) && p.ResourceId == wipId).ToList();
            });

            //// 防止50000条数据溢出报错
            //var wipProductListCount = DB.Query<WipProductVersionReport>()
            //    .Where(p => woIds.Contains(p.WorkOrderId) && p.IsFinish).Count();
            //PagingInfo pagingInfo = new PagingInfo
            //{
            //    PageSize = wipProductListCount,
            //    PageNumber = 1,
            //};
            // 单体测试只取有完成时间的数据
            var wipProductList = woIds.SplitContains(tempIds =>
            {
                return DB.Query<WipProductVersionReport>()
                .Where(p => woIds.Contains(p.WorkOrderId) && p.IsFinish && p.FinishDateTime >= queryTime && p.FinishDateTime <= queryTime.AddDays(1)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            // 批次取采集记录最新出站记录
            var batchWipProductList = woIds.SplitContains(tempIds =>
            {
                return DB.Query<BatchWipProductVersion>()
                .Where(p => woIds.Contains(p.WorkOrderId) && p.IsFinish).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            var batchWipProductIds = batchWipProductList.Select(p => p.Id).ToList();
            var batchWipProcessList = batchWipProductIds.SplitContains(tempIds =>
            {
                return DB.Query<BatchWipProductProcess>().Where(p => tempIds.Contains(p.VersionId)).ToList();
            });
            // 采集记录最新出站记录
            var batchWipRecord = batchWipProcessList.GroupBy(p => p.VersionId).Select(p => new { Qty = p.OrderByDescending(x => x.OutputDate).FirstOrDefault()?.OutputQty, OutputTime = p.OrderByDescending(x => x.OutputDate).FirstOrDefault()?.OutputDate }).ToList();


            // 初始化目标产出和实际产出
            for (int i = 0; i < timeRangeValue.Count; i++)
            {
                targetProduct.Add(0);
                actualProduct.Add(0);
                efficiency.Add(0);
            }
            // 下标
            int index = 0;
            timeRangeValue.ForEach(item =>
            {
                var beginHour = int.Parse(item.Key.Split(new char[] { ':', '-' })[0]);
                var endHour = int.Parse(item.Key.Split(new char[] { ':', '-' })[2]);
                DateTime beginTime = beginHour >= 24 ? new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, beginHour % 24, 0, 0, DateTimeKind.Utc).AddDays(1) : new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, beginHour, 0, 0, DateTimeKind.Utc);
                DateTime endTime = endHour >= 24 ? new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, endHour % 24, 0, 0, DateTimeKind.Utc).AddDays(1) : new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, endHour, 0, 0, DateTimeKind.Utc);
                // 单体
                var timewipProductCount = wipProductList.Count(p => p.FinishDateTime < endTime && p.FinishDateTime >= beginTime);
                // 批次
                var timebatchWipProductCount = batchWipRecord.Where(p => p.OutputTime <=  endTime && p.OutputTime >= beginTime).Sum(p => p.Qty);
                var timeworkOrder = workOrderList.Where(p => p.PlanBeginDate <= beginTime && p.PlanEndDate >= endTime).ToList();
                timeworkOrder.ForEach(wo =>
                {
                    var product = productList.FirstOrDefault(p => p.Id == wo.ProductId);
                    var meter = meterList.FirstOrDefault(p => p.ProductModelId == product.ModelId);
                    targetProduct[index] += meter != null ? (decimal)meter.WorkingHours : (decimal)proModelList.FirstOrDefault(p => p.Id == product.ModelId)?.WorkingHours;
                });
                actualProduct[index] = timewipProductCount + timebatchWipProductCount??0;
                efficiency[index] = targetProduct[index] != 0 ? Math.Round(actualProduct[index] / targetProduct[index] * 100, 0) : 0;
                index++;
            });
        }

        /// <summary>
        /// 计算当前产线班次的时间段
        /// </summary>
        /// <param name="wipId"></param>
        /// <param name="timeRangeIn24"></param>
        /// <returns></returns>
        private List<string> GetTimeRange(double? wipId, List<string> timeRangeIn24)
        {
            var timeNow = DateTime.Now;
            var wipResourceMove = RF.GetById<WipResourceMove>(wipId);
            double? shiftTypeId = null;
            EntityList<Shift> shiftList = null;
            List<string> timeRange = new List<string>();
            if (wipResourceMove != null)
            {
                shiftTypeId = RT.Service.Resolve<WipResourceController>().GetWipResourceShiftTypeId(wipResourceMove, timeNow);
                shiftList = RT.Service.Resolve<ShiftTypeController>().GetShifts(shiftTypeId ?? 0);
            }
            if (shiftList.Any())
            {
                shiftList.ForEach(shift =>
                {
                    var beginHour = shift.BeginTime.Hour;
                    var endHour = shift.EndTime.Hour;
                    endHour += shift.IsOverDay ? 24 : 0;
                    for (var i = beginHour; i < endHour; i++)
                    {
                        var time = "{0}:00-{1}:00".FormatArgs(i, i + 1);
                        var timeIn24 = "{0}:00-{1}:00".FormatArgs(i % 24, (i + 1) % 24 == 0 ? 24 : (i + 1) % 24);
                        timeRange.Add(time);
                        timeRangeIn24.Add(timeIn24);
                    }
                });
            }
            return timeRange;
        }

        /// <summary>
        /// 获取缺陷TOP5柏拉图
        /// </summary>
        /// <param name="woIds">产线Id</param>
        /// <returns></returns>
        public virtual DefectPlato GetDefectPlato(List<double> woIds)
        {
            if (woIds == null || woIds.Count <= 0)
            {
                return new DefectPlato();
            }
            // 柏拉图
            DefectPlato defectPlato = new DefectPlato();
            // 缺陷统计
            List<DefectCount> defectCountList = new List<DefectCount>();
            List<decimal> plato = new List<decimal>();
            // 缺陷统计字典(批次非批次)
            Dictionary<double?, decimal> defectCountDic = new Dictionary<double?, decimal>();
            // 缺陷ids
            List<double?> defectIds = new List<double?>();
            // 生产通用报表
            var wipProductDefectGroup = woIds.SplitContains((woId) =>
            {
                // 防止50000条数据溢出报错
                var wipProductDefectGroupCount = DB.Query<WipProductDefect>().Where(p => woId.Contains(p.Version.WorkOrderId) && p.DefectId != null && p.DefectId != 0).Count();
                PagingInfo pagingInfo = new PagingInfo
                {
                    PageSize = wipProductDefectGroupCount,
                    PageNumber = 1,
                };
                return DB.Query<WipProductDefect>().Where(p => woId.Contains(p.Version.WorkOrderId) && p.DefectId != null && p.DefectId != 0).ToList(pagingInfo);
            }).GroupBy(p => p.DefectId).Select(p => new { defectId = p.Key, count = p.Count() }).ToDictionary(p => p.defectId, p => p.count);
            if (wipProductDefectGroup.Any())
            {
                wipProductDefectGroup.ForEach(defect =>
                {
                    if (defectCountDic.ContainsKey(defect.Key))
                    {
                        defectCountDic[defect.Key] += defect.Value;
                    }
                    else
                    {
                        defectCountDic.Add(defect.Key, defect.Value);
                        defectIds.Add(defect.Key);
                    }
                });
            }
            // 批次生产通用报表
            var bwipProductDefectGroup = woIds.SplitContains((woId) =>
            {
                return DB.Query<BatchWipProductDefectDetail>().Where(p => woId.Contains(p.BatchWipProductDefect.Version.WorkOrderId)).ToList();
            }).GroupBy(p => p.DefectId).Select(p => new { defectId = p.Key, count = p.Count() }).ToDictionary(p => p.defectId, p => p.count);
            if (bwipProductDefectGroup.Any())
            {
                bwipProductDefectGroup.ForEach(defect =>
                {
                    if (defectCountDic.ContainsKey(defect.Key))
                    {
                        defectCountDic[defect.Key] += defect.Value;
                    }
                    else
                    {
                        defectCountDic.Add(defect.Key, defect.Value);
                        defectIds.Add(defect.Key);
                    }
                });
            }

            //查询所有的缺陷
            var defectList = defectIds.SplitContains(tempIds =>
            {
                return DB.Query<Defect>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            defectCountDic = defectCountDic.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
            // 总缺陷数量
            var totalCount = defectCountDic.Sum(p => p.Value);
            // 柏拉折线累计数
            decimal platoPercent = 0;
            if (!defectCountDic.Any())
            {
                return new DefectPlato();
            }
            else if (defectCountDic.Count == 1)
            {
                var first = defectCountDic.First();
                defectCountList.Add(new DefectCount { DefectName = defectList.FirstOrDefault(p => p.Id == first.Key)?.Description, Count = first.Value });
                defectPlato.DefectCountList = defectCountList;
                defectPlato.Plato = new List<decimal> { 100 };
                return defectPlato;
            }
            else
            {
                // 前五
                //var fifthdic = defectCountDic.Count > 5 ? defectCountDic.Take(5) : defectCountDic.Take(defectCountDic.Count - 1);
                if (defectCountDic.Count > 5)
                {
                    var fifthdic = defectCountDic.Take(5);
                    fifthdic.ForEach(dic =>
                    {
                        DefectCount defectCount = new DefectCount
                        {
                            DefectName = defectList.FirstOrDefault(p => p.Id == dic.Key)?.Description,
                            Count = dic.Value,
                        };
                        if (defectCount.DefectName.IsNotEmpty())
                        {
                            platoPercent += dic.Value;
                            defectCountList.Add(defectCount);
                            plato.Add(Math.Round(platoPercent / totalCount * 100, 0));
                        }
                    });
                    defectCountList.Add(new DefectCount { DefectName = "其他", Count = totalCount - platoPercent });
                    plato.Add(100);
                }
                else
                {
                    defectCountDic.ForEach(dic =>
                    {
                        DefectCount defectCount = new DefectCount
                        {
                            DefectName = defectList.FirstOrDefault(p => p.Id == dic.Key)?.Description,
                            Count = dic.Value,
                        };
                        if (defectCount.DefectName.IsNotEmpty())
                        {
                            platoPercent += dic.Value;
                            defectCountList.Add(defectCount);
                            plato.Add(Math.Round(platoPercent / totalCount * 100, 0));
                        }
                    });
                }
                defectPlato.DefectCountList = defectCountList;
                defectPlato.Plato = plato;
                return defectPlato;
            }
        }
    }
}
