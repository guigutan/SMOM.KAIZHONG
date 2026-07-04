using SIE.Api;
using SIE.Domain;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SIE.MES.ForWinform
{
    /// <summary>
    /// XP版本工单控制器
    /// </summary>
    public class WinFormWorkOrderController : WorkOrderController
    {
        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="queryInfo">工单查询信息</param>
        /// <returns>分页工单信息</returns>
        [ApiService("获取工单列表")]
        [return: ApiReturn("工单信息列表 PagingBaseDataInfo")]
        public virtual List<WorkOrderInfo> GetWorkOrdertInfos([ApiParameter("工单查询信息")] WorkOrderQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            EntityList<WorkOrder> workOrders;
            if (queryInfo.StateList != null && queryInfo.StateList.Count > 0)
            {
                workOrders = GetWorkOrders(queryInfo.ResourceId, queryInfo.StateList, queryInfo.Keyword, pagingInfo);
            }
            else
            {
                workOrders = GetWorkOrders(pagingInfo, queryInfo.Keyword, queryInfo.ResourceId);
            }
            var infos = new List<WorkOrderInfo>();
            workOrders.ForEach(workOrder =>
            {
                infos.Add(new WorkOrderInfo(workOrder));
            });
            return infos;
        }

        /// <summary>
        /// 切换产线工单
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        [ApiService("切换产线工单")]
        [return: ApiReturn("工单信息列表 PagingBaseDataInfo")]
        public virtual WorkOrderInfo ChangeWipResourceWorkOrder([ApiParameter("工单查询信息")] double woId, [ApiParameter("工作单元信息")] WIP.Workcell workcell)
        {
            var changeWorkOrder = RT.Service.Resolve<WipController>().ChangeWipResourceWorkOrder(woId, workcell);
            if (changeWorkOrder != null)
            {
                RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = changeWorkOrder.Id });
                var workOrder = RF.GetById<WorkOrder>(changeWorkOrder.Id, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null)
                {
                    var result = new WorkOrderInfo(workOrder);
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取产线在生产的工单
        /// </summary>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ApiService("获取产线在生产的工单")]
        [return: ApiReturn("当前工作单元下生产的工单")]
        public virtual WorkOrderInfo GetWipResourceWorkOrder([ApiParameter("工作单元信息")] WIP.Workcell workcell)
        {

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            var wipLineWorkOrder = Query<WipResourceWorkOrder>()
                .Where(f => f.ResourceId == workcell.ResourceId && f.ProcessId == workcell.ProcessId && f.StationId == workcell.StationId)
                .FirstOrDefault();
            if (wipLineWorkOrder != null)
            {
                var workOrder = RF.GetById<WorkOrder>(wipLineWorkOrder.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null)
                {
                    var result = new WorkOrderInfo(workOrder);
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据id获取工单
        /// </summary>
        /// <param name="woId">工单id</param>
        /// <returns>工单信息</returns>
        [ApiService("根据id获取工单")]
        [return: ApiReturn("工单信息")]
        public virtual WorkOrderInfo GetWoInfoById([ApiParameter("工单id")] double woId)
        {
            var workOrder = RF.GetById<WorkOrder>(woId, new EagerLoadOptions().LoadWithViewProperty());
            if (workOrder != null)
            {
                var result = new WorkOrderInfo(workOrder);
                return result;
            }
            return null;
        }

        /// <summary>
        /// 根据id获取工单包装规则
        /// </summary>
        /// <param name="woId">工单id</param>
        /// <returns>工单包装规则信息</returns>
        [ApiService("根据id获取工单包装规则")]
        [return: ApiReturn("工单包装规则信息")]
        public virtual List<WorkOrderPackageRuleDetail> GetWoPackageRules([ApiParameter("工单id")] double woId)
        {
            var result = Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == woId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return result.ToList();
        }

        /// <summary>
        /// 获取工单工序BOM列表
        /// </summary>
        /// <param name="workeOrderId"></param>
        /// <returns></returns>
        [ApiService("获取工单工序BOM列表")]
        [return: ApiReturn("获取工单工序BOM列表")]
        public virtual List<WorkOrderProcessBom> GetWorkerProcessBomList([ApiParameter("工单Id")] double workeOrderId)
        {
            var result = Query<WorkOrderProcessBom>().Where(m => m.WorkOrderId == workeOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return result.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workeOrderId"></param>
        /// <returns></returns>

        [ApiService("获取工单BOM列表")]
        [return: ApiReturn("获取工单BOM列表")]
        public virtual List<WorkOrderProcessBom> GetWorkOrderBom([ApiParameter("工单Id")] double workeOrderId)
        {
            var result = Query<WorkOrderProcessBom>().Where(m => m.WorkOrderId == workeOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return result.ToList();
        }


    }
}
