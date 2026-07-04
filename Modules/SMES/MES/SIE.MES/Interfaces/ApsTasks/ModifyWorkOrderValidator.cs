using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单修改接口数据验证
    /// </summary>
    public class ModifyWorkOrderValidator
    {
        /// <summary>
        /// 要修改的工单
        /// </summary>
        private readonly IReadOnlyList<ModifyWoData> modifyWoDatas;

        /// <summary>
        /// 工单列表
        /// </summary>
        private readonly EntityList<WorkOrder> workOrderList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_modifyWoDatas"></param>
        public ModifyWorkOrderValidator(IReadOnlyList<ModifyWoData> _modifyWoDatas)
        {
            modifyWoDatas = _modifyWoDatas;
            List<string> woNos = modifyWoDatas.Select(p => p.WorkOrder).ToList();

            workOrderList = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(woNos);
        }

        /// <summary>
        /// 验证工单
        /// </summary>
        /// <returns></returns>
        public List<ModifyWoResult> ValidateModifyWoData()
        {
            var result = new List<ModifyWoResult>();

            List<WorkOrderInfo> workOrderInfos = new List<WorkOrderInfo>();
            workOrderList.ForEach(p =>
            {
                workOrderInfos.Add(new WorkOrderInfo() { WorkOrderId = p.Id });
            });
            var taskInfos = RT.Service.Resolve<IWorkOrderTask>().WorkOrderTask(workOrderInfos);

            foreach (var data in modifyWoDatas)
            {
                var retData = new ModifyWoResult();
                retData.TaskPlanID = data.TaskPlanID;
                retData.IsSuccess = "N";
                retData.WorkOrder = data.WorkOrder;
                result.Add(retData);

                if (data.WorkOrder.IsNullOrEmpty())
                {
                    retData.Message = "工单号不能为空".L10N();
                    continue;
                }

                var workOrder = workOrderList.FirstOrDefault(p => p.No == data.WorkOrder);
                if (workOrder == null)
                {
                    retData.Message = "工单号[{0}]不存在".L10nFormat(data.WorkOrder);
                    continue;
                }

                if (workOrder.State != Core.WorkOrders.WorkOrderState.Release)
                {
                    retData.Message = "工单[{0}]的状态[{1}]不是发放, 不允许修改"
                        .L10nFormat(workOrder.No, EnumViewModel.EnumToLabel(workOrder.State).L10N());
                    continue;
                }
                else
                {
                    //上线状态描述，电子套件
                    string onlineStateDescription = RT.Service.Resolve<TaskReleaseExtensionController>()
                        .CheckWorkOrderUplineState(workOrder, "");

                    if (!string.IsNullOrEmpty(onlineStateDescription))
                    {
                        retData.Message = "工单[{0}]的上线状态不为空, 不允许修改".L10nFormat(workOrder.No);
                        continue;
                    }
                }

                try
                {
                    var workOrderTaskInfos = taskInfos.Where(p => p.WorkOrderId == workOrder.Id).ToList();

                    if (workOrderTaskInfos.Any() && workOrderTaskInfos.Any(p => p.MergedStatus == 2))
                    {
                        var nos = workOrderTaskInfos.Where(p => p.MergedStatus == 2).Select(p => p.No).Distinct().Concat(";");
                        retData.Message = "工单[{0}]关联的任务单:[{1}]存在合并关系，请先在派工管理解除合并关系再修改".L10nFormat(workOrder.No, nos);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    retData.Message = ex.Message;
                    continue;
                }

                retData.IsSuccess = "Y";
            }
            return result;
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>工单</returns>
        public WorkOrder GetWorkOrder(string workOrderNo)
        {
            return workOrderList.FirstOrDefault(x => x.No == workOrderNo);
        }
    }
}
