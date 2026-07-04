using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单数据缓存类
    /// </summary>
    public class WorkOrderDataOwner
    {

        /// <summary>
        /// 下达明细与工单对应字典
        /// </summary>
        readonly Dictionary<string, WorkOrder> workOrdersDictionary = new Dictionary<string, WorkOrder>();

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(string detailId)
        {
            if (workOrdersDictionary.ContainsKey(detailId))
            {
                return workOrdersDictionary[detailId];
            }
            return null;
        }

        /// <summary>
        /// 缓存工单
        /// </summary>
        /// <param name="detailId">下达明细ID</param>
        /// <param name="workOrder">工单</param>
        /// <returns></returns>
        public void CacheWorkOrder(string detailId, WorkOrder workOrder)
        {
            if (!workOrdersDictionary.ContainsKey(detailId))
            {
                workOrdersDictionary.Add(detailId, workOrder);
            }
        }

    }
}
