using SIE.Api;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.ApiModels;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Moves
{
    /// <summary>
    /// 过站采集控制器API
    /// </summary>
    public partial class MoveController : AssemblyController
    {
        private const string EMPLOYEE_ID_NOT_EXISTS = "员工Id不存在！";
        private const string EMPLOYEE_NOT_EXISTS = "员工不存在！";

        /// <summary>
        /// 获取资源信息列表
        /// </summary>
        /// <param name="queryInfo">资源查询信息</param>
        /// <returns>资源信息列表</returns>
        [ApiService("获取资源信息列表")]
        [return: ApiReturn("获取资源信息列表 GetResourceInfos")]
        public virtual ResourceDataInfo GetResourceInfos([ApiParameter("资源查询信息")] ResourceQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException(EMPLOYEE_ID_NOT_EXISTS.L10N());
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException(EMPLOYEE_NOT_EXISTS.L10N());
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var resources = RT.Service.Resolve<WipResourceController>().GetResourcesByUserId(queryInfo.EmployeeId, queryInfo.Keyword, pagingInfo, queryInfo.WorkShopId);
            var result = CreateResourceDataInfos(pagingInfo, resources);
            return result;
        }

        /// <summary>
        /// 创建并获取分页资源信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="resources">资源列表</param>
        /// <returns>分页资源信息</returns>
        private ResourceDataInfo CreateResourceDataInfos(PagingInfo pagingInfo, EntityList<WipResource> resources)
        {
            ResourceDataInfo result = new ResourceDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = resources.TotalCount
            };

            resources.OrderBy(p => p.Name).ForEach(res =>
            {
                var resInfo = new ResourceInfo()
                {
                    Id = res.Id,
                    Code = res.Code,
                    Name = res.Name
                };

                result.ResourceInfos.Add(resInfo);
            });
            return result;
        }

        /// <summary>
        /// 获取工序信息列表
        /// </summary>
        /// <param name="queryInfo">工序查询信息</param>
        /// <returns>工序信息列表</returns>
        [ApiService("获取工序信息列表")]
        [return: ApiReturn("获取工序信息列表 GetProcessDataInfos")]
        public virtual ProcessDataInfo GetProcessDataInfos([ApiParameter("工序查询信息")] ProcessQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException(EMPLOYEE_ID_NOT_EXISTS.L10N());
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException(EMPLOYEE_NOT_EXISTS);
            var processTypes = Enum.GetValues(typeof(ProcessType)).Cast<ProcessType>().Select(e => (int)e).ToList();
            if (!processTypes.Contains(queryInfo.ProcessType))
                throw new ValidationException("工序类型不存在！".L10N());
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var processList = RT.Service.Resolve<ProcessController>().GetProcesssByUserId(queryInfo.EmployeeId, queryInfo.Keyword, new List<ProcessType>() { (ProcessType)queryInfo.ProcessType });
            var result = CreateProcessDataInfos(pagingInfo, processList);
            return result;
        }

        /// <summary>
        /// 获取工序信息列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="processTypes"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("获取多个类型的工序信息列表")]
        [return: ApiReturn("获取工序信息列表 GetProcessDataInfos")]
        public virtual ProcessDataInfo GetProcessDataInfosManyProcessType([ApiParameter("当前员工")] ProcessQueryInfo queryInfo, [ApiParameter("工序类型")] List<int> processTypes)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException(EMPLOYEE_ID_NOT_EXISTS.L10N());
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException(EMPLOYEE_NOT_EXISTS);
          
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            if (processTypes == null||!processTypes.Any())
            {
                throw new ValidationException("请输入工序类型".L10N());
            }
            var processList = RT.Service.Resolve<ProcessController>().GetProcesssByTypes(queryInfo.EmployeeId, queryInfo.Keyword, processTypes);
            var result = CreateProcessDataInfos(pagingInfo, processList);
            //获取工序的时候，默认将挤塑、精加工、热处理、产品最终检工序排在最前方
            var priorityCodes = new List<string> { "挤塑", "精加工", "热处理", "产品最终检" };
            if (result.ProcessInfos.Count > 0)
            {
                result.ProcessInfos.Sort((a, b) =>
                {
                    // 获取两个对象的优先级索引，不在列表里的索引为 -1
                    int indexA = priorityCodes.IndexOf(a.Code ?? "");
                    int indexB = priorityCodes.IndexOf(b.Code ?? "");

                    // 排序规则：
                    // 1. 优先级元素（索引>=0）永远排在 非优先级元素（索引=-1）前面
                    // 2. 优先级元素内部，按 挤塑→精加工→热处理→产品最终检 的顺序排序
                    return (indexA == -1 ? int.MaxValue : indexA) - (indexB == -1 ? int.MaxValue : indexB);
                });
            }
            return result;
        }


        /// <summary>
        /// 获取当前员工下所有工序信息列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("获取当前员工下所有工序信息列表")]
        [return: ApiReturn("获取工序信息列表 GetProcessDataInfos")]
        public virtual ProcessDataInfo GetProcessDataInfosAllProcessType([ApiParameter("当前员工")] ProcessQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException(EMPLOYEE_ID_NOT_EXISTS.L10N());
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException(EMPLOYEE_NOT_EXISTS);

            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var processList = RT.Service.Resolve<ProcessController>().GetProcesssByUserId(queryInfo.EmployeeId, queryInfo.Keyword);
            var result = CreateProcessDataInfos(pagingInfo, processList);
            return result;
        }


        /// <summary>
        /// 创建分页工序信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="processList">工序列表</param>
        /// <returns>分页工序信息</returns>
        private ProcessDataInfo CreateProcessDataInfos(PagingInfo pagingInfo, EntityList<Process> processList)
        {
            ProcessDataInfo result = new ProcessDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = processList.TotalCount
            };
            processList.ForEach(process =>
            {
                var processInfo = new ProcessInfo()
                {
                    Id = process.Id,
                    Name = process.Name,
                    Code = process.Code,
                    Type = EnumViewModel.EnumToLabel(process.Type).L10N(),
                    EumType = (int)process.Type
                };

                result.ProcessInfos.Add(processInfo);
            });
            return result;
        }

        /// <summary>
        /// 获取工位信息列表
        /// </summary>
        /// <param name="queryInfo">工位查询信息</param>
        /// <returns>工位信息列表</returns>
        [ApiService("获取工位信息列表")]
        [return: ApiReturn("获取工位信息列表 GetStationDataInfos")]
        public virtual StationDataInfo GetStationDataInfos([ApiParameter("工位查询信息")] StationQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException(EMPLOYEE_ID_NOT_EXISTS);
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException(EMPLOYEE_NOT_EXISTS);
            if (queryInfo.ResourceId <= 0)
                throw new ValidationException("资源Id不存在！".L10N());
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource == null)
                throw new ValidationException("资源不存在！".L10N());
            if (!RT.Service.Resolve<WipResourceController>().IsExistEmployeeResource(queryInfo.EmployeeId, queryInfo.ResourceId))
                throw new ValidationException("员工[{0}]与资源[{1}]不存在关系！".L10nFormat(employee.Name, resource.Name));
            var processTypes = Enum.GetValues(typeof(ProcessType)).Cast<ProcessType>().Select(e => (int)e).ToList();
            if (!processTypes.Contains(queryInfo.ProcessType) && queryInfo.ProcessType != -1)
                throw new ValidationException("工序类型不存在！".L10N());
            if (queryInfo.ProcessId <= 0)
                throw new ValidationException("工序Id不存在！".L10N());
            var process = RF.GetById<Process>(queryInfo.ProcessId);
            if (process == null)
                throw new ValidationException("工序不存在！".L10N());
            if ((int)process.Type != queryInfo.ProcessType && queryInfo.ProcessType != -1)
                throw new ValidationException("工序的[{0}]工序类型[{1}]与输入的工序类型[{2}]不一致，请确认！".L10nFormat(process.Name, EnumViewModel.EnumToLabel(process.Type).L10N(), EnumViewModel.EnumToLabel((ProcessType)queryInfo.ProcessType).L10N()));
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            if (!RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(queryInfo.ProcessId, queryInfo.EmployeeId))
                throw new ValidationException("员工[{0}]不具有工序[{1}]所要求的技能！".L10nFormat(employee.Name, process.Name));
            var stations = RT.Service.Resolve<StationController>().GetStationsByResourceId(queryInfo.ResourceId, queryInfo.ProcessId, pagingInfo);
            var result = CreateStationDataInfos(pagingInfo, stations);
            return result;
        }

        /// <summary>
        /// 获取工位信息列表
        /// </summary>
        /// <param name="queryInfo">工位查询信息</param>
        /// <returns>工位信息列表</returns>
        [ApiService("获取工位信息列表")]
        [return: ApiReturn("获取工位信息列表 GetStationDataInfoByEmployees")]
        public virtual StationDataInfo GetStationDataInfoByEmployees([ApiParameter("工位查询信息")] StationQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var stations = RT.Service.Resolve<StationController>().GetStationsByResourceId(queryInfo.ResourceId, queryInfo.ProcessId, pagingInfo);
            var result = CreateStationDataInfos(pagingInfo, stations);
            return result;
        }

        /// <summary>
        /// 创建分页工位信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="stations">工位列表</param>
        /// <returns>分页工位信息</returns>
        private StationDataInfo CreateStationDataInfos(PagingInfo pagingInfo, EntityList<Station> stations)
        {
            StationDataInfo result = new StationDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = stations.TotalCount
            };

            stations.ForEach(station =>
            {
                var stationInfo = new StationInfo()
                {
                    Id = station.Id,
                    Code = station.Code,
                    Name = station.Name,
                };

                result.StationInfos.Add(stationInfo);
            });
            return result;
        }

        /// <summary>
        /// 获取过站采集返回信息
        /// </summary>
        /// <param name="queryInfo">工位查询信息</param>
        /// <returns>过站采集返回信息</returns>
        [ApiService("过站采集")]
        [return: ApiReturn("过站采集 MoveCollect")]
        public virtual RstMoveInfo MoveCollect([ApiParameter("采集查询信息")] WipQueryInfo queryInfo)
        {
            ValidateWipQueryInfo(queryInfo);
            var collectBarcode = new CollectBarcode { Code = queryInfo.Sn, Type = BarcodeType.SN };
            var workcell = new Workcell() { EmployeeId = queryInfo.EmployeeId, ResourceId = queryInfo.ResourceId, ProcessId = queryInfo.ProcessId, StationId = queryInfo.StationId };
            var product = Validate(collectBarcode, workcell);
            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(product.WorkOrderId);
            var wipLineWorkOrderEntity = GetWipResourceWorkOrder(workcell);
            if (wipLineWorkOrderEntity == null || wipLineWorkOrderEntity.WorkOrderId != product.WorkOrderId)
            {
                ChangeWipResourceWorkOrder(wo.Id, workcell);
                //RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                //RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(wo.Id);
            }

            Collect(new string[] { queryInfo.Sn }, CollectData.Empty, workcell);
            return new RstMoveInfo()
            {
                Sn = queryInfo.Sn,
                WorkOrderNo = wo.No,
                ProductCode = wo.ProductCode,
                ProductName = wo.ProductName,
                ProductModel = wo.ProductModelName,
                CollectDate = Convert.ToString(DateTime.Now)
            };
        }

        /// <summary>
        /// 验证采集查询信息
        /// </summary>
        /// <param name="queryInfo">采集查询信息</param>
        public virtual void ValidateWipQueryInfo(WipQueryInfo queryInfo)
        {
            if (!queryInfo.Sn.IsNotEmpty())
                throw new ValidationException("条码不能为空！".L10N());
            if (queryInfo.EmployeeId <= 0)
                throw new ValidationException(EMPLOYEE_ID_NOT_EXISTS);
            var employee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (employee == null)
                throw new ValidationException(EMPLOYEE_NOT_EXISTS);
            if (queryInfo.ResourceId <= 0)
                throw new ValidationException("资源Id不存在！".L10N());
            var resource = RF.GetById<WipResource>(queryInfo.ResourceId);
            if (resource == null)
                throw new ValidationException("资源不存在！".L10N());
            if (!RT.Service.Resolve<WipResourceController>().IsExistEmployeeResource(queryInfo.EmployeeId, queryInfo.ResourceId))
                throw new ValidationException("员工[{0}]与资源[{1}]不存在关系！".L10nFormat(employee.Name, resource.Name));
            var processTypes = Enum.GetValues(typeof(ProcessType)).Cast<ProcessType>().Select(e => (int)e).ToList();
            if (!processTypes.Contains(queryInfo.ProcessType))
                throw new ValidationException("工序类型不存在！".L10N());
            if (queryInfo.ProcessId <= 0)
                throw new ValidationException("工序Id不存在！".L10N());
            var process = RF.GetById<Process>(queryInfo.ProcessId);
            if (process == null)
                throw new ValidationException("工序不存在！".L10N());
            if ((int)process.Type != queryInfo.ProcessType)
                throw new ValidationException("工序的[{0}]工序类型[{1}]与输入的工序类型[{2}]不一致，请确认！".L10nFormat(process.Name, EnumViewModel.EnumToLabel(process.Type).L10N(), EnumViewModel.EnumToLabel((ProcessType)queryInfo.ProcessType).L10N()));
            if (!RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(queryInfo.ProcessId, queryInfo.EmployeeId))
                throw new ValidationException("员工[{0}]不具有工序[{1}]所要求的技能！".L10nFormat(employee.Name, process.Name));
            if (queryInfo.StationId <= 0)
                throw new ValidationException("工位Id不存在！".L10N());
            var station = RF.GetById<Station>(queryInfo.StationId);
            if (station == null)
                throw new ValidationException("工位不存在！".L10N());
            if (!RT.Service.Resolve<StationController>().IsExistStation(queryInfo.StationId, queryInfo.ResourceId, queryInfo.ProcessId))
                throw new ValidationException("不存在资源[{0}]和工序[{1}]的工位[{2}]！".L10nFormat(resource.Name, process.Name, station.Name));
        }

        /// <summary>
        /// 验证员工是否有工步所需技能
        /// </summary>
        /// <param name="processId">工步ID</param>
        /// <param name="empId">员工ID</param>
        [ApiService("验证员工是否有工步所需技能")]
        public virtual void IsEmpHasProcessSkillApi([ApiParameter("工步ID")] double processId,
            [ApiParameter("员工ID")] double empId)
        {
            var have = RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(processId, empId);
            if (!have)
            {
                throw new ValidationException("当前员工没有获得工步所需要的技能".L10N());
            }
        }
    }
}
