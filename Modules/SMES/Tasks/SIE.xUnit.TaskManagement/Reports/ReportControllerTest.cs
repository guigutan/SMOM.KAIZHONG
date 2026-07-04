using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WIP;
using SIE.MES.WIP.Runtime;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using SIE.xUnit.Core;
using SIE.xUnit.Defects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.TaskManagement.Reports
{
    /// <summary>
    /// 报工管理测试控制器
    /// </summary>
    public class ReportControllerTest : IClassFixture<TestStarup>
    {
        static ContextControllerTest tsContextCt = RT.Service.Resolve<ContextControllerTest>();
        static DispatchController taskCt = RT.Service.Resolve<DispatchController>();
        static ReportController reportCt = RT.Service.Resolve<ReportController>();
        static DefectTestController tsDefectCt = RT.Service.Resolve<DefectTestController>();
        static WipResourceController resCt = RT.Service.Resolve<WipResourceController>();
        static BarcodeController barcodeCt = RT.Service.Resolve<BarcodeController>();
        static TaskManagementTestController taskTestCt = RT.Service.Resolve<TaskManagementTestController>();

        /// <summary>
        /// 开工
        /// </summary>
        [Fact]
        public virtual EntityList<DispatchTask> ReportTaskStartWork()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            Assert.NotNull(tasks);
            var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTasks = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            var firstTask = dispatchTasks.FirstOrDefault();
            reportCt.TaskStart(firstTask.Id);            
            foreach (var task in dispatchTasks.Where(p => p.Id != firstTask.Id))
            {
                reportCt.StartWork(task);
            }

            return tasks;
        }

        /// <summary>
        /// 报工
        /// </summary>
        [Fact]
        public DispatchTask ReportTask()
        {
            tsContextCt.InitContext();
            var tasks = ReportTaskStartWork();
            Assert.NotNull(tasks);
            var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTasks = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            var dispatchTask = dispatchTasks.FirstOrDefault();
            var lastTask = dispatchTasks.LastOrDefault();
            Assert.NotNull(lastTask);
            reportCt.TaskReportQuick(lastTask.Id);
            var record = reportCt.GetOrCreateMainReportRecord(dispatchTask.Id);
            Assert.NotNull(record);
            var defects1 = reportCt.GetReportDefects(record.Id);
            Assert.NotNull(defects1);
            var defectIds = reportCt.GetMainDefectIds(dispatchTask.Id);
            Assert.NotNull(defectIds);
            var defects = tsDefectCt.GetOrCreateDefects(3);
            Assert.NotNull(defects);
            var taskDetails = taskCt.GetDispatchTaskDetails(dispatchTask.Id);
            Assert.Single(taskDetails);
            var taskDetail = taskDetails.FirstOrDefault();
            var taskDetail1 = taskCt.GetDispatchTaskDetail(dispatchTask.Id, taskDetail.AdoId, AdoType.Employee);
            Assert.Equal(taskDetail.Id, taskDetail1.Id);
            var boms = reportCt.GetTaskProcessBoms(dispatchTask.Id);
            Assert.NotNull(boms);
            var employee = RF.GetById<Employee>(taskDetail.AdoId);
            var shift = resCt.GetWipResourceShift(dispatchTask.WorkOrder.ResourceId.Value, DateTime.Now);
            Assert.NotNull(shift);

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
            reportCt.ReportSave(info);
            reportCt.TaskReport(info, true);
            reportCt.TaskReport(info);
            reportCt.GetQuickReportQty(dispatchTask.Id);

            var taskQueryInfo = new TaskQueryInfo()
            {
                TaskType = 0,
                EmployeeId = RT.IdentityId,
                ResourceId = dispatchTask.ResourceId,
                ProcessId = dispatchTask.ProcessId,
                Priority = (int)dispatchTask.Priority,
                QueryDate = 2,
                ProcessArray = "",
                KeyWord = "",
            };
            reportCt.QueryDispatchTaskInfo(taskQueryInfo);
            reportCt.QueryDispatchTaskNum(taskQueryInfo);
            var reportInfo = reportCt.GetReportTaskInfo(dispatchTask.Id);
            Assert.NotNull(reportInfo);

            var reportRecordInfos = reportCt.QueryReportRecordInfo(dispatchTask.Id);
            Assert.NotNull(reportRecordInfos);
            var associatedTaskInfos = reportCt.GetAssociatedTaskInfos(dispatchTask.Id);
            Assert.NotNull(associatedTaskInfos);

           
            return dispatchTask;
        }

        [Fact]
        public void TestGetData()
        {
            tsContextCt.InitContext();
            var dispatchTask = ReportTask();
            var dispatchTasks = taskCt.GetDispatchTasksByExpression(p => p.WorkOrderId == dispatchTask.WorkOrderId && p.TaskStatus == DispatchTaskStatus.Executing);
            var selectedIds = dispatchTasks.Select(p => p.Id).Distinct().ToList();
            var task = dispatchTasks.FirstOrDefault(p => p.Id != dispatchTask.Id);
            var reportRuleConfig = reportCt.GetReportRuleConfigByProduct(dispatchTask.ProductId);
            Assert.NotNull(reportRuleConfig);

            var config = new FamilyReportRuleConfig() { FamilyId = dispatchTask.WorkOrder.Product.ProductFamilyId.Value };
            config.Config = new ReportRuleConfigInfo() { ReportMode = (int)reportRuleConfig.ReportMode, ModReport = reportRuleConfig.IsModReport ? 1 : 0, ReportQty = reportRuleConfig.ReportQty, IsSyntype = reportRuleConfig.IsSyntype, Family = 0 };

            reportCt.SaveReportRuleConfigs(new List<FamilyReportRuleConfig>() { config });

            var reportPrintConfig = reportCt.GetReportPrintConfig(dispatchTask.WorkOrder.Product.ProductFamilyId.Value);
            Assert.Null(reportPrintConfig);

            var reportRecord = reportCt.GetOrCreateMainReportRecord(dispatchTask.Id);
            Assert.NotNull(reportRecord);

            var reportRecord1 = reportCt.GetOrCreateReportRecord(task.Id, task);
            Assert.NotNull(reportRecord1);

            var record = reportCt.GetReportRecord(dispatchTask.Id);
            Assert.Null(record);

            var records = reportCt.GetReportRecords(dispatchTask.Id, null);
            Assert.NotNull(records);
            var records1 = reportCt.GetReportRecords(dispatchTask.Id, true);
            Assert.NotNull(records1);
            var records2 = reportCt.GetReportRecords(dispatchTask.Id, false);
            Assert.NotNull(records2);

            var records3 = reportCt.GetReportRecordList(dispatchTask.WorkOrder.WorkShopId.Value, dispatchTask.WorkOrder.ResourceId);
            Assert.NotNull(records3);

            var defects = reportCt.GetReportDefects(dispatchTask.WorkOrder.WorkShopId.Value, dispatchTask.WorkOrder.ResourceId);
            Assert.NotNull(defects);

            var recordIds = reportCt.GetReportRecordIds(selectedIds, null);
            Assert.NotNull(recordIds);
            var recordIds1 = reportCt.GetReportRecordIds(selectedIds, true);
            Assert.NotNull(recordIds1);
            var recordIds2 = reportCt.GetReportRecordIds(selectedIds, false);
            Assert.NotNull(recordIds2);

            var reportTasks = reportCt.GetReportDispatchTaskList(new ReportDispatchTaskCriteria() { PlanBeginTime = new ObjectModel.DateRange(), PlanEndTime = new ObjectModel.DateRange() });
            Assert.NotNull(reportTasks);

            var proDispatchReports = reportCt.GetReportDispatchTaskList(new ReportDispatchTaskViewModelCriteria() { BeginTime = new ObjectModel.DateRange() });
            Assert.NotNull(proDispatchReports);

            var proDispatchReports1 = reportCt.GetReportDispatchTaskList(new ProductionDispatchReportCriteria() { BeginTime = new ObjectModel.DateRange() });
            Assert.NotNull(proDispatchReports1);
        }

        [Fact]
        public void AutoReport()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Auto, false, false, 0, true, false, false, 0, false, 0);
            Assert.NotNull(tasks);
            var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTasks = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            
            Assert.NotNull(tasks);            
            var dispatchTasks1 = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            var dispatchTask = dispatchTasks1.FirstOrDefault();
            var taskDetails = taskCt.GetDispatchTaskDetails(dispatchTask.Id);
            Assert.Single(taskDetails);
            var taskDetail = taskDetails.FirstOrDefault();
            var employee = RF.GetById<Employee>(taskDetail.AdoId);
            reportCt.AutoTaskReport(dispatchTasks); 
        }

        [Fact]
        public void AutoWipReport()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(true, true, taskCt.GetDispatchTaskNo(), taskCt.GetDispatchTaskNo() + "001", true, ReportMode.Manual, false, true, false, ReportMode.Auto, true, false, 3, true, false, false, 0, false, 0);
            //var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Auto, false, false, 3, true, false, false, 0, false, 0);
            Assert.NotNull(tasks);
            var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTasks1 = taskCt.GetDispatchTasksByExpression(p => taskIds.Contains(p.Id));
            var dispatchTask = dispatchTasks1.FirstOrDefault();
            var taskDetails = taskCt.GetDispatchTaskDetails(dispatchTask.Id);
            Assert.Single(taskDetails);
            var taskDetail = taskDetails.FirstOrDefault();
            var employee = RF.GetById<Employee>(taskDetail.AdoId);
            var barcodes = barcodeCt.GetBarcodes(dispatchTask.WorkOrderId.Value, false);
            Assert.NotNull(barcodes);
            string barcode = barcodes.FirstOrDefault().Sn;
            decimal okQty = 3;
            decimal ngQty = 0;
            RoutingProcessSign sign = RoutingProcessSign.End;
            WipCollectedEvent collectedEvent = InitCollectedEvent(dispatchTask, employee, dispatchTask.ProcessId.Value, barcode, okQty, ngQty, sign);
            reportCt.AutoTaskReport(collectedEvent);
            reportCt.ValidateWipReport(dispatchTask.WorkOrderId.Value, employee.Id, dispatchTask.ProcessId.Value);
        }

        private WipCollectedEvent InitCollectedEvent(DispatchTask task, Employee employee, double processId, string barcode, decimal okQty, decimal ngQty, RoutingProcessSign sign)
        {
            var stations = RT.Service.Resolve<StationController>().GetStations(processId, new PagingInfo(), "");
            var station = stations.FirstOrDefault();
            double workOrderId = task.WorkOrder.Id;
            double resourceId = station.ResourceId;
            double stationId = station.Id;
            double employeeId = employee.Id;
            reportCt.ValidateWipReport(workOrderId, employeeId, processId);
            var product = new product()
            {
                WorkOrderId = workOrderId,
                Qty = okQty,
                NgQty = ngQty,
                Routing = new routing(),
            };
            product.Routing.CurrentId = processId;
            product.Routing.Processes.Add(new process() { Sign = sign, Id = processId, Type = station.Process.Type.Value });
            var collectBarcode = new CollectBarcode(barcode, BarcodeType.SN);
            var collectData = new CollectData();
            var workcell = new Workcell()
            {
                EmployeeId = employeeId,
                ResourceId = resourceId,
                ProcessId = processId,
                StationId = stationId
            };
            var data = new CollectEventData(product, new CollectBarcode[] { collectBarcode }, collectData, workcell, DateTime.Now);
            WipCollectedEvent collectedEvent = new WipCollectedEvent(data);
            return collectedEvent;
        }
    }
}
