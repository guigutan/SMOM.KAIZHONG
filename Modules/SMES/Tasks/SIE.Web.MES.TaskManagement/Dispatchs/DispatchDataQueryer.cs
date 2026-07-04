using Amazon.S3.Model;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Org.BouncyCastle.Asn1.BC;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.WorkOrders;
using SIE.Utils;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工数据查询者
    /// </summary>
    public class DispatchDataQueryer : DataQueryer
    {
        /// <summary>
        /// 是否派工到产线
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public List<double> IsDispatchTaskWipResource(List<double> taskIds)
        {
            return RT.Service.Resolve<DispatchController>().GetWorkOrderRoutingProcessesByTaskIds(taskIds);
        }

        /// <summary>
        /// 选择资源派工
        /// </summary>
        /// <param name="model"></param>
        public void DispatchTaskWipResource(DispatchTaskViewModel model)
        {
            var taskIds = model.TaskId.Split(',').Select(p => Convert.ToDouble(p)).ToList();
            if (model.WipResourceId == 0)
                throw new ValidationException("资源不能为空".L10N());
            //更新任务单资源
            RT.Service.Resolve<DispatchController>().UpdateTaskResource(taskIds, model.WipResourceId);
        }

        /// <summary>
        /// 排程退回校验
        /// </summary>
        /// <param name="taskId"></param>
        public string SchedulingInfReturnValid(List<double> taskIds)
        {
            var msg = "";
            EntityList<DispatchTask> tasks = new EntityList<DispatchTask>();
            foreach (var taskId in taskIds)
            {
                try
                {
                    //当集合为0或者查出来的任务单没有taskId，就参与校验(因为我们校验、退回的时候，是会把同一个排程任务生成的多个任务单一起校验和退回，判断是为了防止，你在选择多条的时候，这多条中可能包含同一个排程任务的，这样可能会导致重复校验，以及返回重复的任务单)(下面方法也会将同一个排程任务的任务单都查出来)
                    if (tasks.Count == 0 || tasks.All(p => p.Id != taskId))
                        tasks.AddRange(RT.Service.Resolve<DispatchController>().SchedulingInfReturnValid(taskId));
                }
                catch (Exception ex)
                {
                    msg += ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                }
            }
            return msg;
        }

        /// <summary>
        /// 选择两笔或者两笔以上的任务单，判断是否已选对象
        /// </summary>
        /// <param name="dispatchTaskIds">所勾选的任单Ids</param>
        /// <returns></returns>
        public bool GetIsSelectedTaskPerformer(List<double> dispatchTaskIds)
        {
            return RT.Service.Resolve<DispatchController>().IsSelectedTaskPerformer(dispatchTaskIds);
        }

        /// <summary>
        /// 获取可执行对象数据
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <param name="dispatchTaskId">当前任务单Id</param>
        /// <param name="adoType">可执行对象类型</param>
        /// <param name="adoName">可执行对象名称</param>
        /// <returns>可执行对象数据</returns>
        public TaskPerformerInfo GetTaskPerformerInfo(List<double> dispatchTaskIds, double dispatchTaskId, string adoType, string adoName)
        {
            return RT.Service.Resolve<DispatchController>().GetTaskPerformerInfo(dispatchTaskIds, dispatchTaskId, adoType, adoName);
        }

        /// <summary>
        /// 获取配置项是否生成任务单
        /// </summary>
        /// <returns>bool</returns>
        public bool GetWoIsGenerateTask()
        {
            var config = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            return config != null && config.IsGenerate;
        }

        /// <summary>
        /// 获取关联任务单列表
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <returns>关联任务单列表</returns>
        public EntityList<AssociatedTask> GetAssociatedTasks(double dispatchTaskId)
        {
            return RT.Service.Resolve<DispatchController>().GetAssociatedDispatchTaskList(dispatchTaskId);
        }

        /// <summary>
        /// 通过对象信息获取报工管理信息
        /// </summary>
        /// <param name="adoInfo">对象信息</param>
        /// <returns>报工管理信息</returns>
        public EntityList<TaskDetailViewModel> GetReportDispatchTaskList(AdoInfo adoInfo)
        {
            AdoType adoType = (AdoType)EnumViewModel.LabelToEnum(adoInfo.AdoType, typeof(AdoType));
            return RT.Service.Resolve<DispatchController>().CreateTaskDetailViewModels(adoInfo.AdoId, adoType);
        }

        /// <summary>
        /// 验证共模主料的工单是否能进行共模比报工
        /// </summary>
        public RstTaskBillInfo IsCanSyntypeReport(WorkOrder wo)
        {
            return RT.Service.Resolve<DispatchController>().IsCanSyntypeReport(wo);
        }
    }
}