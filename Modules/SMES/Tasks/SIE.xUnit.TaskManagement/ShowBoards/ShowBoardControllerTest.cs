using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.ShowBoards;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.xUnit.Core;
using SIE.xUnit.Defects;
using System;
using System.Linq;
using Xunit;

namespace SIE.xUnit.TaskManagement.ShowBoards
{
    /// <summary>
    /// 看板测试控制器
    /// </summary>
    public class ShowBoardControllerTest : IClassFixture<TestStarup>
    {
        static ContextControllerTest tsContextCt = RT.Service.Resolve<ContextControllerTest>();
        static DispatchController taskCt = RT.Service.Resolve<DispatchController>();
        static ReportController reportCt = RT.Service.Resolve<ReportController>();
        static DefectTestController tsDefectCt = RT.Service.Resolve<DefectTestController>();
        static WipResourceController resCt = RT.Service.Resolve<WipResourceController>();
        static ShowBoardController boardCt = RT.Service.Resolve<ShowBoardController>();
        static TaskManagementTestController taskTestCt = RT.Service.Resolve<TaskManagementTestController>();

        [Fact]
        public virtual void ShowBoard()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            Assert.NotNull(tasks);
            var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTasks = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            foreach (var task in dispatchTasks)
            {
                reportCt.StartWork(task);
            }

            var dispatchTasks1 = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            var dispatchTask = dispatchTasks1.FirstOrDefault();
            var record = reportCt.GetOrCreateMainReportRecord(dispatchTask.Id);
            var defects1 = reportCt.GetReportDefects(record.Id);
            var defectIds = reportCt.GetMainDefectIds(dispatchTask.Id);
            var defects = tsDefectCt.GetOrCreateDefects(3);
            var taskDetails = taskCt.GetDispatchTaskDetails(dispatchTask.Id);
            var taskDetail = taskDetails.FirstOrDefault();
            var taskDetail1 = taskCt.GetDispatchTaskDetail(dispatchTask.Id, taskDetail.AdoId, AdoType.Employee);
            var boms = reportCt.GetTaskProcessBoms(dispatchTask.Id);
            var employee = RF.GetById<Employee>(taskDetail.AdoId);
            var shift = resCt.GetWipResourceShift(dispatchTask.WorkOrder.ResourceId.Value, DateTime.Now);
            taskTestCt.CreateDefectRecords(employee, shift, record, defects);
            var syntypeTasks = reportCt.GetIsSyntypeTasks(dispatchTask.Id, true);
            ReportTaskInfo info = new ReportTaskInfo()
            {
                BatchNo = record.BatchNo,
                RecordId = record.Id,  //0表示新增的
                OkQty = record.OkQty,
                NgQty = record.NgQty,
                ReportNgQty = record.NgQty,
                Remark = record.Remark,
                TaskId = record.DispatchTaskId,
                Hour = record.Hour,
                ProcessId = record.ProcessId,
                StationId = record.StationId,
                WorkOrderId = record.WorkOrderId ?? 0
            };
            info.DefectIds.AddRange(defects.Select(p => p.Id).Distinct());
            info.SyntypeTaskInfos.AddRange(syntypeTasks);
            reportCt.TaskReport(info, true);

            boardCt.GetDayPlanTaskInfos(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetDayPlanTaskInfosOf3Day(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetPlanTaskTotalInfo(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetTotalTaskInfos(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetAbnormalTaskInfos(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetAbnormalTaskInfos1(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetCapacityHourStatistics(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetDayProduceTaskInfo(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetDayDefectInfo(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetDefectRateInfo(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
            boardCt.GetDayProduceTaskInfo1(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId);
        }
    }
}
