using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders.WorkOrderProcessBomGenerators
{
    /// <summary>
    /// 工单工序BOM验证器
    /// </summary>
    public static class WorkOrderProcessBomValidator
    {
        /// <summary>
        /// 验证工单的包装规则
        /// </summary>
        /// <param name="workOrder">工单</param>        
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate(WorkOrder workOrder)
        {
            if (workOrder == null || workOrder.Id <= 0 || workOrder.ProcessBomList == null
                || !workOrder.ProcessBomList.Any())
            {
                return;
            }

            var processBoms = RT.Service.Resolve<WorkOrderController>()
                .GetWoProcessBomList(new List<double> { workOrder.Id });

            foreach (var processBom in workOrder.ProcessBomList)
            {
                if (processBoms.Any(x => x.Id != processBom.Id
                    && x.ItemId == processBom.ItemId
                    && x.RoutingProcessId == processBom.RoutingProcessId
                    && x.WorkStepId == processBom.WorkStepId))
                {
                    throw new ValidationException("工序BOM的物料{0} 和 工序{1} 不能重复"
                        .L10nFormat(processBom.Item.Name, processBom.Process.Name));
                }
            }
        }
    }
}
