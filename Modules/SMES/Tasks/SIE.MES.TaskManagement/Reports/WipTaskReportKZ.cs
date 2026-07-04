using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.WIP.Pressure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 工序生产任务报工(KZ)
    /// </summary>
    public class WipTaskReportKZ : ITaskReportKZ
    {

        /// <summary>
        /// 热处理 报工
        /// </summary>
        /// <param name="reportInfos">报工数据</param>
        public virtual void HeatTreatmentReport(List<ReportInfo> reportInfos)
        {
            TaskReport(reportInfos, "热处理");
        }

        /// <summary>
        /// 耐压采集 报工
        /// </summary>
        /// <param name="reportInfos">报工数据</param>
        public void PressureReport(List<ReportInfo> reportInfos)
        {
            TaskReport(reportInfos, "电性能测试,耐压测试");
        }

        /// <summary>
        /// 包装采集 报工
        /// </summary>
        /// <param name="reportInfos">报工数据</param>
        public void PackingReport(List<ReportInfo> reportInfos, bool IsTaskFinish = true)
        {
            TaskReport(reportInfos, "包装", reportInfos.FirstOrDefault()?.ResourceId, IsTaskFinish);
        }

        /// <summary>
        /// 工序 报工
        /// </summary>
        /// <param name="reportInfos"></param>
        /// <param name="process">工序编码/名称</param>
        /// <param name="resourceId">资源</param>
        /// <param name="IsTaskFinish">是否完工(判断是否满足任务单数量就直接完工)</param>
        public virtual void TaskReport(List<ReportInfo> reportInfos, string process, double? resourceId = null, bool IsTaskFinish = true)
        {
            if (!reportInfos.Any())
                throw new ValidationException("没有要报工的数据");
            var processList = process.Split(',');
            var submitInfos = new List<PdaScanSubmitInfo>();
            reportInfos.GroupBy(p => new { p.WorkOrderId }).ForEach(g =>
            {
                var woId = g.Key.WorkOrderId;
                var list = g.ToList();
                var isAutoFeeding = list.First().IsAutoFeeding;
                var tasks = new EntityList<DispatchTask>();
                //判断是否有指定任务单，主要是 连接器批次包装用到
                if (g.All(p => p.TaskId != null && p.TaskId > 0))
                {
                    var taskIds = g.Select(p => p.TaskId.Value).Distinct().ToList();
                    tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(taskIds);
                }
                else
                {
                    tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { woId });
                }
                //var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByResourceId(resourceId, null, null);
                //当只有包含包装的时候，就只获取含有包装字眼的工序
                var taskList = new List<DispatchTask>();

                if (process.Contains("包装"))
                {
                    taskList = tasks.Where(p => p.WorkOrderId == woId && p.ProcessCode.Contains("包装") || p.ProcessName.Contains("包装")).OrderBy(p => p.PlanBeginTime).ToList();
                }
                else
                {
                    taskList = tasks.Where(p => p.WorkOrderId == woId && (processList.Contains(p.ProcessCode) || processList.Contains(p.ProcessName)) /*&& (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched)*/).OrderBy(p => p.PlanBeginTime).ToList();
                }
                if (resourceId > 0)
                    taskList = taskList.Where(p => p.ResourceId == resourceId).ToList();
                if (taskList.Count == 0)
                    throw new ValidationException("未匹配到对应的[{0}]任务单".L10nFormat(process));
                //把任务单和状态也显示出来
                if (taskList.All(p => p.TaskStatus != DispatchTaskStatus.Executing && p.TaskStatus != DispatchTaskStatus.Dispatched))
                {
                    var nos = taskList.Select(p => p.No).ToList();
                    var status = taskList.Select(p => p.TaskStatus.ToLabel()).ToList();
                    throw new ValidationException("任务单[{0}]状态为[{1}]".L10nFormat(string.Join('、', nos), string.Join("、", status)));
                }
                //把符合的任务单拿出来操作
                taskList = taskList.Where(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched).ToList();

                var totalReportQty = list.Sum(p => p.GoodQty);   //总共报工数量
                var totalRemainQty = 0m; //剩余可报数量
                taskList.ForEach(p =>
                {
                    var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(p).Item2;
                    totalRemainQty += MaxRemainQty;//p.MaxRemainQty;
                    //if (p.Id == taskList.Last().Id)
                    //{
                    //    var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(p).Item2;

                    //    totalRemainQty += MaxRemainQty;//p.MaxRemainQty;
                    //}
                    //else
                    //    totalRemainQty += p.RemainQty;
                });
                if (totalReportQty > totalRemainQty)
                    throw new ValidationException("当前提交报工数量[{0}]已超出剩余可报工数量[{1}]".L10nFormat(totalReportQty, totalRemainQty));

                //同一工单有多个任务时,需要拆分数量报工
                foreach (var task in taskList)
                {
                    var remainQty = (decimal)task.RemainQty; //剩余可报数量

                    //最后一个任务允许超计划,不超容差
                    if (task.Id == taskList.Last().Id && !process.Contains("包装"))
                    {
                        var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2;

                        remainQty = MaxRemainQty;//task.MaxRemainQty;
                    }
                    else if (process.Contains("包装"))
                    {
                        var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2;

                        remainQty = MaxRemainQty;//task.MaxRemainQty;
                    }
                    //分配报工数量
                    var submitList = new List<ReportInfo>();
                    foreach (var p in list.Where(p => (p.TaskId == null || p.TaskId == 0 || p.TaskId == task.Id) && p.GoodQty > 0))
                    {
                        if (remainQty <= 0)
                            break;
                        var tempQty = remainQty > p.GoodQty ? p.GoodQty : remainQty; //分配数,两者取小

                        var packingReportInfo = p.Copy();
                        packingReportInfo.GoodQty = tempQty;
                        submitList.Add(packingReportInfo);

                        p.GoodQty -= tempQty;
                        remainQty -= tempQty;
                        if (process.Contains("包装"))
                        {
                            //记录绑定了哪个任务单
                            DB.Update<WipBatch>().Set(p => p.PackingTaskId, task.Id).Where(wip => wip.BatchNo == p.Sn).Execute();
                        }
                    }

                    if (submitList.Count == 0)
                        continue;

                    PdaScanSubmitInfo submitInfo = submitInfos.FirstOrDefault(p=>p.DispatchTaskId == task.Id);
                    if (submitInfo == null)
                    {
                        submitInfo = new PdaScanSubmitInfo()
                        {
                            //IsSkipValidatePreQty = process.Contains("包装"),  //包装工序允许跳过前置校验
                            ScanType = 1,
                            ResourceId = resourceId ?? task.ResourceId ?? 0,
                            ProcessId = task.ProcessId ?? 0,
                            DispatchTaskId = task.Id,
                            WorkOrderId = woId,
                            IsAutoFeeding = isAutoFeeding,
                            IsTaskFinish = IsTaskFinish,
                            DetailInfos = new List<ScanDetailInfo>()
                        };
                        submitInfos.Add(submitInfo);
                    }
                    submitInfo.DetailInfos.AddRange(
                        submitList.Select(p => new ScanDetailInfo()
                        {
                            Sn = p.Sn,
                            GoodQty = p.GoodQty,
                            Qty = p.GoodQty
                        })
                    );
                }

            });

            if (submitInfos.Count > 0)
            {
                submitInfos.ForEach(submitInfo =>
                {
                    var ret = RT.Service.Resolve<ReportController>().SubmitScanInfo(submitInfo);
                });
            }
        }

        /// <summary>
        /// 校验前置工序任务是否已报工
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="process">工序编码/名称</param>
        public virtual object ValidatePrepareProcessHasReport(string sn, string process)
        {
            WipBatch wipBatch = null;
            wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(sn);
            if (wipBatch == null && process.Contains("包装"))
            {
                var wipPressureSn = RT.Service.Resolve<WipPressureController>().GetWipPressureSn(sn);
                if (wipPressureSn != null)
                {
                    wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(wipPressureSn.WipPressure?.BatchNo);
                }
            }
            if (wipBatch == null)
                throw new ValidationException("标签[{0}]不存在或无法找到对应的工序标签信息".L10nFormat(sn));

            //校验前置任务是否都有该标签的报工记录(如果是CS打印生成的)
            if ((wipBatch.IsPressureSnPrint == null || wipBatch.IsPressureSnPrint == false) && wipBatch.ReportRecordIds.IsNullOrEmpty())
                throw new ValidationException("对应工序标签[{0}]还未进行报工,请确认".L10nFormat(sn));

            //查询工单对应的所有任务单
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { wipBatch.WorkOrderId });
            if (tasks.Count == 0)
                return null;

            var taskProcess = new DispatchTask();
            //当只有包含包装的时候，就只获取含有包装字眼的工序
            if (process.Contains("包装"))
            {
                taskProcess = tasks.FirstOrDefault(p => (p.ProcessCode.Contains("包装") || p.ProcessName.Contains("包装")) && (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched));//当前工序任务
            }
            else
            {
                var processList = process.Split(',');
                taskProcess = tasks.FirstOrDefault(p => (processList.Contains(p.ProcessCode) || processList.Contains(p.ProcessName)) && (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched));//当前工序任务
            }
            if (taskProcess == null)
                throw new ValidationException("未匹配到对应的[{0}]任务单".L10nFormat(process));

            RT.Service.Resolve<ReportController>().ValidatePrepareProcessHasReport(wipBatch, taskProcess);

            return taskProcess;
        }

        /// <summary>
        /// 校验标签是否已报工
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public virtual bool ValidateProcessHasReport(string batchNo, string process)
        {
            return RT.Service.Resolve<ReportController>().ValidateProcessHasReport(batchNo, process, false);

        }
    }
}
