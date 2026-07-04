using DocumentFormat.OpenXml.Bibliography;
using Microsoft.VisualBasic;
using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.Dispatchs;
using SIE.MES.Outsourcing;
using SIE.MES.Outsourcing.Configs;
using SIE.MES.ProcessProperty;
using SIE.MES.WorkOrders.Events;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单逻辑
    /// </summary>
    public class WoRoutingProcessGenerator
    {
        /// <summary>
        /// 工艺路线的工序清单
        /// </summary>
        private readonly EntityList<RoutingProcess> routingProcesses;

        /// <summary>
        /// 工序列表
        /// </summary>
        public EntityList<Process> Processs { get; }

        /// <summary>
        /// 工序清单参数
        /// </summary>
        private readonly EntityList<RoutingProcessParameter> routingProcessParameters;

        /// <summary>
        /// 是否获取工艺路线的工序BOM配置
        /// </summary>
        private readonly bool isGetRoutingBomConfig;

        /// <summary>
        /// 工序BOM配置
        /// </summary>
        private readonly EntityList<RoutingProcessBomConfig> routingProcessBomConfigs;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="routingVersionIds">工艺路线版本Id 列表</param>
        /// <param name="_isGetRoutingBomConfig">是否获取工艺路线的工序BOM配置</param>
        public WoRoutingProcessGenerator(List<double> routingVersionIds, bool _isGetRoutingBomConfig)
        {
            routingProcesses = AppRuntime.Service.Resolve<RoutingController>()
                .GetRoutingProcessList(routingVersionIds);

            //工序列表
            var processIds = routingProcesses.Where(x => x.ProcessId.HasValue)
                .Select(x => x.ProcessId.Value).Distinct().ToList();
            Processs = AppRuntime.Service.Resolve<ProcessController>().GetProcessByIds(processIds, loadViewProperty: false);

            //工序清单参数
            var routingProcessIds = routingProcesses
                .Select(x => x.Id).Distinct().ToList();
            routingProcessParameters = RT.Service.Resolve<RoutingController>()
                .GetRoutingProcessParameters(routingProcessIds);

            //是否获取工艺路线的工序BOM配置
            this.isGetRoutingBomConfig = _isGetRoutingBomConfig;

            //如果工单配置项【工单工序BOM配置项】的【工序bom来源】配置为【产品工序BOM】时，则不获取工艺路线的【工序BOM配置】资料
            //配置项对应 WorkOrderProcessBomSourceConfig()
            //配置在工单功能 typeof(WorkOrder)
            if (isGetRoutingBomConfig)
            {
                //工序BOM 配置列表
                routingProcessBomConfigs = RT.Service.Resolve<RoutingController>()
                    .GetRoutingProcessBomConfigs(routingProcessIds);
            }
        }

        /// <summary>
        /// 生成工单工艺路线工序清单
        /// </summary>
        /// <param name="workOrder">工单</param>        
        /// <param name="setDisplayProperty"></param>
        /// <param name="isGenChildren">是否同时生成子列表</param>
        /// <param name="routingList">工序清单</param>        
        /// <remarks>BS生成工序清单的子列表不能传到前端，在保存的时候才重新生成子</remarks>
        public void GenerateRoutingProcesss(WorkOrder workOrder,
            bool setDisplayProperty,
            bool isGenChildren = true,
            EntityList<WorkOrderRoutingProcess> routingList = null)
        {
            if (routingList == null)
            {
                workOrder.RoutingProcessList.Clear();
            }

            if (!workOrder.VersionId.HasValue)
            {
                return;
            }

            var routingProcessesOfCurrentVersion = routingProcesses
                    .Where(x => x.VersionId == workOrder.VersionId).ToList();

            var routingProcessIds = routingProcessesOfCurrentVersion.Select(x => x.Id).Distinct().ToList();

            var routingProcessParametersCurrentWorkOrder = routingProcessParameters
                .Where(x => routingProcessIds.Contains(x.RoutingProcessId));
            //获取工序属性维护
            var processIds = routingProcessesOfCurrentVersion.Select(p => p.ProcessId.Value).Distinct().ToList();
            var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds);
            //获取工艺路线信息
            var layoutInfoIds = routingProcessesOfCurrentVersion.Select(p => p.LayoutInfoId).Distinct().ToList();
            var layoutInfos = workOrder.LayoutInfoList;//RT.Service.Resolve<WorkOrderController>().GetLayoutInfosByWorkOrderId(workOrder.Id);

            foreach (var routingProcess in routingProcessesOfCurrentVersion)
            {
                var workOrderRoutingProcess = new WorkOrderRoutingProcess();

                workOrderRoutingProcess.LayoutInfoId = routingProcess.LayoutInfoId;
                if (routingList == null || !routingList.Any())
                {
                    CreateWorkRoutingProcess(workOrder, setDisplayProperty, routingProcess,
                        workOrderRoutingProcess);
                    workOrderRoutingProcess.GenerateId();//不生成Id会对后续的参数关系子表的保存导致出错
                }
                else
                {
                    workOrderRoutingProcess = routingList.FirstOrDefault(p => p.ActivityId == routingProcess.ActivityId);
                }

                if (isGenChildren)
                {
                    //生成参数关系
                    var parameters = routingProcessParametersCurrentWorkOrder
                            .Where(x => x.RoutingProcessId == routingProcess.Id).ToList();

                    GenerateParameters(parameters, workOrderRoutingProcess);

                    //生成工单工序BOM配置
                    //如果工单配置项【工单工序BOM配置项】的【工序bom来源】配置为【产品工序BOM】时，则不获取工艺路线的【工序BOM配置】资料
                    //配置项对应 WorkOrderProcessBomSourceConfig()
                    //配置在工单功能 typeof(WorkOrder)
                    if (isGetRoutingBomConfig)
                    {
                        var bomConfigs = routingProcessBomConfigs.Where(x => x.RoutingProcessId == routingProcess.Id).ToList();
                        GenerateBomConfigs(bomConfigs, workOrderRoutingProcess);
                    }
                }

                if (routingList == null || routingList.Count == 0)
                {
                    workOrder.RoutingProcessList.Add(workOrderRoutingProcess);
                }
            }

            if (isGenChildren)
            {
                var workOrderPropertyChanged = RT.Service.Resolve<ErpWorkOrderPropertyChanged>();

                workOrderPropertyChanged.GenerateParameterNextProcess(routingProcessParametersCurrentWorkOrder,
                    routingProcessesOfCurrentVersion, workOrder.RoutingProcessList);
            }

            double? startProcess = null;
            double? endProcess = null;

            if (workOrder.Type == Core.WorkOrders.WorkOrderType.Rework)
            {
                //返工工单只有最后一个工序需要生成任务单不管是不是工序属性有没有配置
                //var layoutInfo = layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
                //var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode);
                //startProcess = processPty.ProcessId;
                //endProcess = processPty.ProcessId;
                //返工工单最后一个工序，他们要求首末工序都是同一个             
                //判断首工序时将PP04的工序排除，在剩余的工序中判断是否为首工序
                startProcess = workOrder.RoutingProcessList.Where(p => p.LayoutInfo == null || (p.LayoutInfo != null && p.LayoutInfo.Steus != "PP04")).OrderByDescending(p => p.Index).FirstOrDefault().ProcessId.Value;
                endProcess = workOrder.RoutingProcessList.OrderByDescending(p => p.Index).FirstOrDefault().ProcessId.Value;
            }
            else
            {
                //判断首工序
                //判断首工序时将PP04的工序排除，在剩余的工序中判断是否为首工序
                foreach (var layoutInfo in layoutInfos.Where(p => p.Steus != "PP04").OrderBy(p => Convert.ToDecimal(p.Vornr)))
                {
                    var config = ConfigService.GetConfig(new OutsourcingReportConfig(), typeof(OutsourcingRequest));

                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
                    if (processPty != null)
                    {
                        //校验配置的工序是否是当前工单整个工艺路线的第一条工序，如果是则按原逻辑判断是否是首工序，非第一条工序则默认不勾上首工序
                        if (!config.ValidStartProcess.IsNullOrEmpty() && config.ValidStartProcess.Split(",").Any(p => p == layoutInfo.ProcessCode && layoutInfo.Vornr != layoutInfos.OrderBy(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault().Vornr))
                        {
                            break;
                        }

                        startProcess = processPty.ProcessId;
                        break;
                    }
                }
                //判断尾工序
                foreach (var layoutInfo in layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)))
                {
                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
                    if (processPty != null)
                    {
                        endProcess = processPty.ProcessId;
                        break;
                    }
                }
            }
            foreach (var item in workOrder.RoutingProcessList)
            {
                item.StartProcess = startProcess;
                item.EndProcess = endProcess;
            }
            //更新任务单首末工序
            RT.Service.Resolve<IDispatchs>().UpdateTaskStartEndProcess(startProcess, endProcess, workOrder.Id);
        }

        /// <summary>
        /// 创建工单工艺路线的工序清单
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="setDisplayProperty"></param>
        /// <param name="routingProcess"></param>
        /// <param name="workOrderRoutingProcess"></param>
        private void CreateWorkRoutingProcess(WorkOrder workOrder, bool setDisplayProperty,
            RoutingProcess routingProcess, WorkOrderRoutingProcess workOrderRoutingProcess)
        {
            workOrderRoutingProcess.Name = routingProcess.Name;
            workOrderRoutingProcess.ActivityId = routingProcess.ActivityId;
            workOrderRoutingProcess.IsOptional = routingProcess.IsOptional;
            workOrderRoutingProcess.IsRepeat = routingProcess.IsRepeat;
            workOrderRoutingProcess.IsGenerateTask = true;//routingProcess.IsGenerateTask;
            workOrderRoutingProcess.IsRequirementTask = routingProcess.IsRequirementTask;

            workOrderRoutingProcess.Sign = routingProcess.ProcessSign;
            workOrderRoutingProcess.ProcessId = routingProcess.ProcessId;
            workOrderRoutingProcess.RoutingProcessId = routingProcess.Id;
            workOrderRoutingProcess.ProcessType = routingProcess.Type;
            workOrderRoutingProcess.CreateSku = routingProcess.CreateSku;
            workOrderRoutingProcess.IsCalculate = routingProcess.IsCalculate;
            workOrderRoutingProcess.IsBuckleMaterial = routingProcess.IsBuckleMaterial;
            workOrderRoutingProcess.StartProcess = routingProcess.StartProcess;
            workOrderRoutingProcess.NormalVictoryId = routingProcess.NormalVictoryId;

            if (setDisplayProperty && routingProcess.NormalVictoryId.HasValue && routingProcess.NormalVictory != null)
            {
                workOrderRoutingProcess.ExtValues["NormalVictoryId_Display"] = routingProcess.NormalVictory.Name;
            }

            workOrderRoutingProcess.RepairVictoryId = routingProcess.RepairVictoryId;

            if (setDisplayProperty && routingProcess.RepairVictoryId.HasValue && routingProcess.RepairVictory != null)
            {
                workOrderRoutingProcess.ExtValues["RepairVictoryId_Display"] = routingProcess.RepairVictory.Name;
            }

            workOrderRoutingProcess.IsStricter = routingProcess.IsStricter;
            workOrderRoutingProcess.Overtime = routingProcess.Overtime;
            workOrderRoutingProcess.IsPassRate = routingProcess.IsPassRate;
            workOrderRoutingProcess.IsBinding = routingProcess.IsBinding;
            workOrderRoutingProcess.IsUnBinding = routingProcess.IsUnBinding;
            workOrderRoutingProcess.Index = routingProcess.Index;
            workOrderRoutingProcess.MaxPassNum = routingProcess.MaxPassNum;
            workOrderRoutingProcess.IsNextMoveIn = routingProcess.IsNextMoveIn;
            workOrderRoutingProcess.EnableMoveInControl = routingProcess.EnableMoveInControl;
            workOrderRoutingProcess.TransferType = routingProcess.TransferType;
            workOrderRoutingProcess.ParentNodeId = routingProcess.ParentNodeId;
            workOrderRoutingProcess.IsGroup = routingProcess.IsGroup;
            workOrderRoutingProcess.GroupId = routingProcess.GroupId;
            workOrderRoutingProcess.WorkOrder = workOrder;
            workOrderRoutingProcess.Outsourcing = routingProcess.Outsourcing;
            //改成保存前，批量获取Id
            //<code>workOrderRoutingProcess.GenerateId();</code>

            Process process = null;
            if (Processs != null)
            {
                process = Processs.FirstOrDefault(x => x.Id == routingProcess.ProcessId);
            }
            else
            {
                process = routingProcess.Process;
            }

            if (process != null)
            {
                workOrderRoutingProcess.SegmentId = process.SegmentId;

                if (setDisplayProperty)
                {
                    workOrderRoutingProcess.ExtValues["ProcessId_Display"] = process.Name;
                    workOrderRoutingProcess.Segment = process.Segment;
                    if (process.SegmentId.HasValue && routingProcess.Process.Segment != null)
                    {
                        workOrderRoutingProcess.ExtValues["SegmentId_Display"] = process.Segment.Name;
                    }
                }
            }
        }

        /// <summary>
        /// 生成参数关系
        /// </summary>
        /// <param name="parameterList">工艺路线工序清单参数列表</param>
        /// <param name="workOrderRoutingProcess">工单工艺路线参数</param>        
        private void GenerateParameters(IList<RoutingProcessParameter> parameterList,
            WorkOrderRoutingProcess workOrderRoutingProcess)
        {
            foreach (var parameter in parameterList)
            {
                var workOrderRoutingProcessParameter = new WorkOrderRoutingProcessParameter
                {
                    RuleId = parameter.RuleId,
                    Description = parameter.Description,
                    ResultType = parameter.Type,
                    Expression = parameter.Expression,
                    ProcessId= workOrderRoutingProcess.Id,
                };

                //改成保存前，批量获取Id
                //<code>workOrderRoutingProcessParameter.GenerateId();<code>

                workOrderRoutingProcess.ParameterList.Add(workOrderRoutingProcessParameter);
            }
        }

        /// <summary>
        /// 生成工单工序BOM配置
        /// </summary>
        /// <param name="bomConfigList">工艺路线工序BOM配置</param>
        /// <param name="workOrderRoutingProcess">工单工艺路线参数</param>
        private void GenerateBomConfigs(IList<RoutingProcessBomConfig> bomConfigList,
            WorkOrderRoutingProcess workOrderRoutingProcess)
        {
            //由工艺路线的工序BOM配置 生成 【工单工序清单与BOM关系】
            foreach (var bomConfig in bomConfigList)
            {
                var workOrderRoutingProcessBomConfig = new WorkOrderRoutingProcessBom
                {
                    ItemId = bomConfig.ItemId,
                    Process = workOrderRoutingProcess,
                    ItemExtProp = bomConfig.ItemExtProp,
                    ItemExtPropName = bomConfig.ItemExtPropName
                };

                //改成保存前批量获取Id
                //<code>workOrderRoutingProcessBomConfig.GenerateId();</code>

                workOrderRoutingProcess.BomConfigList.Add(workOrderRoutingProcessBomConfig);
            }
        }

    }
}
