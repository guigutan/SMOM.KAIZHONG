using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrderArchives;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 工单制造档案报工记录控制器
    /// </summary>
    public class ReportArchiveController : DomainController, IWoArchive
    {
        /// <summary>
        /// 根据工单ID查询
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ReportRecordSimpleInfo> GetReportRecords(double workOrderId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            var reportRecordList = DB.Query<ReportRecord>()
                .Where(p => p.WorkOrderId == workOrderId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            EntityList<ReportRecordSimpleInfo> reportRecords = new EntityList<ReportRecordSimpleInfo>();
            reportRecordList.ForEach(item => {
                var defectList = DB.Query<ReportDefect>().Where(p => p.ReportRecordId == item.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var defects = string.Empty;
                defectList.ForEach(i =>
                {
                    defects += ',' + i.Defect.Description;
                });
                var dispatchTask = RF.GetById<DispatchTask>(item.DispatchTaskId);
                if (dispatchTask != null)
                {
                    ReportRecordSimpleInfo reportRecord = new ReportRecordSimpleInfo
                    {
                        DispatchTaskId = item.DispatchTaskId,
                        DispatchTaskNo = dispatchTask.No,
                        DispatchTaskState = dispatchTask.TaskStatus.ToLabel(),
                        DispatchQty = dispatchTask.DispatchQty,
                        OkQty = dispatchTask.OkQty,
                        NgQty = dispatchTask.NgQty,
                        ReportQty = dispatchTask.ReportQty,
                        WorkOrderID = item.WorkOrderId,
                        RecordReportQty = item.ReportQty,
                        RecordOkQty = item.OkQty,
                        RecordNgQty = item.NgQty,
                        Hour = item.Hour,
                        BatchNo = item.BatchNo,
                        ReportTime = item.ReportTime,
                        Remark = item.Remark,
                        IsReport = item.IsReport,
                        PrincipalId = item.PrincipalId,
                        PrincipalName = item.Principal.Name,
                        Defects = defects,
                        ProcessId = dispatchTask.ProcessId,
                        ProcessName = dispatchTask.Process?.Name,
                        StationId = item.StationId,
                        StationName = item.Station?.Name,
                        SpecificationCode = dispatchTask.Specification?.Code,
                        SpecificationName = dispatchTask.Specification?.Name,
                        IsVirtualPart = dispatchTask.IsVirtualPart,
                        VirtualPartCode = dispatchTask.VirtualPartCode,
                        VirtualPartName = dispatchTask.VirtualPartName,
                        ReportMode = dispatchTask.ReportMode.ToLabel(),
                    };
                    reportRecords.Add(reportRecord);
                }
            });
            reportRecords.SetTotalCount(reportRecordList.TotalCount);
            return reportRecords;
        }
    }
}
