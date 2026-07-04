using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.MES.Outsourcing;
using SIE.MES.WorkOrders;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 保存委外命令
    /// </summary>
    public class SaveOutsourcingCommand : SaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ValidationException"></exception>
        protected override void OnSaving(EntityList data)
        {
            EntityList<OutsourcingRequest> outsourcingRequests = data as EntityList<OutsourcingRequest>;
            if (outsourcingRequests.Any())
            {
                /*
                2）拆分计算单一工序的委外数量。同工单同工序下，各需求单的需求数量之和）不能大于（工单计划数量-工序过站数量）否则报错。
                 */
                var woIds = outsourcingRequests.Select(x => x.WorkOrderId).ToList();
                var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(woIds);
                //获取所有同工单的需求单
                #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                //var allWoRequest = outsourcingRequests.Where(m => woIds.Contains(m.WorkOrderId));
                //var oldList = RT.Service.Resolve<OutsourcingRequestController>().GetWorkOrderRequests(woIds);
                //var noContainsList = oldList.Where(p => !allWoRequest.Select(m => m.Id).ToList().Contains(p.Id));

                //if (!allWoRequest.Any())
                //{
                //    throw new ValidationException("保存失败，找不到需求单的工单".L10N());
                //}
                //if (noContainsList.Any())
                //{
                //    noContainsList = noContainsList.Concat(allWoRequest);
                //}
                //else
                //{
                //    noContainsList = allWoRequest;
                //}
                #endregion
                //同工单内同一工序出现的次数
                var controller = RT.Service.Resolve<WorkOrderController>();
                //所有工单的工序集合
                var routingProcessList = controller.GetRoutingProcessByWoIds(woIds);
                if (!routingProcessList.Any())
                {
                    throw new ValidationException("工单不存在工艺路线".L10N());
                }


                #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                //var workOrderIds = wos.Select(m => m.Id).Distinct().ToList();
                //var processStatisticslists = RT.Service.Resolve<IProcessStatistics>().GetProcessStatisticsListByWorkOrderIds(workOrderIds);
                #endregion
                foreach (var request in outsourcingRequests)
                {
                    var requestWo = wos.FirstOrDefault(m => m.Id == request.WorkOrderId);
                    if (requestWo == null)
                    {
                        throw new ValidationException("保存失败，找不到需求单【{0}】的工单".L10nFormat(request.NO));
                    }
                    if (request.RequestQty <= 0)
                    {
                        throw new ValidationException("委外需求数量必须大于0！".L10N());
                    }
                    if (request.SupplierId <= 0)
                    {
                        throw new ValidationException("供应商不允许为空！".L10N());
                    }
                    if (request.RequestQty < request.OutboundQty)
                    {
                        throw new ValidationException("委外需求数量不能少于委外出库数量！".L10N());
                    }
                    if (request.RequestQty == request.WarehousingQty)
                    {
                        request.OutsourcingState = OutsourcingState.Completed;

                        if (request.OutboundQty >= request.RequestQty)
                            request.OutboundState = OutboundState.Finish;

                    }


                    #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                    //var processStatisticslist = processStatisticslists.Where(m => m.WOId == requestWo.Id).ToList();
                    //RT.Service.Resolve<OutsourcingRequestController>().CheckProcessTime(noContainsList.AsEntityList(), routingProcessList, request, requestWo,processStatisticslist);
                    #endregion

                    if (request.ProcessingOutsourcingInStockList.Any(x => x.IsDirty))
                    {
                        throw new ValidationException("【委外入库列表】修改后，请使用【委外入库列表】的保存命令。".L10N());
                    }
                }
            }
        }
    }
}
