using SIE.Common;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.MES.Outsourcing.Model;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using SIE.Barcodes.WipBatchs;
using SIE.Resources.Enterprises;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class OutsourcingRequestController : DomainController
    {
        /// <summary>
        /// 获取未发料明细
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<UnProcessingSNViewMidel> GetUnProcessingSNViewMidels(OutsourcingRequest request,PagingInfo pagingInfo)
        {
            var query = Query<WipBatch>()
                .LeftJoin<ProcessingOutbound>((x, y) => x.BatchNo == y.SN && y.OutsourcingRequestId == request.Id)
                .Where<ProcessingOutbound>((x, y) => x.WorkOrderId == request.WorkOrderId && y.SN == null);

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            EntityList<UnProcessingSNViewMidel> viewMidels = new EntityList<UnProcessingSNViewMidel>();

            foreach (var l in list)
            {
                UnProcessingSNViewMidel viewMidel = new UnProcessingSNViewMidel();
                viewMidel.Sn = l.BatchNo;
                viewMidel.Qty = l.Qty; 
                viewMidels.Add(viewMidel);
            }
            viewMidels.SetTotalCount(query.Count());
            return viewMidels;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<OutsourcingRequest> Fetch(OutsourcingRequestCriteria criteria)
        {
            var query = Query<OutsourcingRequest>().As("x");
            if (criteria.BeginProcessId.HasValue && !criteria.EndProcessId.HasValue)
            {
                query.Where(m => m.BeginProcess.ProcessId == criteria.BeginProcessId);
            }
            if (criteria.EndProcessId.HasValue && !criteria.BeginProcessId.HasValue)
            {
                query.Where(m => m.EndProcess.ProcessId == criteria.EndProcessId);
            }
            if (criteria.EndProcessId.HasValue && criteria.BeginProcessId.HasValue)
            {

                query.Join<WorkOrderRoutingProcess>("b1", (a, b1) => a.BeginProcessId == b1.Id)
                    .Join<WorkOrderRoutingProcess>("c", (a, c) => a.EndProcessId == c.Id)
                    .Where<WorkOrderRoutingProcess, WorkOrderRoutingProcess>((x, b1, c) => b1.ProcessId == criteria.BeginProcessId && c.ProcessId == criteria.EndProcessId);
            }
            if (!criteria.NO.IsNullOrEmpty())
            {
                query.Where(m => m.NO.Contains(criteria.NO));
            }
            if (criteria.WorkOrderId.HasValue)
            {
                query.Where(m => m.WorkOrderId == criteria.WorkOrderId);
            }
            if (!criteria.SupplierIds.IsNullOrEmpty())
            {
                string[] ids = criteria.SupplierIds.Split(',');
                var doubleIds = new List<double>();
                ids.ForEach(n => doubleIds.Add(double.Parse(n)));
                query.Where(m => doubleIds.Contains((double)m.SupplierId));
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(m => criteria.FactoryId == m.FactoryId);
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(m => criteria.WorkShopId == m.WorkOrder.WorkShopId);
            }
            if (!criteria.WorkShopCode.IsNullOrEmpty())
                query.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkOrder.WorkShopId).WhereIf(criteria.WorkShopCode.IsNotEmpty(), p => p.Code == criteria.WorkShopCode));
            if (criteria.WipResourceId.HasValue)
            {
                query.Where(m => criteria.WipResourceId == m.WorkOrder.ResourceId);
            }
            if (criteria.PlanBeginDate.BeginValue.HasValue)
            {
                query.Where(m => criteria.PlanBeginDate.BeginValue >= m.WorkOrder.PlanBeginDate);
            }
            if (criteria.PlanBeginDate.EndValue.HasValue)
            {
                query.Where(m => criteria.PlanBeginDate.EndValue <= m.WorkOrder.PlanBeginDate);
            }
            if (criteria.OutsourcingState.HasValue)
            {
                query.Where(m => criteria.OutsourcingState == m.OutsourcingState);
            }
            if (!criteria.ProduceName.IsNullOrEmpty())
            {
                query.Where(m => criteria.ProduceName.Contains(m.WorkOrder.Product.Name));
            }
            if (criteria.OutboundState != null)
                query.Where(p => p.OutboundState == criteria.OutboundState);
            if (criteria.ReportState != null)
                query.Where(p => p.ReportState == criteria.ReportState);
            if (!criteria.OutFactory.IsNullOrEmpty())
                query.Where(p => p.OutFactory == criteria.OutFactory);
            if (!criteria.Sn.IsNullOrEmpty())
                query.Exists<ProcessingOutbound>((x, y) => y.Where(p => p.OutsourcingRequestId == x.Id && criteria.Sn.Contains(p.SN)));
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存添加需求单
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void SaveAddModel(AddModel entity)
        {
            var controller = RT.Service.Resolve<WorkOrderController>();
            var routingProcessList = controller.GetRoutingProcess(entity.WorkOrderId);
            if (!routingProcessList.Any())
            {
                throw new ValidationException("工单不存在工艺路线".L10N());
            }
            if (!routingProcessList.Any(m => m.Outsourcing))
            {
                throw new ValidationException("工单不存在委外工序，无法生成委外需求".L10N());
            }
            //if (entity.Qty > entity.WorkOrder.PlanQty) //委外需求数量不能大于工单计划数量
            //{
            //    throw new ValidationException("委外需求数量不能大于工单计划数量".L10N());
            //}
            var routingProcessIds = routingProcessList.Select(s => s.Id).ToList();
            var parameterList = controller.GetWorkOrderRoutingProcessParameter(routingProcessIds);

            var processRoutings = GetOutsourcingProcess(routingProcessList, parameterList);
            if (processRoutings.Any())
            {
                #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                //var currentWos = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(new List<double>() { entity.WorkOrderId });
                //var allWoRequest = RT.Service.Resolve<OutsourcingRequestController>().GetWorkOrderRequests(new List<double>() { entity.WorkOrderId });  
                //var processStatisticslist = RT.Service.Resolve<IProcessStatistics>().GetProcessStatisticsList(entity.WorkOrderId);
                #endregion
                EntityList<OutsourcingRequest> saveEntities = new EntityList<OutsourcingRequest>();
                foreach (var procssList in processRoutings)
                {
                    if (procssList.Any())
                    {
                        var beginProcess = procssList.FirstOrDefault();
                        var endProcess = procssList.LastOrDefault();
                        if (beginProcess != null && endProcess != null)
                        {
                            var newRecord = new OutsourcingRequest()
                            {
                                WorkOrderId = entity.WorkOrderId,
                                WorkOrder = entity.WorkOrder,
                                FactoryId = entity.WorkOrder.FactoryId.HasValue ? entity.WorkOrder.FactoryId.Value : 0,
                                RequestQty = entity.Qty,
                                SupplierId = entity.SupplierId,
                                BeginProcessId = beginProcess.Id,
                                EndProcessId = endProcess.Id,
                                OutsourcingState = OutsourcingState.NotStarted,
                            };
                            var processLinks = "";
                            procssList.ForEach(item => processLinks += item.Id + ",");
                            newRecord.ProcessLinks = processLinks;
                            #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                            //allWoRequest.Add(newRecord);
                            //this.CheckProcessTime(allWoRequest, procssList.AsEntityList(), newRecord, currentWos.FirstOrDefault(), processStatisticslist);
                            #endregion
                            saveEntities.Add(newRecord);
                        }
                    }
                    var number = saveEntities.Count;
                    if (number > 0)
                    {

                        var Nolist = GetOutsourcingRequestCode(number);
                        for (int i = 0; i < number; i++)
                        {
                            saveEntities[i].NO = Nolist[i];
                        }
                        RF.Save(saveEntities);
                    }
                }
            }
        }

        /// <summary>
        /// 获取工序对
        /// </summary>
        /// <param name="routingProcessList"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private List<List<WorkOrderRoutingProcess>> GetOutsourcingProcess(EntityList<WorkOrderRoutingProcess> routingProcessList, EntityList<WorkOrderRoutingProcessParameter> parameterList)
        {
            List<List<WorkOrderRoutingProcess>> workOrderRoutingProcesses = new List<List<WorkOrderRoutingProcess>>();
            var beginProcess = routingProcessList.FirstOrDefault(m => m.Sign == RoutingProcessSign.Start);
            if (beginProcess == null)
            {
                if (routingProcessList.Count == 1 && routingProcessList.First().Outsourcing)
                {
                    workOrderRoutingProcesses.Add(routingProcessList.ToList());
                    return workOrderRoutingProcesses;
                }
                throw new ValidationException("找不到开始工序".L10N());
            }
            List<WorkOrderRoutingProcess> processesList = new List<WorkOrderRoutingProcess>();
            GetOutsourcingList(workOrderRoutingProcesses,
           processesList, parameterList, beginProcess, routingProcessList);
            return workOrderRoutingProcesses;
        }

        /// <summary>
        /// 递归获取每一段工艺路线
        /// </summary>
        /// <param name="workOrderRoutingProcesses"></param>
        /// <param name="processesList"></param>
        /// <param name="parameterList"></param>
        /// <param name="beginProcess"></param>
        /// <param name="routingProcessList"></param>
        private void GetOutsourcingList(List<List<WorkOrderRoutingProcess>> workOrderRoutingProcesses,
            List<WorkOrderRoutingProcess> processesList, EntityList<WorkOrderRoutingProcessParameter> parameterList,
            WorkOrderRoutingProcess beginProcess, EntityList<WorkOrderRoutingProcess> routingProcessList)
        {
            if (beginProcess.Outsourcing)
            {
                processesList.Add(beginProcess);
            }
            //找下一个工序
            var processParameterList = parameterList.Where(w => w.ProcessId == beginProcess.Id).ToList();
            ////生成下级工序
            if (processParameterList.Any())
            {
                foreach (var parameter in processParameterList)
                {
                    if (((ResultType)parameter.ResultType & ResultType.Pass) == ResultType.Pass)
                    {
                        if (!parameter.NextProcessId.HasValue)
                        {
                            if (processesList.Any())
                            {
                                workOrderRoutingProcesses.Add(processesList);
                            }
                            continue;
                        }

                        var nextProcess = routingProcessList.FirstOrDefault(m => m.Id == parameter.NextProcessId);
                        if (nextProcess == null || !nextProcess.Outsourcing)
                        {
                            if (processesList.Any())
                            {
                                workOrderRoutingProcesses.Add(processesList);
                                processesList = new List<WorkOrderRoutingProcess>();
                            }
                            if (nextProcess != null && nextProcess.Sign == RoutingProcessSign.End)
                            {
                                if (processesList.Any())
                                {
                                    workOrderRoutingProcesses.Add(processesList);
                                }
                                processesList = new List<WorkOrderRoutingProcess>();
                            }
                            if (nextProcess != null && !nextProcess.Outsourcing)//当前只是非委外继续往下找
                            {
                                GetOutsourcingList(workOrderRoutingProcesses, processesList, parameterList, nextProcess, routingProcessList);
                            }
                        }
                        else
                        {
                            GetOutsourcingList(workOrderRoutingProcesses, processesList, parameterList, nextProcess, routingProcessList);
                        }

                    }
                }
            }
            else
            {
                if (processesList.Any())
                {
                    workOrderRoutingProcesses.Add(processesList);
                }
            }
        }


        /// <summary>
        ///获取委外需求单编码
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<string> GetOutsourcingRequestCode(int number)
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(OutsourcingRequest));
            if (config == null || config.BacodeRule == null)
            {
                throw new ValidationException("未找到委外需求单编码生成规则,请检查规则配置".L10N());
            }

            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule, number).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual int SaveTemporaryAddModel(TemporaryAddModel entity)
        {
            var index = 0;
            var controller = RT.Service.Resolve<WorkOrderController>();

            var routingProcessList = controller.GetRoutingProcess(entity.WorkOrderId).OrderBy(m => m.Index).AsEntityList();
            if (!routingProcessList.Any())
            {
                throw new ValidationException("工单不存在工艺路线".L10N());
            }

            //if (routingProcessList.Count == 1)
            //{
            //    throw new ValidationException("工单只有一个工序，不允许委外！".L10N());
            //}
            var routingProcessIds = routingProcessList.Select(s => s.Id).ToList();
            var parameterList = controller.GetWorkOrderRoutingProcessParameter(routingProcessIds);

            List<List<WorkOrderRoutingProcess>> workOrderRoutingProcesses = new List<List<WorkOrderRoutingProcess>>();
            var beginProcess = routingProcessList.FirstOrDefault(m => m.Id == entity.BeginRoutingProcess.Id);
            if (beginProcess == null)
            {
                throw new ValidationException("找不到开始工序".L10N());
            }

            List<WorkOrderRoutingProcess> processesList = new List<WorkOrderRoutingProcess>();
            GetProcessLinkList(workOrderRoutingProcesses,
           processesList, parameterList, beginProcess, routingProcessList);

            if (workOrderRoutingProcesses.Any())
            {
                var startProcess = entity.BeginRoutingProcess;
                var endProcess = entity.EndRoutingProcess;

                #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                //var currentWos = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(new List<double>() { entity.WorkOrderId });
                //var allWoRequest = RT.Service.Resolve<OutsourcingRequestController>().GetWorkOrderRequests(new List<double>() { entity.WorkOrderId });
                //var processStatisticslist = RT.Service.Resolve<IProcessStatistics>(). GetProcessStatisticsList(entity.WorkOrderId);
                #endregion
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    EntityList<OutsourcingRequest> outsourcingRequests = new EntityList<OutsourcingRequest>();
                    foreach (var processLink in workOrderRoutingProcesses)
                    {
                        if (processLink.FindIndex(m => m.Id == startProcess.Id) >= 0
                        && processLink.FindIndex(m => m.Id == endProcess.Id) >= 0)
                        {
                            index++;
                            var newRecord = new OutsourcingRequest()
                            {
                                WorkOrderId = entity.WorkOrderId,
                                WorkOrder = entity.WorkOrder,
                                FactoryId = entity.WorkOrder.FactoryId.HasValue ? entity.WorkOrder.FactoryId.Value : 0,
                                RequestQty = entity.Qty,
                                SupplierId = entity.SupplierId,
                                BeginProcessId = beginProcess.Id,
                                EndProcessId = endProcess.Id,
                                OutsourcingState = OutsourcingState.NotStarted,
                            };
                            StringBuilder processLinks = new StringBuilder();
                            foreach (var item in processLink)
                            {
                                if (item.Id < endProcess.Id)
                                {
                                    processLinks.AppendLine(item.Id + ",");
                                }
                                else if (item.Id == endProcess.Id)
                                {
                                    processLinks.AppendLine(item.Id + ",");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            newRecord.ProcessLinks = processLinks.ToString();
                            newRecord.ProcessLinks = newRecord.ProcessLinks.TrimEnd(',');
                            #region 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差 ps:该代码保留 后续可能项目会用得上 请勿删除
                            //allWoRequest.Add(newRecord);
                            //this.CheckProcessTime(allWoRequest, processLink.AsEntityList(), newRecord, currentWos.FirstOrDefault(), processStatisticslist);
                            #endregion
                            outsourcingRequests.Add(newRecord);
                        }
                    }
                    var number = outsourcingRequests.Count;
                    if (number > 0)
                    {

                        var Nolist = GetOutsourcingRequestCode(number);
                        for (int i = 0; i < number; i++)
                        {
                            outsourcingRequests[i].NO = Nolist[i];
                        }
                        RF.Save(outsourcingRequests);
                    }


                    tran.Complete();
                }

            }
            return index;
        }

        /// <summary>
        /// 获取工序链
        /// </summary>
        /// <param name="workOrderRoutingProcesses"></param>
        /// <param name="processesList"></param>
        /// <param name="parameterList"></param>
        /// <param name="beginProcess"></param>
        /// <param name="routingProcessList"></param>
        private void GetProcessLinkList(List<List<WorkOrderRoutingProcess>> workOrderRoutingProcesses,
            List<WorkOrderRoutingProcess> processesList, EntityList<WorkOrderRoutingProcessParameter> parameterList,
            WorkOrderRoutingProcess beginProcess, EntityList<WorkOrderRoutingProcess> routingProcessList)
        {
            processesList.Add(beginProcess);
            //找下一个工序
            var processParameterList = parameterList.Where(w => w.ProcessId == beginProcess.Id).ToList();
            ////生成下级工序
            foreach (var parameter in processParameterList)
            {
                if (((ResultType)parameter.ResultType & ResultType.Pass) == ResultType.Pass)
                {
                    if (!parameter.NextProcessId.HasValue)
                    {
                        if (processesList.Any())
                        {
                            workOrderRoutingProcesses.Add(processesList);
                        }
                        continue;
                    }
                    var nextProcess = routingProcessList.FirstOrDefault(m => m.Id == parameter.NextProcessId);
                    if (nextProcess == null || nextProcess.Sign == RoutingProcessSign.End)
                    {
                        if (processesList.Any())
                        {
                            workOrderRoutingProcesses.Add(processesList);
                            processesList.Add(nextProcess);
                            processesList = new List<WorkOrderRoutingProcess>();
                        }
                        if (nextProcess != null && nextProcess.Sign != RoutingProcessSign.End)//当前非空非结束继续往下找
                        {
                            GetProcessLinkList(workOrderRoutingProcesses, processesList, parameterList, nextProcess, routingProcessList);
                        }
                    }
                    else
                    {
                        GetProcessLinkList(workOrderRoutingProcesses, processesList, parameterList, nextProcess, routingProcessList);
                    }

                }
            }
        }

        /// <summary>
        /// 根据工单Id和工序获取工序委外需求单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        public virtual EntityList<OutsourcingRequest> GetOutsourcingRequestsByWoIdProcessCode(double workOrderId,string processCode)
        {
            var list = Query<OutsourcingRequest>().Where(p => p.WorkOrderId == workOrderId && p.BeginProcess.Process.Code == processCode).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取指定工单的所有需求单
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<OutsourcingRequest> GetWorkOrderRequests(List<double> woIds)
        {
            return Query<OutsourcingRequest>().Where(m => woIds.Contains(m.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取需求单
        /// </summary>
        /// <param name="cardIds"></param>
        /// <returns></returns>
        public virtual EntityList<OutsourcingRequest> GetOutsourcingRequestByIds(List<double> cardIds)
        {
            return cardIds.SplitContains(ids => Query<OutsourcingRequest>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 删除需求单
        /// </summary>
        /// <param name="requestIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void DeleteRequest(List<double> requestIds)
        {
            var requests = GetOutsourcingRequestByIds(requestIds);
            foreach (var request in requests)
            {
                if (request.OutsourcingState != OutsourcingState.NotStarted)
                {
                    throw new ValidationException("需求单【{0}】不为未开始状态，删除失败".L10nFormat(request.NO));
                }
                request.PersistenceStatus = PersistenceStatus.Deleted;
            }
            RF.Save(requests);
        }
        /// <summary>
        /// 同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差  该方法暂时保留 勿删 项目可能要使用 但产品要求放开
        /// </summary>
        /// <param name="allWoRequest"></param>
        /// <param name="routingProcessList"></param>
        /// <param name="request"></param>
        /// <param name="requestWo"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void CheckProcessTime(EntityList<OutsourcingRequest> allWoRequest, EntityList<WorkOrderRoutingProcess> routingProcessList, OutsourcingRequest request, WorkOrder requestWo, List<ProcessStatisticsEventInfo> processStatisticsEventInfos)
        {
            Dictionary<double, int> processAppearTime = new Dictionary<double, int>();
            var allRequests = allWoRequest.Where(m => m.WorkOrderId == request.WorkOrderId).ToList();
            foreach (var curRequest in allRequests)
            {
                if (!curRequest.ProcessLinks.IsNullOrEmpty())
                {
                    var wipProcess = curRequest.ProcessLinks.Split(',').ToList();
                    foreach (var process in wipProcess)
                    {
                        if (!process.IsNullOrEmpty() && process != "\r\n")
                        {
                            if (processAppearTime.ContainsKey(double.Parse(process)))
                            {
                                processAppearTime[double.Parse(process)] += (int)curRequest.RequestQty;
                            }
                            else
                            {
                                processAppearTime.Add(double.Parse(process), (int)curRequest.RequestQty);
                            }
                        }
                    }
                }
            }
            foreach (var process in processAppearTime.Keys)
            {
                var routingProcess = routingProcessList.FirstOrDefault(m => m.WorkOrderId == request.WorkOrderId && m.Id == process);
                var passQty = routingProcess == null ? 0 : processStatisticsEventInfos
                    .Where(m => m.ProcessId == routingProcess.ProcessId
                    && m.ProcessIndex == routingProcess.Index).Sum(p => p.InputQty);
                if (processAppearTime[process] > requestWo.PlanQty - passQty)
                {
                    throw new ValidationException("同工单同工序下，各需求单的需求数量之和不能大于工单计划数量-工序过站数量之差！".L10N());
                }
            }
        }
    }
}



