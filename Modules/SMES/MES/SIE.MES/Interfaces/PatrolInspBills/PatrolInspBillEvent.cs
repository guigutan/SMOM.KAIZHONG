using SIE.EventMessages.PatrolInspBills;
using SIE.MES.WorkOrders;
using System.Collections.Generic;

namespace SIE.MES.Interfaces.PatrolInspBills
{
    /// <summary>
    /// QMS巡检单获取工单的工序列表
    /// </summary>
    public class PatrolInspBillEvent : IPatrolInspBillEvent
    {
        /// <summary>
        /// 根据工单Id列表获取对应的工序Id列表
        /// </summary>
        /// <param name="woIds">工单Id集合</param>
        /// <returns>工序Id集合</returns>
        public List<double> GetProcessList(List<double> woIds)
        {
            return RT.Service.Resolve<WorkOrderController>().GetProcessIdList(woIds);
        }
    }
}
