using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.MES.TaskManagement.TaskConfigs;
using SIE.MES.WorkOrders;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessTaskLists
{
    /// <summary>
    /// 工序任务清单
    /// </summary>
    public class ProcessTaskListController : DomainController
    {
        /// <summary>
        /// 获取工序任务清单明细
        /// </summary>
        /// <param name="processTaskListCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessTaskListViewModel> GetProcessTaskListList(ProcessTaskListCriteria processTaskListCriteria)
        {

            EntityList<ProcessTaskListViewModel> processTaskListViewModels = new EntityList<ProcessTaskListViewModel>();
            //工单状态（只显示工单状态为：发放、生产中）
            var q = Query<WorkOrderRoutingProcess>()
                .Join<WorkOrder>((worp, wo) => worp.WorkOrderId == wo.Id)
                .Join<Process>((worp, prc) => worp.ProcessId == prc.Id)
                .LeftJoin<WorkOrder, Enterprise>("ent", (wo, ent) => wo.FactoryId == ent.Id)
                .LeftJoin<WorkOrder, WipResource>((wo, res) => wo.ResourceId == res.Id)
                .LeftJoin<WorkOrder, Item>((wo, im) => wo.ProductId == im.Id)
                .LeftJoin<WorkOrder, Enterprise>("ws", (wo, ws) => wo.WorkShopId == ws.Id)
                .Where(worp => worp.IsRequirementTask
               && (worp.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release || worp.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing)
               ).Select<WorkOrder, Process, Enterprise, WipResource, Item, Enterprise>((worp, wo, prc, ent, res, im, ws) => new
               {
                   FactoryName = ent.Name,
                   No = wo.No,
                   Id = worp.Id,
                   PlanBeginDate = wo.PlanBeginDate,
                   PlanEndDate = wo.PlanEndDate,
                   WorkOrderPlanQty = wo.PlanQty,
                   ProductCode = im.Code,
                   ProductName = im.Name,
                   ProcessName = prc.Name,
                   ResourcepName = res.Name,
                   WoState = wo.State,
                   WorkShopName = ws.Name,
               });
            if (!processTaskListCriteria.No.IsNullOrEmpty())
            {
                q.Where<WorkOrder>((worp, wo) => wo.No.Contains(processTaskListCriteria.No));
            }
            if (processTaskListCriteria.ProductId.HasValue)
            {
                q.Where<WorkOrder, Item>((worp, wo, im) => wo.ProductId == processTaskListCriteria.ProductId);
            }
            if (processTaskListCriteria.ProcessId.HasValue)
            {
                q.Where<Process>((worp, prc) => worp.ProcessId == processTaskListCriteria.ProcessId);
            }
            if (processTaskListCriteria.FactoryId.HasValue)
            {
                q.Where<WorkOrder, Enterprise>((worp, wo, ent) => wo.FactoryId == processTaskListCriteria.FactoryId);
            }
            if (processTaskListCriteria.WorkShopId.HasValue)
            {
                q.Where<WorkOrder, Enterprise>((worp, wo, ws) => wo.WorkShopId == processTaskListCriteria.WorkShopId);
            }
            if (processTaskListCriteria.ResourceId.HasValue)
            {
                q.Where<WorkOrder, WipResource>((worp, wo, res) => wo.ResourceId == processTaskListCriteria.ResourceId);
            }
            if (processTaskListCriteria.PlanBeginTime.BeginValue.HasValue)
                q.Where<WorkOrder>((worp, wo) => wo.PlanBeginDate >= processTaskListCriteria.PlanBeginTime.BeginValue);
            if (processTaskListCriteria.PlanBeginTime.EndValue.HasValue)
                q.Where<WorkOrder>((worp, wo) => wo.PlanBeginDate <= processTaskListCriteria.PlanBeginTime.EndValue);

            var totalCoutn = q.Count();
            var result = q.OrderBy(processTaskListCriteria.OrderInfoList).ToList<ProcessTaskListViewModel>(processTaskListCriteria.PagingInfo);

            var routingProcessIds = new List<double?>();
            result.ForEach(item =>
            {
                routingProcessIds.Add(double.Parse(item.Id));
            });
            var distasks = routingProcessIds.SplitContains(ids =>
            {
                return Query<DispatchTask>().Where(m => ids.Contains(m.RoutingProcessId)).ToList();
            });
            foreach (var processTaskListViewModel in result)
            {
                processTaskListViewModel.SendQty = distasks.Where(m => m.RoutingProcessId.ToString() == processTaskListViewModel.Id).Sum(m => m.SendQty);
                processTaskListViewModel.TasksGeneratedQty = distasks.Where(m => m.RoutingProcessId.ToString() == processTaskListViewModel.Id).Sum(m => m.DispatchQty);
            }
            processTaskListViewModels.AddRange(result);
            processTaskListViewModels.SetTotalCount(totalCoutn);
            return processTaskListViewModels;
        }

        /// <summary>
        /// 获取当前工单工序已派的任务单
        /// </summary>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<ResourcesTasksViewModel> GetResourcesTasksViewModels(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, ProcessTaskListViewModel entity)
        {
            var routingProcessId = double.Parse(entity.Id);
            var query = Query<DispatchTask>()
                .Join<WorkOrderRoutingProcess>((x, y) => x.RoutingProcessId == y.Id)
                .Join<WorkOrderRoutingProcess, Process>((y, z) => y.ProcessId == z.Id)
                .Join<Employee>((x, e) => x.UpdateBy == e.Id)
              .Select<WorkOrderRoutingProcess, Process, Employee>((x, y, z, e) => new
              {
                  DispatchTaskId = x.Id,
                  No = x.No,
                  ReportQty = x.ReportQty,
                  DispatchQty = x.DispatchQty,
                  AssociatedWorkOrder = x.AssociatedWorkOrder,
                  ProcessName = z.Name,
                  TaskStatus = x.TaskStatus,
                  TaskPerformer = x.TaskPerformer,
                  UpdateByName = e.Name,
                  UpdateDate = x.UpdateDate,
                  ProcessHourSum=x.ProcessHourSum,
                  ExpectedProductionTime=x.ExpectedProductionTime,
                  ProcessStandardHour=x.ProcessStandardHour,


              }).Where(x => x.RoutingProcessId == routingProcessId);
            var count = query.Count();
            var result = query.ToList<ResourcesTasksViewModel>(pagingInfo);
            EntityList<ResourcesTasksViewModel> resourcesTasksViewModels = new EntityList<ResourcesTasksViewModel>();
            resourcesTasksViewModels.AddRange(result);
            resourcesTasksViewModels.SetTotalCount(count);
            return resourcesTasksViewModels;
        }

        /// <summary>
        /// 拆分派工任务
        /// </summary>
        /// <param name="splitTaskVM"></param>
        /// <returns></returns>
        public virtual void SplitDispatchTask(SplitTaskViewModel splitTaskVM)
        {
            if (splitTaskVM == null)
            {
                throw new ValidationException("参数不能为空".L10N());
            }
            if (splitTaskVM.Qty <= 0)
            {
                throw new ValidationException("本次生成数量必须大于0".L10N());
            }
            if (splitTaskVM.Copies <= 0)
            {
                throw new ValidationException("份数必须大于或等于1".L10N());
            }

            if (splitTaskVM.WoNo.IsNullOrEmpty())
            {
                throw new ValidationException("工单号数据丢失".L10N());
            }

            var maxQty = splitTaskVM.DispatchQty - splitTaskVM.DispatchedTaskQty;
            if (splitTaskVM.Qty * splitTaskVM.Copies > maxQty)
            {
                throw new ValidationException("(本次生成数*份数)必须小于{0}（工单工序数量-已生成任务数）！".L10nFormat(maxQty));
            }
            var workOrder = Query<WorkOrder>().Where(m => m.No == splitTaskVM.WoNo).FirstOrDefault(new EagerLoadOptions().LoadWith(WorkOrder.ProcessBomListProperty));

            var routingProcess = RF.GetById<WorkOrderRoutingProcess>(splitTaskVM.RountingPrcossId);
            var taskBomList = new EntityList<TaskProcessBom>();
            workOrder.ProcessBomList.Where(f => f.ProcessId == routingProcess.ProcessId).ForEach(f =>
            {
                TaskProcessBom taskProcessBom = new TaskProcessBom();
                taskProcessBom.ItemId = f.ItemId;
                taskProcessBom.Qty = f.SingleQty;
                taskProcessBom.ProcessId = f.ProcessId.Value;
                taskBomList.Add(taskProcessBom);
            });
            var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();

            var taskList = new EntityList<DispatchTask>();
            for (int i = 0; i < splitTaskVM.Copies; i++)
            {
                EntityList<TaskProcessBom> boms = new EntityList<TaskProcessBom>();
                boms.Clone(taskBomList, new CloneOptions(CloneActions.IdProperty | CloneActions.NormalProperties | CloneActions.RefEntities));
                var task = new DispatchTask()
                {
                    No = RT.Service.Resolve<DispatchController>().GetTaskNo(taskConfig),
                    SendQty = 0,
                    ReportQty = 0,
                    OkQty = 0,
                    NgQty = 0,
                    FactoryId = workOrder.FactoryId,
                    PlanBeginTime = workOrder.PlanBeginDate,
                    PlanEndTime = workOrder.PlanEndDate,
                    AssociatedWorkOrder = workOrder.No,
                    IsVirtualPart = false,
                    IsSyntype = workOrder.IsCommonMode,
                    TechNo = workOrder.ProcessTechOrderCode,
                    Proportion = workOrder.Proportion,
                    ReportMode = ReportMode.Manual,
                    Priority = DispatchTaskPriority.Normal,
                    TaskStatus = DispatchTaskStatus.ToDispatch,
                    ProductId = workOrder.ProductId,
                    WorkOrderId = workOrder.Id,
                    WorkOrder = workOrder,
                    ResourceId = workOrder.ResourceId,
                    WorkShopId = workOrder.WorkShopId,
                    MergedStatus = MergedStatus.Normal,
                    IsMainTask = true,
                    IsSyntypeReport = workOrder.IsCommonMode
                };
                if (workOrder.IsCommonMode)
                {
                    task.Associated = Associated.Syntype;
                    task.IsMainTask = workOrder.IsMainMaterial;
                }
                task.GenerateId();
                task.ProcessId = routingProcess.ProcessId;
                task.RoutingProcessId = routingProcess.Id;
                task.Seq = routingProcess.Index;
                task.Boms.AddRange(boms);
                task.DispatchQty = splitTaskVM.Qty;
                task.PersistenceStatus = PersistenceStatus.New;
                var standardHourSet = Query<StandardHourSet>().Where(m => m.ProcessId == task.ProcessId && m.WipResourceId == task.ResourceId && m.ProductId == task.ProductId).FirstOrDefault();
                RT.Service.Resolve<DispatchController>().ComputTime(task, standardHourSet);
                taskList.Add(task);
            }
            RF.BatchInsert(taskList);
        }
    }
}
