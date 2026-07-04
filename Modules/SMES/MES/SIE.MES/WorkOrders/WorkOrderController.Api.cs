using SIE.Api;
using SIE.Common;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Items;
using SIE.MES.WorkOrders.Events;
using SIE.MES.WorkOrders.Models;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单控制器 API接口
    /// </summary>
    public partial class WorkOrderController : DomainController
    {
        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="queryInfo">工单查询信息</param>
        /// <returns>分页工单信息</returns>
        [ApiService("获取工单列表")]
        [return: ApiReturn("分页工单信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWorkOrdertInfos([ApiParameter("工单查询信息")] WorkOrderQueryInfo queryInfo)
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
                workOrders = GetWorkOrders(queryInfo.ResourceId, queryInfo.StateList, queryInfo.Keyword, pagingInfo);
            else
                workOrders = GetWorkOrders(pagingInfo, queryInfo.Keyword, queryInfo.ResourceId);
            var infos = new List<BaseDataInfo>();
            workOrders.ForEach(workOrder =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workOrder.Id,
                    Code = workOrder.No,
                    Name = workOrder.No,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = workOrders.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取工单工艺路线工序列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>工序列表</returns>
        [ApiService("获取工单工艺路线工序列表")]
        [return: ApiReturn("工序列表 List<BaseDataInfo>")]
        public virtual List<BaseDataInfo> GetRoutingProcessInfo([ApiParameter("工单ID")] double workOrderId)
        {
            var data = new List<BaseDataInfo>();
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
                throw new ValidationException("找不到该工单！".L10N());
            foreach (var routingProces in workOrder.RoutingProcessList)
            {
                var baseData = new BaseDataInfo();
                baseData.Id = routingProces.ProcessId ?? 0;
                baseData.Code = routingProces.Process?.Name;
                baseData.Name = routingProces.Process?.Name;
                data.Add(baseData);
            }
            return data;
        }
    }
}