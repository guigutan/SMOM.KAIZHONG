using DocumentFormat.OpenXml.EMMA;
using NPOI.SS.Formula.Functions;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.ApiModels;
using SIE.Core.Barcodes;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Inspection;
using SIE.EventMessages.IOT;
using SIE.EventMessages.MES.Inspection;
using SIE.Items;
using SIE.KZ.Base.SmomControl;
using SIE.MES.LoadItems;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.TaskManagement.Events;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.MES.WorkReportPlans;
using SIE.Packages.ItemLabels;
using SIE.Packages.ItemLabels.Configs;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工控制器
    /// </summary>
    public partial class ReportController : DomainController, ITaskReport
    {
        private static DispatchController _dispatchController = RT.Service.Resolve<DispatchController>();

        /// <summary>
        /// 汇总条件不能为空
        /// </summary>
        private readonly string _sumExpNull = "汇总条件不能为空".L10N();

        #region 取样净重详情表

        /// <summary>
        /// 创建记录，更新工序BOM取样净重
        /// </summary>
        /// <param name="ProcessBomId"></param>
        /// <param name="Weight"></param>
        /// <param name="taskId"></param>
        /// <param name="oldWeight"></param>
        public virtual void CreateWeightOfSamplingReport(double? ProcessBomId, decimal Weight, double taskId, decimal oldWeight)
        {
            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                WeightOfSamplingReport report = new WeightOfSamplingReport();
                report.WorkOrderProcessBomId = ProcessBomId;
                report.DispatchTaskId = taskId;
                report.Weight = Weight;
                report.OldWeight = oldWeight;
                RF.Save(report);

                //RT.Service.Resolve<WorkOrderController>().UpdateWorkProcessBomWeight(ProcessBomId, Weight);
                tran.Complete();
            }
        }

        /// <summary>
        /// 取样净重详情表查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WeightOfSamplingReport> CriteriaWeightOfSamplingReport(WeightOfSamplingReportCriteria criteria)
        {
            var q = Query<WeightOfSamplingReport>();

            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.WorkOrder.Product.Name.Contains(criteria.ProductName));
            if (!criteria.Process.IsNullOrEmpty())
                q.Where(p => p.WorkOrderProcessBom.Process.Code.Contains(criteria.Process));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        /// <summary>
        /// 校验工序上工序是否已经报工完
        /// </summary>
        /// <param name="wipBatchId"></param>
        /// <param name="curProcessId"></param>
        public virtual void ValidationLastProcessReport(double wipBatchId, double curProcessId)
        {
            var wipBatch = RF.GetById<WipBatch>(wipBatchId, new EagerLoadOptions().LoadWithViewProperty());

            var wo = RF.GetById<WorkOrder>(wipBatch.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
            //获取当前工序，工单工序位置
            var curRoutingProcess = wo.RoutingProcessList.FirstOrDefault(p => p.ProcessId == curProcessId);
            //获取上一个工序，工单工序位置
            var lastRoutingProcessIds = wo.RoutingProcessList.Where(p => p.Index < curRoutingProcess.Index).Select(p => p.ProcessId).Distinct().ToList();

            //查询工单对应的所有任务单
            var taskPres = RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == wo.Id && lastRoutingProcessIds.Contains(p.ProcessId));
            if (taskPres.Count == 0)
                return;

            //校验前置任务是否都有该标签的报工记录
            if (wipBatch.ReportRecordIds.IsNullOrEmpty())
            {
                var list = GetReportWipBatchsByWipBatchId(wipBatch.Id);
                if (list.Count == 0)
                    throw new ValidationException("对应工序标签[{0}]还未进行报工,请确认".L10nFormat(wipBatch.BatchNo));
                //补全异常数据
                wipBatch.ReportRecordIds = list.OrderBy(p => p.CreateDate).Select(p => p.ReportRecordId.ToString()).Concat(",");
                RF.Save(wipBatch);
            }

            //校验当前工序是否已报工
            ValidateProcessHasReport(wipBatch.BatchNo, curRoutingProcess.Process.Code);

            var recordIds = wipBatch.ReportRecordIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var records = RT.Service.Resolve<ReportController>().GetReportRecordsByIds(recordIds.Select(p => double.Parse(p)).ToList(), true);

            //拆分标签不允许进行前置工序报工
            //if (wipBatch.SourceNo.IsNotEmpty() && records.Any(p => p.DispatchTaskId == currTask.Id))
            //{
            //    throw new ValidationException("任务单[{0}]已存在标签[{1}]的报工数据,请确认".L10nFormat(currTask.No, wipBatch.BatchNo));
            //}

            List<string> processList = new List<string>();
            foreach (var task in taskPres)
            {
                if (task.ProcessCode.Contains("电性能测试") || task.ProcessName.Contains("电性能测试"))
                    continue;
                if (!records.Any(p => p.ProcessCode == task.ProcessCode))
                {
                    if (!processList.Contains(task.ProcessCode))
                        processList.Add(task.ProcessCode);
                }
            }
            if (processList.Count > 0)
                throw new ValidationException("对应工序标签[{0}]还未完成前工序[{1}]任务报工,请确认".L10nFormat(wipBatch.BatchNo, processList.Concat("、")));
        }

        /// <summary>
        /// 校验工序是否报工完成
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual void ValidationSeqTask(List<double> wipBatchIds)
        {
            var wipBatchs = Query<WipBatch>().Where(p => wipBatchIds.Contains(p.Id)).ToList();

            string msg = "";
            foreach (var wipBatch in wipBatchs)
            {
                var currTask = RF.GetById<DispatchTask>(wipBatch.DispatchTaskId.Value, new EagerLoadOptions().LoadWithViewProperty());
                //查询工单对应的所有任务单
                var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { currTask.WorkOrderId.Value });

                var recordIds = wipBatch.ReportRecordIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
                var records = RT.Service.Resolve<ReportController>().GetReportRecordsByIds(recordIds.Select(p => double.Parse(p)).ToList(), true);
                var taskPres = tasks.Where(p => p.Seq < currTask.Seq).ToList();  //前置工序
                List<string> processList = new List<string>();
                foreach (var task in taskPres)
                {
                    if (task.ProcessCode.Contains("电性能测试") || task.ProcessName.Contains("电性能测试"))
                        continue;
                    if (!records.Any(p => p.ProcessCode == task.ProcessCode))
                    {
                        if (!processList.Contains(task.ProcessCode))
                            processList.Add(task.ProcessCode);
                    }
                }
                if (processList.Count > 0)
                    msg += "对应工序标签[{0}]还未完成前工序[{1}]任务报工,请确认".L10nFormat(wipBatch.BatchNo, processList.Concat("、"));
                //throw new ValidationException("对应工序标签[{0}]还未完成前工序[{1}]任务报工,请确认".L10nFormat(wipBatch.BatchNo, processList.Concat("、")));
            }
            if (msg != "")
                throw new ValidationException(msg);
        }

        /// <summary>
        /// 获取报工规则配置
        /// </summary>
        /// <param name="familyId">产品族ID</param>
        /// <returns>报工规则配置</returns>
        public virtual ReportRuleConfig GetReportRuleConfig(double familyId)
        {
            return Query<ReportRuleConfig>().Where(p => p.ProductFamilyId == familyId).FirstOrDefault();
        }

        /// <summary>
        /// 通过产品Id获取报工规则配置
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>报工规则配置</returns>
        public virtual ReportRuleConfig GetReportRuleConfigByProduct(double productId)
        {
            return Query<ReportRuleConfig>().Exists<ProductFamily>(
                 (x, y) => y.Join<Item>((c, d) =>
                         c.Id == d.ProductFamilyId && d.Id == productId)
                     .Where(p => p.Id == x.ProductFamilyId)).FirstOrDefault();
        }

        /// <summary>
        /// 保存报工规则配置
        /// </summary>
        /// <param name="configs">规则配置列表</param>
        public virtual void SaveReportRuleConfigs(List<FamilyReportRuleConfig> configs)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                configs.ForEach(config =>
                {
                    SaveReportRuleConfig(config.FamilyId, config.Config);
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存报工规则配置
        /// </summary>
        /// <param name="familyId">产品族ID</param>
        /// <param name="configInfo">报工规则配置信息</param>
        void SaveReportRuleConfig(double familyId, ReportRuleConfigInfo configInfo)
        {
            var config = GetReportRuleConfig(familyId);
            if (config == null)
                config = new ReportRuleConfig() { ProductFamilyId = familyId };
            config.ReportMode = (ReportMode)configInfo.ReportMode;
            config.IsSyntype = configInfo.IsSyntype;
            config.IsModReport = configInfo.ModReport == 1;
            config.ReportQty = configInfo.ReportQty;
            config.IsExpendItem = configInfo.IsExpendItem;
            RF.Save(config);
        }

        /// <summary>
        /// 获取产品族打印模板配置
        /// </summary>
        /// <param name="familyId">产品族ID</param>
        /// <returns>打印模板配置</returns>
        public virtual ReportPrintConfig GetReportPrintConfig(double familyId)
        {
            return Query<ReportPrintConfig>().Where(p => p.ProductFamilyId == familyId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取未报工的数据（即保存的数据，只能有一个）
        /// </summary>
        /// <param name="dispatchTaskId">任务Id</param>
        /// <returns>报工记录</returns>
        public virtual ReportRecord GetReportRecord(double dispatchTaskId)
        {
            return Query<ReportRecord>().Where(p => p.DispatchTaskId == dispatchTaskId && !p.IsReport).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 报工任务单查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>任务单</returns>
        public virtual EntityList<ReportDispatchTask> GetReportDispatchTaskList(ReportDispatchTaskCriteria criteria)
        {
            IEntityQueryer<ReportDispatchTask> query = Query<ReportDispatchTask>();
            //if (!criteria.IsUnscheduledInProgress)
            //{
            //    query = query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => employee.Id == RT.IdentityId && (
            //    p.AdoType == AdoType.Employee && p.AdoId == RT.IdentityId ||
            //       p.AdoType == AdoType.EmployeeGroup && employee.EmployeeGroupId > 0 && employee.EmployeeGroupId == p.AdoId ||
            //       p.AdoType == AdoType.WorkGroup && employee.WorkGroupId > 0 && employee.WorkGroupId == p.AdoId
            //        )));
            //}
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.TaskStatus.HasValue)
                query.Where(p => p.TaskStatus == criteria.TaskStatus);
            if (criteria.ProductId.HasValue)
                query.Where(p => p.ProductId == criteria.ProductId);
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id && y.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.ReportMode.HasValue)
                query.Where(p => p.ReportMode == criteria.ReportMode);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (criteria.PlanBeginTime.BeginValue.HasValue)
                query.Where(p => p.PlanBeginTime >= criteria.PlanBeginTime.BeginValue);
            if (criteria.PlanBeginTime.EndValue.HasValue)
                query.Where(p => p.PlanBeginTime <= criteria.PlanBeginTime.EndValue);
            if (criteria.PlanEndTime.BeginValue.HasValue)
                query.Where(p => p.PlanEndTime >= criteria.PlanEndTime.BeginValue);
            if (criteria.PlanEndTime.EndValue.HasValue)
                query.Where(p => p.PlanEndTime <= criteria.PlanEndTime.EndValue);
            if (criteria.IsShowDispatchTask && !criteria.IsUnscheduledInProgress)
                query.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing
                && p.TaskStatus != DispatchTaskStatus.Dispatching && p.TaskStatus != DispatchTaskStatus.ToDispatch
                );
            if (criteria.IsUnscheduledInProgress && !criteria.IsShowDispatchTask)
                query.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.ToDispatch);
            if (criteria.IsUnscheduledInProgress && criteria.IsShowDispatchTask)
            {
                query.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing
                    || p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.ToDispatch);
            }
            query.Where(p => p.MergedStatus != MergedStatus.Merged);
            query.OrderBy(criteria.OrderInfoList);
            //if (!criteria.WorkOrderNo.IsNullOrEmpty())
            //{
            //    List<double> taskIdList = GetDispatchTaskIds(criteria.WorkOrderNo);
            //    if (taskIdList.Count > 0)
            //    {
            //        var exp = taskIdList.CreateContainsExpression<ReportDispatchTask>("x", "Id");
            //        return query.Where(exp).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //    }
            //}
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 报工任务单查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>任务单</returns>
        public virtual EntityList<ProductionDispatchReport> GetReportDispatchTaskList(ReportDispatchTaskViewModelCriteria criteria)
        {
            var query = Query<ProductionDispatchReport>();
            if (criteria.EmployeeId.HasValue && criteria.EmployeeId.Value > 0)
            {

                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => (
              p.AdoType == AdoType.Employee && p.AdoId == employee.Id && employee.Id == criteria.EmployeeId.Value
                     )));
            }
            else if (criteria.WorkGroupId.HasValue && criteria.WorkGroupId.Value > 0)
            {
                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => (
                    p.AdoType == AdoType.WorkGroup && employee.WorkGroupId > 0 && employee.WorkGroupId == p.AdoId && employee.WorkGroupId == criteria.WorkGroupId.Value
                     )));
            }
            else
            {
                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => (
                  (p.AdoType == AdoType.Employee && p.AdoId == employee.Id) ||
                  (p.AdoType == AdoType.WorkGroup && employee.WorkGroupId > 0 && employee.WorkGroupId == p.AdoId) ||
                  (p.AdoType == AdoType.EmployeeGroup && employee.EmployeeGroupId > 0 && employee.EmployeeGroupId == p.AdoId)
                         )));
            }

            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (criteria.ProductId.HasValue)
                query.Where(p => p.ProductId == criteria.ProductId);
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                List<double> taskids = GetDispatchTaskIds(criteria.WorkOrderNo);
                query.Where(p => taskids.Contains(p.Id));
            }
            if (criteria.WorkOrderState.HasValue)
                query.Where(p => p.WorkOrderState == criteria.WorkOrderState);
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.TaskStatus.HasValue)
                query.Where(p => p.TaskStatus == criteria.TaskStatus);
            if (criteria.BeginTime.BeginValue.HasValue)
                query.Where(p => p.BeginTime >= criteria.BeginTime.BeginValue);
            if (criteria.BeginTime.EndValue.HasValue)
                query.Where(p => p.BeginTime <= criteria.BeginTime.EndValue);

            query.Where(p => p.MergedStatus != MergedStatus.Merged && p.TaskStatus != DispatchTaskStatus.Dispatching && p.TaskStatus != DispatchTaskStatus.ToDispatch);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 报工任务单查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>任务单</returns>
        public virtual EntityList<ProductionDispatchReport> GetReportDispatchTaskList(ProductionDispatchReportCriteria criteria)
        {

            var query = Query<ProductionDispatchReport>();
            if (criteria.EmployeeId.HasValue && criteria.EmployeeId.Value > 0)
            {

                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => (
              p.AdoType == AdoType.Employee && p.AdoId == employee.Id && employee.Id == criteria.EmployeeId.Value
                     )));
            }
            else if (criteria.WorkGroupId.HasValue && criteria.WorkGroupId.Value > 0)
            {
                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => (
                    p.AdoType == AdoType.WorkGroup && employee.WorkGroupId > 0 && employee.WorkGroupId == p.AdoId && employee.WorkGroupId == criteria.WorkGroupId.Value
                     )));
            }
            else
            {
                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id).Join<Employee>((p, employee) => (
                  (p.AdoType == AdoType.Employee && p.AdoId == employee.Id) ||
                  (p.AdoType == AdoType.WorkGroup && employee.WorkGroupId > 0 && employee.WorkGroupId == p.AdoId) ||
                  (p.AdoType == AdoType.EmployeeGroup && employee.EmployeeGroupId > 0 && employee.EmployeeGroupId == p.AdoId)
                         )));
            }

            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (criteria.ProductId.HasValue)
                query.Where(p => p.ProductId == criteria.ProductId);
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                List<double> taskids = GetDispatchTaskIds(criteria.WorkOrderNo);
                query.Where(p => taskids.Contains(p.Id));
            }
            if (criteria.WorkOrderState.HasValue)
                query.Where(p => p.WorkOrderState == criteria.WorkOrderState);
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.TaskStatus.HasValue)
                query.Where(p => p.TaskStatus == criteria.TaskStatus);
            if (criteria.BeginTime.BeginValue.HasValue)
                query.Where(p => p.BeginTime >= criteria.BeginTime.BeginValue);
            if (criteria.BeginTime.EndValue.HasValue)
                query.Where(p => p.BeginTime <= criteria.BeginTime.EndValue);

            query.Where(p => p.MergedStatus != MergedStatus.Merged && p.TaskStatus != DispatchTaskStatus.Dispatching && p.TaskStatus != DispatchTaskStatus.ToDispatch);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单Id的任务单Id列表
        /// </summary>
        /// <param name="woNo">工单编码</param>
        /// <returns>任务单Id列表</returns>
        private List<double> GetDispatchTaskIds(string woNo)
        {
            var taskIds = new List<double>();
            var tasksWithWo = Query<DispatchTask>().Exists<WorkOrder>((a, b) => b.Where(f => a.WorkOrderId == f.Id && f.No.Contains(woNo))).ToList();
            if (tasksWithWo.Count > 0)
                taskIds.AddRange(tasksWithWo.Select(p => p.Id).Distinct());
            var associatedTasks = Query<AssociatedTask>().Exists<DispatchTask>(
                    (x, y) => y.Join<WorkOrder>((c, d) => c.WorkOrderId == d.Id && d.No.Contains(woNo))
                        .Where(p => p.Id == x.TaskId)).ToList();
            if (associatedTasks.Count > 0)
                taskIds.AddRange(associatedTasks.Select(p => p.DispatchTaskId).Distinct());
            if (taskIds.Count > 0)
                taskIds = taskIds.Distinct().ToList();
            return taskIds;
        }

        /// <summary>
        /// 获取已报工记录
        /// </summary>
        /// <param name="dispatchTaskId">派工任务单ID</param>
        /// <param name="isReport">是否报表</param>
        /// <returns>报工记录列表</returns>
        public virtual EntityList<ReportRecord> GetReportRecords(double dispatchTaskId, bool? isReport)
        {
            var query = Query<ReportRecord>().Where(p => p.DispatchTaskId == dispatchTaskId);
            if (isReport.HasValue)
                query.Where(p => p.IsReport == isReport);
            return query.OrderByDescending(p => p.ReportTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取七日的报工记录列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>七日的报工记录列表</returns>
        public virtual EntityList<ReportRecord> GetReportRecordList(double workShopId, double? resourceId)
        {
            var now = RF.Find<DispatchTask>().GetDbTime();
            var startDate = new DateTime(now.Year, now.Month, now.Day);
            DateTime start = startDate.AddDays(-6);
            DateTime end = startDate.AddDays(1).AddSeconds(-1);
            if (resourceId.HasValue)
                return Query<ReportRecord>().Exists<DispatchTask>((a, b) => b.Where(p => p.Id == a.DispatchTaskId && p.WorkShopId == workShopId && p.ResourceId == resourceId && a.ReportTime >= start && a.ReportTime <= end)).ToList();
            else
                return Query<ReportRecord>().Exists<DispatchTask>((a, b) => b.Where(p => p.Id == a.DispatchTaskId && p.WorkShopId == workShopId && a.ReportTime >= start && a.ReportTime <= end)).ToList();
        }

        /// <summary>
        /// 获取报工记录
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id集合</param>
        /// <param name="isReport">是否已报工</param>
        /// <returns>报工记录</returns>
        public virtual List<double> GetReportRecordIds(List<double> dispatchTaskIds, bool? isReport)
        {
            var reportRecords = dispatchTaskIds.SplitContains((tempTaskIds) =>
             {
                 var query = Query<ReportRecord>();
                 if (isReport.HasValue)
                     query.Where(p => p.IsReport == isReport);
                 return query.Where(p => tempTaskIds.Contains(p.DispatchTaskId) && p.PrincipalId == RT.IdentityId).ToList();
             });

            return reportRecords.Select(p => p.Id).ToList<double>();
        }

        /// <summary>
        /// 获取主报工记录
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <returns>报工记录</returns>
        public virtual ReportRecord GetOrCreateMainReportRecord(double dispatchTaskId)
        {
            var curTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (curTask == null)
                throw new ValidationException("该任务单数据已更新，请重新查询后操作！".L10N());
            if (!curTask.IsMainTask && (curTask.IsSyntypeReport || curTask.MergedStatus == MergedStatus.Merged))
            {
                //共模辅工单任务单共模比报工||合并子单报工
                dispatchTaskId = GetMainTaskId(dispatchTaskId).Id;
            }
            return GetOrCreateReportRecord(dispatchTaskId, curTask);
        }

        /// <summary>
        /// 视图界面新增报工
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <returns></returns>
        public virtual ReportRecord GetNewReportRecord(double dispatchTaskId)
        {
            var curTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (curTask == null)
                throw new ValidationException("该任务单数据已更新，请重新查询后操作！".L10N());
            if (!curTask.IsMainTask && (curTask.IsSyntypeReport || curTask.MergedStatus == MergedStatus.Merged))
            {
                //共模辅工单任务单共模比报工||合并子单报工
                dispatchTaskId = GetMainTaskId(dispatchTaskId).Id;
            }
            return CreateNewReportRecord(dispatchTaskId, curTask);
        }

        /// <summary>
        /// 获取报工记录
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <param name="task">报工任务单</param>
        /// <returns>报工记录</returns>
        public virtual ReportRecord GetOrCreateReportRecord(double dispatchTaskId, DispatchTask task)
        {
            var record = GetReportRecord(dispatchTaskId);
            if (record == null)
                return CreateReportRecord(dispatchTaskId, task);
            else
            {
                var defects = GetReportDefects(record.Id);
                record.DefectIds = defects.Select(p => p.Id).ToList();
                record.DefectNames = string.Join(";", defects.Select(p => p.DefectDescription));
            }
            return record;
        }

        /// <summary>
        /// 报工界面新增报工
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual ReportRecord CreateNewReportRecord(double dispatchTaskId, DispatchTask task)
        {
            return CreateReportRecord(dispatchTaskId, task);
        }

        private decimal GetTaskToReportQty(DispatchTask task)
        {
            decimal reportQty = 0;
            var familyId = task.Product.ProductFamilyId;
            if (familyId.HasValue)
            {
                var config = GetReportRuleConfig(familyId.Value);
                if (config != null)
                {
                    if (config.ReportMode == ReportMode.Auto && config.ReportQty > 0)
                        reportQty = config.ReportQty;
                    else if (config.ReportMode == ReportMode.Manual)
                    {
                        decimal toReportQty = task.DispatchQty - task.ReportQty > 0 ? task.DispatchQty - task.ReportQty : 0; //任务单剩余待报工数
                        if (config.IsModReport || config.ReportQty > toReportQty)
                        {
                            reportQty = toReportQty;
                        }
                        else if (config.ReportQty > 0)
                        {
                            reportQty = config.ReportQty;
                        }
                    }
                }
            }
            return reportQty;
        }

        /// <summary>
        /// 获取报工缺陷记录
        /// </summary>
        /// <param name="recordId">报工记录ID</param>
        /// <returns>缺陷记录</returns>
        public virtual EntityList<ReportDefect> GetReportDefects(double recordId)
        {
            return Query<ReportDefect>().Where(p => p.ReportRecordId == recordId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当日的报工缺陷列表
        /// </summary>
        /// <returns>当日的报工缺陷列表</returns>
        public virtual EntityList<ReportDefect> GetReportDefects(double workShopId, double? resourceId)
        {
            //当天(0时0分0秒和23时59分59秒)
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            DateTime sToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime eToday = sToday.AddDays(1).AddSeconds(-1);
            if (resourceId != null && resourceId.HasValue)
                return Query<ReportDefect>().Exists<ReportRecord>(
                        (x, y) => y.Join<DispatchTask>((c, d) => c.DispatchTaskId == d.Id && d.WorkShopId == workShopId && d.ResourceId == resourceId)
                            .Where(p => p.Id == x.ReportRecordId && p.ReportTime >= sToday && p.ReportTime <= eToday)).ToList();
            else
                return Query<ReportDefect>().Exists<ReportRecord>(
                                    (x, y) => y.Join<DispatchTask>((c, d) => c.DispatchTaskId == d.Id && d.WorkShopId == workShopId)
                                        .Where(p => p.Id == x.ReportRecordId && p.ReportTime >= sToday && p.ReportTime <= eToday)).ToList();
        }

        /// <summary>
        /// 获取主任务单缺陷列表
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <returns>缺陷列表</returns>
        public virtual List<double> GetMainDefectIds(double dispatchTaskId)
        {
            return Query<ReportDefect>().Join<ReportRecord>((x, y) => y.Id == x.ReportRecordId && y.DispatchTaskId == dispatchTaskId && !y.IsReport).ToList()
                .Select(p => p.DefectId).ToList();
        }

        /// <summary>
        /// 获取任务单BOM
        /// </summary>
        /// <param name="dispatchTaskId">选中任务单</param>
        /// <returns>BOM</returns>
        public virtual EntityList<TaskProcessBom> GetTaskProcessBoms(double dispatchTaskId)
        {
            var mainTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (mainTask == null)
            {
                throw new ValidationException("没找到派工任务单".L10N());
            }
            var dispatchIds = new List<double>();
            if (mainTask.MergedStatus == MergedStatus.Normal)
            {
                dispatchIds.Add(dispatchTaskId);
            }
            else if (mainTask.MergedStatus == MergedStatus.MergeRows)
            {
                var ass = RT.Service.Resolve<DispatchController>().GetAssociatedTaskListByTaskId(dispatchTaskId);
                if (ass.Count > 0)
                {
                    dispatchIds.AddRange(ass.Select(p => p.TaskId).ToList());
                }
            }
            else
            {
                //
            }

            return RT.Service.Resolve<DispatchController>().GetTaskProcessBomList(dispatchIds);
        }

        #region 任务单开工

        /// <summary>
        /// 任务单开工
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="dispatchTask"></param>
        /// <returns></returns>
        public virtual bool StartWork(Employee employee, DispatchTask dispatchTask)
        {
            if (dispatchTask == null)
            {
                throw new ValidationException("任务单数据有误！".L10N());
            }
            CheckByWorkplan(employee.Id, dispatchTask);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //待派工 派工中 则生成派工明细，并执行派工
                if (dispatchTask.TaskStatus == DispatchTaskStatus.Dispatching || dispatchTask.TaskStatus == DispatchTaskStatus.ToDispatch)
                {
                    AdoInfo adoInfo = RT.Service.Resolve<DispatchController>().CreateAdoInfo(dispatchTask, employee.Id, employee.Name, "员工", AdoGroup.EmployeeGroup);
                    var dispatchTaskDetail = RT.Service.Resolve<DispatchController>().CreateDispatchTaskDetail(dispatchTask, adoInfo, AdoType.Employee);

                    var dispatchTaskDetails = RT.Service.Resolve<DispatchController>().GetDispatchTaskDetails(dispatchTask.Id);
                    if (dispatchTaskDetails.Any())
                    {
                        var isExsitedDetail = dispatchTaskDetails.Where(m => m.DispatchTaskId == dispatchTaskDetail.DispatchTaskId && m.AdoName == dispatchTaskDetail.AdoName &&
                          m.AdoType == dispatchTaskDetail.AdoType);
                        if (isExsitedDetail == null || !isExsitedDetail.Any())
                        {
                            RF.Save(dispatchTaskDetail);
                        }
                    }
                    else
                    {
                        RF.Save(dispatchTaskDetail);
                    }
                    dispatchTask.TaskStatus = DispatchTaskStatus.Dispatched;
                    var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new System.Collections.Generic.List<double>() { dispatchTask.Id });
                    if (!errMsg.IsNullOrEmpty())
                    {
                        throw new ValidationException(errMsg);
                    }
                }
                //判断当前用户是否有报工权限
                if (!CheckUserPermission(dispatchTask.Id, employee.Id))
                {
                    throw new ValidationException("您没有当前报工任务的报工权限".L10N());
                }

                var newdispatchTask = RF.GetById<DispatchTask>(dispatchTask.Id, new EagerLoadOptions().LoadWithViewProperty());
                //开始开工
                StartWork(newdispatchTask);
                tran.Complete();
            }
            return true;
        }


        /// <summary>
        /// IOT工序标签开工
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resource"></param>
        /// <param name="IsSyncIotStatus"></param>
        /// <returns></returns>
        public virtual WipBatchQueue StartIOTWorkTask(WipBatchQueue queue, WipResource resource, bool IsSyncIotStatus = true)
        {
            if (queue == null)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");
            queue = RF.GetById<WipBatchQueue>(queue.Id, new EagerLoadOptions().LoadWithViewProperty());
            //return queue;

            //调用IOT接口下发开工指令
            var info = new IotTaskInfo()
            {
                TaskNo = queue.BatchNo,
                ResourceCode = resource.Code,
                InitQty = queue.IotQty,
            };
            RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfo(info);

            return queue;
        }
        /// <summary>
        /// 获取工序标签IOT数据
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual WipBatchQueue GetIOTWorkTask(WipBatchQueue queue, WipResource resource)
        {
            if (queue == null)
                throw new ValidationException("标签数据不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");
            queue = RF.GetById<WipBatchQueue>(queue.Id, new EagerLoadOptions().LoadWithViewProperty());
            //return queue;

            //调用IOT接口获取IOT任务数据
            var info = new IotTaskInfo()
            {
                TaskNo = queue.BatchNo,
                ResourceCode = resource.Code,
            };
            var data = RT.Service.Resolve<IIotTaskReport>().GetIotTaskInfo(info);

            if (data == null)
                throw new ValidationException("获取IOT数据失败");

            if (data.TaskNo != queue.BatchNo)
            {
                throw new ValidationException("IOT设备资源[{0}]标签号[{1}]与当前标签号[{2}]不一致,请确认".L10nFormat(resource.Code, data.TaskNo, queue.BatchNo));
            }
            //更新IOT产出数
            DB.Update<WipBatchQueue>().Set(p => p.IotQty, data.OutPutQty).Where(p => p.Id == queue.Id).Execute();

            queue = RF.GetById<WipBatchQueue>(queue.Id, new EagerLoadOptions().LoadWithViewProperty());

            return queue;
        }
        /// <summary>
        /// 暂停IOT任务单
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual WipBatchQueue PauseIOTWorkTask(WipBatchQueue queue, WipResource resource)
        {
            //暂停前,保存IOT产出数
            try
            {
                queue = RT.Service.Resolve<ReportController>().GetIOTWorkTask(queue, resource);
            }
            catch (Exception)
            {
            }
            //重置IOT任务信息
            var info = new IotTaskInfo()
            {
                TaskNo = queue.BatchNo + "-1",
                ResourceCode = resource.Code,
                InitQty = queue.IotQty
            };
            RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfo(info);

            //暂停任务单
            queue = RF.GetById<WipBatchQueue>(queue.Id, new EagerLoadOptions().LoadWithViewProperty());

            return queue;

        }

        /// <summary>
        /// IOT任务开工
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="resource"></param>
        /// <param name="IsSyncIotStatus"></param>
        /// <returns></returns>
        public virtual DispatchTask StartIOTWorkTask(DispatchTask dispatchTask, WipResource resource, bool IsSyncIotStatus = true)
        {
            if (dispatchTask == null)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");
            var task = RF.GetById<DispatchTask>(dispatchTask.Id, new EagerLoadOptions().LoadWithViewProperty());
            if (IsSyncIotStatus)
            {
                if (task.IotStatus == IotStatus.ToDispatch || task.IotStatus == IotStatus.Pause)
                {
                    //调用IOT接口下发开工指令
                    var info = new IotTaskInfo()
                    {
                        TaskNo = task.No,
                        ResourceCode = resource.Code,
                        InitQty = task.IotQty,
                    };
                    RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfo(info);
                }

                task.IotStatus = IotStatus.Executing;
            }
            task.TaskStatus = DispatchTaskStatus.Executing;
            task.OldTaskStatus = null;
            //更新IOT状态
            DB.Update<DispatchTask>()
                .Set(p => p.IotStatus, task.IotStatus)
                .Set(p => p.TaskStatus, task.TaskStatus)
                .Set(p => p.OldTaskStatus, task.OldTaskStatus)
                .Where(p => p.Id == dispatchTask.Id).Execute();
            task.MarkSaved();
            return task;
        }
        /// <summary>
        /// 获取IOT任务报工数据
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual DispatchTask GetIOTWorkTask(DispatchTask dispatchTask, WipResource resource)
        {
            if (dispatchTask == null)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");
            //调用IOT接口获取IOT任务数据
            var info = new IotTaskInfo()
            {
                TaskNo = dispatchTask.No,
                ResourceCode = resource.Code,
            };
            var data = RT.Service.Resolve<IIotTaskReport>().GetIotTaskInfo(info);
            //info.OutPutQty = dispatchTask.IotQty + 2;
            //var data = info;

            if (data == null)
                throw new ValidationException("获取IOT数据失败");

            if (data.TaskNo != dispatchTask.No)
            {
                throw new ValidationException("IOT设备资源[{0}]任务号[{1}]与当前任务号[{2}]不一致,请确认".L10nFormat(resource.Code, data.TaskNo, dispatchTask.No));
            }
            //更新IOT产出数
            DB.Update<DispatchTask>().Set(p => p.IotQty, data.OutPutQty).Where(p => p.Id == dispatchTask.Id).Execute();

            var task = RF.GetById<DispatchTask>(dispatchTask.Id, new EagerLoadOptions().LoadWithViewProperty());

            return task;
        }

        /// <summary>
        /// 暂停IOT任务单
        /// </summary>
        /// <param name="task"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual DispatchTask PauseIOTWorkTask(DispatchTask task, WipResource resource)
        {
            task = RF.GetById<DispatchTask>(task.Id, new EagerLoadOptions().LoadWithViewProperty());
            if (task.TaskStatus != DispatchTaskStatus.Executing)
            {
                throw new ValidationException("只有状态为执行中的派工单才能暂停，请检查".L10N());
            }
            if (task.IotStatus == IotStatus.Executing)
            {
                //暂停前,保存IOT产出数
                try
                {
                    task = RT.Service.Resolve<ReportController>().GetIOTWorkTask(task, resource);
                }
                catch (Exception ex)
                {
                }
                //重置IOT任务信息
                var info = new IotTaskInfo()
                {
                    TaskNo = task.No + "-1",
                    ResourceCode = resource.Code,
                    InitQty = task.IotQty
                };
                RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfo(info);
            }
            //暂停任务单
            task.OldTaskStatus = DispatchTaskStatus.Executing;
            task.TaskStatus = DispatchTaskStatus.Pause;
            task.IotStatus = IotStatus.Pause;
            DB.Update<DispatchTask>()
                .Set(p => p.IotStatus, task.IotStatus)
                .Set(p => p.TaskStatus, task.TaskStatus)
                .Set(p => p.OldTaskStatus, task.OldTaskStatus)
                .Where(p => p.Id == task.Id).Execute();
            task.MarkSaved();
            return task;

        }

        /// <summary>
        /// 开工IOT任务单(共模)
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="resource"></param>
        /// <param name="IsSyncIotStatus"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> StartIOTWorkTask(List<double> taskIds, WipResource resource, bool IsSyncIotStatus = true)
        {
            var tasks = GetDispatchTasksByIds(taskIds);
            if (tasks.Count == 0)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");

            if (IsSyncIotStatus)
            {
                if (tasks.Any(p => p.IotStatus == IotStatus.ToDispatch || p.IotStatus == IotStatus.Pause))
                {
                    //调用IOT接口下发开工指令
                    var info = tasks.Select(p => new IotTaskInfo()
                    {
                        TaskNo = p.No,
                        ResourceCode = resource.Code,
                        InitQty = p.IotQty,
                    }).ToList();
                    RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfos(info, resource.Code);
                }
                //更新任务单及IOT状态
                DB.Update<DispatchTask>()
                    .Set(p => p.IotStatus, IotStatus.Executing)
                    .Set(p => p.TaskStatus, DispatchTaskStatus.Executing)
                    .Set(p => p.OldTaskStatus, (DispatchTaskStatus?)null)
                    .Where(p => taskIds.Contains(p.Id)).Execute();
            }
            else
            {

                //更新任务单状态
                DB.Update<DispatchTask>()
                    .Set(p => p.TaskStatus, DispatchTaskStatus.Executing)
                    .Set(p => p.OldTaskStatus, (DispatchTaskStatus?)null)
                    .Where(p => taskIds.Contains(p.Id)).Execute();
            }

            tasks = GetDispatchTasksByIds(taskIds);
            return tasks;
        }

        /// <summary>
        /// 获取IOT产出数(共模)
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<DispatchTask> GetIOTWorkTask(List<double> taskIds, WipResource resource)
        {
            var tasks = GetDispatchTasksByIds(taskIds);
            if (tasks.Count == 0)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");

            var taskNos = tasks.OrderBy(t => t.No).Select(p => p.No).Concat(",");
            //调用IOT接口获取IOT任务数据
            var info = tasks.Select(p => new IotTaskInfo()
            {
                TaskNo = p.No,
                ResourceCode = resource.Code,
                //InitQty = p.IotQty,
            }).ToList();
            var datas = RT.Service.Resolve<IIotTaskReport>().GetIotTaskInfos(resource.Code);

            if (datas == null)
                throw new ValidationException("获取IOT数据失败");
            var iotTaskNos = datas.Where(p => p.TaskNo.IsNotEmpty()).OrderBy(p => p.TaskNo).Select(p => p.TaskNo).Concat(","); ;
            if (iotTaskNos != taskNos)
            {
                throw new ValidationException("IOT设备资源[{0}]任务号[{1}]与当前任务号[{2}]不一致,请确认".L10nFormat(resource.Code, iotTaskNos, taskNos));
            }
            //更新IOT产出数
            foreach (var task in tasks)
            {
                var data = datas.FirstOrDefault(p => p.TaskNo == task.No);
                if (data != null)
                {
                    //data.OutPutQty = task.IotQty + 50;
                    DB.Update<DispatchTask>().Set(p => p.IotQty, data.OutPutQty).Where(p => p.Id == task.Id).Execute();
                }
            }
            tasks = GetDispatchTasksByIds(taskIds);
            return tasks;
        }


        /// <summary>
        /// 暂停IOT任务单(共模)
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<DispatchTask> PauseIOTWorkTask(List<double> taskIds, WipResource resource)
        {
            var tasks = GetDispatchTasksByIds(taskIds);
            if (tasks.Count == 0)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");

            var ids = tasks.Where(p => p.IotStatus == IotStatus.Executing).Select(p => p.Id).ToList();
            if (ids.Count > 0)
            {
                //暂停前,保存IOT产出数
                try
                {
                    tasks = RT.Service.Resolve<ReportController>().GetIOTWorkTask(ids, resource);
                }
                catch (Exception)
                {
                }
                //重置IOT任务信息
                var info = tasks.Select(p => new IotTaskInfo()
                {
                    TaskNo = p.No + "-1",
                    ResourceCode = resource.Code,
                    InitQty = p.IotQty,
                }).ToList();
                RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfos(info, resource.Code);

            }
            //暂停任务单
            DB.Update<DispatchTask>().Set(p => p.IotStatus, IotStatus.Pause).Where(p => taskIds.Contains(p.Id) && p.IotStatus == IotStatus.Executing).Execute();
            DB.Update<DispatchTask>().Set(p => p.TaskStatus, DispatchTaskStatus.Pause).Set(p => p.OldTaskStatus, DispatchTaskStatus.Executing).Where(p => taskIds.Contains(p.Id) && p.TaskStatus == DispatchTaskStatus.Executing).Execute();

            tasks = GetDispatchTasksByIds(taskIds);
            return tasks;
        }

        /// <summary>
        /// 完工IOT状态(共模)
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> FinishIotStatus(List<double> taskIds, WipResource resource)
        {
            var tasks = GetDispatchTasksByIds(taskIds);
            if (tasks.Count == 0)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");

            var finishTasks = tasks.Where(p => p.TaskStatus == DispatchTaskStatus.Finished /*&& p.IotStatus != IotStatus.Executing*/).ToList();
            if (finishTasks.Count > 0)
            {
                //重置IOT任务信息
                var info = finishTasks.Select(p => new IotTaskInfo()
                {
                    TaskNo = p.No + "-2",
                    ResourceCode = resource.Code,
                    InitQty = p.IotQty
                }).ToList();
                RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfos(info, resource.Code);

                var ids = finishTasks.Select(p => p.Id).ToList();
                //完工IOT状态
                DB.Update<DispatchTask>().Set(p => p.IotStatus, IotStatus.Finished).Where(p => ids.Contains(p.Id)).Execute();

                tasks = GetDispatchTasksByIds(taskIds);
            }

            return tasks;

        }
        /// <summary>
        /// 完工IOT状态
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual DispatchTask FinishIotStatus(DispatchTask dispatchTask, WipResource resource)
        {
            if (dispatchTask == null)
                throw new ValidationException("任务单不能为空");
            if (resource == null)
                throw new ValidationException("资源不能为空");

            if (dispatchTask.IotStatus == IotStatus.Executing)
            {
                //重置IOT任务信息
                var info = new IotTaskInfo()
                {
                    TaskNo = dispatchTask.No + "-2",
                    ResourceCode = resource.Code,
                    InitQty = dispatchTask.IotQty
                };
                RT.Service.Resolve<IIotTaskReport>().SetIotTaskInfo(info);
            }
            //完工IOT状态
            DB.Update<DispatchTask>().Set(p => p.IotStatus, IotStatus.Finished).Where(p => p.Id == dispatchTask.Id).Execute();

            var task = RF.GetById<DispatchTask>(dispatchTask.Id, new EagerLoadOptions().LoadWithViewProperty());

            return task;


        }
        /// <summary>
        /// 根据报工方案做校验
        /// </summary>
        /// <param name="reportStaffId"></param>
        /// <param name="dispatchTask"></param>
        /// <param name="workReportPlan"></param>
        /// <exception cref="ValidationException"></exception>
        private void CheckByWorkplan(double reportStaffId, DispatchTask dispatchTask, WorkReportPlan workReportPlan = null)
        {
            //获取报工方案，取校验逻辑

            if (workReportPlan == null)
            {
                workReportPlan = this.GetWorkReportPlan(dispatchTask.ProcessId);
            }
            ////报工方案配置不为空时候 需校验开工条件
            if (workReportPlan != null)
            {
                if (workReportPlan.IsCheckWOStatus)
                {
                    bool isMergeReport = IsMergeReport(dispatchTask);
                    this.ValidateTaskWorkOrder(dispatchTask, isMergeReport);
                }
                if (workReportPlan.IsCheckEmployeeSkills)
                {
                    RT.Service.Resolve<WorkReportPlansController>().CheckEmployeeSkills(
                        reportStaffId,
                        new EntityList<DispatchTask>() { dispatchTask },
                        out string erroMessage);
                    if (!string.IsNullOrEmpty(erroMessage))
                    {
                        throw new ValidationException(erroMessage);
                    }
                }
                if (workReportPlan.IsEquipmentSpotCheck)//设备点检 待需求完善
                {

                }
                if (workReportPlan.IsMaterialKitCompleteness)//齐套校验 待需求完善
                {

                }
                if (workReportPlan.IsMoldSpotCheck)//模具校验 待需求完善
                {

                }
            }
        }
        private void ReportCheckByWorkplan(double reportStaffId, DispatchTask dispatchTask, WorkReportPlan workReportPlan = null)
        {
            //获取报工方案，取校验逻辑

            if (workReportPlan == null)
            {
                workReportPlan = this.GetWorkReportPlan(dispatchTask.ProcessId);
            }
            ////报工方案配置不为空时候 需校验开工条件
            if (workReportPlan != null)
            {
                if (workReportPlan.IsReportCheckWOStatus)
                {
                    bool isMergeReport = IsMergeReport(dispatchTask);
                    this.ValidateTaskWorkOrder(dispatchTask, isMergeReport);
                }
                if (workReportPlan.IsRepCheckEmpSkills)
                {
                    RT.Service.Resolve<WorkReportPlansController>().CheckEmployeeSkills(
                        reportStaffId,
                        new EntityList<DispatchTask>() { dispatchTask },
                        out string erroMessage);
                    if (!string.IsNullOrEmpty(erroMessage))
                    {
                        throw new ValidationException(erroMessage);
                    }
                }
                if (workReportPlan.IsReportEquipmentStatus) //IsReportEquipmentSpotCheck)//设备点检 待需求完善
                {

                }
                if (workReportPlan.IsRepMaterialKitComp)//IsMaterialKitCompleteness)//齐套校验 待需求完善
                {

                }
                if (workReportPlan.IsReportMoldStatus)//模具校验 待需求完善
                {

                }
            }
        }



        /// <summary>
        /// 开工
        /// 开工条件：
        /// 1、派工任务配置项是否设置报工顺序--Y 存在优先级级别比当前任务高--Y提示优先级更高的任务必须先完工--结束
        ///                               --N 可以开工
        /// 2、可以开工时验证是否工单首任务--Y工单状态修改为生产中
        ///                             --N结束
        /// </summary>
        /// <param name="task">任务单</param>
        public virtual void StartWork(DispatchTask task)
        {
            //验证开工条件
            ValidateStartWork(task);
            string[] workorderNos = task.AssociatedWorkOrder.Split(';');
            EntityList<WorkOrder> wolist = new EntityList<WorkOrder>();
            if (workorderNos.Length > 0)
            {
                //验证工单首工序
                Expression<Func<WorkOrder, bool>> exp = p => workorderNos.Contains(p.No) && p.State == Core.WorkOrders.WorkOrderState.Release;
                var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(exp, null);
                foreach (var no in workorderNos)
                {
                    var wo = workOrders.FirstOrDefault(p => p.No == no);
                    if (wo == null)
                    {
                        continue;
                    }
                    if (!IsWorkOrderFirstTaskStart(wo.Id))
                    {
                        if (wo.IsPause == YesNo.Yes)
                        {
                            throw new ValidationException("工单已暂停，不能进行开工操作".L10N());
                        }
                        wo.State = Core.WorkOrders.WorkOrderState.Producing;
                        wo.ActuStartDate = RF.Find<WorkOrder>().GetDbTime();
                        wolist.Add(wo);
                    }
                }
            }
            var associatedTasks = RT.Service.Resolve<DispatchController>().GetAssociatedDispatchTaskList(task.Id).Select(p => p.Task).AsEntityList(); //关联任务单
            associatedTasks.ForEach(p => p.TaskStatus = DispatchTaskStatus.Executing);
            task.TaskStatus = DispatchTaskStatus.Executing;

            // 开工创建开始记录
            var logs = GenerateTaskOptStartLog(new EntityList<DispatchTask> { task });

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(wolist);
                RF.Save(task);
                RF.Save(associatedTasks);
                RF.Save(logs);
                //发布任务开工消息
                EntityList<DispatchTask> dispatchTasks = new EntityList<DispatchTask>();
                dispatchTasks.Add(task);
                dispatchTasks.AddRange(associatedTasks);
                RT.EventBus.Publish(new DispatchTaskStartUp(dispatchTasks));
                tran.Complete();
            }
        }

        /// <summary>
        /// 判断是否工单首任务开工
        /// 工单的所有任务单都没开工，则为首任务单
        /// </summary>
        /// <returns>首任务开工返回true，不是返回false</returns>
        private bool IsWorkOrderFirstTaskStart(double workOrderId)
        {
            return Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId && p.TaskStatus >= DispatchTaskStatus.Executing).Count() > 0;
        }

        /// <summary>
        /// 任务单开工验证
        /// </summary>
        /// <param name="task">任务单</param>
        private void ValidateStartWork(DispatchTask task)
        {
            if (task.ReportMode == ReportMode.Auto)
            {
                throw new ValidationException("手动报工的任务单才能开工".L10N());
            }
            if (task.TaskStatus != DispatchTaskStatus.Dispatched)
            {
                throw new ValidationException("任务单状态不是已派工，不能进行开工操作".L10N());
            }
            ValidateIsCanStartTask(task.PlanBeginTime, task.Priority, true);
        }

        /// <summary>
        /// 验证派工任务是否可以开工
        /// </summary>
        /// <param name="curPlanBeginTime">当前开工任务计划开始时间</param>
        /// <param name="priority">当前开工任务优先级</param>
        /// <param name="throwExc">不能开工是否抛异常</param>
        /// <returns>能开工返回true，不能开工返回false</returns>
        void ValidateIsCanStartTask(DateTime curPlanBeginTime, DispatchTaskPriority priority, bool throwExc)
        {
            var config = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            ValidateIsCanStartTask(config, curPlanBeginTime, priority, throwExc);
        }

        /// <summary>
        /// 验证派工任务是否可以开工
        /// </summary>
        /// <param name="curPlanBeginTime">当前开工任务计划开始时间</param>
        /// <param name="priority">当前开工任务优先级</param>
        /// <param name="throwExc">不能开工是否抛异常</param>
        /// <returns>能开工返回true，不能开工返回false</returns>
        bool ValidateIsCanStartTask(DispatchTaskConfigValue config, DateTime curPlanBeginTime, DispatchTaskPriority priority, bool throwExc)
        {
            if (config == null || !config.ReportOrder.HasValue)
            {
                return true;
            }
            var needTask = GetFirstNeedStartTask(config.ReportOrder.Value, curPlanBeginTime, priority);
            if (needTask != null && throwExc)
            {
                throw new ValidationException("任务列表配置[{0}]，需先执行任务单[{1}]".L10nFormat(config.ReportOrder.ToLabel(), needTask.No));
            }
            return needTask == null;
        }

        /// <summary>
        /// 获取执行顺序的第一条数据（排除当前选择的）
        /// </summary>
        /// <param name="orderMod">执行顺序方式</param>
        /// <param name="curPlanBeginTime">当前任务计划开始时间</param>
        /// <returns>任务</returns>
        DispatchTask GetFirstNeedStartTask(ReportOrder orderMod, DateTime curPlanBeginTime, DispatchTaskPriority priority)
        {
            var query = Query<DispatchTask>().Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id)
                .Join<Employee>((p, employee) => employee.Id == RT.IdentityId && (p.AdoType == AdoType.Employee && p.AdoId == RT.IdentityId ||
                   p.AdoType == AdoType.EmployeeGroup && employee.EmployeeGroupId > 0 && employee.EmployeeGroupId == p.AdoId
                   || p.AdoType == AdoType.WorkGroup && employee.WorkGroupId > 0 && employee.WorkGroupId == p.AdoId)));

            query.Where(p => p.TaskStatus < DispatchTaskStatus.Pause);
            query.Where(p => p.ReportMode == ReportMode.Manual);
            if (orderMod == ReportOrder.BeginDate)
            {
                query.Where(p => p.PlanBeginTime < curPlanBeginTime);
            }
            else
            {
                //如果当前单据状态是紧急，则返回
                if (priority == DispatchTaskPriority.Urgency)
                    return null;
                query.Where(p => p.Priority == DispatchTaskPriority.Urgency);
            }
            return query.OrderBy(p => p.PlanBeginTime).FirstOrDefault();
        }
        #endregion

        #region 报工
        /// <summary>
        /// 获取派工任务单报工信息
        /// </summary>
        /// <param name="dispatchTaskId">派工任务单ID</param>
        /// <returns>任务单报工信息</returns>
        public virtual ReportTaskInfo GetReportTaskRecordInfo(double dispatchTaskId)
        {
            var record = RT.Service.Resolve<ReportController>().GetOrCreateMainReportRecord(dispatchTaskId);
            var task = record.DispatchTask;
            WorkOrder wo = null;
            if (task.WorkOrderId == null)
            {
                string wordorderno = task.AssociatedWorkOrder.Split(';')[0];
                wo = Query<WorkOrder>().Where(p => p.No == wordorderno).ToList(null, new EagerLoadOptions().LoadWithViewProperty())[0];
            }
            else
            {
                wo = RF.GetById<WorkOrder>(task.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
            }
            const string ymdhms = "yyyy/MM/dd HH:mm:ss";
            const string hms = "HH:mm:ss";
            ReportTaskInfo info = new ReportTaskInfo()
            {
                TaskId = task.Id,
                TaskNo = task.No,
                TaskQty = task.DispatchQty,
                ItemId = wo.ProductId,
                ItemName = wo.WorkOrderProductName,
                WorkOrderId = wo.Id,
                WorkOrderNo = wo.No,
                AssociatedWorkOrder = task.AssociatedWorkOrder,
                WorkShopName = wo.WorkShopName,
                ResourceId = wo.ResourceId,
                ResourceName = wo.ResourceName,
                ProcessId = record.ProcessId,
                ProcessName = record.ProcessName,
                StationId = record.StationId,
                OkQty = task.OkQty,
                NgQty = task.NgQty,
                ToReportQty = task.DispatchQty - task.ReportQty - task.SuspectQty,
                ReportOkQty = record.OkQty,
                ReportNgQty = record.NgQty,
                Hour = record.Hour,
                Proportion = task.Proportion,
                BatchNo = RT.Service.Resolve<ReportController>().GetReportBatchNo(),
                //BatchNo = record.BatchNo,
                Remark = record.Remark,
                IsSyntype = true,
                PlanBeginDate = task.PlanBeginTime,
                PlanEndDate = task.PlanEndTime,
                ActualBeginDate = task.BeginTime,
                PdaPlanTime = (task.PlanBeginTime.ToString(ymdhms) + " - " + task.PlanEndTime.ToString(hms)),
                PdaActualTime = task.BeginTime == null ? "" : (task.BeginTime.Value.ToString(ymdhms) + " - " + task.EndTime?.ToString(hms)),
                DefectNames = string.Join("、", record.Defects.Select(p => p.Defect.Description)),
                Equipments = string.Join("、", task.Details.SelectMany(p => p.Equipments).Select(p => p.Equipment.Name)),
                IsTaskFinish = true,
                IsValidatePrepare = true,
            };
            info.DefectIds.AddRange(record.Defects.Select(p => p.DefectId));
            var syntypeTasks = GetIsSyntypeTasks(dispatchTaskId, true);
            if (syntypeTasks.Count > 0)
                info.SyntypeTaskInfos.AddRange(syntypeTasks);
            return info;
        }

        /// <summary>
        /// 获取共模任务单信息
        /// </summary>
        /// <param name="dispatchTaskId">主任务单ID</param>
        /// <returns>共模任务单信息列表</returns>
        public virtual List<ReportTaskInfo> GetIsSyntypeTasks(double dispatchTaskId, bool toPDA = false)
        {
            List<ReportTaskInfo> rst = new List<ReportTaskInfo>();
            var task = RF.GetById<DispatchTask>(dispatchTaskId);
            if (task == null || !task.IsSyntype || !task.IsSyntypeReport)
                return rst;
            double mainTaskId = 0;
            if (task.IsMainTask)
                mainTaskId = dispatchTaskId;
            else
            {
                //辅单先找到主单，多个的时候排序取第一个
                //考虑下如果第一个已经报完，显示第二个
                mainTaskId = GetMainTaskId(dispatchTaskId).Id;
            }
            //获取共模任务单 
            var associatedTasks = RT.Service.Resolve<DispatchController>().GetAssociatedTasks(p => p.DispatchTaskId == mainTaskId && p.Task.Associated == Associated.Syntype).Select(p => p.Task).ToList();
            if (associatedTasks.Count == 0)
                return rst;
            var mainTaskRecord = GetOrCreateMainReportRecord(dispatchTaskId);
            associatedTasks.ForEach(associatedTask =>
            {
                var record = GetOrCreateReportRecord(associatedTask.Id, associatedTask);
                bool isNewRecord = record.PersistenceStatus == PersistenceStatus.New;
                ReportTaskInfo info = new ReportTaskInfo()
                {
                    TaskId = associatedTask.Id,
                    TaskNo = associatedTask.No,
                    TaskQty = associatedTask.DispatchQty,
                    ToReportQty = associatedTask.DispatchQty - record.ReportQty,
                    BatchNo = record.BatchNo,
                    ReportNgQty = isNewRecord ? 0 : record.NgQty,
                    ReportOkQty = isNewRecord ? (mainTaskRecord.OkQty + mainTaskRecord.NgQty) * (decimal)associatedTask.Proportion / (decimal)task.Proportion : record.OkQty,
                    RecordId = record.Id,
                    IsSyntype = true,
                    OkQty = associatedTask.OkQty,
                    NgQty = associatedTask.NgQty,
                    Proportion = associatedTask.Proportion,
                    WorkOrderId = record.WorkOrderId.Value,
                    ItemId = record.WorkOrder.ProductId,
                    ProcessId = record.ProcessId,
                    DefectNames = record.DefectNames
                };
                if (record.DefectIds != null)
                    info.DefectIds.AddRange(record.DefectIds);
                if (toPDA)
                {
                    var wo = RF.GetById<WorkOrder>(associatedTask.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                    if (wo != null)
                    {
                        info.ItemId = wo.ProductId;
                        info.ItemName = wo.WorkOrderProductName;
                        info.WorkOrderId = wo.Id;
                        info.WorkOrderNo = wo.No;
                        info.WorkShopName = wo.WorkShopName;
                        info.ResourceId = wo.ResourceId;
                        info.ResourceName = wo.ResourceName;
                    }

                    info.ProcessName = record.ProcessName;
                    info.StationId = record.StationId;
                    info.ToReportQty = associatedTask.DispatchQty - record.ReportQty;
                    info.Hour = record.Hour;
                    info.Remark = record.Remark;
                    info.PlanBeginDate = associatedTask.PlanBeginTime;
                    info.PlanEndDate = associatedTask.PlanEndTime;
                    info.ActualBeginDate = associatedTask.BeginTime;
                    info.PdaPlanTime = (associatedTask.PlanBeginTime.ToString("yyyy/MM/dd HH:mm:ss") + " - " + associatedTask.PlanEndTime.ToString("HH:mm:ss"));
                    info.PdaActualTime = associatedTask.BeginTime == null ? "" : (associatedTask.BeginTime.Value.ToString("yyyy/MM/dd HH:mm:ss") + " - " + associatedTask.EndTime?.ToString("HH:mm:ss"));
                }
                rst.Add(info);
            });
            return rst;
        }

        /// <summary>
        /// 获取主任务单Id
        /// </summary>
        /// <param name="dispatchTaskId">辅料单Id</param>
        /// <returns>主任务单Id</returns>
        private DispatchTask GetMainTaskId(double dispatchTaskId)
        {
            DispatchTask mainTask;
            var ass = RT.Service.Resolve<DispatchController>().GetAssociatedTaskListByTaskId(dispatchTaskId);
            if (ass.Count == 0) throw new ValidationException("关联表中没有找到任务单数据TaskId={0}".L10nFormat(dispatchTaskId));
            var config = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            if (config == null || !config.ReportOrder.HasValue || config.ReportOrder == ReportOrder.BeginDate)
            {
                var firTask = ass.Where(p => p.TaskTaskStatus == DispatchTaskStatus.Executing).OrderBy(p => p.TaskPlanBeginTime).FirstOrDefault();
                if (firTask == null)
                {//没有执行中的单//判断是否是被合并单,  状态：[合并行]是合并后的单，不会再次合并，所以不会存在于关联表TaskId中，[已合并]是被合并的单
                    var normalTask = ass.Where(p => p.MergedStatus == MergedStatus.Normal).OrderBy(p => p.TaskPlanBeginTime).FirstOrDefault();
                    if (normalTask != null)
                        mainTask = normalTask.DispatchTask;
                    else
                    { //都是被合并的单,通过关联表找合并单（主单）
                        var merDispatchTaskId = ass.FirstOrDefault().DispatchTaskId;
                        var assTask = RT.Service.Resolve<DispatchController>().GetAssociatedTaskByMergeTaskId(merDispatchTaskId);
                        if (assTask == null) throw new ValidationException("任务单[Id={0}]是已合并单，但关联表没有该单数据".L10nFormat(merDispatchTaskId));
                        mainTask = assTask.DispatchTask;
                    }
                }
                else//执行中的单不会包含已合并的单
                    mainTask = firTask.DispatchTask;
            }
            else
            {
                var firTask = ass.Where(p => p.TaskTaskStatus == DispatchTaskStatus.Executing).OrderByDescending(p => p.TaskPriority).ThenBy(p => p.TaskPlanBeginTime).FirstOrDefault();
                if (firTask == null)
                { //没有执行中的单,取紧急的单
                    var normalTask = ass.Where(p => p.MergedStatus == MergedStatus.Normal).OrderByDescending(p => p.TaskPriority).ThenBy(p => p.TaskPlanBeginTime).FirstOrDefault();
                    if (normalTask != null)
                        mainTask = normalTask.DispatchTask;
                    else
                    { //都是被合并的单,通过关联表找合并单（主单）
                        var merDispatchTaskId = ass.FirstOrDefault().DispatchTaskId;
                        var assTask = RT.Service.Resolve<DispatchController>().GetAssociatedTaskByMergeTaskId(merDispatchTaskId);
                        if (assTask == null) throw new ValidationException("任务单[Id={0}]是已合并单，但关联表没有该单数据".L10nFormat(merDispatchTaskId));
                        mainTask = assTask.DispatchTask;
                    }
                }
                else
                    mainTask = firTask.DispatchTask;
            }
            return mainTask;
        }

        /// <summary>
        /// 验证班组
        /// </summary>
        /// <returns>班组Id</returns>
        private double ValidateWorkGroupId(double employeeId)
        {
            var employee = RF.GetById<Employee>(employeeId);
            if (employee == null || !employee.WorkGroupId.HasValue)
                throw new ValidationException("员工未维护班组".L10N());
            return employee.WorkGroupId.Value;
        }
        #endregion

        #region 自动报工记录 
        /// <summary>
        /// 获取自动报工记录
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="batchNo">生产批次号</param>
        /// <returns>自动报工记录</returns>
        private AutoReportRecord GetAutoReportRecord(double workOrderId, Workcell workcell, string batchNo)
        {
            var query = Query<AutoReportRecord>()
                .Where(p => p.WorkOrderId == workOrderId && p.ProcessId == workcell.ProcessId && p.StationId == workcell.StationId && p.EmployeeId == workcell.EmployeeId);
            if (!batchNo.IsNullOrEmpty())
                query.Where(p => p.WipBatchNo == batchNo);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取工单剩余待自动报工记录
        /// </summary>
        /// <param name="workOrderId">工单Id</param> 
        /// <returns>自动报工记录集合</returns>
        private EntityList<AutoReportRecord> GetAutoReportRecords(double workOrderId)
        {
            return Query<AutoReportRecord>()
                .Where(p => p.WorkOrderId == workOrderId && p.OkQty > 0)
                .ToList();
        }

        /// <summary>
        /// 更新自动报工记录
        /// </summary>
        /// <param name="collectedResult">采集结果</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="isEndProcess">是否结束工序</param>
        /// <param name="okQty">合格数量</param>
        /// <param name="ngQty">不合格数量</param>
        /// <param name="batchNo">生产批次号</param>
        /// <returns>自动报工记录</returns>
        private AutoReportRecord UpdateAutoReportRecord(ResultType collectedResult, Workcell workcell, double workOrderId, double productId, bool isEndProcess, decimal okQty, decimal ngQty, string batchNo)
        {
            if (collectedResult != ResultType.Fail)
            {
                decimal reportQty = okQty + ngQty;
                var update = DB.Update<AutoReportRecord>()
                    .Set(p => p.ToReportQty, p => p.ToReportQty + reportQty)
                    .Set(p => p.TotalOkQty, p => p.TotalOkQty + okQty)
                    .Set(p => p.TotalNgQty, p => p.TotalNgQty + ngQty)
                    .Set(p => p.OkQty, p => p.OkQty + okQty)
                    .Set(p => p.NgQty, p => p.NgQty + ngQty)
                    .Where(p => p.WorkOrderId == workOrderId && p.ProcessId == workcell.ProcessId && p.StationId == workcell.StationId && p.EmployeeId == workcell.EmployeeId);
                if (!batchNo.IsNullOrEmpty())
                    update.Where(p => p.WipBatchNo == batchNo);
                var result = update.Execute();
                if (result == 0)
                {
                    var record = new AutoReportRecord()
                    {
                        WorkOrderId = workOrderId,
                        ProductId = productId,
                        WipResourceId = workcell.ResourceId,
                        ProcessId = workcell.ProcessId,
                        StationId = workcell.StationId,
                        IsEndProcess = isEndProcess,
                        EmployeeId = workcell.EmployeeId,
                        TotalNgQty = ngQty,
                        TotalOkQty = okQty,
                        ToReportQty = reportQty,
                        OkQty = okQty,
                        NgQty = ngQty,
                        WipBatchNo = batchNo
                    };
                    RF.Save(record);
                    return record;
                }
            }
            return GetAutoReportRecord(workOrderId, workcell, batchNo);
        }
        #endregion

        #region 报工（Web、移动端）
        #region 自动报工
        /// <summary>
        /// 验证生产自动报工任务执行顺序
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">员工ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>可以生产返回true，否则抛异常</returns>
        public virtual bool ValidateWipReport(double workOrderId, double employeeId, double processId)
        {
            //1、工单是否存在任务 
            var workOrderTasks = _dispatchController.GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && !p.IsVirtualPart);
            if (workOrderTasks.Count == 0)
            {
                return true;
            }
            if (workOrderTasks.Any(p => p.TaskStatus == DispatchTaskStatus.Pause))
            {
                throw new ValidationException("工单任务单[{0}]已暂停，请先恢复，再进行生产".L10nFormat(string.Join(",", workOrderTasks.Where(p => p.TaskStatus == DispatchTaskStatus.Pause).Select(p => p.No))));
            }
            //2、是否存在工序任务
            if (workOrderTasks.Any(p => p.ProcessId.HasValue) && !workOrderTasks.Any(p => p.ProcessId == processId))
            {
                return true;
            }
            EntityList<DispatchTask> toValidateTasks = new EntityList<DispatchTask>();
            var processTasks = workOrderTasks.Where(p => p.ProcessId == processId);
            if (processTasks.Any())
            {
                toValidateTasks.AddRange(processTasks);
            }
            else
            {
                toValidateTasks.AddRange(workOrderTasks);
            }
            if (toValidateTasks.Any(p => p.TaskStatus == DispatchTaskStatus.ToDispatch || p.TaskStatus == DispatchTaskStatus.Dispatching))
            {
                throw new ValidationException("工单存在未派工状态的工序任务单，请先派工".L10N());
            }
            if (toValidateTasks.Any(p => p.ReportMode != ReportMode.Auto))
            {
                throw new ValidationException("不允许采集，当前条码所属工单任务单报工方式非自动报工".L10N());
            }
            var employee = RF.GetById<Employee>(employeeId);
            if (employee == null)
            {
                throw new EntityNotFoundException(typeof(Employee), employeeId);
            }
            var taskDetails = toValidateTasks.SelectMany(p => p.Details);
            if (!taskDetails.Any(f => (f.AdoType == AdoType.Employee && f.AdoId == employeeId) || (f.AdoType == AdoType.WorkGroup && f.AdoId == employee.WorkGroupId) || (f.AdoType == AdoType.EmployeeGroup && f.AdoId == employee.EmployeeGroupId)))
            {
                throw new ValidationException("当前工单不在任务生产计划内".L10N());
            }
            return true;
        }

        /// <summary>
        /// 1、结束工序：Y：报更新固定数量任务和规格件任务
        /// 2、报工工序任务：            
        /// </summary>
        /// <param name="collectedEvent"> 采集后事件</param>
        public virtual void AutoTaskReport(WipCollectedEvent collectedEvent)
        {
            var collectedData = collectedEvent.Data;
            //if (collectedData.CollectData.Result == ResultType.Fail)
            //    return;
            var workOrderId = collectedData.Product.WorkOrderId;
            var product = collectedData.Product;
            var workcell = collectedData.Workcell;
            string barcode = collectedData.Barcodes[0].Code;
            var currentProcess = product.Routing.Current;
            var batch = collectedData.CollectData.OutputBatch;
            if (!CanReportProcess(currentProcess.Type))
                return;
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
                throw new ValidationException("报工工单不能为空".L10N());
            if (!IsAutoReportMode(workOrderId))
                return;
            if (ValidateRepeatReport(workcell, barcode, collectedData.Barcodes[0].Type) && (batch == null || !(batch.BatchNo.IsNotEmpty() || batch.SubBatchNo.IsNotEmpty())))
                return;
            bool isEndProcess = currentProcess.IsEnd;  //是否完工下线  

            decimal okQty;
            if (batch == null || !(batch.BatchNo.IsNotEmpty() || batch.SubBatchNo.IsNotEmpty()))
                okQty = product.Qty;
            else
                okQty = batch.Qty;
            var toReportRecord = UpdateAutoReportRecord(collectedData.CollectData.Result, workcell, workOrderId, workOrder.ProductId, isEndProcess, okQty, product.NgQty, batch?.BatchNo);
            if (toReportRecord == null)
            {
                return;
            }

            //虚拟件任务手动报工，自动报工不处理
            AutoTaskReport(collectedData.CollectDate, workOrderId, workcell.ResourceId, toReportRecord);
        }

        /// <summary>
        /// 自动报工
        /// </summary>
        /// <param name="collectedDate">报工时间</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="toReportRecord">自动报工记录</param>        
        private void AutoTaskReport(DateTime collectedDate, double workOrderId, double resourceId, AutoReportRecord toReportRecord)
        {
            EntityList<ReportRecord> records = CreateReportRecord(collectedDate, workOrderId, resourceId, toReportRecord);

            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                if (records.Count > 0)  //有报工
                {
                    toReportRecord.OkQty = 0;
                    toReportRecord.ToReportQty = 0;
                    RF.Save(toReportRecord);
                }

                RF.Save(records);

                records.ForEach(record =>
                {
                    UpdateDispatchTaskBeginTime(record.DispatchTaskId, collectedDate);
                    UpdateDispatchTaskQty(record);
                    AutoReportTaskStart(record.DispatchTaskId);
                    UpdateDispatchTaskState(record.DispatchTaskId, collectedDate);
                    //若任务存在相关物料需求，按单位定额更新物料使用信息
                    UpdateTaskProcessBom(record);
                });

                tran.Complete();
            }
        }

        /// <summary>
        /// 自动报工
        /// </summary>
        /// <param name="dispatchTask">待报工任务单</param>
        public virtual void AutoTaskReport(EntityList<DispatchTask> dispatchTask)
        {
            foreach (var task in dispatchTask)
            {
                if (task.ReportMode != ReportMode.Auto || !task.WorkOrderId.HasValue)
                    continue;
                DateTime reportTime = RF.Find<WorkOrder>().GetDbTime();
                var records = GetAutoReportRecords(task.WorkOrderId.Value);
                records.ForEach(record =>
                {
                    AutoTaskReport(reportTime, task.WorkOrderId.Value, record.WipResourceId, record);
                });
            }
        }

        /// <summary>
        /// 验证是否自动报工
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <exception cref="ValidationException">手动报工方式抛异常</exception>
        /// <returns>自动报工返回true，否则返回false</returns>
        private bool IsAutoReportMode(double workOrderId)
        {
            var model = RT.Service.Resolve<DispatchController>().GetTaskReportModel(workOrderId);
            if (model == null)
                return false;
            if (model == ReportMode.Manual)
                throw new ValidationException("自动报工失败，工单报工方式为手动报工".L10N());
            return true;
        }

        /// <summary>
        /// 验证重复报工
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <returns>重复返回true，否则返回false</returns>
        private bool ValidateRepeatReport(Workcell workcell, string barcode, BarcodeType type)
        {
            return RT.Service.Resolve<WipProductVersionController>().IsProcessMoveOutSuccess(barcode, type, workcell.ResourceId, workcell.ProcessId);
        }

        /// <summary>
        /// 判断工序是否可以报工
        /// 维修工序和返工工序不报工
        /// </summary>
        /// <param name="type">工序类型</param>
        /// <returns>能报工返回true，否则返回false</returns>
        private bool CanReportProcess(ProcessType type)
        {
            return !(type == ProcessType.Fix || type == ProcessType.BatchFix || type == ProcessType.Rework);
        }

        /// <summary>
        /// 创建报工记录
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>
        /// <param name="curTask"></param>
        /// <returns></returns>
        private ReportRecord CreateReportRecord(double dispatchTaskId, DispatchTask curTask)
        {
            DispatchTask task = new DispatchTask();
            if (!task.IsMainTask)
                task = RF.GetById<DispatchTask>(dispatchTaskId);
            else
                task = curTask;
            var reportrecord = new ReportRecord() { DispatchTask = task };
            reportrecord.OkQty = GetTaskToReportQty(task);
            reportrecord.WorkOrderId = task.WorkOrderId;
            reportrecord.ProcessId = task.ProcessId;
            var employee = RF.GetById<Employee>(RT.IdentityId);
            reportrecord.Principal = employee;
            reportrecord.PrincipalName = employee?.Name;
            reportrecord.TotalOkQty = task.OkQty;
            reportrecord.TotalNgQty = task.NgQty;
            reportrecord.DispatchTaskProportion = task.Proportion;
            reportrecord.DispatchTaskNo = task.No;
            reportrecord.WorkOrder = task.WorkOrder;
            return reportrecord;
        }

        /// <summary>
        /// 创建报工记录
        /// </summary>
        /// <param name="collectedDate">采集时间</param>
        /// <param name="toReportRecord">自动报工记录</param>        
        /// <param name="shift">班次</param>
        /// <param name="exp">报工任务查询条件</param>
        /// <returns>报工记录集合</returns>
        private EntityList<ReportRecord> CreateReportRecord(DateTime collectedDate, AutoReportRecord toReportRecord, Shift shift, Expression<Func<DispatchTask, bool>> exp)
        {
            var tasks = _dispatchController.GetDispatchTasksByExpression(exp);
            EntityList<ReportRecord> records = new EntityList<ReportRecord>();
            //根据规格件分组，规格件组内只报累计数量（数量+规格件+工序模式，同一个工序会有多个）
            tasks.GroupBy(p => p.SpecificationId).ForEach(gTask =>
            {
                var gTasks = gTask.ToList();
                //规格件任务，同类规格件累计报工数量=报工数
                decimal toReportOkQty = toReportRecord.OkQty;
                foreach (var speTask in gTasks)
                {
                    if (speTask.ReportQty == speTask.DispatchQty)
                    {
                        toReportOkQty -= speTask.DispatchQty;
                        continue;
                    }
                    if (toReportOkQty <= 0)//已经分配完
                        break;


                    //要求报工数量  单次报工数量  任务单剩余可报工数量
                    //1、要求报工数量剩余数 >= 任务单剩余可报工数量 <= 单次报工数量 --> 任务单剩余可报工数量
                    //2、要求报工数量剩余数 < 任务单剩余可报工数量 <= 单次报工数量  --> 要求报工数量剩余数
                    //3、任务单剩余可报工数量 > 单次报工数量 < 要求报工数量剩余数 --> 单次报工数量
                    //3、任务单剩余可报工数量 > 单次报工数量 >= 要求报工数量剩余数 --> 要求报工数量剩余数
                    var actualReportQty = CreateReportRecord(collectedDate, toReportRecord, shift, records, toReportOkQty, speTask);
                    toReportOkQty -= actualReportQty; //speTask.ReportQty;

                }
            });
            return records;
        }

        private decimal CreateReportRecord(DateTime collectedDate, AutoReportRecord toReportRecord, Shift shift, EntityList<ReportRecord> records,
              decimal toReportOkQty, DispatchTask speTask)
        {
            decimal taskToReportQty = (speTask.DispatchQty - speTask.ReportQty) / speTask.SingleQty;   //任务剩余可报工数

            //本次报工实际报工数量
            decimal actualReportQty = toReportOkQty >= taskToReportQty ? taskToReportQty : toReportOkQty;

            ReportRecord record = CreateReportRecord(collectedDate, toReportRecord, shift, speTask, actualReportQty);
            //toReportNgQty -= toReportNgQty;            
            records.Add(record);
            speTask.ReportQty += actualReportQty;
            return actualReportQty;
        }

        /// <summary>
        /// 创建报工记录
        /// </summary>
        /// <param name="collectedDate">报工时间</param>
        /// <param name="toReportRecord">自定报工记录</param>
        /// <param name="shift">班次</param>
        /// <param name="speTask">报工任务</param>
        /// <param name="toReportQty">报工数量</param>
        /// <returns>报工记录</returns>
        private ReportRecord CreateReportRecord(DateTime collectedDate, AutoReportRecord toReportRecord, Shift shift, DispatchTask speTask, decimal toReportQty)
        {
            decimal okQty = toReportQty * speTask.SingleQty;
            ReportRecord record = new ReportRecord()
            {
                DispatchTask = speTask,
                ReportQty = okQty,
                OkQty = okQty,
                //NgQty = toReportNgQty,
                BatchNo = GetReportBatchNo(),
                IsReport = true,
                ReportTime = collectedDate,
                Shift = shift,
                PrincipalId = toReportRecord.EmployeeId,
                //WorkGroupId = ValidateWorkGroupId(toReportRecord.EmployeeId),
                WorkOrderId = toReportRecord.WorkOrderId,
                ProcessId = toReportRecord.ProcessId,
                StationId = toReportRecord.StationId,
                ExamineState = ReportRecordExamineState.Confirmed,
            };
            return record;
        }

        /// <summary>
        /// 创建报工记录
        /// </summary>
        /// <param name="collectedDate">采集时间</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="toReportRecord">自动报工记录</param>        
        /// <returns>报工记录集合</returns>
        private EntityList<ReportRecord> CreateReportRecord(DateTime collectedDate, double workOrderId, double resourceId, AutoReportRecord toReportRecord)
        {
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(resourceId, collectedDate);
            EntityList<ReportRecord> records = new EntityList<ReportRecord>();
            Expression<Func<DispatchTask, bool>> exp = p => p.WorkOrderId == workOrderId && !p.IsVirtualPart && (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched);
            if (toReportRecord.IsEndProcess)
            {
                //更新固定数量任务&&规格件任务信息
                Expression<Func<DispatchTask, bool>> speExp = exp.And(p => p.ProcessId == null);
                records.AddRange(CreateReportRecord(collectedDate, toReportRecord, shift, speExp));
            }
            //更新工序任务信息
            Expression<Func<DispatchTask, bool>> exp1 = exp.And(p => p.ProcessId == toReportRecord.ProcessId);
            records.AddRange(CreateReportRecord(collectedDate, toReportRecord, shift, exp1));
            return records;
        }
        #endregion

        /// <summary>
        /// 检查当前用户是否有报工权限
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public virtual bool CheckUserPermission(double dispatchTaskId, double employeeId)
        {
            //如果派工类型不是员工、班组、员工组的都允许通过
            var dispatchTask = Query<DispatchTask>().Where(p => p.Id == dispatchTaskId).Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id)
                        .Join<Employee>((d, e) => e.Id == employeeId
                        && ((e.WorkGroupId == d.AdoId && d.AdoType == AdoType.WorkGroup)
                        || (e.EmployeeGroupId == d.AdoId && d.AdoType == AdoType.EmployeeGroup)
                        || (e.Id == d.AdoId && d.AdoType == AdoType.Employee)))).FirstOrDefault();
            var dispatchTaskWithStation = Query<DispatchTask>().Where(p => p.Id == dispatchTaskId).Exists<DispatchTaskDetail>
                ((x, y) => y.Where(p => p.DispatchTaskId == x.Id && p.AdoType == AdoType.Station)).FirstOrDefault();
            return dispatchTask != null || dispatchTaskWithStation != null;

        }

        /// <summary>
        /// 校验单任务报工 (同一条线（资源）+物料+工序，没有报工完，不允许下工单报工；)
        /// </summary>
        /// <param name="dispatchTask"></param>
        public virtual void ValidateReportSingleTask(DispatchTask dispatchTask)
        {
            var config = ConfigService.GetConfig(new ReportRecordDetailConfig(), typeof(ReportRecord));
            var process = config?.AllowMultiTaskReportProcess.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (config?.IsValidateReportSingleTask == true)
            {
                var task = DB.Query<DispatchTask>().Where(p =>
                    p.ResourceId == dispatchTask.ResourceId
                    && p.ProductId == dispatchTask.ProductId
                    && p.ProcessId == dispatchTask.ProcessId
                    && p.TaskStatus == DispatchTaskStatus.Executing
                    && p.Id != dispatchTask.Id
                    ).FirstOrDefault();
                if (process.Length > 0 && task != null && process.Contains(task.Process?.Code))
                {
                    task = null;
                }
                if (task != null)
                    throw new ValidationException("当前资源[{0}]派工单号[{1}]在生产执行中，请先完成该派工任务生产".L10nFormat(dispatchTask.ResourceCode, task.No));
            }
        }


        /// <summary>
        /// 手动任务报工（Web、移动端调用）
        /// </summary>
        /// <param name="taskInfo">任务报工信息</param>
        /// <param name="isReport">是否报工，true报工，false保存</param>
        /// <param name="isGetSyntypeTask">是否重新获取工模任务单信息</param>
        public virtual ReportRecord TaskReport(ReportTaskInfo taskInfo, bool isReport, bool isGetSyntypeTask = false)
        {
            if (taskInfo == null)
                throw new ValidationException("报工信息为空".L10N());
            var reportStaffId = taskInfo.StaffId.HasValue ? taskInfo.StaffId.Value : RT.IdentityId;
            //判断当前用户是否有报工权限
            if (!CheckUserPermission(taskInfo.TaskId, reportStaffId))
            {
                throw new ValidationException("您没有当前报工任务的报工权限".L10N());
            }
            WorkReportPlan workReportPlan = GetWorkReportPlan(taskInfo.ProcessId);

            var mainRecord = GetNewReportRecord(taskInfo.TaskId);
            var dispatchTask = RF.GetById<DispatchTask>(mainRecord.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            ReportCheckByWorkplan(reportStaffId, dispatchTask, workReportPlan);
            CheckTaskFinishInsp(dispatchTask);//验证是否需要首检，是时则验证是否已完成首检

            if (taskInfo.IsValidatePrepare)
            {
                //校验产前准备
                RT.Service.Resolve<ProcessPrepareRecordsController>().ValidateProcessPrepare(dispatchTask);
                //校验开机准备
                RT.Service.Resolve<PreStartupSetupRecordsController>().ValidateStartupSetupPrepare(dispatchTask);
            }
            //校验单任务报工
            if (!taskInfo.IsSuspect)
                ValidateReportSingleTask(dispatchTask);

            if (mainRecord.Id == 0)
            {
                mainRecord.GenerateId();
            }
            if (taskInfo.ReportEmployeeId > 0)
            {
                var employee = RF.GetById<Employee>(taskInfo.ReportEmployeeId);
                mainRecord.Principal = employee;
                mainRecord.PrincipalName = employee.Name;
            }
            mainRecord.ReportQty = taskInfo.OkQty + taskInfo.NgQty + taskInfo.ReworkQty;
            mainRecord.BatchNo = taskInfo.BatchNo;
            mainRecord.OkQty = taskInfo.OkQty;
            mainRecord.NgQty = taskInfo.NgQty;
            mainRecord.ReworkQty = taskInfo.ReworkQty;
            mainRecord.StationId = taskInfo.StationId;
            mainRecord.Hour = taskInfo.Hour;
            mainRecord.Remark = taskInfo.Remark;
            mainRecord.DispatchTaskId = taskInfo.TaskId;
            mainRecord.WorkOrderId = taskInfo.WorkOrderId;
            mainRecord.SuspectQty = taskInfo.SuspectQty;
            mainRecord.ExamineState = workReportPlan.IsNeedCheck ? ReportRecordExamineState.ToConfirm : ReportRecordExamineState.Confirmed;
            if (mainRecord.SuspectQty > 0)
                mainRecord.ExamineState = ReportRecordExamineState.ToConfirm;
            mainRecord.IsRework = taskInfo.ReworkQty > 0 ? true : false;
            mainRecord.SourceType = taskInfo.SourceType;
            if (taskInfo.BatchNo.IsNullOrEmpty() && !dispatchTask.IsVirtualPart && dispatchTask.Specification == null
                && (dispatchTask.EndProcess == true || dispatchTask.Process == null))
            {
                mainRecord.BatchNo = GetReportBatchNo();
            }
            if (isGetSyntypeTask)
            {
                taskInfo.SyntypeTaskInfos = GetIsSyntypeTasks(taskInfo.TaskId);
            }
            var layoutInfo = dispatchTask.WorkOrder.LayoutInfoList.Where(p => p.ProcessCode == dispatchTask.ProcessCode).FirstOrDefault();
            if (layoutInfo != null)
            {
                mainRecord.Vornr = layoutInfo.Vornr;
                mainRecord.Steus = layoutInfo.Steus;
                mainRecord.Zcode = layoutInfo.Zcode;
            }
            // 报工
            TaskReport(dispatchTask, taskInfo, mainRecord, isReport, workReportPlan);
            return mainRecord;
        }

        /// <summary>
        /// 获取工序对应的报工方案或系统默认方案
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private WorkReportPlan GetWorkReportPlan(double? processId)
        {
            WorkReportPlan workReportPlan = null;
            //存在工序则按工序去报工方案 如无则取回
            if (processId.HasValue)
            {
                workReportPlan = Query<WorkReportPlan>()
                  .Exists<ProcessInfo>((d, p) => p.Where(f => f.WorkReportPlanId == d.Id && f.ProcessId == processId.Value))
                  .Where(m => m.EnableStatus == true)
                  .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            }
            if (workReportPlan == null)
            {
                workReportPlan = Query<WorkReportPlan>().Where(m => m.IsDefault && m.EnableStatus == true).FirstOrDefault();
                if (workReportPlan == null)
                {
                    throw new ValidationException("报工方案未进行初始化或未启用，不能进行报工，请先进行初始化或启用作业！".L10N());
                }
            }

            return workReportPlan;
        }


        /// <summary>
        /// 生成物料耗用记录
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="taskInfo"></param>
        private EntityList<WoCostItem> GenerateConsumptionRecords(DispatchTask dispatchTask, ReportTaskInfo taskInfo)
        {
            EntityList<WoCostItem> woCostItems = new EntityList<WoCostItem>();
            //非规格件 且非虚拟件 
            if (dispatchTask.SpecificationId.HasValue || dispatchTask.IsVirtualPart)
            {
                return woCostItems;
            }

            if (dispatchTask.WorkOrder == null && !dispatchTask.AssociatedWorkOrder.IsNullOrEmpty())
            {
                var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(dispatchTask.AssociatedWorkOrder);
                if (wo == null)
                {
                    throw new ValidationException("任务单的工单丢失，无法生成物料耗用记录，请检查！".L10N());
                }
                dispatchTask.WorkOrder = wo;
            }
            List<WorkOrderBom> recoilItems;
            var productBase = taskInfo.OkQty;
            var processBomList = dispatchTask.WorkOrder.ProcessBomList;//工序BOM中的数据
            var bomList = dispatchTask.WorkOrder.BomList;//工单BOM中的数据
            if (dispatchTask.EndProcess == true || !dispatchTask.ProcessId.HasValue)//末工序倒扣 或//按数量报工
            {
                //存在于工单BOM为反冲物料同时在工序BOM中不存在的，才进行末工序倒扣
                recoilItems = bomList.Where(m => m.IsRecoilItem && !m.IsVritualItem).ToList();
                if (recoilItems.Any() && processBomList.Any())
                {
                    var processBomItemIds = processBomList.Select(m => m.ItemId).ToList();
                    recoilItems = recoilItems.Where(m => !processBomItemIds.Contains(m.ItemId)).ToList();
                }
                woCostItems.AddRange(CreateRecoilRecord(dispatchTask, taskInfo, recoilItems, ref productBase));
            }
            if (dispatchTask.ProcessId.HasValue)//按工序报工生产，每个工序执行倒扣
            {
                //以下逻辑每次报工都倒扣，前提在工单BOM为反冲物料同时存在工序bom中
                recoilItems = bomList.Where(m => m.IsRecoilItem && !m.IsVritualItem).ToList();
                if (recoilItems.Any() && processBomList.Any())
                {
                    productBase = taskInfo.OkQty;
                    var processBomItemIds = processBomList.Where(m => m.ProcessId == dispatchTask.ProcessId).Select(m => m.ItemId).ToList();
                    recoilItems = recoilItems.Where(m => processBomItemIds.Contains(m.ItemId)).ToList();
                    woCostItems.AddRange(CreateRecoilRecord(dispatchTask, taskInfo, recoilItems, ref productBase));
                }
            }
            return woCostItems;
        }

        /// <summary>
        /// 生成物料耗用记录
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="taskInfo"></param>
        private EntityList<WoCostItem> GenerateConsumptionRecords(DispatchTask dispatchTask, ReportRecord taskInfo)
        {
            EntityList<WoCostItem> woCostItems = new EntityList<WoCostItem>();
            //非规格件 且非虚拟件 
            if (dispatchTask.SpecificationId.HasValue || dispatchTask.IsVirtualPart)
            {
                return woCostItems;
            }

            if (dispatchTask.WorkOrder == null && !dispatchTask.AssociatedWorkOrder.IsNullOrEmpty())
            {
                var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(dispatchTask.AssociatedWorkOrder);
                if (wo == null)
                {
                    throw new ValidationException("任务单的工单丢失，无法生成物料耗用记录，请检查！".L10N());
                }
                dispatchTask.WorkOrder = wo;
            }
            List<WorkOrderBom> recoilItems;
            var productBase = taskInfo.OkQty;
            var processBomList = dispatchTask.WorkOrder.ProcessBomList;//工序BOM中的数据
            var bomList = dispatchTask.WorkOrder.BomList;//工单BOM中的数据
            if (dispatchTask.EndProcess == true || !dispatchTask.ProcessId.HasValue)//末工序倒扣 或//按数量报工
            {
                //存在于工单BOM为反冲物料同时在工序BOM中不存在的，才进行末工序倒扣
                recoilItems = bomList.Where(m => m.IsRecoilItem && !m.IsVritualItem).ToList();
                if (recoilItems.Any() && processBomList.Any())
                {
                    var processBomItemIds = processBomList.Select(m => m.ItemId).ToList();
                    recoilItems = recoilItems.Where(m => !processBomItemIds.Contains(m.ItemId)).ToList();
                }
                woCostItems.AddRange(CreateRecoilRecord(dispatchTask, taskInfo, recoilItems, ref productBase));
            }
            if (dispatchTask.ProcessId.HasValue)//按工序报工生产，每个工序执行倒扣
            {
                //以下逻辑每次报工都倒扣，前提在工单BOM为反冲物料同时存在工序bom中
                recoilItems = bomList.Where(m => m.IsRecoilItem && !m.IsVritualItem).ToList();
                if (recoilItems.Any() && processBomList.Any())
                {
                    productBase = taskInfo.OkQty;
                    var processBomItemIds = processBomList.Where(m => m.ProcessId == dispatchTask.ProcessId).Select(m => m.ItemId).ToList();
                    recoilItems = recoilItems.Where(m => processBomItemIds.Contains(m.ItemId)).ToList();
                    woCostItems.AddRange(CreateRecoilRecord(dispatchTask, taskInfo, recoilItems, ref productBase));
                }
            }
            return woCostItems;
        }

        /// <summary>
        /// 生成倒扣料记录
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="record"></param>
        /// <param name="recoilItems"></param>
        /// <param name="productBase"></param>

        private EntityList<WoCostItem> CreateRecoilRecord(DispatchTask dispatchTask, ReportRecord record, List<WorkOrderBom> recoilItems, ref decimal productBase)
        {
            EntityList<WoCostItem> woCostItems = new EntityList<WoCostItem>();
            if (!recoilItems.Any())
            {
                return woCostItems;
            }
            var productFamilyId = dispatchTask.Product.ProductFamilyId;
            if (!productFamilyId.HasValue)
            {
                return woCostItems;
            }
            var config = RT.Service.Resolve<ReportController>().GetReportRuleConfig(productFamilyId.Value);
            //如果配置项中，不合格数量计入倒扣基数=是 时，倒扣产品基数=报工单的合格数量+不合格数量，如果合格数量计入倒扣基数=否 时，倒扣产品基数=报工单的合格数量
            if (config.IsExpendItem)
            {
                productBase = record.OkQty + record.NgQty;
            }

            var dbtime = RF.Find<WoCostItem>().GetDbTime();
            woCostItems = RT.Service.Resolve<BackflushMaterialController>().CreateDeductItems(
                "",
                dispatchTask.ResourceId,
                dispatchTask.ProcessId,
                record.StationId,
                dispatchTask.FactoryId ?? 0,
                dispatchTask.WorkOrderId ?? 0,
                productBase,
                recoilItems,
                Core.Items.RetrospectType.Single,
                dbtime
            );
            RF.BatchInsert(woCostItems);
            woCostItems.MarkSaved();
            return woCostItems;
        }

        /// <summary>
        /// 生成倒扣料记录
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="taskInfo"></param>
        /// <param name="recoilItems"></param>
        /// <param name="productBase"></param>

        private EntityList<WoCostItem> CreateRecoilRecord(DispatchTask dispatchTask, ReportTaskInfo taskInfo, List<WorkOrderBom> recoilItems, ref decimal productBase)
        {
            EntityList<WoCostItem> woCostItems = new EntityList<WoCostItem>();
            if (!recoilItems.Any())
            {
                return woCostItems;
            }
            var productFamilyId = dispatchTask.Product.ProductFamilyId;
            if (!productFamilyId.HasValue)
            {
                return woCostItems;
            }
            var config = RT.Service.Resolve<ReportController>().GetReportRuleConfig(productFamilyId.Value);
            //如果配置项中，不合格数量计入倒扣基数=是 时，倒扣产品基数=报工单的合格数量+不合格数量，如果合格数量计入倒扣基数=否 时，倒扣产品基数=报工单的合格数量
            if (config.IsExpendItem)
            {
                productBase = taskInfo.OkQty + taskInfo.NgQty;
            }

            var dbtime = RF.Find<WoCostItem>().GetDbTime();

            woCostItems = RT.Service.Resolve<BackflushMaterialController>().CreateDeductItems(
                "",
                dispatchTask.ResourceId,
                dispatchTask.ProcessId,
                taskInfo.StationId,
                dispatchTask.FactoryId ?? 0,
                dispatchTask.WorkOrderId ?? 0,
                productBase,
                recoilItems,
                Core.Items.RetrospectType.Single,
                dbtime
            );

            RF.BatchInsert(woCostItems);
            woCostItems.MarkSaved();

            return woCostItems;
        }
        /// <summary>
        /// 替换字符串末尾位置中指定的字符串
        /// </summary>
        /// <param name="s">源串</param>
        /// <param name="searchStr">查找的串</param>
        /// <param name="replaceStr">替换的目标串</param>
        string TrimEndString(string s, string searchStr, string replaceStr)
        {
            var result = s;
            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    return result;
                }
                if (s.Length < searchStr.Length)
                {
                    return result;
                }
                if (s.IndexOf(searchStr, s.Length - searchStr.Length, searchStr.Length, StringComparison.Ordinal) > -1)
                {
                    result = s.Substring(0, s.Length - searchStr.Length);
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        /// <summary>
        /// 自动上料扣料(适用于成品工单包装报工)
        /// </summary>
        /// <param name="record"></param>
        /// <param name="task"></param>
        /// <param name="wipBatches"></param>
        private void AutoFeedingDeductionItems(ReportRecord record, DispatchTask task, EntityList<WipBatch> wipBatches)
        {
            var bomList = task.WorkOrder.ProcessBomList.Where(p => p.ProcessId == task.ProcessId).ToList();//工单工序BOM中的数据
            if (bomList.Count == 0)
                return;
            var dic = wipBatches.ToDictionary(p =>
            {
                if (p.BatchNo.Contains("*"))
                    return p.BatchNo.Split("*")[1];
                return p.BatchNo;
            });
            var labelNos = dic.Keys.ToList();
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelDatas(labelNos);
            if (itemLabels.Count != wipBatches.Count)
                throw new ValidationException("批次标签数据与物料标签数据不一致,自动上料失败");

            foreach (var itemLabel in itemLabels)
            {
                var bom = bomList.FirstOrDefault(p => p.ItemId == itemLabel.ItemId);
                var wipBatch = dic.GetValue<WipBatch>(itemLabel.Label, null);
                if (wipBatch == null)
                    throw new ValidationException("物料标签[{0}]未匹配到报工批次,自动上料失败");
                //生成上料记录
                FeedingRecord feedingRecord = new FeedingRecord()
                {
                    DispatchTaskId = task.Id,
                    ResourceId = task.ResourceId,
                    ItemLabelId = itemLabel.Id,
                    FeedingQty = wipBatch.Qty,
                    DeductedQty = wipBatch.Qty,
                    RemainingQty = 0,
                    PersistenceStatus = PersistenceStatus.New,
                    FeedingItemLabel = itemLabel.Label,
                    ItemId = itemLabel?.ItemId
                };
                RF.Save(feedingRecord);
                //生成扣料记录
                var deductionRecord = new DeductionRecord()
                {
                    ReportRecordId = record.Id,
                    ResourceId = feedingRecord.ResourceId,
                    ItemLabelId = feedingRecord.ItemLabelId,
                    DeductedQty = wipBatch.Qty,
                    SingleQty = bom?.SingleQty ?? 0,
                    Weight = bom.Weight ?? 0,
                    FeedingItemLabel = feedingRecord.FeedingItemLabel,
                };
                RF.Save(deductionRecord);
                //更新物料标签
                itemLabel.Qty -= wipBatch.Qty;
                if (itemLabel.Qty > 0)
                {
                    itemLabel.ItemLabelState = ItemLabelState.Receive;
                    itemLabel.Isuse = false;
                }
                RF.Save(itemLabel);
            }

        }

        /// <summary>
        /// 按报工记录批量扣料(凯中)
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="records"></param>
        private void DeductionItems(DispatchTask dispatchTask, EntityList<ReportRecord> records)
        {
            var wo = dispatchTask.WorkOrder;
            var bomList = dispatchTask.WorkOrder.ProcessBomList.Where(p => p.ProcessId == dispatchTask.ProcessId).ToList();//工单工序BOM中的数据
            if (bomList.Count == 0)
                return;
            //根据资源获取上料记录
            var feedingRecords = RT.Service.Resolve<FeedingRecordController>().GetFeedingRecordsByResourceId(dispatchTask.ResourceId ?? 0, new EagerLoadOptions().LoadWith(FeedingRecord.ItemLabelProperty).LoadWithViewProperty(), true);
            if (feedingRecords.Count == 0)
                throw new ValidationException("任务单对应的资源[{0}]没有上料数据".L10nFormat(dispatchTask.Resource?.Code));
            foreach (var record in records)
            {
                //按报工记录扣料
                var errors = DeductionItems(record, bomList, feedingRecords);
                if (errors.Count > 0)
                    throw new ValidationException(errors.Concat(";\r\n"));
            }
        }

        /// <summary>
        /// 执行扣料
        /// </summary>
        /// <param name="record"></param>
        /// <param name="bomList"></param>
        /// <param name="feedingRecords"></param>
        /// <returns></returns>
        private List<string> DeductionItems(ReportRecord record, List<WorkOrderProcessBom> bomList, EntityList<FeedingRecord> feedingRecords)
        {
            var errors = new List<string>();
            if (record == null)
                return errors;
            var deductionRecords = new EntityList<DeductionRecord>();

            //校验物料标签所在工厂与工单的工序BOM的发料工厂是否一致
            var config = ConfigService.GetConfig(new ItemLabelConfig(), typeof(ItemLabel));
            //不校验工厂物料清单
            var itemCodes = feedingRecords.Select(p => p.ItemCode).Distinct().ToList();
            var unValidFactoryItems = RT.Service.Resolve<ItemController>().GetUnValidFactoryItemsByItemCodes(itemCodes);

            foreach (var bom in bomList)
            {
                var deductionQty = record.ReportQty * (bom.Weight ?? 0);    //扣料数 = 报工数 * 取样净重
                if (record.ReportQty == 0 && record.SuspectQty > 0)  //适用可疑品扣料场景
                    deductionQty = (record.SuspectQty ?? 0) * (bom.Weight ?? 0);    //扣料数 = 报工数 * 取样净重
                var sumRemainingQty = feedingRecords.Where(p => p.ItemId == bom.ItemId).Sum(p => p.RemainingQty);
                if (deductionQty > sumRemainingQty)
                {
                    errors.Add("物料[{0}]上料数量不足".L10nFormat(bom.ItemCode));
                }
                if (errors.Count > 0)
                    continue;
                //按创建时间先后顺序扣料 (增加供料区优先扣料逻辑)
                foreach (var p in feedingRecords.Where(p => p.ItemId == bom.ItemId).OrderBy(p => p.DeductionSeq).ThenBy(p => p.CreateDate))
                {
                    if (deductionQty <= 0)
                        break;
                    if (p.RemainingQty <= 0)
                        continue;

                    if (config != null && config.IsValidFactory == true && unValidFactoryItems.All(a => a.ItemCode != p.ItemCode) && bom.Werks != p.ItemLabel.Factory.Code)
                    {
                        errors.Add("当前物料[{0}]，上料标签工厂[{1}]与发料工厂[{2}]不一致".L10nFormat(p.ItemCode, p.ItemLabel.Factory.Code, bom.Werks));
                    }
                    if (errors.Count > 0)
                        continue;

                    var tempQty = deductionQty < p.RemainingQty ? deductionQty : p.RemainingQty.Value;
                    p.DeductedQty = (p.DeductedQty ?? 0) + tempQty;
                    p.RemainingQty = p.FeedingQty - p.DeductedQty - p.BlankingQty;
                    p.ItemLabel.Qty -= tempQty;
                    if (p.ItemLabel.Qty < 0)
                        throw new ValidationException("物料标签[{0}]可用数不足以扣料".L10nFormat(p.ItemLabel.Label));
                    var deductionRecord = new DeductionRecord()
                    {
                        ReportRecordId = record.Id,
                        ResourceId = p.ResourceId,
                        ItemLabelId = p.ItemLabelId,
                        DeductedQty = tempQty,
                        SingleQty = bom.SingleQty,
                        Weight = bom.Weight,
                        FeedingItemLabel = p.FeedingItemLabel,
                    };
                    deductionRecords.Add(deductionRecord);
                    deductionQty -= tempQty;

                    p.ItemLabel.MarkSaved();
                    p.MarkSaved();

                    DB.Update<ItemLabel>()
                        .Set(x => x.Qty, x => x.Qty - tempQty)
                        .Where(x => x.Id == p.ItemLabelId).Execute();
                    DB.Update<FeedingRecord>()
                        .Set(x => x.DeductedQty, x => x.DeductedQty + tempQty)
                        .Set(x => x.RemainingQty, x => x.RemainingQty - tempQty)
                        .Where(x => x.Id == p.Id).Execute();
                }
            }
            RF.Save(deductionRecords);

            return errors;
        }

        /// <summary>
        /// 任务报工
        /// </summary>
        /// <param name="task">任务单</param>
        /// <param name="taskInfo">主任务单报工记录</param>
        /// <param name="mainRecord">主任务单报工记录</param>
        /// <param name="isReport">是否报工，true报工，false保存</param>
        /// <param name="workReportPlan">报工方案</param>
        void TaskReport(DispatchTask task, ReportTaskInfo taskInfo, ReportRecord mainRecord, bool isReport, WorkReportPlan workReportPlan)
        {
            var staffId = taskInfo.StaffId.HasValue ? taskInfo.StaffId : RT.IdentityId;
            List<double> defectIds = taskInfo.DefectIds;
            List<ReportTaskInfo> synTypeTaskInfo = taskInfo.SyntypeTaskInfos;

            ValidateTask(task, taskInfo);
            ValidateReportQty(mainRecord, task, isReport, taskInfo.IsSuspect, taskInfo.SourceType);

            EntityList<ReportRecord> records = new EntityList<ReportRecord>();
            records.Add(mainRecord);
            DateTime dbTime = RF.Find<ReportRecord>().GetDbTime();
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(task.ResourceId.Value, dbTime);  //资源可能没有

            bool isMergeReport = IsMergeReport(task);
            WorkOrder wo = this.ValidateTaskWorkOrder(task, isMergeReport);
            SetReportRecord(mainRecord, defectIds, wo, dbTime, shift, staffId);

            var toProductInspRecords = CreateMergeTaskReportRecord(mainRecord, shift, records, workReportPlan.IsNeedCheck);
            //成品报检
            bool isProductInsp = false;
            if (task.SpecificationCode.IsNullOrEmpty() && !task.IsVirtualPart && toProductInspRecords.Any())
                isProductInsp = ReportProductInsp(toProductInspRecords, mainRecord);
            if (isProductInsp) // 生成成品报检检验状态默认为待检
            {
                mainRecord.InspectionStatus = Enums.InspectionStatus.WaitInspection;
            }

            if (isReport && !isMergeReport && !taskInfo.IsSkipValidatePreQty)
                ValidateWorkOrderTaskOkQty(mainRecord, synTypeTaskInfo, task, wo, records, workReportPlan.IsReportQuantity);

            EntityList<WoCostItem> woCostItems = new EntityList<WoCostItem>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                mainRecord.IsReport = true;
                RF.Save(mainRecord);
                if (isReport && records.All(p => p.ExamineState == ReportRecordExamineState.Confirmed))
                {

                    records.ForEach(record =>
                    {
                        record.IsReport = true;
                        //更新派工任务信息
                        UpdateDispatchTaskBeginTime(record.DispatchTaskId, dbTime);
                        UpdateDispatchTaskQty(record);
                        UpdateDispatchTaskState(record.DispatchTaskId, dbTime, taskInfo.IsTaskFinish, taskInfo.SourceType);
                        //批次号生产物料标签
                        //ReportBatchGenerateItemLabel(record);
                        //若任务存在相关物料需求，按单位定额更新物料使用信息
                        UpdateTaskProcessBom(record);

                    });
                    if (!isProductInsp && task.EndProcess == true)
                    {
                        //工单状态和完工数更新
                        records.ForEach(record =>
                        {
                            UpdateTaskWorkOrder(record);
                        });

                        //成品入库
                        ReportProductStorage(mainRecord);
                    }
                    // 生成物料耗用记录
                    woCostItems.AddRange(GenerateConsumptionRecords(task, taskInfo));

                }
                RF.Save(records);

                //凯中客制扣料逻辑
                if (!taskInfo.IsAutoFeeding)
                    DeductionItems(task, records);

                tran.Complete();
            }

            if (woCostItems.Count > 0)
            {
                RT.Service.Resolve<BackflushMaterialController>().ExecuteBackflushMaterialAsync(woCostItems, dispatchTaskId: task.Id);
            }
        }

        /// <summary>
        /// 创建合并报工记录
        /// </summary>
        /// <param name="mainRecord">主任务报工记录</param>
        /// <param name="shift">班次</param>
        /// <param name="records">子任务报工记录</param>
        /// <param name="needCheck">是否启用报工确认</param>
        /// <returns>返回待成品报检的报工记录</returns>
        private EntityList<ReportRecord> CreateMergeTaskReportRecord(ReportRecord mainRecord, Shift shift, EntityList<ReportRecord> records, bool needCheck)
        {
            var toProductInspRecords = new EntityList<ReportRecord>();
            if (mainRecord.OkQty > 0)
                toProductInspRecords.Add(mainRecord);
            var task = mainRecord.DispatchTask;
            if (task.MergedStatus != MergedStatus.MergeRows)
                return toProductInspRecords;

            decimal toReportOkQty = mainRecord.OkQty;
            decimal toReportNgQty = mainRecord.NgQty;
            var childTasks = _dispatchController.GetMergeChildDispatchTask(task.Id).Select(p => p.Task);
            if (childTasks.Any())
                toProductInspRecords.Clear();
            foreach (var childTask in childTasks)
            {
                ValidateTaskWorkOrder(childTask.WorkOrderId ?? 0, true);
                if (toReportOkQty <= 0 && toReportNgQty <= 0)
                    break;
                decimal canReportQty = childTask.DispatchQty - childTask.ReportQty;  //任务单剩余可报工数量
                decimal actualReportOkQty = toReportOkQty <= canReportQty ? toReportOkQty : canReportQty;
                decimal canReportNgQty = canReportQty - actualReportOkQty;
                decimal actualReportNgQty = toReportNgQty <= canReportNgQty ? toReportNgQty : canReportNgQty;
                var record = new ReportRecord()
                {
                    DispatchTask = childTask,
                    ReportQty = actualReportOkQty + actualReportNgQty,
                    OkQty = actualReportOkQty,
                    NgQty = actualReportNgQty,
                    IsReport = true,
                    ReportTime = mainRecord.ReportTime,
                    Shift = shift,
                    PrincipalId = mainRecord.PrincipalId,
                    WorkGroupId = mainRecord.WorkGroupId,
                    WorkOrderId = childTask.WorkOrderId,
                    ProcessId = mainRecord.ProcessId,
                    StationId = mainRecord.StationId,
                    ExamineState = needCheck ? ReportRecordExamineState.ToConfirm : ReportRecordExamineState.Confirmed,
                    SourceId = mainRecord.Id,
                };

                if (record.NgQty > 0 && mainRecord.Defects.Count > 0)
                    record.Defects.AddRange(CreateDefects(record, mainRecord.Defects.Select(p => p.DefectId).Distinct().ToList()));
                toReportOkQty -= actualReportOkQty;
                toReportNgQty -= actualReportNgQty;
                records.Add(record);

                if (record.OkQty > 0)
                    toProductInspRecords.Add(record);
            }
            return toProductInspRecords;
        }

        /// <summary>
        /// 任务是否合并报工
        /// </summary>
        /// <param name="task">派工任务</param>
        /// <returns>存在合并报工，返回true，否则返回false</returns>
        public virtual bool IsMergeReport(DispatchTask task)
        {
            //只要任务单或者关联任务单的工单存在任务单合并情况的都不验证报工数量
            if (task.MergedStatus == MergedStatus.Merged || task.MergedStatus == MergedStatus.MergeRows)
                return true;
            else
                return _dispatchController.IsWorkOrderTaskMerge(task.WorkOrderNo);  //共模？？
        }

        /// <summary>
        /// 校验前一个工序是否生成任务单
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="ValidationException"></exception>
        private void ValidateHasCreateFrontTask(DispatchTask task)
        {
            var wo = task.WorkOrder;
            var layout = wo.LayoutInfoList.Where(p => p.ProcessCode == task.Process?.Name).FirstOrDefault();
            if (layout != null)
            {
                var frontLayouts = wo.LayoutInfoList.Where(p => Convert.ToDecimal(p.Vornr) < Convert.ToDecimal(layout.Vornr) && p.Steus != "PP04").ToList();
                var frontProcesses = frontLayouts.Select(p => p.ProcessCode).Distinct().ToList();

                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessNames(frontProcesses);
                foreach (var fontLayout in frontLayouts.OrderByDescending(p => Convert.ToDecimal(p.Vornr)))
                {
                    //改为只校验排程点
                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == fontLayout.ProcessCode && (p.Scheduling == true/* || p.DispatchWork == true*/));
                    if (processPty != null)
                    {
                        var fontTask = RT.Service.Resolve<DispatchController>().GetDispatchTaskByWoProcess(task.WorkOrderId.GetValueOrDefault(), processPty.ProcessId);
                        if (fontTask == null)
                            throw new ValidationException("前工序[{0}]未报工".L10nFormat(fontLayout.ProcessCode));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 验证工单任务累计报工合格数
        /// </summary>
        /// <param name="mainRecord">主任务单报工记录</param>
        /// <param name="synTypeTaskInfo">共模任务单报工记录集合</param>
        /// <param name="task">主任务单</param>
        /// <param name="wo">工单</param>
        /// <param name="records">待报工记录集合</param>
        /// <param name="isReportQuantity">报工数量允许大于前工序报工数量</param>
        private void ValidateWorkOrderTaskOkQty(ReportRecord mainRecord, List<ReportTaskInfo> synTypeTaskInfo, DispatchTask task, WorkOrder wo, EntityList<ReportRecord> records
            , bool isReportQuantity)
        {



            //1、验证工序任务
            if (task.ProcessId.HasValue)
            {
                //校验前工序是否已生成任务单
                ValidateHasCreateFrontTask(task);
                //2、验证是否首工序 
                if (IsWorkOrderFirstProcessTask(wo.Id, task.Seq))
                    ValidateVirtualPartReportQty(task, mainRecord, wo.Id);
                else
                    ValidateProcessSpecReportQty(task, mainRecord, isReportQuantity);
            }
            else
                ValidateVirtualPartReportQty(task, mainRecord, wo.Id);
            if (task.IsSyntype && task.IsSyntypeReport)
            {
                //共模任务单，且共模比报工，主任务单跟辅任务单同时报，且报工数量需要满足共模比
                if (synTypeTaskInfo.Count <= 0)
                    throw new ValidationException("共模比的共模任务单报工，辅任务单报工数据不能为空".L10N());
                synTypeTaskInfo.ForEach(synType =>
                {
                    records.Add(SynTypeTaskReport(mainRecord, synType));
                });
            }
        }

        /// <summary>
        /// 更新任务单工单完工数和工单状态
        /// </summary>
        /// <param name="record">报工记录</param>
        private void UpdateTaskWorkOrder(ReportRecord record)
        {
            if (record.DispatchTask.IsVirtualPart || record.DispatchTask.MergedStatus == MergedStatus.MergeRows)
                return; //虚拟件不需要更新工单数量
            if (!record.WorkOrderId.HasValue)
                return;
            double workOrderId = record.WorkOrderId.Value;
            decimal reportQty = 0;
            //是否存在工序任务，取结束工序任务的累计报工数
            reportQty = GetEndProcessTaskReportQty(workOrderId);
            if (reportQty == -1)
            {
                //是否存在规格件任务，取规格件任务的累计报工数
                reportQty = GetSpecificationTaskOkQty(workOrderId);
                if (reportQty == -1)
                {
                    //取固定数量
                    reportQty = GetFixedQtyTaskQty(workOrderId, p => p.OkQty);
                }
            }
            DB.Update<WorkOrder>().Set(p => p.FinishQty, (int)reportQty).Where(p => p.Id == workOrderId).Execute();

            //当工单的完工数大于等于计划数量*（1+交货容差%）时再把工单更新为完工
            var wo = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (wo != null)
            {
                decimal Uebto = 0;
                decimal.TryParse(wo.Uebto, out Uebto);

                decimal qty = 0;
                //通过接口获取集团【是否启用制卡数量维护】数据
                EntityList<MtartZtflRelation> ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(new List<string>() { wo.ProductMtart });
                //当启用了容差或者制卡数量为空、为0 的时候
                if (ztflRelations.Any(p => p.Mtart == wo.ProductMtart && p.IsUebto == true) || wo.Ztfl == null || wo.Ztfl == 0)
                    qty = wo.PlanQty * (1 + Uebto / 100);
                else
                    qty = wo.Ztfl.Value;


                if (wo.FinishQty >= (int)Math.Ceiling(qty))
                {
                    wo.State = Core.WorkOrders.WorkOrderState.Finish;
                    wo.ActuFinishDate = record.ReportTime;
                    wo.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(wo);
                }
            }
            //DB.Update<WorkOrder>().Set(p => p.State, Core.WorkOrders.WorkOrderState.Finish)
            //    .Set(p => p.ActuFinishDate, record.ReportTime).Where(p => p.Id == workOrderId && p.FinishQty >= p.PlanQty).Execute();
        }

        /// <summary>
        /// 更新任务单工序BOM用量
        /// </summary>
        /// <param name="record">报工记录</param>
        private void UpdateTaskProcessBom(ReportRecord record)
        {
            DB.Update<TaskProcessBom>()
                .Set(p => p.UseQty, p => p.UseQty + p.Qty * (record.OkQty + record.NgQty))
                .Where(p => p.DispatchTaskId == record.DispatchTaskId && p.ProcessId == record.ProcessId)
                .Execute();
        }

        /// <summary>
        /// 设置报工记录数据
        /// </summary>
        /// <param name="mainRecord">报工记录</param>
        /// <param name="defectIds">主任务单缺陷集合</param>
        /// <param name="wo">工单</param>
        /// <param name="dbTime">报工日期，数据库时间</param>
        /// <param name="shift">班制</param>
        /// <param name="staffId">员工Id</param>
        private void SetReportRecord(ReportRecord mainRecord, List<double> defectIds, WorkOrder wo, DateTime dbTime, Shift shift, double? staffId)
        {
            mainRecord.Shift = shift;
            mainRecord.WorkOrder = wo;
            //mainRecord.WorkGroupId = ValidateWorkGroupId(staffId.HasValue ? staffId.Value : RT.IdentityId);
            mainRecord.ReportTime = dbTime;
            mainRecord.PrincipalId = RT.IdentityId;
            if (mainRecord.NgQty > 0)
                mainRecord.Defects.AddRange(CreateDefects(mainRecord, defectIds));
        }

        /// <summary>
        /// 更新任务单数量
        /// </summary>
        /// <param name="record">报工记录</param>
        public virtual void UpdateDispatchTaskQty(ReportRecord record)
        {
            var records = GetReportRecords(record.DispatchTaskId, null);
            var okQty = records.Sum(p => p.OkQty);
            var ngQty = records.Sum(p => p.NgQty);
            var reworkQty = records.Sum(p => p.ReworkQty);
            decimal reportQty = okQty + ngQty + reworkQty;
            DB.Update<DispatchTask>()
                       .Set(p => p.OkQty, okQty)
                       .Set(p => p.NgQty, ngQty)
                       .Set(p => p.ReworkQty, reworkQty)
                       .Set(p => p.ReportQty, reportQty)
                       .Where(p => p.Id == record.DispatchTaskId)
                       .Execute();
        }

        /// <summary>
        /// 更新手工报工数量
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <param name="manualReportQty"></param>
        public virtual void UpdateDispatchTaskManualReportQty(double dispatchTaskId, decimal manualReportQty)
        {

            DB.Update<DispatchTask>()
                       .Set(p => p.ManualReportQty, p => p.ManualReportQty + manualReportQty)
                       .Where(p => p.Id == dispatchTaskId)
                       .Execute();
        }

        /// <summary>
        /// 更新任务单开始时间
        /// </summary>
        /// <param name="dispatchTaskId">任务单ID</param>
        /// <param name="dbTime">数据库时间</param>
        private void UpdateDispatchTaskBeginTime(double dispatchTaskId, DateTime dbTime)
        {
            DB.Update<DispatchTask>()
                          .Set(p => p.BeginTime, dbTime)
                          .Where(p => p.Id == dispatchTaskId && p.ReportQty <= 0)
                          .Execute();
        }

        /// <summary>
        /// 校验任务单是否直接完工(弹窗)
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="goodQty"></param>
        /// <param name="suspectQty"></param>
        /// <returns></returns>
        public virtual string ValidQtyUpdateTaskStatePopUp(DispatchTask dispatchTask, decimal goodQty, decimal suspectQty)
        {
            var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(dispatchTask.ProcessId.Value, dispatchTask.ProductId);
            var qty = dispatchTask.ReportQty + dispatchTask.SuspectQty + suspectQty + goodQty;

            //decimal Uebto = 0;
            //decimal.TryParse(dispatchTask.WorkOrder.Uebto, out Uebto);

            var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(dispatchTask);

            if (qty >= dispatchTask.DispatchQty && qty < maxReportQty)
            {
                if (processPty != null && processPty.IsReportValid == true)
                {
                    var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value);
                    var dic = tasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
                    decimal lastReportQty = 0;
                    decimal lastSuspectQty = 0;
                    if (dic != null && dic.Count > 0)
                    {
                        lastReportQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.ReportQty);
                        lastSuspectQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.SuspectQty);
                    }
                    return "首工序已报工数量{0}，可疑品数量{1}，当前任务数量{2},已报工数量{3}，可疑品数量{4}，是否将任务单已完成".L10nFormat(lastReportQty, lastSuspectQty, dispatchTask?.DispatchQty ?? 0, dispatchTask?.ReportQty ?? 0, dispatchTask?.SuspectQty ?? 0);
                }
            }
            return null;
        }

        /// <summary>
        /// 手工校验
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <returns></returns>
        public virtual bool ValidFirstProcessTaskState(DispatchTask dispatchTask)
        {
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value, null);

            var processTasks = tasks.Where(p => p.Id != dispatchTask.Id && p.ProcessId == dispatchTask.ProcessId).ToList();

            var Seq = tasks.OrderBy(p => p.Seq).FirstOrDefault().Seq;
            //首工序或者当前首工序多个任务单且其他单存在非完工、非关闭、非暂停
            if (Seq == dispatchTask.Seq || (processTasks.Count != 0 && processTasks.Any(p => p.TaskStatus != DispatchTaskStatus.Finished && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Pause)))
            {
                if (dispatchTask.ReportQty + dispatchTask.SuspectQty >= dispatchTask.DispatchQty)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 校验任务单是否直接完工(非首工序校验)(不弹窗)
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="goodQty"></param>
        /// <param name="suspectQty"></param>
        /// <returns></returns>
        public virtual bool ValidQtyUpdateTaskState(DispatchTask dispatchTask, decimal goodQty, decimal suspectQty)
        {
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value, null);
            var processTasks = tasks.Where(p => p.Id != dispatchTask.Id && p.ProcessId == dispatchTask.ProcessId).ToList();

            var dic = tasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
            //当没有前工序的时候，就跳过不校验
            if (dic.Any(p => p.Key < dispatchTask.Seq))
            {
                //获取前任务单
                var lastProTasks = dic.Where(p => p.Key < dispatchTask.Seq).OrderByDescending(p => p.Key).FirstOrDefault().Value;
                //当前任务单工序，没有其他任务单的时候(相同工单、同一工序，只有一条任务单的时候)
                if (processTasks.Count == 0)
                {
                    //当前任务单，已经报工数+当前工序的可疑品数 = 前工序合格数+前工序可疑品数时，更新为完工
                    if (dispatchTask.ReportQty + goodQty + dispatchTask.SuspectQty + suspectQty == lastProTasks.Sum(p => p.OkQty) + lastProTasks.Sum(p => p.SuspectQty))
                        return true;
                }
                //同工序，多个任务单的时候
                else
                {
                    //其他任务单都已经完成的时候，前工序的合格数+前工序的可疑品数-其他任务单的已报工数-其他任务单的可疑品数 = 当前任务单的已报工数+当前任务单的可疑品
                    if (processTasks.All(p => p.TaskStatus == DispatchTaskStatus.Finished))
                    {
                        if (lastProTasks.Sum(p => p.OkQty) + lastProTasks.Sum(p => p.SuspectQty) - processTasks.Sum(p => p.ReportQty) - processTasks.Sum(p => p.SuspectQty) == dispatchTask.ReportQty + goodQty + dispatchTask.SuspectQty + suspectQty)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 更新任务单状态
        /// </summary>
        /// <param name="taskId">任务单ID</param>
        /// <param name="dbTime">数据库时间</param>
        /// <param name="SourceType">报工类型</param>
        public virtual void UpdateDispatchTaskState(double taskId, DateTime dbTime, bool IsTaskFinish = true, Enums.SourceType? SourceType = null)
        {
            var dispatchTask = RF.GetById<DispatchTask>(taskId, new EagerLoadOptions().LoadWithViewProperty());
            if (dispatchTask == null)
            {
                throw new ValidationException("数据Id在系统中不存在对应的任务单，请检查".L10N());
            }

            //decimal Uebto = 0;
            //decimal.TryParse(dispatchTask.WorkOrder.Uebto, out Uebto);

            var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(dispatchTask);

            #region 

            //bool isFinish = false;
            //var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //var newMaterialPro = new List<string>();
            //if (!config.NewMaterialProValid.IsNullOrEmpty())
            //{
            //    newMaterialPro = config.NewMaterialProValid.Split(',').ToList();
            //}
            ////新材料的工序按照原来的方式去做
            //if (newMaterialPro.Count > 0 && newMaterialPro.Contains(dispatchTask.ProcessCode))
            //{
            //    //分两种情况完成任务单，是否满足dispatchTask.ReportQty + dispatchTask.SuspectQty >= dispatchTask.DispatchQty 或者满足dispatchTask.ReportQty + dispatchTask.SuspectQty == dispatchTask.DispatchQty * (1 + Uebto)
            //    if ((IsTaskFinish == true && dispatchTask.ReportQty + dispatchTask.SuspectQty >= dispatchTask.DispatchQty) || (IsTaskFinish == false && dispatchTask.ReportQty + dispatchTask.SuspectQty == dispatchTask.DispatchQty * (1 + Uebto / 100)))
            //    {
            //        isFinish = true;
            //    }
            //}
            ////非新材料的按照新逻辑
            //else
            //{
            //    //弹窗校验
            //    var popUpStr = ValidQtyUpdateTaskStatePopUp(dispatchTask, 0, 0);
            //    //当popUpStr为空的时候，就说明没有弹窗，那么就IsTaskFinish就默认为false，只有popUpStr不为null的时候，IsTaskFinish才有可能为true
            //    //逻辑:
            //    //  1.只有扫码报工、手动报工会弹窗，弹窗的时候，校验IsTaskFinish(true的时候)为true直接关闭(直接完工)，popUpStr为null，就证明不弹窗，不弹窗就直接按照其他逻辑去校验ValidQtyUpdateTaskState方法(非首工序校验)或者校验ValidFirstProcessTaskState方法(首工序校验)
            //    //  2.当不为扫码报工、不为手动报工时候，就不会弹窗，那么就要校验ValidQtyUpdateTaskState方法(非首工序校验)或者校验ValidFirstProcessTaskState方法(首工序校验)
            //    if ((SourceType != Enums.SourceType.Report_Manual && SourceType != Enums.SourceType.Report_Scan && (ValidQtyUpdateTaskState(dispatchTask, 0, 0) || ValidFirstProcessTaskState(dispatchTask))) ||
            //        ((SourceType == Enums.SourceType.Report_Scan || SourceType == Enums.SourceType.Report_Manual) && ((IsTaskFinish == true && popUpStr != null) || (popUpStr == null && (ValidQtyUpdateTaskState(dispatchTask, 0, 0) || ValidFirstProcessTaskState(dispatchTask)))))
            //        )
            //    {
            //        isFinish = true;
            //    }
            //}
            //if(isFinish)
            #endregion
            //分两种情况完成任务单，是否满足dispatchTask.ReportQty + dispatchTask.SuspectQty >= dispatchTask.DispatchQty 或者满足dispatchTask.ReportQty + dispatchTask.SuspectQty == dispatchTask.DispatchQty * (1 + Uebto)
            bool isFinish = false;
            var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //没有配置项就用原逻辑
            if (config.IsValidScanQty == true)
            {
                if (RT.Service.Resolve<DispatchController>().IsReportScanRemainQty(dispatchTask, SourceType))
                {
                    var allTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == dispatchTask.WorkOrderId, null);
                    var lastPTasks = allTasks.Where(p => p.Seq < dispatchTask.Seq).GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList()).OrderByDescending(p => p.Key).FirstOrDefault().Value;
                    var pTasks = allTasks.Where(p => p.ProcessId == dispatchTask.ProcessId).ToList();
                    //当 前工序的任务单状态全部为完工或关闭且可疑品为0时
                    if (lastPTasks != null && lastPTasks.All(p => (p.TaskStatus == DispatchTaskStatus.Finished || p.TaskStatus == DispatchTaskStatus.Closed) && p.SuspectQty == 0))
                    {
                        //当前任务单的（已报工数+可疑品数）+当前工序其余已完工任务单的（已报工数+可疑品数）>=前一工序的合格数时，将当前任务单的状态更新为完工；反之不完工
                        isFinish = pTasks.Sum(p => p.ReportQty + p.SuspectQty) >= lastPTasks.Sum(p => p.OkQty);
                    }
                }
            }
            else
            {
                isFinish = (IsTaskFinish == true && dispatchTask.ReportQty + dispatchTask.SuspectQty >= dispatchTask.DispatchQty) || (IsTaskFinish == false && dispatchTask.ReportQty + dispatchTask.SuspectQty == maxReportQty);
            }

            if (isFinish)
            {
                if (dispatchTask.TaskStatus != DispatchTaskStatus.Finished)
                {
                    var res = DB.Update<DispatchTask>()
                                .Set(p => p.TaskStatus, DispatchTaskStatus.Finished)
                                .Set(p => p.EndTime, dbTime)
                                .Where(p => p.Id == taskId)
                                .Execute();
                    if (res > 0)
                        RT.EventBus.Publish(new DispatchTaskFinish(dispatchTask));

                    //这边会遇到多种复杂的场景，以下重新打开就是为了应对,重新打开任务单，让他们可以继续生产
                    var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value, null);
                    var dic = tasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
                    //当没有前工序的时候，就跳过不打开
                    if (dic.Any(p => p.Key < dispatchTask.Seq))
                    {
                        //获取前任务单
                        var lastProTasks = dic.Where(p => p.Key < dispatchTask.Seq).OrderByDescending(p => p.Key).FirstOrDefault().Value;
                        //当前工序的全部任务单
                        var curProTasks = tasks.Where(p => p.Seq == dispatchTask.Seq).ToList();
                        //当前工序的总报工数+总可疑品数<前工序总合格数+总可疑品数，且当前工序任务单状态都为完工或者关闭状态，就要打开当前任务单
                        if (curProTasks.All(p => p.TaskStatus == DispatchTaskStatus.Finished || p.TaskStatus == DispatchTaskStatus.Closed) && curProTasks.Sum(p => p.ReportQty) + curProTasks.Sum(p => p.SuspectQty) < lastProTasks.Sum(p => p.OkQty) + lastProTasks.Sum(p => p.SuspectQty))
                        {
                            DB.Update<DispatchTask>().Set(p => p.TaskStatus, DispatchTaskStatus.Executing).Set(p => p.EndTime, p => null).Where(p => p.Id == taskId).Execute();
                        }
                    }
                }
            }
            else
            {
                DB.Update<DispatchTask>()
                        .Set(p => p.TaskStatus, DispatchTaskStatus.Executing)
                        //.Set(p => p.EndTime, dbTime)
                        .Where(p => p.Id == taskId && (p.ReportQty > 0 || p.SuspectQty > 0))
                        .Execute();
                //只要有一张任务单是执行中，工单就更新为生产中
                DB.Update<WorkOrder>()
                    .Set(p => p.State, Core.WorkOrders.WorkOrderState.Producing)
                    .Where(p => p.Id == dispatchTask.WorkOrderId && p.State != Core.WorkOrders.WorkOrderState.Finish)
                    .Execute();
            }
        }

        /// <summary>
        /// 自动报工任务开工
        /// </summary>
        /// <param name="taskId">任务单ID</param> 
        private void AutoReportTaskStart(double taskId)
        {
            DB.Update<DispatchTask>()
                           .Set(p => p.TaskStatus, DispatchTaskStatus.Executing)
                           .Where(p => p.Id == taskId && (p.ReportQty > 0 || p.SuspectQty > 0) && p.TaskStatus == DispatchTaskStatus.Dispatched)
                           .Execute();
        }

        /// <summary>
        /// 共模任务单报工
        /// </summary>
        /// <param name="mainRecord">主任务单报工记录</param>
        /// <param name="synType">辅任务单报工信息</param>
        /// <param name="isSyntype">是否共模报工</param>
        /// <returns>辅任务单报工记录</returns>
        private ReportRecord SynTypeTaskReport(ReportRecord mainRecord, ReportTaskInfo synType)
        {
            ReportRecord record = new ReportRecord();
            if (synType.RecordId > 0)
                record = RF.GetById<ReportRecord>(synType.RecordId);
            var dispatchTask = RF.GetById<DispatchTask>(synType.TaskId);
            if (dispatchTask == null)
                throw new ValidationException("未找到辅料报工任务单".L10N());
            var mainDispatchTask = RF.GetById<DispatchTask>(mainRecord.DispatchTaskId);
            if (mainDispatchTask == null)
                throw new ValidationException("主料任务单不存在".L10N());
            ValidateTaskWorkOrder(synType.WorkOrderId);
            record.ReportQty = synType.ReportOkQty + synType.ReportNgQty;
            record.BatchNo = synType.BatchNo;
            record.NgQty = synType.ReportNgQty;
            record.OkQty = synType.ReportOkQty;
            record.ReportQty = synType.ReportOkQty + synType.ReportNgQty;
            record.StationId = synType.StationId;
            record.Hour = synType.Hour;
            record.Remark = synType.Remark;
            record.Defects.AddRange(CreateDefects(record, synType.DefectIds));
            record.WorkOrderId = synType.WorkOrderId;
            record.StationId = synType.StationId;
            record.ReportTime = mainRecord.ReportTime;
            record.Shift = mainRecord.Shift;
            record.PrincipalId = RT.IdentityId;
            record.DispatchTask = dispatchTask;
            record.WorkGroupId = mainRecord.WorkGroupId;

            if (synType.BatchNo.IsNullOrEmpty() && !dispatchTask.IsVirtualPart && dispatchTask.Specification == null
                && (dispatchTask.EndProcess == true || dispatchTask.Process == null))
            {
                record.BatchNo = RT.Service.Resolve<ReportController>().GetReportBatchNo();
            }

            ValidateReportQty(record, dispatchTask, true);
            //其他共模任务本次报工数必须与该任务满足一定的共模比 
            decimal proportion = (decimal)dispatchTask.Proportion / (decimal)mainDispatchTask.Proportion;
            if ((record.OkQty + record.NgQty) != proportion * (mainRecord.OkQty + mainRecord.NgQty))
                throw new ValidationException("辅料模具数为{0}，合格数量加不合格数未按照工模比填写".L10nFormat(dispatchTask.Proportion));
            return record;
        }

        /// <summary>
        /// 创建报工缺陷集合
        /// </summary>
        /// <param name="record">报工记录</param>
        /// <param name="defectIds">缺陷集合</param>
        /// <returns></returns>
        private IEnumerable<ReportDefect> CreateDefects(ReportRecord record, List<double> defectIds)
        {
            return defectIds.Select(defectId => new ReportDefect()
            {
                DefectId = defectId,
                ReportRecord = record
            });
        }

        /// <summary>
        /// 验证报工任务单
        /// </summary>
        /// <param name="task">报工记录</param>
        /// <returns>报工任务单</returns>
        private void ValidateTask(DispatchTask task, ReportTaskInfo taskInfo)
        {
            if (task == null)
                throw new ValidationException("未找到报工任务单".L10N());
            if (!taskInfo.IsSuspect)
            {
                if (task.TaskStatus != DispatchTaskStatus.Executing && task.TaskStatus != DispatchTaskStatus.Dispatched)
                    throw new ValidationException("只有状态为[执行中、已派工]的任务单可报工".L10N());
            }
            if (!task.IsMainTask)
                throw new ValidationException("报工失败，任务单非主任务单".L10N());
            if (!task.ResourceId.HasValue)
                throw new ValidationException("报工失败，任务单的资源不能为空".L10N());
        }

        /// <summary>
        /// 验证任务单报工数量
        /// </summary>
        /// <param name="reportRecord">报工记录</param>
        /// <param name="task">任务单</param>
        /// <param name="isReport">是否报工</param>
        /// <param name="isSuspect">是否可疑品报工</param>
        private void ValidateReportQty(ReportRecord reportRecord, DispatchTask task, bool isReport, bool isSuspect = false, SIE.MES.TaskManagement.Reports.Enums.SourceType? SourceType = null)
        {
            if (reportRecord.SuspectQty == 0)
            {
                if (reportRecord.OkQty < 0)
                    throw new ValidationException("合格数不能小于0".L10N());
                if (reportRecord.NgQty < 0)
                    throw new ValidationException("不合格数不能小于0".L10N());
                //报工信息保存不做任何验证
                if (!isReport)
                    return;
                if (reportRecord.NgQty == 0 && reportRecord.OkQty == 0 && reportRecord.ReworkQty == 0)
                    throw new ValidationException("报工数量不能都为0".L10N());
            }
            if (!isSuspect)
            {
                //decimal Uebto = 0;
                //decimal.TryParse(task.WorkOrder.Uebto, out Uebto);

                var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task, SourceType).Item1;

                if (reportRecord.OkQty + reportRecord.NgQty + task.SuspectQty + task.ReportQty > maxReportQty)
                {
                    throw new ValidationException("不允许超过容差[{0}]".L10nFormat(maxReportQty));
                }

                //验证报工数量，合格数量+不合格数量不能超任务可报工数量 
                //decimal canReportQty = task.MaxRemainQty + task.ExcessReportQty;
                //if (canReportQty < reportRecord.OkQty + reportRecord.NgQty)
                //    throw new ValidationException("报工数量超任务单[{0}]可报工数量{1}".L10nFormat(task.No, canReportQty));

            }
        }

        /// <summary>
        /// 验证报工任务单工单
        /// </summary>
        /// <param name="task">报工任务</param>
        /// <param name="isMerge">是否合并报工</param>
        /// <returns>工单</returns>
        public virtual WorkOrder ValidateTaskWorkOrder(DispatchTask task, bool isMerge = false)
        {
            WorkOrder wo = null;
            if (task.MergedStatus == MergedStatus.Normal)
            {
                if (task.WorkOrderId == null)
                    throw new ValidationException("未找到任务单所属工单，请检查数据是否完整".L10N());
                wo = ValidateTaskWorkOrder(task.WorkOrderId.Value, isMerge);
            }
            return wo;
        }

        /// <summary>
        /// 验证报工任务单工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="isMerge">是否合并报工</param>
        /// <returns>工单</returns>
        private WorkOrder ValidateTaskWorkOrder(double workOrderId, bool isMerge = false)
        {
            WorkOrder wo = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (wo == null)
                throw new ValidationException("未找到任务单所属工单，请检查数据是否完整".L10N());
            if (wo.State != Core.WorkOrders.WorkOrderState.Producing && wo.State != Core.WorkOrders.WorkOrderState.Release && !isMerge)
                throw new ValidationException("工单[{0}]非生产中，不允许报工".L10nFormat(wo.No));
            if (wo.IsPause == YesNo.Yes)
                throw new ValidationException("工单[{0}]已暂停，不允许报工".L10nFormat(wo.No));
            return wo;
        }

        /// <summary>
        /// 验证虚拟件任务单报工数量
        /// </summary>
        /// <param name="task">报工任务</param>
        /// <param name="reportRecord">报工记录</param>
        /// <param name="workOrderId">工单ID</param>
        void ValidateVirtualPartReportQty(DispatchTask task, ReportRecord reportRecord, double workOrderId)
        {
            if (task.IsVirtualPart)
                return;
            //3、是否关联虚拟件任务
            var isRef = IsRefVirtualPartTask(workOrderId);
            if (isRef)
            {
                //累计报工数量不可超出虚拟件已报合格数量/单位定额
                decimal reportQty = GetVirtualPartTaskMinOkQty(workOrderId);   //虚拟件已报工数量
                decimal totalReportOkQty = GetTaskTotalReportOkQty(workOrderId, task);
                decimal totalReportQty = reportRecord.OkQty + reportRecord.NgQty + totalReportOkQty;   //累计报工数=当前任务单待报工数+任务单已报工数量（同工序任务多个）
                if (totalReportQty > reportQty)
                    throw new ValidationException("累计报工数量[{0}]超出虚拟件已报数量[{1}]，请修改报工数量".L10nFormat(totalReportQty, (int)reportQty));
            }
            //累计报工数量不可超出任务数量，前面已验证
        }

        /// <summary>
        /// 获取任务单累计已报工数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="task">报工任务</param>
        /// <returns>累计已报工合格数量</returns>
        private decimal GetTaskTotalReportOkQty(double workOrderId, DispatchTask task)
        {
            //1、规格件+工序：规格件工序任务累计报工合格数量
            //2、规格件：规格件任务单累计报工合格数
            //3、工序任务：工序任务累计报工合格数
            //4、固定数量：固定数量累计报工合格数
            if (task.SpecificationId.HasValue)
            {
                if (task.ProcessId.HasValue)
                {
                    return GetSpeProcessTotalQty(workOrderId, task.SpecificationId.Value, task.ProcessId.Value, p => p.ReportQty / p.SingleQty);
                }
                return GetSpecificationTaskQty(workOrderId, task.SpecificationId.Value, p => p.ReportQty / p.SingleQty);
            }
            else
            {
                if (task.ProcessId.HasValue)
                {
                    return GetTaskTotalQty(workOrderId, task.Seq, p => p.ReportQty / p.SingleQty);
                }
                return GetFixedQtyTaskQty(workOrderId, p => p.ReportQty); //固定数量累计已报工合格数
            }
        }

        /// <summary>
        /// 验证工序规格件报工数量
        /// </summary>
        /// <param name="task">报工任务单</param>
        /// <param name="reportRecord">报工记录</param>
        /// <param name="processReportingRelationship">是否卡工序前后关系和数量前后关系</param> 
        private void ValidateProcessSpecReportQty(DispatchTask task, ReportRecord reportRecord, bool processReportingRelationship)
        {
            //1、规格件任务：累计报工数量不可超出同规格件前置工序任务的报工数 
            //2、非规格任务：累计报工数量不可超出前置工序任务的报工数
            int seq = task.Seq;
            int preSeq = GetTaskSeq(p => p.WorkOrderId == task.WorkOrderId && p.ProcessId != null && p.Seq < seq && p.Seq > 0);
            if (preSeq <= 0)  //未找到前置工序
                return;
            bool isSpecificationTask = task.SpecificationId.HasValue;  //是否规格件任务 
            decimal preReportQty = 0;
            if (isSpecificationTask)
            {
                //累计报工数量不可超出同规格件前置工序任务的报工合格数 
                preReportQty = GetSpeProcessTotalQty(task.WorkOrderId.Value, task.SpecificationId.Value, preSeq, p => p.OkQty / p.SingleQty);
            }
            else
            {
                //累计报工数量不可超出前置工序任务的报工数
                preReportQty = GetTaskTotalQty(task.WorkOrderId.Value, preSeq, p => p.OkQty / p.SingleQty);
            }
            //获取配置项是否卡数量
            if (!processReportingRelationship)
            {
                //获取前置任务
                var preTask = GetTask(p => p.WorkOrderId == task.WorkOrderId && p.ProcessId != null && p.Seq < seq && p.Seq > 0);
                //当前工序任务累计报工数量=合格+不合格
                IsCheckProcessReportingRelationship(task, reportRecord, seq, isSpecificationTask, preReportQty, preTask);
            }
        }

        /// <summary>
        /// 工序报工 前后工序关系是否卡控
        /// </summary>
        /// <param name="task"></param>
        /// <param name="reportRecord"></param>
        /// <param name="seq"></param>
        /// <param name="isSpecificationTask"></param>
        /// <param name="preReportQty"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void IsCheckProcessReportingRelationship(DispatchTask task, ReportRecord reportRecord, int seq, bool isSpecificationTask, decimal preReportQty, DispatchTask preTask)
        {
            decimal reportQty = isSpecificationTask ? GetSpeProcessTotalQty(task.WorkOrderId.Value, task.SpecificationId.Value, seq, p => p.ReportQty / p.SingleQty) : GetTaskTotalQty(task.WorkOrderId.Value, seq, p => p.ReportQty / p.SingleQty);
            decimal totalReportQty = reportRecord.OkQty + reportRecord.NgQty + reportQty;   //当前工序累计报工数，已报工+待报工

            if (totalReportQty > preReportQty)
            {
                throw new ValidationException("累计报工数量[{0}]超出{2}前置工序[{3}]任务的报工数量[{1}]，请修改报工数量".L10nFormat(totalReportQty, preReportQty, isSpecificationTask ? "同规格件" : "", preTask.Process?.Code));
            }

            if (preReportQty <= 0)
            {
                throw new ValidationException("非首工序报工的前工序累计报工数量[{0}]应大于0，否则无法进行本工序报工".L10nFormat(preReportQty));
            }
        }

        /// <summary>
        /// 获取任务单任务顺序
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <returns>任务顺序</returns>
        DispatchTask GetTask(Expression<Func<DispatchTask, bool>> exp)
        {
            return Query<DispatchTask>().Where(exp).OrderByDescending(p => p.Seq).FirstOrDefault();
        }

        /// <summary>
        /// 获取任务单任务顺序
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <returns>任务顺序</returns>
        int GetTaskSeq(Expression<Func<DispatchTask, bool>> exp)
        {
            return Query<DispatchTask>().Where(exp).OrderByDescending(p => p.Seq).Select(p => p.Seq).FirstOrDefault<int>();
        }

        /// <summary>
        /// 获取工单结束工序任务累计报工合格数 
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>累计报工合格数，-1表示工单没有工序任务</returns>
        decimal GetEndProcessTaskReportQty(double workOrderId)
        {
            var seq = GetTaskSeq(p => p.WorkOrderId == workOrderId && p.ProcessId != null && p.Seq > 0);
            if (seq <= 0)
                return -1;
            return GetProcessMinOkQty(workOrderId, seq);
        }

        /// <summary>
        /// 获取工序最小报工合格数，如果有规格件，取规格件最小合格数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="seq">工序任务顺序</param>
        /// <returns>报工合格数</returns>
        decimal GetProcessMinOkQty(double workOrderId, int seq)
        {
            //按照规格件分组，取规格件最小合格数
            return RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.Seq == seq).GroupBy(p => p.SpecificationId).Select(p => new { TotalOkQty = p.Sum(f => f.OkQty / f.SingleQty) }).Min(p => p.TotalOkQty);
        }

        /// <summary>
        /// 获取固定数量累计已报工（合格）数量（排查工序任务、规格件任务、虚拟任务等组合任务）
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sumExp">汇总条件：合格数：p => p.OkQty、报工数:p => p.ReportQty</param>
        /// <returns>累计已报工合格数量</returns>
        decimal GetFixedQtyTaskQty(double workOrderId, Func<DispatchTask, decimal> sumExp)
        {
            if (!IsExistFixedQtyTask(workOrderId))
                return -1;
            if (sumExp == null)
                throw new ValidationException(_sumExpNull);
            var tasks = RT.Service.Resolve<DispatchController>()
                .GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.SpecificationId == null && p.ProcessId == null && !p.IsVirtualPart);
            return tasks.Sum(sumExp);
        }

        /// <summary>
        /// 获取工序任务累计报工（合格）数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="seq">工序任务顺序</param>
        /// <param name="sumExp">汇总条件：合格数：p => p.OkQty / p.SingleQty、报工数:p => p.ReportQty / p.SingleQty</param>
        /// <returns>工序任务累计报工合格数</returns>
        decimal GetTaskTotalQty(double workOrderId, int seq, Func<DispatchTask, decimal> sumExp)
        {
            if (sumExp == null)
                throw new ValidationException(_sumExpNull);
            return RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.Seq == seq).Sum(sumExp);
        }

        /// <summary>
        /// 获取工单规格件工序任务累计报工（合格）数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="specificationId">规格件ID</param>
        /// <param name="seq">工序任务顺序</param>
        /// <param name="sumExp">汇总条件：合格数：p => p.OkQty / p.SingleQty、报工数:p => p.ReportQty / p.SingleQty</param>
        /// <returns>规格件工序任务累计报工合格数</returns>
        decimal GetSpeProcessTotalQty(double workOrderId, double specificationId, int seq, Func<DispatchTask, decimal> sumExp)
        {
            if (sumExp == null)
                throw new ValidationException(_sumExpNull);
            return RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.ProcessId != null && p.SpecificationId == specificationId && p.Seq == seq).Sum(sumExp);
        }

        /// <summary>
        /// 获取工单规格件工序任务累计报工（合格）数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="specificationId">规格件ID</param>
        /// <param name="seq">工序任务顺序</param>
        /// <param name="sumExp">汇总条件：合格数：p => p.OkQty / p.SingleQty、报工数:p => p.ReportQty / p.SingleQty</param>
        /// <returns>规格件工序任务累计报工合格数</returns>
        decimal GetSpeProcessTotalQty(double workOrderId, double specificationId, double processId, Func<DispatchTask, decimal> sumExp)
        {
            if (sumExp == null)
                throw new ValidationException(_sumExpNull);
            return RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.SpecificationId == specificationId && p.ProcessId == processId).Sum(sumExp);
        }

        /// <summary>
        /// 获取工单规格件任务累计报工（合格）数
        /// 排除工序任务
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="specificationId">规格件ID</param>
        /// <param name="seq">工序任务顺序</param>
        /// <param name="sumExp">汇总条件：合格数：p => p.OkQty / p.SingleQty、报工数:p => p.ReportQty / p.SingleQty</param>
        /// <returns>规格件工序任务累计报工合格数</returns>
        decimal GetSpecificationTaskQty(double workOrderId, double specificationId, Func<DispatchTask, decimal> sumExp)
        {
            if (sumExp == null)
                throw new ValidationException(_sumExpNull);
            return RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.SpecificationId == specificationId && p.ProcessId == null).Sum(sumExp);
        }

        /// <summary>
        /// 获取工单规格件工序任务累计报工最小合格数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>累计报工最小合格数，-1表示工单没有规格件任务</returns>
        decimal GetSpecificationTaskOkQty(double workOrderId)
        {
            //根据规格件分组，取最小报工合格数
            if (!IsExistSpecificationTask(workOrderId))
                return -1;
            return RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.SpecificationId != null).GroupBy(p => p.SpecificationId).Select(p => new { TotalOkQty = p.Sum(f => f.OkQty / f.SingleQty) }).Min(p => p.TotalOkQty);
        }

        /// <summary>
        /// 获取工单虚拟件任务单最小报工合格数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>虚拟件任务单最小报工合格数</returns>
        decimal GetVirtualPartTaskMinOkQty(double workOrderId)
        {
            //根据虚拟件分组，取最小报工合格数
            var virtualPartTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == workOrderId && p.IsVirtualPart);
            return virtualPartTasks.GroupBy(p => p.VirtualPartCode).Select(p => new { TotalOkQty = p.Sum(f => f.OkQty / f.SingleQty) }).Min(p => p.TotalOkQty);
        }

        /// <summary>
        /// 判断工单是否存在虚拟件任务单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool IsRefVirtualPartTask(double workOrderId)
        {
            Expression<Func<DispatchTask, bool>> exp = p => p.WorkOrderId == workOrderId && p.IsVirtualPart;
            return _dispatchController.IsExistDispatchTask(exp);
        }

        /// <summary>
        /// 判断当前工序任务是否工单首工序任务
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="seq">任务单顺序</param>
        /// <returns>是首工序任务返回true，否则返回false</returns>
        bool IsWorkOrderFirstProcessTask(double workOrderId, int seq)
        {
            Expression<Func<DispatchTask, bool>> exp = p => p.WorkOrderId == workOrderId && p.ProcessId != null && p.Seq > 0;
            var firstTask = _dispatchController.GetDispatchTaskByExpression(exp, p => p.Seq).FirstOrDefault();
            return firstTask != null && firstTask.Seq == seq;
        }

        /// <summary>
        /// 判断工单是否存在规格件任务单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool IsExistSpecificationTask(double workOrderId)
        {
            Expression<Func<DispatchTask, bool>> exp = p => p.WorkOrderId == workOrderId && p.SpecificationId != null;
            return _dispatchController.IsExistDispatchTask(exp);
        }

        /// <summary>
        /// 判断工单是否存在固定数量任务单
        /// 排除工序、规格、虚拟件任务单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool IsExistFixedQtyTask(double workOrderId)
        {
            Expression<Func<DispatchTask, bool>> exp = p => p.WorkOrderId == workOrderId && p.SpecificationId == null && p.ProcessId == null && !p.IsVirtualPart;
            return _dispatchController.IsExistDispatchTask(exp);
        }
        #endregion

        #region 配置项
        /// <summary>
        /// 获取报工记录批次号
        /// </summary>
        /// <returns>报工记录批次号</returns>
        public virtual string GetReportBatchNo()
        {
            var config = ConfigService.GetConfig(new ReportRecordDetailConfig(), typeof(ReportRecord));
            if (config == null || !config.ReportBatchNoRuleId.HasValue)
                return "";
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.ReportBatchNoRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 校验报工批次号是否已经存在
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public virtual bool ExistReportBatchNo(string batchNo)
        {
            return Query<ReportRecord>().Where(p => p.BatchNo == batchNo).Count() > 0;
        }

        /// <summary>
        /// 获取报工记录打印模板
        /// </summary>
        /// <returns>报工记录打印模板</returns>
        public virtual PrintTemplate GetReportPrintemplate()
        {
            var config = ConfigService.GetConfig(new ReportRecordDetailConfig(), typeof(ReportRecord));
            if (config == null || !config.ReportPrintTemplateId.HasValue)
                return null;
            return config.ReportPrintTemplate;
        }
        #endregion

        #region 任务单首件报检

        /// <summary>
        /// 创建首件报检事件
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual FirstInspEvent CreateFirstInspEvent(DispatchTask task)
        {
            var workOrderNos = task.AssociatedWorkOrder.Split(';');
            var workOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(workOrderNos.First());
            if (workOrder == null)
            {
                throw new ValidationException("任务单对应的工单不存在".L10N());
            }
            var firstInspEvent = new FirstInspEvent()
            {
                DispatchTaskNo = task.No,
                WorkOrderId = workOrder.Id,
                ItemId = task.ProductId,
                ItemCode = task.Product.Code,
                ProcessId = task.ProcessId ?? 0,
                ProcessName = task.Process?.Name,
                ResourceId = task.ResourceId ?? 0,
                ShopId = task.WorkShopId ?? 0,
                EmployeeId = RT.IdentityId,
                CustomerId = workOrder.CustomerId,
                FirstInspQty = task.InspQty,
                CollectionDate = DateTime.Now,
                IsEndProcess = task.EndProcess == true,
                IsStartProcess = task.StartProcess == true
            };
            return firstInspEvent;
        }

        /// <summary>
        /// 首件报检
        /// </summary>
        /// <param name="task"></param>
        public virtual void ReportFirstInsp(DispatchTask task)
        {
            var firstInspEvent = CreateFirstInspEvent(task);
            RT.Service.Resolve<IFirstInsp>().GenerateTaskFirstInsp(firstInspEvent);
        }

        /// <summary>
        /// 验证是否完成首检
        /// </summary>
        /// <param name="task"></param>
        public virtual void CheckTaskFinishInsp(DispatchTask task)
        {
            var firstInspEvent = CreateFirstInspEvent(task);
            RT.Service.Resolve<IFirstInsp>().CheckTaskFinishInsp(firstInspEvent);
        }
        #endregion

        #region 任务单成品报检

        /// <summary>
        /// 创建成品报检事件
        /// </summary>
        /// <param name="reportRecordList"></param>
        /// <param name="mainRecord"></param>
        /// <returns></returns>
        public virtual List<ProductInspEvent> CreateProductInspEventList(IList<ReportRecord> reportRecordList, ReportRecord mainRecord)
        {
            var inspEventList = new List<ProductInspEvent>();
            foreach (var reportRecord in reportRecordList)
            {
                var task = reportRecord.DispatchTask;
                var inspEvent = new ProductInspEvent()
                {
                    WorkOrderId = reportRecord.WorkOrderId.Value,
                    ItemId = task.ProductId,
                    ProcessId = task.ProcessId ?? 0,
                    ResourceId = task.ResourceId ?? 0,
                    ShopId = task.WorkShopId ?? 0,
                    EmployeeId = RT.IdentityId,
                    CustomerId = reportRecord.WorkOrder.CustomerId,
                    CollectionDate = DateTime.Now,
                    DispatchTaskId = task.Id,
                    ReportRecordId = mainRecord.Id,
                    OkQty = mainRecord.OkQty,
                    IsEndProcess = task.EndProcess == true,
                    IsStartProcess = task.StartProcess == true
                };
                inspEventList.Add(inspEvent);
            }
            return inspEventList;
        }

        /// <summary>
        /// 成品报检
        /// </summary>
        /// <param name="reportRecordList"></param>
        /// <param name="mainRecord"></param>
        public virtual bool ReportProductInsp(IList<ReportRecord> reportRecordList, ReportRecord mainRecord)
        {
            var productInspEventList = CreateProductInspEventList(reportRecordList, mainRecord);
            foreach (var productInspEvent in productInspEventList)
            {
                productInspEvent.BatchNo = mainRecord.BatchNo;
            }
            return RT.Service.Resolve<IProductInsp>().GenerateTaskProductInsp(productInspEventList);
        }
        #endregion

        #region 任务单成品入库
        /// <summary>
        /// 成品入库（不需要成品检，直接入库）
        /// </summary>
        public virtual void ReportProductStorage(ReportRecord mainRecord)
        {
            var task = mainRecord.DispatchTask;
            if (mainRecord.OkQty > 0 && (task.Process == null || task.EndProcess == true))
            {
                if (mainRecord.BatchNo.IsNullOrEmpty())
                    throw new ValidationException("请输入批次号或维护批次号编码规则".L10N());
                var woId = 0d;
                if (!mainRecord.DispatchTask.AssociatedWorkOrder.IsNullOrEmpty())
                {
                    woId = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(mainRecord.DispatchTask.AssociatedWorkOrder).Id;
                }
                var storageEvent = new ToStorageBarcodeEvent()
                {
                    WorkOrderId = mainRecord.WorkOrderId.HasValue ? mainRecord.WorkOrderId.Value : woId,
                    Barcode = mainRecord.BatchNo,
                    Qty = mainRecord.OkQty,
                    CollectionDate = DateTime.Now,
                    DispatchTaskId = mainRecord.DispatchTaskId
                };
                RT.Service.Resolve<IToStorageBarcode>().ToStorageBarcode(storageEvent);
            }
        }

        /// <summary>
        /// 成品入库（成品检合格后，回传入库）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        public virtual void ToStorageTaskBarcode(ToStorageBarcodeEvent toStorageEvent)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //查询报工记录
                var reportRecord = RF.GetById<ReportRecord>(toStorageEvent.ReportRecordId);

                //更新工单完工数和状态
                EndProcessUpdateWorkOrder(reportRecord);

                //成品入库
                ReportProductStorage(reportRecord);

                tran.Complete();
            }
        }

        /// <summary>
        /// 更新工单完工数
        /// </summary>
        /// <param name="reportRecord">报工记录</param>
        private void EndProcessUpdateWorkOrder(ReportRecord reportRecord)
        {
            // 关联报工记录
            var batchNo = Query<ReportRecord>().LeftJoin<AssociatedTask>((rr, t) => rr.DispatchTaskId == t.DispatchTaskId)
                .Where<AssociatedTask>((rr, t) => t.TaskId == reportRecord.DispatchTaskId).Where(rr => rr.ReportTime == reportRecord.ReportTime).Select(rr => new { rr.BatchNo }).ToList<string>().FirstOrDefault();
            if (batchNo.IsNotEmpty())
            {
                reportRecord.BatchNo = batchNo;
            }

            // 查询任务单是否为末工序
            var task = Query<DispatchTask>().Where(p => p.Id == reportRecord.DispatchTaskId).Select(p => new
            {
                TaskId = p.Id,
                EndProcess = p.EndProcess,
            }).ToList<DispatchTaskInfo>().FirstOrDefault();
            // 完工数
            if (task != null && task.EndProcess == true)
            {
                //更新工单完工数和状态
                DB.Update<WorkOrder>().Set(p => p.FinishQty, p => p.FinishQty + reportRecord.OkQty).Where(p => p.Id == reportRecord.WorkOrderId).Execute();
                DB.Update<WorkOrder>().Set(p => p.State, Core.WorkOrders.WorkOrderState.Finish)
                    .Set(p => p.ActuFinishDate, reportRecord.ReportTime).Where(p => p.Id == reportRecord.WorkOrderId && p.FinishQty >= p.PlanQty).Execute();
            }
        }

        /// <summary>
        /// 检验合格更新报工记录信息（任务单）
        /// </summary>
        /// <param name="toStorageEvent"></param>
        public virtual void ToUpdateTaskReportState(ToStorageBarcodeEvent toStorageEvent)
        {
            //var taskId = Query<ReportRecord>().Where(p => p.Id == toStorageEvent.ReportRecordId).Select(p => new { p.DispatchTaskId }).ToList<double>().Distinct().ToList();
            //if (!taskId.Any())
            //{
            //    return;
            //}
            //var dispatchTaskId = taskId.First();
            //var task = Query<DispatchTask>().Where(p => p.Id == dispatchTaskId).Select(p => new { Report_Qty = p.ReportQty }).FirstOrDefault();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //查询报工记录
                var inspectionResult = (InspectionResult?)toStorageEvent.InspectionResult;
                var inspectionStatus = (Enums.InspectionStatus?)toStorageEvent.InspectionStatus;
                DB.Update<ReportRecord>()
                    .Set(p => p.InspectionResult, inspectionResult)
                    .Set(p => p.InspectionStatus, inspectionStatus)
                    .Set(p => p.ProcessMode, toStorageEvent.ProcessMode)
                    .Where(p => p.Id == toStorageEvent.ReportRecordId).Execute();

                //if (task.ReportQty <= 0)
                //{
                //    DB.Update<DispatchTask>().Set(p => p.TaskStatus, DispatchTaskStatus.Executing).Where(p => p.Id == dispatchTaskId).Execute();
                //}
                tran.Complete();
            }
        }

        /// <summary>
        /// 检验不合格更新报工记录数量（任务单）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        public virtual void ToUpdateTaskReportQty(ToStorageBarcodeEvent toStorageEvent)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //查询报工记录
                var reportRecord = RF.GetById<ReportRecord>(toStorageEvent.ReportRecordId);

                reportRecord.NgQty += reportRecord.OkQty;
                reportRecord.OkQty = 0;
                reportRecord.InspectionResult = (InspectionResult?)toStorageEvent.InspectionResult;
                reportRecord.InspectionStatus = (Enums.InspectionStatus?)toStorageEvent.InspectionStatus;
                reportRecord.ProcessMode = toStorageEvent.ProcessMode;

                var defectIds = toStorageEvent.DefectIds;
                if (defectIds.Count > 0)
                {
                    defectIds.ForEach(p =>
                    {
                        ReportDefect reportDefect = new ReportDefect
                        {
                            ReportRecordId = reportRecord.Id,
                            DefectId = p,
                        };
                        reportRecord.Defects.Add(reportDefect);
                    });
                }

                RF.Save(reportRecord);
                //DB.Update<ReportRecord>()
                //    .Set(p => p.OkQty, 0)
                //    .Set(p => p.NgQty, p => p.NgQty + p.OkQty)
                //    .Where(p => p.Id == reportRecord.Id)
                //    .Execute();


                var reportRecordTask = reportRecord.DispatchTask;
                reportRecordTask.OkQty -= reportRecord.OkQty;
                reportRecordTask.ReportQty -= reportRecord.OkQty;
                reportRecordTask.TaskStatus = DispatchTaskStatus.Executing;
                RF.Save(reportRecordTask);

                var associatedTask = Query<AssociatedTask>().Where(p => p.TaskId == reportRecord.DispatchTaskId).FirstOrDefault();
                if (associatedTask != null)
                {
                    var mainTask = associatedTask.DispatchTask;
                    mainTask.OkQty -= reportRecord.OkQty;
                    mainTask.ReportQty -= reportRecord.OkQty;
                    mainTask.TaskStatus = DispatchTaskStatus.Executing;
                    RF.Save(mainTask);

                    var mainRecord = mainTask.Records.FirstOrDefault(p => p.ReportTime == reportRecord.ReportTime);
                    if (mainRecord != null)
                    {
                        mainRecord.OkQty -= reportRecord.OkQty;
                        mainRecord.NgQty += reportRecord.OkQty;
                        RF.Save(mainRecord);
                    }
                }
                tran.Complete();
            }
        }
        #endregion

        #region 执行记录

        /// <summary>
        /// 感觉任务单id获取操作记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual EntityList<ReportOperateLog> GetReportOptLog(double taskId)
        {
            return Query<ReportOperateLog>().Where(p => p.DispatchTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 开工、恢复生成开始记录
        /// </summary>
        /// <param name="tasks"></param>
        public virtual EntityList<ReportOperateLog> GenerateTaskOptStartLog(EntityList<DispatchTask> tasks)
        {
            EntityList<ReportOperateLog> logs = new EntityList<ReportOperateLog>();
            foreach (var task in tasks)
            {
                ReportOperateLog log = new ReportOperateLog
                {
                    DispatchTaskId = task.Id,
                    StartTime = DateTime.Now,
                    StartOpterId = RT.IdentityId,
                };
                logs.Add(log);
            }
            return logs;
        }

        /// <summary>
        /// 暂停更新记录结束信息
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public virtual EntityList<ReportOperateLog> UpdateTaskOptStopLog(EntityList<DispatchTask> tasks)
        {
            EntityList<ReportOperateLog> logs = new EntityList<ReportOperateLog>();
            // 任务单id
            var taskIds = tasks.Select(p => p.Id).ToList();
            //  拿到每个任务单最新的记录(开始时间倒序)
            var lastOptLogs = Query<ReportOperateLog>().Where(p => taskIds.Contains(p.DispatchTaskId) && p.EndTime == null).ToList();
            foreach (var log in lastOptLogs)
            {
                log.EndTime = DateTime.Now;
                log.EndOpterId = RT.IdentityId;
            }
            return lastOptLogs;
        }

        /// <summary>
        /// 根据工序获取报工记录
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public virtual EntityList<ReportRecordExamine> GetReportRecordExaminesByProcessId(double ProcessId)
        {
            var list = Query<ReportRecordExamine>().Where(p => p.ProcessId == ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据工单Id获取报工记录
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<ReportRecordExamine> GetReportRecordExaminesByWoNos(List<string> woNos)
        {
            return woNos.SplitContains(nos =>
            {
                return Query<ReportRecordExamine>().Where(p => nos.Contains(p.Wo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 查询报工记录审核
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ReportRecordExamine> QueryReportRecordExamine(ReportRecordExamineCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<ReportRecordExamine>();
            }
            var query = Query<ReportRecordExamine>();
            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.Wo.IsNotEmpty())
            {
                query.Where(p => p.Wo.Contains(criteria.Wo));
            }
            if (criteria.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Code.Contains(criteria.ProductCode));
            }
            if (criteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(criteria.ProductName));
            }
            if (criteria.ResourceId != 0 && criteria.ResourceId != null)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId);
            }
            //if (criteria.WorkShopId != 0 && criteria.WorkShopId != null)
            //{
            //    query.Where(p => p.WorkShopId == criteria.WorkShopId);
            //}
            if (!criteria.WorkShopCode.IsNullOrEmpty())
                query.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criteria.WorkShopCode.IsNotEmpty(), p => p.Code == criteria.WorkShopCode));
            if (criteria.InspectionStatus.HasValue)
            {
                query.Where(p => p.InspectionStatus == criteria.InspectionStatus.Value);
            }
            if (criteria.InspectionResult.HasValue)
            {
                query.Where(p => p.InspectionResult == criteria.InspectionResult.Value);
            }
            if (criteria.ExamineState.HasValue)
            {
                query.Where(p => p.ExamineState == criteria.ExamineState.Value);
            }
            if (criteria.ProcessId != 0 && criteria.ProcessId != null)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId);
            }
            if (criteria.ReportTime.BeginValue.HasValue)
            {
                query.Where(p => p.ReportTime >= criteria.ReportTime.BeginValue);
            }
            if (criteria.ReportTime.EndValue.HasValue)
            {
                query.Where(p => p.ReportTime <= criteria.ReportTime.EndValue);
            }

            if (criteria.WipBatchNos.IsNotEmpty())
            {
                query.Exists<ReportWipBatch>((x, y) => y.Where(p => p.BatchNo.Contains(criteria.WipBatchNos) && p.ReportRecordId == x.Id));
            }
            if (!criteria.Licha.IsNullOrEmpty())
            {
                query.Exists<DeductionRecord>((x, y) => y.Where(p => p.ReportRecordId == x.Id && p.ItemLabel.Licha.Contains(criteria.Licha)));

                //var symbol = "=";
                //if (criteria.Licha.Contains("%"))
                //    symbol = "like";
                //query.Where(p => p.SQL<bool>(@"EXISTS (
                //SELECT 1
                //FROM TM_REPORT_WIP_BATCH T1 INNER JOIN Item_Label ON Item_Label.LABEL = T1.Batch_No and Item_Label.LICHA {0} '{1}' AND Item_Label.IS_PHANTOM = 0
                //WHERE T1.REPORT_RECORD_ID = T0.ID AND T1.INV_ORG_ID = 1 AND T1.IS_PHANTOM = '0')".L10nFormat(symbol, criteria.Licha)));
            }
            if (criteria.SourceType.HasValue)
            {
                query.Where(p => p.SourceType == criteria.SourceType.Value);
            }
            if (!criteria.ShortDescription.IsNullOrEmpty())
            {
                query.Where(p => p.Product.ShortDescription.Contains(criteria.ShortDescription));
            }

            query.OrderByDescending(p => p.CreateTime).OrderByDescending(p => p.No);
            var list = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //匹配批次号
            var ids = list.Select(p => (double?)p.Id).ToList();
            var wipBatchs = ids.SplitContains(temp =>
            {
                return Query<ReportWipBatch>().Where(p => temp.Contains(p.ReportRecordId)).ToList();
            });

            var batchNos = wipBatchs.Select(p => p.BatchNo).Distinct().ToList();
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelDatas(batchNos);

            foreach (var p in list)
            {
                var bNos = wipBatchs.Where(x => x.ReportRecordId == p.Id).Select(x => x.BatchNo).ToList();
                p.WipBatchNos = bNos.Concat(",");
                var lichas = itemLabels.Where(p => bNos.Contains(p.Label)).Select(p => p.Licha).Distinct().ToList();
                p.Lichas = lichas.Concat(",");
            }
            return list;
        }

        /// <summary>
        /// 查询报工记录审核（导出专用优化版本）
        /// 针对大数据量导出场景优化，避免LoadWithViewProperty导致的全量关联查询性能问题
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>报工记录审核列表</returns>
        public virtual EntityList<ReportRecordExamine> QueryReportRecordExamineForExport(ReportRecordExamineCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<ReportRecordExamine>();
            }

            var query = Query<ReportRecordExamine>();

            // 基础筛选条件
            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.Wo.IsNotEmpty())
            {
                query.Where(p => p.Wo.Contains(criteria.Wo));
            }
            if (criteria.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Code.Contains(criteria.ProductCode));
            }
            if (criteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(criteria.ProductName));
            }
            if (criteria.ResourceId != 0 && criteria.ResourceId != null)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId);
            }
            if (!criteria.WorkShopCode.IsNullOrEmpty())
            {
                query.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criteria.WorkShopCode.IsNotEmpty(), p => p.Code == criteria.WorkShopCode));
            }
            if (criteria.InspectionStatus.HasValue)
            {
                query.Where(p => p.InspectionStatus == criteria.InspectionStatus.Value);
            }
            if (criteria.InspectionResult.HasValue)
            {
                query.Where(p => p.InspectionResult == criteria.InspectionResult.Value);
            }
            if (criteria.ExamineState.HasValue)
            {
                query.Where(p => p.ExamineState == criteria.ExamineState.Value);
            }
            if (criteria.ProcessId != 0 && criteria.ProcessId != null)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId);
            }
            // 时间范围筛选（导出场景必选）
            if (criteria.ReportTime.BeginValue.HasValue)
            {
                query.Where(p => p.ReportTime >= criteria.ReportTime.BeginValue);
            }
            if (criteria.ReportTime.EndValue.HasValue)
            {
                query.Where(p => p.ReportTime <= criteria.ReportTime.EndValue);
            }
            if (criteria.WipBatchNos.IsNotEmpty())
            {
                query.Exists<ReportWipBatch>((x, y) => y.Where(p => p.BatchNo.Contains(criteria.WipBatchNos) && p.ReportRecordId == x.Id));
            }
            if (!criteria.Licha.IsNullOrEmpty())
            {
                query.Exists<DeductionRecord>((x, y) => y.Where(p => p.ReportRecordId == x.Id && p.ItemLabel.Licha.Contains(criteria.Licha)));
            }
            if (criteria.SourceType.HasValue)
            {
                query.Where(p => p.SourceType == criteria.SourceType.Value);
            }
            if (!criteria.ShortDescription.IsNullOrEmpty())
            {
                query.Where(p => p.Product.ShortDescription.Contains(criteria.ShortDescription));
            }

            // 关键优化：不使用LoadWithViewProperty，避免全量关联查询
            // 使用空的EagerLoadOptions，只加载基础字段
            var loadOptions = new EagerLoadOptions();

            query.OrderByDescending(p => p.CreateTime).OrderByDescending(p => p.No);
            var list = query.ToList(new PagingInfo() { PageNumber = 1, PageSize = 100000000 }, new EagerLoadOptions().LoadWithViewProperty());

            // 优化：批量加载批次号数据
            if (list.Count > 0)
            {
                var ids = list.Select(p => (double?)p.Id).Distinct().ToList();

                // 使用SplitContains批量查询，避免单次查询数据量过大
                var wipBatchs = ids.SplitContains(idList =>
                {
                    return Query<ReportWipBatch>().Where(p => idList.Contains(p.ReportRecordId)).ToList();
                });

                if (wipBatchs.Count > 0)
                {
                    var batchNos = wipBatchs.Select(p => p.BatchNo).Distinct().ToList();

                    // 批量获取标签数据（减少接口调用）
                    var itemLabels = batchNos.SplitContains(nos =>
                     {
                         return Query<ItemLabel>().Where(p => nos.Contains(p.Label)).ToList();
                     });

                    // 使用字典优化匹配性能，避免多次遍历
                    var batchDict = wipBatchs.GroupBy(x => x.ReportRecordId)
                        .ToDictionary(g => g.Key, g => g.Select(x => x.BatchNo).ToList());

                    var labelDict = itemLabels
                        .Where(p => batchNos.Contains(p.Label))
                        .GroupBy(p => p.Label)
                        .ToDictionary(g => g.Key, g => g.Select(p => p.Licha).FirstOrDefault());

                    foreach (var item in list)
                    {
                        if (batchDict.TryGetValue(item.Id, out var batches))
                        {
                            item.WipBatchNos = string.Join(",", batches);

                            var lichas = batches
                                .Select(b => labelDict.TryGetValue(b, out var licha) ? licha : null)
                                .Where(l => !string.IsNullOrEmpty(l))
                                .Distinct()
                                .ToList();
                            item.Lichas = string.Join(",", lichas);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 根据ids获取报工记录
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="needView"></param>
        /// <returns></returns>
        public virtual EntityList<ReportRecord> GetReportRecordsByIds(List<double> ids, bool needView = false)
        {
            EagerLoadOptions egl = new EagerLoadOptions();
            if (needView)
            {
                egl.LoadWithViewProperty();
            }
            return ids.SplitContains(tempIds =>
            {
                return Query<ReportRecord>().Where(p => tempIds.Contains(p.Id)).ToList(null, egl);
            });
        }
        /// <summary>
        /// 手动完工确认
        /// </summary>
        /// <param name="dispatchTaskIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ReportFinish(List<double> dispatchTaskIds)
        {
            List<double> noExecutingTaskIds = new List<double>();
            dispatchTaskIds.SplitDataExecute(tempIds =>
          {
              noExecutingTaskIds.AddRange(Query<DispatchTask>().Where(m => m.TaskStatus != DispatchTaskStatus.Executing && tempIds.Contains(m.Id)).Select(p => new
              {
                  p.Id
              }).ToList<double>());
          });
            if (noExecutingTaskIds.Any())
            {
                throw new ValidationException("派工任务存在不为【执行中】的数据，不能完工确认，请检查".L10N());
            }

            //存在待确认的报工任务集合  
            var dispatchTaskHasToConfirm = dispatchTaskIds.SplitContains(tempIds =>
            {
                return Query<DispatchTask>().Exists<ReportRecord>((p, q) => q.Where(m => m.DispatchTaskId == p.Id && m.ExamineState == ReportRecordExamineState.ToConfirm))
                .Where(p => tempIds.Contains(p.Id)).ToList();
            });
            if (dispatchTaskHasToConfirm.Any())
            {
                throw new ValidationException("存在待确认的报工记录，请先审核确认".L10N());
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var time = RF.Find<DispatchTask>().GetDbTime();
                dispatchTaskIds.SplitDataExecute(tempIds =>
            {
                DB.Update<DispatchTask>()
                 .Set(p => p.TaskStatus, DispatchTaskStatus.Finished)
                 .Set(p => p.EndTime, time)
                .Where(m => tempIds.Contains(m.Id)).Execute();
            });
                tran.Complete();
            }
        }


        /// <summary>
        /// 报工记录审核确认命令
        /// </summary>
        /// <param name="recordIds"></param>
        public virtual void TaskExamineConfirm(List<double> recordIds)
        {
            var records = GetReportRecordsByIds(recordIds, true);
            if (!records.Any())
            {
                throw new ValidationException("报工记录数据异常".L10N());
            }
            if (records.Any(p => p.ExamineState != ReportRecordExamineState.ToConfirm))
            {
                throw new ValidationException("报工记录审核状态不为待确认".L10N());
            }
            StringBuilder stringBuilder = new StringBuilder();
            // 数据库时间
            DateTime dbTime = RF.Find<ReportRecord>().GetDbTime();

            // 被合并的报工记录
            var mergeRecords = GetMergeRecordsBySourceId(recordIds);

            // 所有任务单
            var taskIds = records.Select(p => p.DispatchTaskId).ToList();
            taskIds.AddRange(mergeRecords.Select(p => p.DispatchTaskId).ToList());

            // 任务单对应的工序bom
            var taskProcessBoms = GetTaskProcessBomsByTaskIds(taskIds);
            // 任务单
            var taskList = GetDispatchTasksByIds(taskIds);

            // 查询成品报检
            var allRecordIds = new List<double>();
            allRecordIds.AddRange(recordIds);
            allRecordIds.AddRange(mergeRecords.Select(p => p.Id));
            allRecordIds = allRecordIds.Distinct().ToList();
            var inspDic = RT.Service.Resolve<IFirstInsp>().GetRecordInspLogByIds(allRecordIds);

            foreach (var record in records)
            {
                if (record.InspectionStatus != null)
                {
                    if (record.InspectionResult == null)
                    {
                        stringBuilder.Append("任务单{0}批次号{1}的报工记录未检验完成，无法进行报工确认".L10nFormat(record.DispatchTaskNo, record.BatchNo));
                    }
                    else if (record.InspectionResult != null && record.InspectionResult == InspectionResult.Fail)
                    {
                        stringBuilder.Append("任务单{0}批次号{1}的报工记录检验结果不合格，无法进行报工确认".L10nFormat(record.DispatchTaskNo, record.BatchNo));
                    }
                }
                if (record.SourceId == null)
                {
                    var task = taskList.FirstOrDefault(p => p.Id == record.DispatchTaskId);
                    var sumReportQty = records.Where(p => p.DispatchTaskId == task.Id).Sum(p => p.ReportQty);
                    if (sumReportQty > task.DispatchQty - task.ReportQty - task.SuspectQty + task.ExcessReportQty)
                    {
                        throw new ValidationException("报工数量[{0}]超任务单[{1}]可报工数量[{2}]".L10nFormat(sumReportQty, task.No, task.DispatchQty));
                    }
                }

            }
            if (stringBuilder.Length > 0)
            {
                throw new ValidationException("{0}".L10nFormat(stringBuilder.ToString()));
            }

            EntityList<WoCostItem> woCostItems = new EntityList<WoCostItem>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var record in records)
                {
                    inspDic.TryGetValue(record.Id, out var checkNo);
                    var sameTaskRecords = mergeRecords.Where(p => p.SourceId == record.Id).ToList();
                    var task = taskList.FirstOrDefault(p => p.Id == record.DispatchTaskId);
                    sameTaskRecords.Add(record);
                    foreach (var same in sameTaskRecords)
                    {
                        inspDic.TryGetValue(same.Id, out var sameCheckNo);
                        same.ExamineState = ReportRecordExamineState.Confirmed;
                        same.IsReport = true;
                        //更新派工任务信息
                        UpdateDispatchTaskBeginTime(same.DispatchTaskId, dbTime);
                        UpdateDispatchTaskQty(same);
                        UpdateDispatchTaskState(same.DispatchTaskId, dbTime);
                        //若任务存在相关物料需求，按单位定额更新物料使用信息
                        UpdateTaskProcessBom(same);
                        if (task.EndProcess == true && sameCheckNo.IsNullOrEmpty())
                        {
                            //更新工单完工数
                            UpdateTaskWorkOrder(same);
                        }
                    }
                    if (task.EndProcess == true && checkNo.IsNullOrEmpty())
                    {
                        // 成品入库
                        ReportProductStorage(record);
                    }
                    // 创建耗用单
                    woCostItems.AddRange(GenerateConsumptionRecords(task, record));
                }

                RF.Save(records);
                RF.Save(mergeRecords);
                RF.Save(taskProcessBoms);
                tran.Complete();
            }

            if (woCostItems.Count > 0)
            {
                RT.Service.Resolve<BackflushMaterialController>().ExecuteBackflushMaterialAsync(woCostItems);
            }
        }

        /// <summary>
        /// 验证报工记录
        /// </summary>
        /// <param name="record"></param>
        /// <param name="task"></param>
        private void ValidateRecordTask(ReportRecord record, DispatchTask task)
        {
            //if (task.ReportQty + record.OkQty + record.NgQty > task.DispatchQty)
            //{
            //    throw new ValidationException("任务单{0}剩余可报工数为{1}，当前报工数合格{2}，不合格{3}".L10nFormat(task.No, task.DispatchQty - task.ReportQty, record.OkQty, record.NgQty));
            //}
            if (task.TaskStatus == DispatchTaskStatus.Finished)
            {
                throw new ValidationException("当前确认记录报工数大于任务单剩余报工数量{0}".L10nFormat(task.DispatchQty - task.ReportQty));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        private EntityList<TaskProcessBom> GetTaskProcessBomsByTaskIds(List<double> taskIds)
        {
            return taskIds.SplitContains(tempIds =>
            {
                return Query<TaskProcessBom>().Where(p => tempIds.Contains(p.DispatchTaskId)).ToList();
            });
        }

        /// <summary>
        /// 根据任务单id获取
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        private EntityList<DispatchTask> GetDispatchTasksByIds(List<double> taskIds)
        {
            return taskIds.SplitContains(tempIds =>
            {
                return Query<DispatchTask>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取被合并的报工记录
        /// </summary>
        /// <param name="recordIds"></param>
        /// <returns></returns>
        private EntityList<ReportRecord> GetMergeRecordsBySourceId(List<double> recordIds)
        {
            return recordIds.SplitContains(tempIds =>
            {
                return Query<ReportRecord>().Where(p => p.SourceId != null && tempIds.Contains((double)p.SourceId)).ToList();
            });
        }

        /// <summary>
        /// 报工记录审核撤回命令
        /// </summary>
        /// <param name="recordIds"></param>
        public virtual void TaskExamineRevoke(List<double> recordIds)
        {
            var records = GetReportRecordsByIds(recordIds);
            if (!records.Any())
            {
                throw new ValidationException("报工记录数据异常".L10N());
            }
            if (records.Any(p => p.ExamineState != ReportRecordExamineState.ToConfirm))
            {
                throw new ValidationException("报工记录审核状态不为待确认".L10N());
            }
            records.ForEach(p =>
            {
                p.ExamineState = ReportRecordExamineState.Revoke;
            });
            RF.Save(records);
        }

        /// <summary>
        /// 获取任务单BOM
        /// </summary>
        /// <param name="SortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="dispatchTaskId">选中任务单</param>
        /// <returns>BOM</returns>
        public virtual EntityList<ResourcesTasksViewModel> GetResourcesTasksViewModels(IList<OrderInfo> SortInfo, PagingInfo pagingInfo, double dispatchTaskId)
        {
            EntityList<ResourcesTasksViewModel> resourcesTasksViewModels = new EntityList<ResourcesTasksViewModel>();
            var dispatchTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (dispatchTask == null || (dispatchTask.TaskStatus != DispatchTaskStatus.Executing && dispatchTask.TaskStatus != DispatchTaskStatus.Dispatched))
            {
                return resourcesTasksViewModels;
            }
            var adoInfos = Query<DispatchTaskDetail>().Where(m => m.DispatchTaskId == dispatchTaskId).Select((x) => new { Id = x.AdoId, Name = x.AdoName }).ToList<BaseDataInfo>();
            if (adoInfos == null || adoInfos.Count == 0)
            {
                return resourcesTasksViewModels;
            }
            var adoIds = adoInfos.Select(m => m.Id);
            var adoNames = adoInfos.Select(m => m.Name);
            var models = Query<DispatchTask>().Join<DispatchTaskDetail>((x, y) => x.Id == y.DispatchTaskId && x.Id != dispatchTaskId
             && adoIds.Contains(y.AdoId) && adoNames.Contains(y.AdoName))
             .Where(x => x.TaskStatus == DispatchTaskStatus.Dispatched || x.TaskStatus == DispatchTaskStatus.Executing)
             .OrderBy(SortInfo).OrderByDescending(m => m.UpdateDate).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var model in models)
            {

                resourcesTasksViewModels.Add(new ResourcesTasksViewModel
                {
                    AssociatedWorkOrder = model.AssociatedWorkOrder,
                    DispatchQty = model.DispatchQty,
                    ReportQty = model.ReportQty,
                    No = model.No,
                    PlanBeginTime = model.PlanBeginTime,
                    PlanEndTime = model.PlanEndTime,
                    ProcessName = model.ProcessName,
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    //预计生产时长（H）-{（任务数-已报工数）*工序标准工时（分）/60}
                    //RemainingTime = model.ExpectedProductionTime > 0 ? 0 : model.ExpectedProductionTime - ((model.DispatchQty - model.ReportQty) * model.ProcessStandardHour / 60),
                    TaskStatus = model.TaskStatus,
                    ProcessStandardHour = model.ProcessStandardHour,
                    ExpectedProductionTime = model.ExpectedProductionTime,
                    ProcessHourSum = model.ProcessHourSum,
                    UpdateDate = model.UpdateDate,
                    UpdateByName = model.UpdateByName,
                    TaskPerformer = model.TaskPerformer,
                });
            }
            resourcesTasksViewModels.SetTotalCount(models.TotalCount);
            return resourcesTasksViewModels;
        }

        #endregion

        #region 转入标签

        /// <summary>
        /// 根据任务单id获取转入标签记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual EntityList<ReportTransferLabel> GetReportTransferLabels(double taskId)
        {
            return Query<ReportTransferLabel>().Where(p => p.DispatchTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        ///  校验当前标签一转入
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="labelNo"></param>
        /// <returns></returns>
        public virtual bool IsExistsTransferLabels(double taskId, string labelNo)
        {
            var transLabel = Query<ReportTransferLabel>().Where(p => p.DispatchTaskId == taskId && p.LabelNo == labelNo).FirstOrDefault();
            return transLabel != null;
        }


        /// <summary>
        ///  获取已转入的标签
        /// </summary>
        /// <param name="labelNos"></param>
        /// <returns></returns>
        public virtual EntityList<ReportTransferLabel> GetTransferLabelsByLabel(List<string> labelNos)
        {
            return labelNos.SplitContains(tmpNos =>
            {
                return Query<ReportTransferLabel>().Where(p => tmpNos.Contains(p.LabelNo)).ToList();
            });

        }
        #endregion

        #region 报工扫描标签记录


        /// <summary>
        ///  获取扫描标签记录
        /// </summary>
        /// <param name="labelNos"></param>
        /// <returns></returns>
        public virtual EntityList<ReportScanLabelLog> GetReportScanLabelLogsByLabel(List<string> labelNos)
        {
            return labelNos.SplitContains(tmpNos =>
            {
                return Query<ReportScanLabelLog>().Where(p => tmpNos.Contains(p.LabelNo)).ToList();
            });

        }
        #endregion
    }
}