using SIE.Common;
using SIE.Domain;
using SIE.MES.Capacitys;
using SIE.MES.DashBoard.KzBoard.Datas;
using SIE.MES.ProductAgingProcesss;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.HeatTreatments;
using SIE.MES.TaskManagement.HeatTreatments.Datas;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.MES.DashBoard.KzBoard
{
    /// <summary>
    /// 看板公共类
    /// </summary>
    public class KzBoardCommon
    {

        /// <summary>
        /// 生成老化看板数据
        /// </summary>
        /// <param name="reportRecordExamines"></param>
        /// <param name="heatTreatmentList"></param>
        /// <param name="workOrders"></param>
        /// <returns></returns>
        public List<ProductAgingData> GenerateProductAgingData(EntityList<ReportRecordExamine> reportRecordExamines,
            EntityList<HeatTreatment> heatTreatmentList, EntityList<WorkOrder> workOrders, EntityList<ReportWipBatch> reportWipBatchList)
        {
            List<ProductAgingData> data = new List<ProductAgingData>();
            var productIds = workOrders.Select(p => p.ProductId).Distinct().ToList();
            //产品老化工艺时间
            var agingProcesses = RT.Service.Resolve<ProductAgingProcessController>().GetProductAgingProcessesByProductId(productIds);
            //挤塑报工记录
            var extrusions = reportRecordExamines.Where(p => p.ProcessCode == "挤塑").AsEntityList();
            var extrusionsIds = extrusions.Select(p => p.Id).ToList();
            //获取待老化产品数量
            var waitAgings = GetWaitAgings(extrusions, heatTreatmentList, reportWipBatchList);
            //老化中数量
            var agings = GetAgings(extrusions, heatTreatmentList, reportWipBatchList);
            //可用数量
            var available = GetAvailable(reportRecordExamines);
            //入炉数据
            var heats = GetHeatTreatments(workOrders);
            var productCodes = workOrders.Select(p => p.ProductCode).Distinct().ToList();
            foreach (var itemProductCode in productCodes)
            {
                var workOrder = workOrders.Where(p => p.ProductCode == itemProductCode).FirstOrDefault();
                var entity = new ProductAgingData();
                entity.ProductCode = itemProductCode;
                entity.ProductModel = workOrder?.ProductShortDescription ?? "";
                entity.WaitAging = waitAgings.Where(p => p.ProductId == workOrder.ProductId).FirstOrDefault()?.Qty ?? 0;
                entity.Aging = agings.Where(p => p.ProductId == workOrder.ProductId).FirstOrDefault()?.Qty ?? 0;
                entity.Available = available.Where(p => p.ProductId == workOrder.ProductId).FirstOrDefault()?.Qty ?? 0;
                entity.CurrentInProcessNum = entity.Aging + entity.Available;
                var entityList = heats.Where(p => p.MaterialCode == itemProductCode).ToList();
                List<ProductAgingDtl> dtl = new List<ProductAgingDtl>();
                var agingProcesse = agingProcesses.Where(p => p.ItemCode == itemProductCode).FirstOrDefault();

                entity.Data = new List<ProductAgingDtl>();
                //计算明细数量汇总
                var mergedList = GetProductAgingDtlList(entityList, agingProcesse);
                entity.Data.AddRange(mergedList);
                data.Add(entity);
            }
            //计算最小/最大在制数
            return GetPlatingProcess(workOrders, data, agingProcesses);
        }

        /// <summary>
        /// 计算明细数量汇总
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="agingProcesse"></param>
        /// <returns></returns>
        private List<ProductAgingDtl> GetProductAgingDtlList(List<KzHeatTreatmentInfo> entityList, ProductAgingProcess agingProcesse)
        {
            List<ProductAgingDtl> dtl = new List<ProductAgingDtl>();
            foreach (var heat in entityList)
            {
                var capacity = agingProcesse?.Capacity ?? 0;
                var time = heat.CreateDate.AddHours(Convert.ToDouble(capacity));
                time = time.Minute == 0 && time.Second == 0 ? time.AddHours(1) : time;//取整时间
                dtl.Add(new ProductAgingDtl()
                {
                    AgingNum = heat.DevName,
                    InFurnaceNum = heat.Count00 ?? 0,
                    AgingOutTime = time.ToString("yyyy-MM-dd HH"),
                });
            }
            var mergedList = dtl
                .GroupBy(item => new { item.AgingNum, item.AgingOutTime })
                .Select(group => new ProductAgingDtl
                {
                    AgingNum = group.Key.AgingNum,
                    AgingOutTime = group.Key.AgingOutTime,
                    InFurnaceNum = group.Sum(item => item.InFurnaceNum)
                })
                .ToList();
            return mergedList;
        }

        /// <summary>
        /// 计算最小/最大在制数
        /// </summary>
        /// <param name="workOrders"></param>
        /// <param name="productAgingDatas"></param>
        /// <param name="agingProcesses"></param>
        /// <returns></returns>
        private List<ProductAgingData> GetPlatingProcess(EntityList<WorkOrder> workOrders, List<ProductAgingData> productAgingDatas, EntityList<ProductAgingProcess> agingProcesses)
        {
            var workOrderIds = workOrders.Select(p => p.Id).ToList();
            var platingWorkOrderIds = workOrderIds.SplitContains(ids =>
            {
                return DB.Query<LayoutInfo>().Where(p => ids.Contains(p.WorkOrderId) && p.ProcessCode == "电镀").Select(p => p.WorkOrderId).ToList();
            }); 
            List<string> productCodes = new List<string>();//电镀产品
            foreach (var item in workOrders.OrderByDescending(p => p.CreateDate))
            {
                if (platingWorkOrderIds.Any(p => p.WorkOrderId == item.Id) && !productCodes.Any(p => p == item.ProductCode))
                    productCodes.Add(item.ProductCode);
            }
            var productIds = workOrders.Select(p => p.ProductId).Distinct().ToList();
            var mrbs = workOrders.Select(p => p.WorkShopCode).Distinct().ToList();
            var schedulingInfValues = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfValue(productIds, mrbs);
            //标准产能
            var standardCapacitys = RT.Service.Resolve<StandardCapacityController>().GetStandardCapacityByItemIds(productIds);
            foreach (var item in productAgingDatas)
            {
                var schedulingInLists = schedulingInfValues.Where(p => p.SchedulingInf.Item.Code == item.ProductCode)
                    .OrderBy(p => p.Date).ToList().GroupBy(p=>p.Date).ToDictionary(p=>p.Key,p=>p.ToList());

                var agingProcesse = agingProcesses.Where(p => p.ItemCode == item.ProductCode).FirstOrDefault();
                var standardCapacity = standardCapacitys.Where(p => p.ItemCode == item.ProductCode).FirstOrDefault();
                var standardNum = (standardCapacity?.Capacity ?? 0) * ((agingProcesse?.Capacity ?? 0) + 2);
                var dispatchQty = 0M;
                Dictionary<int, decimal> dic = new Dictionary<int, decimal>();
                int i = 1;
                foreach (var schedulingIn in schedulingInLists)
                {
                    var value1 = 0M;
                    var value2 = 0M;
                    foreach (var scheduling in schedulingIn.Value)
                    {
                        value1 += scheduling.Value1 ?? 0;
                        value2 += scheduling.Value2 ?? 0;
                    }
                    dic.Add(i++, value1);
                    dic.Add(i++, value2);
                }
                int inext = DateTime.Now.Hour<8?6:8;
                if (productCodes.Any(p => p == item.ProductCode))//有电镀
                {
                    dispatchQty = dic.Where(p => p.Key > 1 && p.Key < inext-2).Sum(p => p.Value);
                    item.MaxInProcessNum = dic.Where(p => p.Key > 1 && p.Key < inext).Sum(p => p.Value) + standardNum;
                }
                else//无电镀
                {
                    dispatchQty = dic.Where(p => p.Key > 1 && p.Key < inext-4).Sum(p => p.Value);
                    item.MaxInProcessNum = dic.Where(p => p.Key > 1 && p.Key < inext-2).Sum(p => p.Value) + standardNum;
                }
                item.MinInProcessNum = dispatchQty + standardNum;
            }
            return productAgingDatas;
        }

        /// <summary>
        /// 获取产品的入炉数据
        /// </summary>
        /// <param name="workOrders"></param>
        /// <returns></returns>
        private List<KzHeatTreatmentInfo> GetHeatTreatments(EntityList<WorkOrder> workOrders)
        {
            var productCodes = workOrders.Select(p => p.ProductCode).Distinct().ToList();
            var heatTreatments = RT.Service.Resolve<HeatTreatmentController>().GetHeatTreatmentList(productCodes, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(1));
            List<KzHeatTreatmentInfo> heats = new List<KzHeatTreatmentInfo>();
            var heatTreatmentGroup = heatTreatments.GroupBy(p => p.Barcode).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var item in heatTreatmentGroup)
            {
                //只要有入炉，但是不要有出炉
                if (item.Value.Any(p => p.OperationType == OperationType.In) && !item.Value.Any(p => p.OperationType == OperationType.Out))
                    heats.Add(item.Value.OrderBy(p => p.CreateDate).FirstOrDefault());
            }
            return heats;
        }

        /// <summary>
        /// 获取待老化产品数量
        /// </summary>
        /// <param name="reportRecordExamines"></param>
        /// <param name="heatTreatmentList"></param>
        /// <returns></returns>
        private List<DicListData> GetWaitAgings(EntityList<ReportRecordExamine> reportRecordExamines, EntityList<HeatTreatment> heatTreatmentList, EntityList<ReportWipBatch> wipBatches)
        {
            List<DicListData> waitAgings = new List<DicListData>();
            //剔除全部都是不合格的
            foreach (var item in reportRecordExamines.Where(p=>p.ReportQty > p.NgQty))
            {
                var lableLists = wipBatches.Where(p => p.ReportRecordId == item.Id).Select(p => p.BatchNo).ToList();
                foreach (var lable in lableLists)
                {
                    //不能存在出入炉类型的，但是允许存在为空的
                    if (heatTreatmentList.Any(p => p.Barcode == lable && (p.OperationType == OperationType.In || p.OperationType == OperationType.Out)))
                    {
                        waitAgings.Add(new DicListData()
                        {
                            ProductId = item.ProductId,
                            Qty = 0
                        });
                    }
                    else
                    {
                        var wipBatche = wipBatches.Where(p => p.BatchNo == lable).FirstOrDefault();
                        waitAgings.Add(new DicListData()
                        {
                            ProductId = item.ProductId,
                            Qty = wipBatche?.Qty ?? 0
                        });
                    }
                }

                //if (heatTreatmentList.Any(p => lableLists.Contains(p.Barcode)))
                //{
                //    foreach (var lable in lableLists)
                //    {
                //        if (heatTreatmentList.Any(p => p.Barcode == lable))
                //        {
                //            waitAgings.Add(new DicListData()
                //            {
                //                ProductId = item.ProductId,
                //                Qty = 0
                //            });
                //        }
                //        else
                //        {
                //            var wipBatche = wipBatches.Where(p => p.BatchNo == lable).FirstOrDefault();
                //            waitAgings.Add(new DicListData()
                //            {
                //                ProductId = item.ProductId,
                //                Qty = wipBatche?.Qty ?? 0
                //            });
                //        }
                //    }
                //}
                //else
                //{
                //    //var qty = wipBatches.Where(p => lableLists.Contains(p.BatchNo)).Sum(p => p.Qty);
                //    //waitAgings.Add(new DicListData()
                //    //{
                //    //    ProductId = item.ProductId,
                //    //    Qty = qty
                //    //});
                //}
            }
            return waitAgings.GroupBy(p => p.ProductId).Select(p => new DicListData() { ProductId = p.Key, Qty = p.Sum(q => q.Qty) }).ToList();
        }

        /// <summary>
        /// 获取老化中产品数量
        /// </summary>
        /// <param name="reportRecordExamines"></param>
        /// <param name="heatTreatmentList"></param>
        /// <param name="wipBatches"></param>
        /// <returns></returns>
        private List<DicListData> GetAgings(EntityList<ReportRecordExamine> reportRecordExamines, EntityList<HeatTreatment> heatTreatmentList, EntityList<ReportWipBatch> wipBatches)
        {
            List<DicListData> waitAgings = new List<DicListData>();
            foreach (var item in reportRecordExamines)
            {
                var lableLists = wipBatches.Where(p => p.ReportRecordId == item.Id).Select(p => p.BatchNo).ToList();
                if (heatTreatmentList.Any(p => lableLists.Contains(p.Barcode)))
                {
                    foreach (var lable in lableLists)
                    {
                        var heatTreatment = heatTreatmentList.Where(p => p.Barcode == lable).ToList();
                        if (heatTreatment.Count == 0)
                            continue;
                        //if (heatTreatment.Count > 1)//已出炉
                        //不存在入炉的都不算进去
                        if (heatTreatment.Any(p => p.OperationType == OperationType.Out) || heatTreatment.All(p => p.OperationType != OperationType.In))
                        {
                            waitAgings.Add(new DicListData()
                            {
                                ProductId = item.ProductId,
                                Qty = 0
                            });
                        }
                        //else if (heatTreatment.Count == 1)//入炉未出
                        //存在入炉，但是不存在出炉
                        else if(heatTreatment.Any(p=>p.OperationType == OperationType.In) && heatTreatment.All(p => p.OperationType != OperationType.Out))
                        {
                            var wipBatche = wipBatches.Where(p => p.BatchNo == lable).FirstOrDefault();
                            waitAgings.Add(new DicListData()
                            {
                                ProductId = item.ProductId,
                                Qty = wipBatche?.Qty ?? 0
                            });
                        }
                    }
                }
                else//未老化
                {
                    waitAgings.Add(new DicListData()
                    {
                        ProductId = item.ProductId,
                        Qty = 0
                    });
                }
            }
            return waitAgings.GroupBy(p => p.ProductId).Select(p => new DicListData() { ProductId = p.Key, Qty = p.Sum(q => q.Qty) }).ToList();
        }

        /// <summary>
        /// 可用数量
        /// </summary>
        /// <returns></returns>
        private List<DicListData> GetAvailable(EntityList<ReportRecordExamine> reportRecordExamines)
        {
            List<DicListData> waitAgingheats = new List<DicListData>();
            List<DicListData> waitAgingprecisions = new List<DicListData>();
            var heats = reportRecordExamines.Where(p => p.ProcessCode == "热处理").ToList();
            foreach (var item in heats)
            {
                waitAgingheats.Add(new DicListData()
                {
                    ProductId = item.ProductId,
                    Qty = item.RecordOkQty
                });
            }
            var precisions = reportRecordExamines.Where(p => p.ProcessCode == "精加工").ToList();
            foreach (var item in precisions)
            {
                waitAgingprecisions.Add(new DicListData()
                {
                    ProductId = item.ProductId,
                    Qty = item.ReportQty + item.SuspectQty
                });
            }

            var numheats = waitAgingheats.GroupBy(p => p.ProductId).Select(p => new DicListData() { ProductId = p.Key, Qty = p.Sum(q => q.Qty) }).ToList();
            var numprecisions = waitAgingprecisions.GroupBy(p => p.ProductId).Select(p => new DicListData() { ProductId = p.Key, Qty = p.Sum(q => q.Qty) }).ToList();
            foreach (var item in numheats)
            {
                var precision = numprecisions.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                item.Qty = item.Qty - (precision?.Qty??0);
            }
            return numheats;
        }

    }
}
