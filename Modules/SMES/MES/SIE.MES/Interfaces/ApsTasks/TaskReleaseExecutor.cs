using NPOI.SS.Formula.Functions;
using SIE.Common;
using SIE.Common.Messages;
using SIE.Core.Items;
using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单下达执行者
    /// </summary>
    public class TaskReleaseExecutor
    {
        /// <summary>
        /// 下达计划数据集合
        /// </summary>
        private readonly IReadOnlyList<ReleasePlanData> releasePlanDatas;

        /// <summary>
        /// 工单下达数据验证器
        /// </summary>
        private readonly TaskReleaseValidator taskReleaseValidator;

        /// <summary>
        /// 工单生成器
        /// </summary>
        private readonly WorkOrderGenerator workOrderGenerator;

        /// <summary>
        /// 数据库的时间
        /// </summary>
        private readonly DateTime dateTime = RF.Find<WorkOrder>().GetDbTime();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_releasePlanDatas">下达计划数据集合</param>
        public TaskReleaseExecutor(IReadOnlyList<ReleasePlanData> _releasePlanDatas)
        {
            this.releasePlanDatas = _releasePlanDatas;

            //下达数据验证
            taskReleaseValidator = new TaskReleaseValidator(_releasePlanDatas);

            //工单生成器
            workOrderGenerator = new WorkOrderGenerator(releasePlanDatas, taskReleaseValidator, dateTime);
        }

        /// <summary>
        /// 计划任务下达
        /// </summary>
        /// <returns>下达结果集合</returns>
        public List<ReleasePlanResult> TaskReleasePlanResult()
        {
            List<ReleasePlanResult> releasePlanResults = new List<ReleasePlanResult>();

            //计划任务关联
            EntityList<TaskUnion> taskUnions = new EntityList<TaskUnion>();

            //计划任务关联明细
            EntityList<TaskUnionDetail> taskUnionDetails = new EntityList<TaskUnionDetail>();

            //已经创建的工单
            EntityList<WorkOrder> workOrders = new EntityList<WorkOrder>();

            //打印模板设置
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates
                = new EntityList<Core.Items.LabelPrintTemplate>();

            //工单工艺路线布局
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts
                = new EntityList<WorkOrderRoutingLayout>();

            foreach (var curReleasePlanData in releasePlanDatas)
            {
                var curReleasePlanResult = new ReleasePlanResult(curReleasePlanData.PlanTaskId);

                try
                {
                    taskReleaseValidator.ValidataReleasePlanData(curReleasePlanData, curReleasePlanResult); //10.验证当前的下达计划数据
                    if (!curReleasePlanResult.IsSuccess)
                    {
                        continue;
                    }

                    TaskUnion currentTaskUnion = CreateTaskUnion(curReleasePlanData);
                    taskUnions.Add(currentTaskUnion);
                    if (!curReleasePlanResult.IsSuccess) //20.创建计划任务关联
                    {
                        continue;
                    }

                    //辅单先创建，主单后创建，任务单生成是根据主单加载辅料工单 //先创建组合板工单
                    foreach (var curReleasePlanDetail in curReleasePlanData.Details
                        .OrderBy(p => p.IsMainItem)
                        .OrderByDescending(p => p.IsPanelWorkOrder))
                    {
                        var workOrder = workOrderGenerator.ReleaseCreateWrorkOrder(curReleasePlanData,
                            curReleasePlanDetail, curReleasePlanResult, labelPrintTemplates,
                            workOrderRoutingLayouts, workOrders);

                        if (!curReleasePlanResult.IsSuccess) //30.创建工单及保存工单
                        {
                            break;
                        }

                        workOrders.Add(workOrder);

                        taskUnionDetails.Add(CreateTaskUnionDetail(curReleasePlanDetail, workOrder, currentTaskUnion));
                    }
                }
                catch (Exception exc)
                {
                    var excMsg = "计划下达异常: {0} ".L10nFormat(exc.Message); //60. 创建失败的下达结果
                    TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, excMsg);
                }
                finally
                {
                    //不成功，则不保存计划关联
                    if (curReleasePlanResult != null && !curReleasePlanResult.IsSuccess)
                    {
                        var toRemoveTaskUnion = taskUnions
                            .FirstOrDefault(x => x.PlanTaskId == curReleasePlanResult.PlanTaskId);

                        // 20240319修改，但存在1单2明细，2.1失效删除表头导致2.2引用失效
                        // 或 1单1明细，1单1明细，两单相同，第1单1明细失效删除表头导致第2单1明细引用失效
                        var otherTaskDetailCount = releasePlanDatas.GroupBy(p => p.PlanTaskId).Sum(p => p.Sum(x => x.Details.Count));
                        if (toRemoveTaskUnion != null && otherTaskDetailCount < 2)
                        {
                            taskUnions.Remove(toRemoveTaskUnion);
                        }

                        foreach (var releaseDetailResult in curReleasePlanResult.Details)
                        {
                            var taskUnionDetail = taskUnionDetails
                                .FirstOrDefault(x => x.DetailId == releaseDetailResult.DetailId);

                            if (taskUnionDetail != null)
                            {
                                taskUnionDetails.Remove(taskUnionDetail);
                            }
                        }
                    }

                    releasePlanResults.Add(curReleasePlanResult);
                }
            }

            //工单号为空的，生成工单号
            BatachSetWorkOrderNos(workOrders);

            //验证工单号是否重复
            WorkOrderValidator.ValidateNoIfDuplicate(workOrders);

            //批量保存数据
            BatachSave(taskUnions, taskUnionDetails, workOrders, labelPrintTemplates, workOrderRoutingLayouts);

            //保存工单日志
            SaveWorkOrderLogs(releasePlanResults);

            //更新工艺路线版本引用次数
            foreach (var group in workOrders.Where(x => x.VersionId.HasValue)
                .GroupBy(x => x.VersionId.Value)
                .Select(g => new { VersionId = g.Key, Count = g.Count() }))
            {
                RT.Service.Resolve<RoutingController>().UpdateVersionRefTimes(group.VersionId, group.Count);
            }

            return releasePlanResults;
        }

        /// <summary>
        /// Ebs下达
        /// </summary>
        /// <returns></returns>
        public List<ReleasePlanResult> EbsTaskReleasePlanResult()
        {
            List<ReleasePlanResult> releasePlanResults = new List<ReleasePlanResult>();

            //计划任务关联
            EntityList<TaskUnion> taskUnions = new EntityList<TaskUnion>();

            //计划任务关联明细
            EntityList<TaskUnionDetail> taskUnionDetails = new EntityList<TaskUnionDetail>();

            //已经创建的工单
            EntityList<WorkOrder> workOrders = new EntityList<WorkOrder>();

            // 历史已创建的工单
            EntityList<WorkOrder> hisWorkOrders = new EntityList<WorkOrder>();
            hisWorkOrders.AddRange(GetExistWorkOrder());

            //打印模板设置
            EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates
                = new EntityList<Core.Items.LabelPrintTemplate>();

            //工单工艺路线布局
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts
                = new EntityList<WorkOrderRoutingLayout>();

            foreach (var curReleasePlanData in releasePlanDatas)
            {
                var curReleasePlanResult = new ReleasePlanResult(curReleasePlanData.PlanTaskId);

                try
                {
                    taskReleaseValidator.EbsValidataReleasePlanData(curReleasePlanData, curReleasePlanResult); //10.验证当前的下达计划数据
                    if (!curReleasePlanResult.IsSuccess)
                    {
                        continue;
                    }

                    //TaskUnion currentTaskUnion = CreateTaskUnion(curReleasePlanData);
                    //taskUnions.Add(currentTaskUnion);
                    if (!curReleasePlanResult.IsSuccess) //20.创建计划任务关联
                    {
                        continue;
                    }

                    //辅单先创建，主单后创建，任务单生成是根据主单加载辅料工单 //先创建组合板工单
                    foreach (var curReleasePlanDetail in curReleasePlanData.Details
                        .OrderBy(p => p.IsMainItem)
                        .OrderByDescending(p => p.IsPanelWorkOrder))
                    {
                        var workOrderTuple = workOrderGenerator.EbsReleaseCreateWrorkOrder(curReleasePlanData,
                            curReleasePlanDetail, curReleasePlanResult, labelPrintTemplates,
                            workOrderRoutingLayouts, workOrders, hisWorkOrders);
                        var workOrder = workOrderTuple?.Item1;
                        var isNew = workOrderTuple?.Item2 ?? false;

                        if (!curReleasePlanResult.IsSuccess) //30.创建工单及保存工单
                        {
                            break;
                        }
                        if (workOrder != null && isNew)
                        {
                            workOrders.Add(workOrder);
                        }
                        //taskUnionDetails.Add(CreateTaskUnionDetail(curReleasePlanDetail, workOrder, currentTaskUnion));
                    }
                }
                catch (Exception exc)
                {
                    var excMsg = "计划下达异常: {0} ".L10nFormat(exc.Message); //60. 创建失败的下达结果
                    TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, excMsg);
                }
                finally
                {
                    //不成功，则不保存计划关联
                    if (curReleasePlanResult != null && !curReleasePlanResult.IsSuccess)
                    {
                        var toRemoveTaskUnion = taskUnions
                            .FirstOrDefault(x => x.PlanTaskId == curReleasePlanResult.PlanTaskId);

                        // 20240319修改，但存在1单2明细，2.1失效删除表头导致2.2引用失效
                        // 或 1单1明细，1单1明细，两单相同，第1单1明细失效删除表头导致第2单1明细引用失效
                        var otherTaskDetailCount = releasePlanDatas.GroupBy(p => p.PlanTaskId).Sum(p => p.Sum(x => x.Details.Count));
                        if (toRemoveTaskUnion != null && otherTaskDetailCount < 2)
                        {
                            taskUnions.Remove(toRemoveTaskUnion);
                        }

                        foreach (var releaseDetailResult in curReleasePlanResult.Details)
                        {
                            var taskUnionDetail = taskUnionDetails
                                .FirstOrDefault(x => x.DetailId == releaseDetailResult.DetailId);

                            if (taskUnionDetail != null)
                            {
                                taskUnionDetails.Remove(taskUnionDetail);
                            }
                        }
                    }

                    releasePlanResults.Add(curReleasePlanResult);
                }
            }

            //工单号为空的，生成工单号
            BatachSetWorkOrderNos(workOrders);

            //验证工单号是否重复
            WorkOrderValidator.ValidateNoIfDuplicate(workOrders);

            //工单打印模板
            RF.BatchInsert(labelPrintTemplates);
            //工单            
            //设置打印模板和工单工艺路线布局的Id
            workOrders.ForEach(wo =>
            {
                wo.TemplateId = wo.Template.Id;
                wo.LayoutId = wo.Layout.Id;
            });

            RF.BatchInsert(workOrders);

            //工单工艺路线布局
            RF.BatchInsert(workOrderRoutingLayouts);

            //工单BOM
            var workOrderBoms = workOrders.SelectMany(x => x.BomList).AsEntityList();
            workOrderBoms.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
            });
            RF.BatchInsert(workOrderBoms);

            //工单联副产品
            var workOrderOutputProduct = workOrders.SelectMany(x => x.WorkOrderOutputProductList).AsEntityList();
            workOrderOutputProduct.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
            });
            RF.BatchInsert(workOrderOutputProduct);


            //工单工序清单的列表
            var routingProcesses = workOrders.SelectMany(x => x.RoutingProcessList).AsEntityList();
            routingProcesses.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
            });
            RF.BatchInsert(routingProcesses);

            //工单工序清单的参数列表
            var processParameters = routingProcesses.SelectMany(x => x.ParameterList).AsEntityList();
            processParameters.ForEach(x =>
            {
                x.ProcessId = x.Process.Id;
                if (x.NextProcess != null)
                {
                    x.NextProcessId = x.NextProcess.Id;
                }
            });
            RF.BatchInsert(processParameters);

            //工单工序清单的工序BOM配置列表
            var routingBomConfigs = routingProcesses.SelectMany(x => x.BomConfigList).AsEntityList();
            routingBomConfigs.ForEach(x =>
            {
                x.ProcessId = x.Process.Id;
            });
            RF.BatchInsert(routingBomConfigs);

            //工单工序BOM清单的列表
            var processBoms = workOrders.SelectMany(x => x.ProcessBomList).AsEntityList();
            //设置工序BOM的工单工序清单的ID
            processBoms.ForEach(x =>
            {
                x.RoutingProcessId = x.RoutingProcess.Id;
                x.WorkOrderId = x.WorkOrder.Id;
            });
            RF.BatchInsert(processBoms);

            //工单与包装规则关系的列表
            var packageRuleDetails = workOrders.SelectMany(x => x.PackageRuleDetailList).AsEntityList();
            packageRuleDetails.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
            });
            RF.BatchInsert(packageRuleDetails);

            //包装单位与工序关系
            var processPackingUnits = packageRuleDetails.SelectMany(x => x.WorkOrderProcessPackingUnitList).AsEntityList();
            processPackingUnits.ForEach(x =>
            {
                x.PackageRuleId = x.PackageRule.Id;
            });
            RF.BatchInsert(processPackingUnits);

            BatchEditSave(hisWorkOrders);


            //保存工单日志
            SaveWorkOrderLogs(releasePlanResults);

            //更新工艺路线版本引用次数
            foreach (var group in workOrders.Where(x => x.VersionId.HasValue)
                .GroupBy(x => x.VersionId.Value)
                .Select(g => new { VersionId = g.Key, Count = g.Count() }))
            {
                RT.Service.Resolve<RoutingController>().UpdateVersionRefTimes(group.VersionId, group.Count);
            }

            return releasePlanResults;
        }

        private void BatchEditSave(EntityList<WorkOrder> workOrders)
        {
            // 修改
            RF.Save(workOrders);
            RF.Save(workOrders.SelectMany(p => p.BomList).AsEntityList());
        }

        private void SaveWorkOrderLogs(List<ReleasePlanResult> releasePlanResults)
        {
            string successMsg = "下达成功!".L10N();

            EntityList<WorkOrderLog> workOrderLogs = new EntityList<WorkOrderLog>();
            foreach (var releasePlanResult in releasePlanResults)
            {
                var releasePlanData = this.releasePlanDatas
                    .FirstOrDefault(x => x.PlanTaskId == releasePlanResult.PlanTaskId);

                if (releasePlanData == null)
                {
                    continue;
                }

                foreach (var releasePlanDetail in releasePlanData.Details)
                {
                    try
                    {
                        WorkOrder workOrder = workOrderGenerator.GetWorkOrder(releasePlanDetail.DetailId);

                        if (workOrder == null)
                        {
                            continue;
                        }

                        WorkOrderLog log = RT.Service.Resolve<WorkOrderController>()
                           .CreateWorkOrderLog(workOrder.Id, WorkOrderLogType.Release, "Ebs操作", dateTime);

                        workOrderLogs.Add(log);

                        //工单创建事件通知发布
                        RT.EventBus.Publish(new WorkOrderCreateEvent() { WorkOrderId = workOrder.Id });

                        releasePlanResult.Details.Add(TaskReleaseHelper.CreateReleaseDetailResult(releasePlanDetail.DetailId,
                            releasePlanDetail.ProcessTechOrderCode, "Ebs操作成功[{0}]".L10nFormat(workOrder.No), workOrder.No));
                    }
                    catch (Exception exMsg)
                    {
                        var message = "计划下达保存工单异常: {0} ".L10nFormat(exMsg.Message);

                        releasePlanResult.Details.Add(TaskReleaseHelper.CreateReleaseDetailResult(releasePlanDetail.DetailId,
                            releasePlanDetail.ProcessTechOrderCode, message, string.Empty));
                        TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, false, string.Empty);
                    }
                }

                if (releasePlanResult.IsSuccess)
                {
                    TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, true, successMsg);
                }
            }

            //批量获取工单日志的Id
            TaskReleaseHelper.BatchSetIds(workOrderLogs);
            RF.BatchInsert(workOrderLogs);
        }

        private EntityList<WorkOrder> GetExistWorkOrder()
        {
            List<string> workOrderNos = new List<string>();
            foreach(var item in releasePlanDatas)
            {
                workOrderNos.Add(item.Details[0].WorkOrder);
            }
            var workOrderList = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(workOrderNos);
            return workOrderList;
        }

        private void BatachSave(EntityList<TaskUnion> taskUnions, EntityList<TaskUnionDetail> taskUnionDetails,
            EntityList<WorkOrder> workOrders, EntityList<LabelPrintTemplate> labelPrintTemplates,
            EntityList<WorkOrderRoutingLayout> workOrderRoutingLayouts)
        {
            //工单打印模板
            RF.BatchInsert(labelPrintTemplates);

            //工单工艺路线布局
            RF.BatchInsert(workOrderRoutingLayouts);

            //计划关联
            RF.BatchInsert(taskUnions);

            //工单            
            //设置打印模板和工单工艺路线布局的Id
            workOrders.ForEach(wo =>
            {
                wo.TemplateId = wo.Template.Id;
                wo.LayoutId = wo.Layout.Id;
                wo.PersistenceStatus = PersistenceStatus.New;
            });
            
            RF.BatchInsert(workOrders);

            //计划关联明细            
            taskUnionDetails.ForEach(taskUnionDetail =>
            {
                taskUnionDetail.TaskUnionId = taskUnionDetail.TaskUnion.Id;
                taskUnionDetail.WorkOrderId = taskUnionDetail.WorkOrder.Id;
                taskUnionDetail.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(taskUnionDetails);

            //工单BOM
            var workOrderBoms = workOrders.SelectMany(x => x.BomList).AsEntityList();
            workOrderBoms.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(workOrderBoms);

            //工单联副产品
            var workOrderOutputProduct = workOrders.SelectMany(x => x.WorkOrderOutputProductList).AsEntityList();
            workOrderOutputProduct.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(workOrderOutputProduct);

            //工单工序清单的列表
            var routingProcesses = workOrders.SelectMany(x => x.RoutingProcessList).AsEntityList();
            routingProcesses.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(routingProcesses);

            //工单工序清单的参数列表
            var processParameters = routingProcesses.SelectMany(x => x.ParameterList).AsEntityList();
            processParameters.ForEach(x =>
            {
                x.ProcessId = x.Process.Id;
                if (x.NextProcess != null)
                {
                    x.NextProcessId = x.NextProcess.Id;
                }
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(processParameters);

            //工单工序清单的工序BOM配置列表
            var routingBomConfigs = routingProcesses.SelectMany(x => x.BomConfigList).AsEntityList();
            routingBomConfigs.ForEach(x =>
            {
                x.ProcessId = x.Process.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(routingBomConfigs);

            //工单工序BOM清单的列表
            var processBoms = workOrders.SelectMany(x => x.ProcessBomList).AsEntityList();
            //设置工序BOM的工单工序清单的ID
            processBoms.ForEach(x =>
            {
                x.RoutingProcessId = x.RoutingProcess.Id;
                x.WorkOrderId = x.WorkOrder.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(processBoms);

            //工单与包装规则关系的列表
            var packageRuleDetails = workOrders.SelectMany(x => x.PackageRuleDetailList).AsEntityList();
            packageRuleDetails.ForEach(x =>
            {
                x.WorkOrderId = x.WorkOrder.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(packageRuleDetails);

            //包装单位与工序关系
            var processPackingUnits = packageRuleDetails.SelectMany(x => x.WorkOrderProcessPackingUnitList).AsEntityList();
            processPackingUnits.ForEach(x =>
            {
                x.PackageRuleId = x.PackageRule.Id;
                x.PersistenceStatus = PersistenceStatus.New;
            });
            RF.BatchInsert(processPackingUnits);
        }

        private static void BatachSetWorkOrderNos(EntityList<WorkOrder> workOrders)
        {
            var workOrdersNoIsEmpty = workOrders.Where(x => x.No.IsNullOrEmpty()).ToList();
            if (workOrdersNoIsEmpty.Any())
            {
                var noList = RT.Service.Resolve<WorkOrderController>()
                    .GetWorkOrderNos(workOrdersNoIsEmpty.Count);
                for (int i = 0; i < noList.Count(); i++)
                {
                    workOrdersNoIsEmpty[i].No = noList[i];
                }
            }
        }

        /// <summary>
        /// 创建计划任务关联工单实体
        /// </summary>
        /// <param name="releasePlanData">下达计划数据</param>
        /// <returns>计划任务关联工单实体</returns>
        private TaskUnion CreateTaskUnion(ReleasePlanData releasePlanData)
        {
            var newTaskUnion = new TaskUnion();
            newTaskUnion.PlanTaskId = releasePlanData.PlanTaskId;
            newTaskUnion.PlanNo = releasePlanData.PlanNo;
            newTaskUnion.MouldId = releasePlanData.MouldId;
            newTaskUnion.MouldBarId = releasePlanData.MouldBarId;
            newTaskUnion.IsSameMode = releasePlanData.IsSameMode;
            newTaskUnion.WorkShopId = releasePlanData.WorkShopId;
            newTaskUnion.ResourceId = releasePlanData.WipResourceId;
            return newTaskUnion;
        }

        /// <summary>
        /// 创建计划任务明细实体
        /// </summary>
        /// <param name="releasePlanDetail">下达计划明细</param>
        /// <param name="workOrder">工单</param>
        /// <param name="taskUnion">计划任务关联</param>        
        /// <returns>计划任务明细实体</returns>
        private TaskUnionDetail CreateTaskUnionDetail(ReleasePlanDetail releasePlanDetail, WorkOrder workOrder, TaskUnion taskUnion)
        {
            var curTaskDetail = new TaskUnionDetail();
            curTaskDetail.DetailId = releasePlanDetail.DetailId;
            curTaskDetail.CustomerId = releasePlanDetail.CustomerId;
            curTaskDetail.ProcessTechOrderCode = releasePlanDetail.ProcessTechOrderCode;
            curTaskDetail.ProductionOrderNo = releasePlanDetail.ProductionOrderCode;
            curTaskDetail.IsMainItem = releasePlanDetail.IsMainItem;
            curTaskDetail.TotalQty = 0;
            curTaskDetail.IsFinish = false;
            curTaskDetail.WorkOrder = workOrder;
            curTaskDetail.TaskUnion = taskUnion;

            return curTaskDetail;
        }
    }
}
