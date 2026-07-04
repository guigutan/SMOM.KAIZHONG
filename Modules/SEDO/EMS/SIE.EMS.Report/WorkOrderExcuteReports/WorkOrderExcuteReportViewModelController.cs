using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepairs.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表查询器
    /// </summary>
    public class WorkOrderExcuteReportViewModelController : DomainController
    {

        /// <summary>
        /// 获取备件库统计列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual WorkOrderExcuteReportInfo GetSpartPartMitReport(WorkOrderExcuteReportViewModelCriteria criteria)
        {
            var workOrderExcuteReportInfo = new WorkOrderExcuteReportInfo();

            //汇总信息
            CounInfo counInfo = new CounInfo();
            //折线图数据
            List<ChartInfo> chartInfoList = new List<ChartInfo>();
            //表格信息
            TableInfo tableInfo = new TableInfo();
            tableInfo.ClounNameList = new List<string>();
            tableInfo.ClounNameList.Add("月份".L10N());

            //2022-5-01；
            var foreachMonth = new DateTime(criteria.BeginMonth.Value.Year, criteria.BeginMonth.Value.Month, 1);
            var end = criteria.EndMonth.Value.AddMonths(1);
            var endMonth = new DateTime(end.Year, end.Month, 1);


            //取开始结束期间的工单且根据状态过滤全部找出来
            var EquipRepairBills = RT.Service.Resolve<RepairController>().GetEquipRepairBills(criteria.FactoryId, criteria.DepartmentId, criteria.RepairType, foreachMonth, endMonth);

            while (foreachMonth < endMonth)
            {
                //本月第一天到下个月第一天的区间
                var monthBegin = foreachMonth.Date;//本月第一天
                var monthEnd = foreachMonth.AddMonths(1).Date;//下个月第一天
                var sparePartMixReportRecordList = EquipRepairBills.Where(p => p.CreateDate >= monthBegin && p.CreateDate < monthEnd).ToList();
                ChartInfo chartInfo;
                if (sparePartMixReportRecordList.Any())
                {
                    chartInfo = new  ChartInfo()
                    {
                        SummaryTime = foreachMonth.Date,
                        Month = foreachMonth.ToString("yyyy年MM月").L10N(),
                        WorkOrderQty = sparePartMixReportRecordList.Count(p => p.CreateDate != null),
                        CompleteQty = sparePartMixReportRecordList.Count(m => m.RepairState == EquipRepairState.Completed),
                    };
                    if (chartInfo.CompleteQty != 0)
                    {
                        chartInfo.CompleteRate = Math.Round(((decimal)chartInfo.CompleteQty / (decimal)chartInfo.WorkOrderQty), 3);
                    }
                }
                else
                {
                    chartInfo = new ChartInfo()
                    {
                        SummaryTime = foreachMonth.Date,
                        Month= foreachMonth.ToString("yyyy年MM月").L10N(),
                        WorkOrderQty = 0,
                        CompleteQty = 0,
                        CompleteRate = 0
                    };
                }
                chartInfoList.Add(chartInfo);
                //表格数据构建动态表头列
                tableInfo.ClounNameList.Add(foreachMonth.ToString("yyyy年MM月").L10N());

                foreachMonth = foreachMonth.AddMonths(1);
            }

            List<List<string>> rowResult = new List<List<string>>();//行转为列

                var row1 = new List<string>() { "工单总数".L10N() };
                var row2 = new List<string>() { "工单完成数".L10N() };
                var row3 = new List<string>() { "工单完成率".L10N() };

            foreach (var charinfo in chartInfoList)
            {
                row1.Add(charinfo.WorkOrderQty.ToString());
                row2.Add(charinfo.CompleteQty.ToString());
                row3.Add(charinfo.CompleteRate != 0 ? Math.Round(charinfo.CompleteRate * 100, 1).ToString() + "%" : "0");
            }

            rowResult.Add(row1);
            rowResult.Add(row2);
            rowResult.Add(row3);


            //报修不算总单,待维修，维修中，待确认，待平分，已完成，暂停中，取消，关闭  这些算总单；已完成状态算完成；待维修，维修中，待确认，待平分，暂停中算未完成
            List<EquipRepairState> stateList = new List<EquipRepairState>() {
                 EquipRepairState.WaitRepair, EquipRepairState.Repairing, EquipRepairState.WaitConfirm, EquipRepairState.WaitScore, EquipRepairState.Suspending
            };
            var WorkCount = EquipRepairBills.Count();
            var CompleteCount = EquipRepairBills.Count(p => p.RepairState == EquipRepairState.Completed);
            var WaitCompleteCount = EquipRepairBills.Count(p => stateList.Contains(p.RepairState));

            #region   填充汇总信息
            counInfo.WorkCount = WorkCount;
            counInfo.CompleteCount = CompleteCount;
            counInfo.WaitCompleteCount = WaitCompleteCount;
            if (CompleteCount != 0)
            {
                counInfo.CompleteRate = Math.Round((decimal)CompleteCount / (decimal)WorkCount * 100, 1);
            }
            workOrderExcuteReportInfo.CounInfo = counInfo;
            #endregion

            //填充折现图信息
            workOrderExcuteReportInfo.ChartInfo = chartInfoList;
            //填充表格信息
            tableInfo.Datas = rowResult;
            workOrderExcuteReportInfo.TableInfo = tableInfo;

            return workOrderExcuteReportInfo;
        }
    }
}
