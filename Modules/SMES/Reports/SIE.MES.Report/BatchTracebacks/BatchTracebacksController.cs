using IronPython.Compiler.Ast;

using Microsoft.Scripting.Interpreter;

using NPOI.POIFS.Properties;

using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Domain;
using SIE.EventMessages;
using SIE.MES.PackingQC;
using SIE.MES.Report.BatchWipProducts;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 控制器
    /// </summary>
    public partial class BatchTracebacksController : DomainController
    {

        #region 新增逻辑 - 2026-6-5号
        /// <summary>
        /// 获取批次追溯关键件明细列表（根据批次号和报工记录计算耗用量分配）
        /// </summary>
        /// <param name="batchNo">标签号</param>
        /// <param name="reportRecord">报工记录</param>
        /// <param name="Qty">数量</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns>关键件记录列表</returns>
        public virtual EntityList<KeyComponentPartDtl> GetBatchTracebackKeyDtlViewModelList(string batchNo, ReportRecord reportRecord, decimal Qty,
            PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            // 先查询 KeyComponentPartDtl 表，如果该 batchNo 已存在数据则直接返回
            var existingData = Query<KeyComponentPartDtl>()
                .Where(x => x.BacthNo == batchNo)
                .ToList();

            if (existingData.Any())
            {
                // 数据已存在，直接返回
                return existingData;
            }

            // 结果集合
            var resultList = new EntityList<KeyComponentPartDtl>();

            var reportTime = new DateRange() { DateRangeType = DateRangeType.All };

            ReportRecordExamineCriteria examineCriteria = new ReportRecordExamineCriteria() { WipBatchNos = batchNo, ProcessId = reportRecord.ProcessId, ReportTime = reportTime };
            // 获取报工记录的数据
            var reportRecordExamineList = RT.Service.Resolve<ReportController>().QueryReportRecordExamine(examineCriteria);
            var wipBatchNoList = new List<string>();
            var workOrderNo = string.Empty;
            if (reportRecordExamineList.Any())
            {
                var reportRecordExamine = reportRecordExamineList.FirstOrDefault();
                workOrderNo = reportRecordExamine.Wo;
                wipBatchNoList = SplitLabelNumber(reportRecordExamine.WipBatchNos);
            }

            var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackKeyDtlsByReportRecordId(reportRecord.Id, pagingInfo, sortInfo);
            if (list.Any())
            {
                var itemCodeList = list.Select(x => x.ItemCode).Distinct().ToList();
                var workOrderProcessBomDic = Query<WorkOrderProcessBom>()
                    .Where(x => x.WorkOrder.No == workOrderNo && itemCodeList.Contains(x.Item.Code))
                    .ToList()
                    .ToDictionary(x => x.Item.Code, x => x.Weight);

                // 通过物料编码分组，先计算出每一个标签对应的耗用量 = 等于每一组物料的扣料数量之合除了标签数
                var listGroup = list.GroupBy(x => x.ItemCode);

                // 记录每个批次的使用情况：Key=ItemLabelLot, Value=已使用量
                // 用于下一个标签号判断该批次是否还有剩余
                var labelBatchUsageDic = new Dictionary<string, decimal>();

                foreach (var wipBatchNo in wipBatchNoList)
                {
                    foreach (var group in listGroup)
                    {
                        // 从字典获取权重（安全取值，避免空值报错）
                        if (!workOrderProcessBomDic.TryGetValue(group.Key, out var weight) || !weight.HasValue)
                        {
                            continue;
                        }

                        // 耗用量 - 这个物料要消耗的总量
                        var consumptionQty = weight.Value * Qty;

                        // ✅ 修复：按照标签批次分组，并按批次号排序（先进先出）
                        var labelBatchGroup = group.GroupBy(x => x.ItemLabelLot).OrderBy(x => x.Key).ToList();
                        decimal sumQty = 0; // 已累积的扣料总量

                        // 遍历每个标签批次，分配耗用量
                        foreach (var labelBatch in labelBatchGroup)
                        {
                            var itemLabelLot = labelBatch.Key;

                            // ✅ 修复：批次内按 SourceSn 排序，逐条顺序使用（而非按比例分配）
                            var sortedItems = labelBatch.OrderBy(x => x.SourceSn).ToList();
                            var currentBatchDeductedQty = sortedItems.Sum(x => x.DeductedQty);

                            // 检查该批次是否还有剩余量
                            decimal usedQty = labelBatchUsageDic.ContainsKey(itemLabelLot) ? labelBatchUsageDic[itemLabelLot] : 0;
                            decimal remainingQty = currentBatchDeductedQty - usedQty;

                            // 如果该批次已用完，跳过
                            if (remainingQty <= 0)
                            {
                                continue;
                            }

                            // ✅ 修复：批次内逐条记录顺序使用，用完一条再用下一条
                            foreach (var item in sortedItems)
                            {
                                var itemSourceSn = item.SourceSn;
                                var itemDeductedQty = item.DeductedQty;
                                var itemKey = $"{itemLabelLot}_{itemSourceSn}";

                                // 检查这条记录是否有剩余
                                decimal itemUsedQty = labelBatchUsageDic.ContainsKey(itemKey) ? labelBatchUsageDic[itemKey] : 0;
                                decimal itemRemainingQty = itemDeductedQty - itemUsedQty;

                                if (itemRemainingQty <= 0)
                                {
                                    continue; // 这条记录已用完，用下一条
                                }

                                // 当前还需要多少
                                var needQty = consumptionQty - sumQty;

                                if (needQty <= 0)
                                {
                                    break; // 已满足需求，跳出批次内循环
                                }

                                // 这条记录能提供的量（取剩余量和需求量的较小值）
                                var canProvideQty = Math.Min(itemRemainingQty, needQty);

                                if (canProvideQty > 0)
                                {
                                    // 创建关键件记录
                                    KeyComponentPartDtl keyComponent = new KeyComponentPartDtl()
                                    {
                                        BacthNo = wipBatchNo,
                                        ProcessCode = item.ProcessCode,
                                        SourceSn = item.SourceSn,
                                        ItemLabelLot = item.ItemLabelLot,
                                        DeductedQty = Math.Round(canProvideQty, 8), // 实际使用量
                                        ItemCode = item.ItemCode,
                                        ItemName = item.ItemName,
                                        ShortDescription = item.ShortDescription,
                                        Unit = item.Unit,
                                        BacthQty = consumptionQty
                                    };
                                    resultList.Add(keyComponent);

                                    // 更新累计使用量
                                    sumQty += canProvideQty;

                                    // 记录批次使用量
                                    if (!labelBatchUsageDic.ContainsKey(itemLabelLot))
                                    {
                                        labelBatchUsageDic[itemLabelLot] = 0;
                                    }
                                    labelBatchUsageDic[itemLabelLot] += canProvideQty;

                                    // 记录记录级别使用量
                                    labelBatchUsageDic[itemKey] = itemUsedQty + canProvideQty;
                                }

                                // 如果已满足需求，跳出批次内循环
                                if (sumQty >= consumptionQty)
                                {
                                    break;
                                }
                            }

                            // 如果已满足需求，跳出批次循环
                            if (sumQty >= consumptionQty)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            // 保存新增的数据到数据库
            if (resultList.Any())
            {
                RF.Save(resultList);
            }

            return resultList.Where(x => x.BacthNo == batchNo).AsEntityList();
        }

        /// <summary>
        /// 拆分标签号
        /// </summary>
        /// <param name="wipBatchNoList"></param>
        /// <returns></returns>
        private List<string> SplitLabelNumber(string wipBatchNoList)
        {
            var result = new List<string>();
            // 每组内按逗号分割
            var labels = wipBatchNoList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var label in labels)
            {
                var trimmed = label.Trim();
                if (trimmed.IsNotEmpty() && !result.Contains(trimmed)) // 检查是否已存在
                    result.Add(trimmed);
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 根据标签号和工序编码获取开机准备记录
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="processCode"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<BatchTracebackPreSetup> GetBatchTracebackPreSetups(string batchNo, string processCode, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var list = Query<BatchTracebackPreSetup>()
                .Join<ReportRecord>((x, y) => x.DispatchTaskId == y.DispatchTaskId)
                .Join<ReportRecord, Process>((x, y) => x.ProcessId == y.Id && y.Code == processCode)
                .Join<ReportRecord, ReportWipBatch>((x, y) => x.Id == y.ReportRecordId && y.BatchNo == batchNo)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;

        }

        /// <summary>
        /// 根据可疑品标签Id获取缺陷代码
        /// </summary>
        /// <param name="suspectProductLabelId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<BatchTracebackDefetctDtl> GetBatchTracebackDefetctDtlsById(double suspectProductLabelId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var list = Query<BatchTracebackDefetctDtl>().Where(p => p.SuspectProductLabelId == suspectProductLabelId).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据id获取产品缺陷记录明细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<BatchTracebackDefetctLabelDtl> GetBatchTracebackDefetctLabelDtlsById(string batchNo, string ProcessCode, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var list = Query<BatchTracebackDefetctLabelDtl>().Where(p => p.BatchNo == batchNo && p.Process.Code == ProcessCode).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;

        }

        /// <summary>
        /// 根据报工记录id获取产品生产关键件明细
        /// </summary>
        /// <param name="reportRecordId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<BatchTracebackKeyDtl> GetBatchTracebackKeyDtlsByReportRecordId(double reportRecordId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var list = Query<BatchTracebackKeyDtl>().Where(p => p.ReportRecordId == reportRecordId).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据id获取批次采集记录明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityList<BatchTracebackReportDtl> GetBatchTracebackReportDtlsByIds(double id, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var list = Query<BatchTracebackReportDtl>().Where(p => p.Id == id).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<BatchTracebackReport> CriteriaBatchTracebackReports(BatchTracebackReportCriteria criteria)
        {
            var q = DB.Query<BatchTracebackReport>("btr")
                .Join<ReportRecord>("rr", (x, y) => x.ReportRecordId == y.Id)
                .Join<ReportRecord, Process>("p", (x, y) => x.ProcessId == y.Id);

            // 处理单体条码：通过装箱明细表查询对应的标签号 - 2026-5-30 - yjb
            string unitBarcodeBatchNo = null;
            if (criteria.UnitBarcode.IsNotEmpty())
            {
                // 从装箱明细表查询：根据单体条码(ProductLabel)获取对应的标签号(BatchLabel)
                var packingDetail = Query<PackingDetail>()
                    .Where(p => p.ProductLabel == criteria.UnitBarcode)
                    .FirstOrDefault();
                unitBarcodeBatchNo = packingDetail?.BatchLabel;
                criteria.BatchNo = unitBarcodeBatchNo.IsNullOrEmpty() ? "无数据" : unitBarcodeBatchNo;
            }

            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (!criteria.ItemLabelLot.IsNullOrEmpty())
            {
                q.Exists<BatchTracebackKeyDtl>((x, y) => y.Join<BatchTracebackReportDtl>((x1, y1) => x1.ReportRecordId == y1.ReportRecordId && y1.Id == x.Id).Where(p => p.ItemLabelLot.Contains(criteria.ItemLabelLot)));
            }
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.Product.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.WorkShopName.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.Fevor.Contains(criteria.WorkShopName));
            if (criteria.ProcessId > 0)
                q.Where(p => p.ReportRecord.ProcessId == criteria.ProcessId);
            if (criteria.NextProcessId > 0)
                q.Where(p => p.NextProcessId == criteria.NextProcessId);
            if (criteria.BatchType == BatchType.Rework)
            {
                q.Join<WipBatch>((x, y) => x.BatchNo == y.BatchNo && (y.IsSuspectProduct == YesNo.No || y.IsSuspectProduct == null) && y.IsRework == true);
            }
            if (criteria.BatchType == BatchType.Scraped)
            {
                q.Join<WipBatch>((x, y) => x.BatchNo == y.BatchNo && (y.IsSuspectProduct == YesNo.No || y.IsSuspectProduct == null) && y.IsScraped == true);
            }
            if (criteria.BatchType == BatchType.Good)
            {
                q.Join<WipBatch>((x, y) => x.BatchNo == y.BatchNo && (y.IsSuspectProduct == YesNo.No || y.IsSuspectProduct == null) && y.IsScraped == false && y.IsRework == false);
            }
            if (criteria.BatchType == BatchType.Suspect)
            {
                q.Join<WipBatch>((x, y) => x.BatchNo == y.BatchNo && y.IsSuspectProduct == YesNo.Yes);
            }
            if (criteria.IsFinish == YesNo.Yes)
            {
                q.Where(p => p.NextProcessId == null || p.NextProcessId == 0);
            }
            if (criteria.IsFinish == YesNo.No)
            {
                q.Where(p => p.NextProcessId > 0);
            }
            if (!criteria.BlueLabel.IsNullOrWhiteSpace())
            {
                q.Where(p => p.BlueLabel.Contains(criteria.BlueLabel));
            }
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            if (!criteria.ToolCode.IsNullOrEmpty())
            {
                var query = " 1 = 1";
                if (criteria.ToolCode.Contains("%"))
                    query = $" like '{criteria.ToolCode}'";
                else
                    query = $" = '{criteria.ToolCode}'";
                //直接将查找开机准备记录的SQL拿过来
                q.Where(p => p.SQL<bool>($@"exists( SELECT 1
FROM (SELECT PSSR.ID ID, PSSR.TOOL_CODE TOOL_CODE, PSSR.DISPATCH_TASK_ID DISPATCH_TASK_ID, PSSR.TOOL_NAME TOOL_NAME, PSSR.TOOL_STATE TOOL_STATE, PSSR.DRAWING_NO DRAWING_NO, PSSR.UNIQUE_CODE UNIQUE_CODE, PSSR.CHECKER_FIXTURE_TYPE CHECKER_FIXTURE_TYPE
FROM PRE_START_SETUP_REC PSSR
WHERE pssr.IS_PHANTOM = 0 AND pssr.INV_ORG_ID = rr.INV_ORG_ID AND PSSR.INV_ORG_ID = rr.INV_ORG_ID AND PSSR.IS_PHANTOM = '0') TT0
    INNER JOIN TM_REPORT_RECORD TT1 ON TT0.DISPATCH_TASK_ID = TT1.DISPATCH_TASK_ID AND TT1.INV_ORG_ID = rr.INV_ORG_ID AND TT1.IS_PHANTOM = '0'
    INNER JOIN TECH_PROCESS TT2 ON TT1.PROCESS_ID = TT2.ID AND TT2.CODE = p.code AND TT2.INV_ORG_ID = rr.INV_ORG_ID AND TT2.IS_PHANTOM = '0'
    INNER JOIN TM_REPORT_WIP_BATCH TT3 ON TT1.ID = TT3.REPORT_RECORD_ID AND TT3.BATCH_NO = btr.Batch_no AND TT3.INV_ORG_ID = rr.INV_ORG_ID AND TT3.IS_PHANTOM = '0'
    LEFT OUTER JOIN TM_DISP_TASK TT4 ON TT0.DISPATCH_TASK_ID = TT4.ID
    where TT0.tool_code {query})"));
                //q.Exists<BatchTracebackPreSetup>((z, y) =>
                //y.Join<ReportRecord>((x, y) => x.DispatchTaskId == y.DispatchTaskId)
                //.Join<ReportRecord, Process>((x, y) => x.ProcessId == y.Id && y.Code == y.SQL<string>("p.Code"))
                //.Join<ReportRecord, ReportWipBatch>((x, y) => x.Id == y.ReportRecordId && y.BatchNo == z.BatchNo)
                //.Where(p => p.ToolCode.Contains(criteria.ToolCode))
                //);
            }

            var list = q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var batchNos = list.Select(p => p.BatchNo).Distinct().ToList();
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(batchNos);
            //计算批次类型
            foreach (var wipBatch in wipBatchs)
            {
                var P_Type = RT.Service.Resolve<WipBatchController>().GetWipBatchType(wipBatch);
                list.Where(p => p.BatchNo == wipBatch.BatchNo).ForEach(p => p.BatchType = P_Type);
            }
            //计算工单返工数
            var woNos = list.Select(p => p.WorkOrderNo).Distinct().ToList();
            var reportRecords = RT.Service.Resolve<ReportController>().GetReportRecordExaminesByWoNos(woNos);
            foreach (var item in woNos)
            {
                var reworkQty = reportRecords.Where(p => p.Wo == item).Sum(p => p.ReworkQty);
                var suspectQty = reportRecords.Where(p => p.Wo == item).Sum(p => p.SuspectQty);
                list.Where(p => p.WorkOrderNo == item).ForEach(p => { p.ReworkQty = reworkQty; p.SuspectQty = suspectQty; });
            }
            //获取委外发货明细
            var processingOutbounds = RT.Service.Resolve<Outsourcing.OutsourcingController>().GetProcessingOutboundsBySns(batchNos);
            //获取委外收货明细
            var processingInStocks = RT.Service.Resolve<Outsourcing.OutsourcingController>().GetProcessingInStocksBySns(batchNos);

            list.ForEach(p => p.IsOutsourcing = false);
            foreach (var batchNo in batchNos)
            {
                //在委外发货中存在,在委外收货中不存在,就是还在委外中
                if (processingOutbounds.Any(p => p.SN == batchNo) && processingInStocks.All(p => p.SN != batchNo))
                {
                    list.Where(p => p.BatchNo == batchNo).ForEach(p => p.IsOutsourcing = true);
                }
            }

            var nextProcessIds = list.Where(p => p.NextProcessId > 0).Select(p => p.NextProcessId.Value).Distinct().ToList();
            var nextProcesses = RT.Service.Resolve<ProcessController>().GetProcessByIds(nextProcessIds, true);
            foreach (var nextProcess in nextProcesses)
            {
                list.Where(p => p.NextProcessId == nextProcess.Id).ForEach(p => p.NextProcessCode = nextProcess.Code);
            }
            //根据工单号获取可疑品标签
            var suspectProductLabels = RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabelsByWoNos(woNos);
            foreach (var woNo in woNos)
            {
                //计算报废数
                var ScrapQty = suspectProductLabels.Where(p => p.WorkOrderNo == woNo).Sum(p => p.ScrapQty);
                list.Where(p => p.WorkOrderNo == woNo).ForEach(p => p.ScrapQty = ScrapQty);
            }

            return list;
        }

        /// <summary>
        /// 根据标签号和工单号获取装箱明细
        /// </summary>
        /// <param name="batchNo">标签号</param>
        /// <param name="workOrderNo">工单号</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<PackingDetail> GetPackingDetailsByBatchNoAndWoNo(string batchNo, string workOrderNo, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var list = Query<PackingDetail>()
                .Where(p => p.BatchLabel == batchNo && p.WorkOrderNo == workOrderNo)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }
    }
}
