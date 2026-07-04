using SIE.Domain;
using SIE.EventMessages.Release;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// APS与MES计划任务控制器类
    /// </summary>
    public class ApsMesTaskController : DomainController
    {
        /// <summary>
        /// 获取计划任务关联工单
        /// </summary>
        /// <param name="planTaskId">计划任务ID</param>
        /// <returns>计划任务关联实体</returns>
        public virtual TaskUnion GetTaskUnion(string planTaskId)
        {
            var querys = Query<TaskUnion>();
            if (!planTaskId.IsNullOrWhiteSpace())
            {
                querys.Where(x => x.PlanTaskId == planTaskId);
            }
            return querys.FirstOrDefault();
        }

        /// <summary>
        /// 获取计划任务关联工单
        /// </summary>
        /// <param name="planTaskIds">计划任务ID列表</param>
        /// <returns>计划任务关联实体</returns>
        public virtual EntityList<TaskUnion> GetTaskUnions(List<string> planTaskIds)
        {
            return planTaskIds.SplitContains(tempIds =>
            {
                return Query<TaskUnion>()
                     .Where(x => tempIds.Contains(x.PlanTaskId))
                     .ToList();
            });
        }

        /// <summary>
        /// 获取计划任务关联明细集合
        /// </summary>
        /// <param name="taskUnionIds">计划任务ID</param>
        /// <returns>计划任务明细集合</returns>
        public virtual EntityList<TaskUnionDetail> GetTaskUnionDetails(List<double> taskUnionIds)
        {
            return taskUnionIds.SplitContains(tempIds =>
            {
                return Query<TaskUnionDetail>()
                    .Where(x => tempIds.Contains(x.TaskUnionId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取计划任务关联明细集合
        /// </summary>
        /// <param name="isFinish">是否已经全部报工</param>
        /// <param name="isMainItem">是否主料</param>
        /// <returns>计划任务关联明细集合</returns>
        public virtual EntityList<TaskUnionDetail> GetTaskUnionDetails(bool? isFinish, bool? isMainItem)
        {
            var querys = Query<TaskUnionDetail>();
            if (isFinish.HasValue)
            {
                querys.Where(x => x.IsFinish == isFinish);
            }
            if (isMainItem.HasValue)
            {
                querys.Where(x => x.IsMainItem == isMainItem);
            }

            return querys.ToList();
        }


        /// <summary>
        /// 获取计划任务关联明细实体
        /// </summary>
        /// <param name="detailIds">明细ID列表</param>
        /// <returns>计划任务明细实体</returns>
        public virtual EntityList<TaskUnionDetail> GetTaskUnionDetails(List<string> detailIds)
        {
            return detailIds.SplitContains(tempIds =>
            {
                return Query<TaskUnionDetail>()
                    .Where(x => tempIds.Contains(x.DetailId))
                    .ToList();
            });
        }


        /// <summary>
        /// 获取待报工的计划任务
        /// </summary>
        /// <returns>计划任务</returns>
        public virtual EntityList<TaskUnion> GetToReportTasks()
        {
            return Query<TaskUnion>()
                .Exists<TaskUnionDetail>((t, p) => p.Where(f => f.TaskUnionId == t.Id && !f.IsFinish))
                .ToList();
        }

        /// <summary>
        /// 获取待报工的计划任务明细
        /// </summary>
        /// <returns>计划任务明细</returns>
        public virtual EntityList<TaskUnionDetail> GetToReportTaskDetails(List<double> taskUnionIds)
        {
            return taskUnionIds.SplitContains((taskUnionIdList) =>
            {
                return Query<TaskUnionDetail>()
                .Join<TaskUnion>((d, u) => d.TaskUnionId == u.Id && taskUnionIdList.Contains(u.Id))
                .Where(p => !p.IsFinish)
                .ToList();
            });
        }

        /// <summary>
        /// 获取计划任务关联明细实体
        /// </summary>
        /// <param name="detailId">明细ID</param>
        /// <returns>计划任务明细实体</returns>
        public virtual TaskUnionDetail GetTaskUnionDetail(string detailId)
        {
            var querys = Query<TaskUnionDetail>();
            if (!detailId.IsNullOrWhiteSpace())
            {
                querys.Where(x => x.DetailId == detailId);
            }
            return querys.FirstOrDefault();
        }

        /// <summary>
        /// 获取工单报工集合
        /// </summary>
        /// <param name="workOrderIds">工单ID集合</param>
        /// <returns>工单报工集合</returns>
        public virtual EntityList<WorkOrderReport> GetWorkOrderReports(List<double> workOrderIds)
        {
            var querys = workOrderIds.SplitContains(tempIds =>
            {
                return Query<WorkOrderReport>().Where(x => tempIds.Contains(x.WorkOrderId)).ToList();
            }); 
            return querys;
        }

        /// <summary>
        /// 报工成功后更新TaskUnionWorkOrder和WorkOrderReport
        /// </summary>
        /// <param name="mesDailyScheduleEvents">Mes的生产日进度信息集合</param>
        /// <param name="taskUnionDtls">计划任务关联工单集合</param>
        public virtual void UpdateTaskUnionWorkOrderReport(List<MesDailySchedulePlanData> mesDailyScheduleEvents,
            EntityList<TaskUnionDetail> taskUnionDtls)
        {
            if (mesDailyScheduleEvents == null)
            {
                return;
            }
            var workOrderIds = taskUnionDtls.Select(x => x.WorkOrderId).ToList();
            var workOrderReports = GetWorkOrderReports(workOrderIds);

            foreach (var mesDailyItem in mesDailyScheduleEvents)
            {
                var curTaskUnionDetail = taskUnionDtls.FirstOrDefault(x => x.WorkOrderId == mesDailyItem.WorkOrderId
                                                            && x.DetailId == mesDailyItem.DetailId);
                if (curTaskUnionDetail != null)
                {
                    curTaskUnionDetail.TotalQty += mesDailyItem.FinishedAmount;
                    if (curTaskUnionDetail.TotalQty >= mesDailyItem.PlanQty)
                        curTaskUnionDetail.IsFinish = true;
                    RF.Save(curTaskUnionDetail);
                }

                var curWorkOrderReport = workOrderReports.FirstOrDefault(x => x.WorkOrderId == mesDailyItem.WorkOrderId);
                if (curWorkOrderReport != null)
                {
                    curWorkOrderReport.StatisticId = mesDailyItem.StatisticId;
                    RF.Save(curWorkOrderReport);
                }
                else
                {
                    var newWorkOrderReport = new WorkOrderReport();
                    newWorkOrderReport.StatisticId = mesDailyItem.StatisticId;
                    newWorkOrderReport.WorkOrderId = mesDailyItem.WorkOrderId;
                    RF.Save(newWorkOrderReport);
                }
            }
        }
    }
}
