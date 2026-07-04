using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Core.ApiModels;
using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises.Models;
using SIE.Resources.WipResources;
using SIE.Security;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 企业模型控制器  API
    /// </summary>
    public partial class EnterpriseController : DomainController
    {
        /// <summary>
        /// 获取员工所属部门信息
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>员工部门信息(不带员工信息)</returns>
        [ApiService("根据员工ID获取部门")]
        [return: ApiReturn("员工部门信息 EmployeeDepartmentInfo")]
        public virtual EmployeeDepartmentInfo GetDepartment([ApiParameter("员工ID")] double employeeId)
        {
            var department = GetEmployeeDepartment(employeeId);
            return new EmployeeDepartmentInfo()
            {
                DepartmentId = department.Id,
                DepartmentCode = department.Code,
                DepartmentName = department.Name
            };
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="keyword">部门编码/名称</param>
        /// <param name="pageNumber">页数，为空查第一页</param>
        /// <param name="pageSize">页数据数量，为空查所有</param>
        /// <returns>部门列表</returns>
        [ApiService("获取部门列表")]
        [return: ApiReturn("部门信息列表 List<EmployeeDepartmentInfo>")]
        public virtual List<EmployeeDepartmentInfo> GetDepartments([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1
            };
            var departments = GetResourceWorkShops(pagingInfo, keyword);
            var infos = new List<EmployeeDepartmentInfo>();
            departments.ForEach(department =>
            {
                infos.Add(new EmployeeDepartmentInfo()
                {
                    DepartmentId = department.Id,
                    DepartmentCode = department.Code,
                    DepartmentName = department.Name
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取工厂下的部门列表
        /// </summary>
        /// <param name="keyword">部门编码/名称</param>
        /// <param name="pageNumber">页数，为空查第一页</param>
        /// <param name="pageSize">页数据数量，为空查所有</param>
        /// <param name="factoryId">工厂id</param>
        /// <returns>工厂下的部门列表</returns>
        [ApiService("获取工厂下的部门列表")]
        [return: ApiReturn("工厂下的部门列表")]
        public virtual List<BaseDataInfo> GetFactoryDepartments([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber,
            [ApiParameter("页数据数量，为空查所有")] int? pageSize, [ApiParameter("工厂id")] double factoryId)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1
            };
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword, factoryId);
            departments.ForEach(p => infos.Add(new BaseDataInfo { Id = p.Id, Code = p.Code, Name = p.Name }));
            return infos;
        }

        /// <summary>
        /// 获取工厂下的车间列表
        /// </summary>
        /// <param name="keyword">车间编码/名称</param>
        /// <param name="pageNumber">页数，为空查第一页</param>
        /// <param name="pageSize">页数据数量，为空查所有</param>
        /// <param name="factoryId">工厂id</param>
        /// <returns>工厂下的车间列表</returns>
        [ApiService("获取工厂下的车间列表")]
        [return: ApiReturn("工厂下的车间列表")]
        public virtual List<BaseDataInfo> GetFactoryWorkShops([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber,
            [ApiParameter("页数据数量，为空查所有")] int? pageSize, [ApiParameter("工厂id")] double factoryId)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1
            };
            List<BaseDataInfo> infos = new List<BaseDataInfo>();
            var workshop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, factoryId);
            workshop.ForEach(p => infos.Add(new BaseDataInfo { Id = p.Id, Code = p.Code, Name = p.Name }));
            return infos;
        }

        /// <summary>
        /// 获取车间信息列表
        /// </summary>
        /// <returns>车间信息列表</returns>
        [ApiService("获取车间列表")]
        [return: ApiReturn("车间列表 List<WorkShopInfo>")]
        public virtual List<EnterpriseInfo> GetWorkShopInfoList()
        {
            List<EnterpriseInfo> workShopInfoList = new List<EnterpriseInfo>();
            var workShopList = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Shop);
            workShopList.ForEach(p =>
            {
                var workShopInfo = new EnterpriseInfo()
                {
                    WorkShopId = p.Id,
                    WorkShopCode = p.Code,
                    WorkShopName = p.Name
                };

                workShopInfoList.Add(workShopInfo);
            });

            return workShopInfoList;
        }

        /// <summary>
        /// 通过车间Id获取产线列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <returns>产线列表</returns>
        [ApiService("获取产线列表")]
        [return: ApiReturn("产线列表 List<ResourceInfo>")]
        public virtual List<ResourceInfo> GetResourceInfoList([ApiParameter("车间Id")] double workShopId)
        {
            List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
            var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(new PagingInfo(),null, workShopId);
            //  var resourceList = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workShopId, srcTypeList, null, string.Empty);
            resourcesList.ForEach(p =>
            {
                var resourceInfo = new ResourceInfo()
                {
                    ResourceId = p.Id,
                    ResourceCode = p.Code,
                    ResourceName = p.Name
                };
                resourceInfoList.Add(resourceInfo);
            });

            return resourceInfoList;
        }

        /// <summary>
        /// 通过车间Id获取产线列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <returns>产线列表</returns>
        [ApiService("获取产线列表")]
        [return: ApiReturn("产线列表 List<ResourceInfo>")]
        public virtual List<ResourceInfo> GetAndonResourceInfoList([ApiParameter("车间Id")] double workShopId)
        {
            List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
            var resourceList = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workShopId, srcTypeList, null, string.Empty);
            resourceList.ForEach(p =>
            {
                var resourceInfo = new ResourceInfo()
                {
                    ResourceId = p.Id,
                    ResourceCode = p.Code,
                    ResourceName = p.Name
                };
                resourceInfoList.Add(resourceInfo);
            });

            return resourceInfoList;
        }


        /// <summary>
        /// 获取工厂列表
        /// </summary>
        /// <param name="queryInfo">分页关键字查询信息</param>
        /// <returns>分页工厂信息</returns>
        [ApiService("获取工厂列表")]
        [return: ApiReturn("分页工厂信息 GetFactoryInfos")]
        public virtual PagingBaseDataInfo GetFactoryInfos([ApiParameter("分页关键字查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var workshops = GetEnterpriseShops(EnterpriseType.Plant,pagingInfo, queryInfo.Keyword);
            var infos = new List<BaseDataInfo>();
            workshops.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = workshops.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取工厂列表
        /// </summary>
        /// <returns></returns>
        [ApiService("获取工厂列表")]
        [return: ApiReturn("分页工厂信息 GetFactoryInfos")]
        public virtual List<BaseDataInfo> GetFactoryInfoList()
        {
            var rst = new List<BaseDataInfo>();
            var code = new List<string>();
            var workshops = GetEnterpriseByEmployee(EnterpriseType.Plant, code);
            workshops.ForEach(p =>
            {
                rst.Add(new BaseDataInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });
            return rst;
        }
        /// <summary>
        /// 获取车间列表
        /// </summary>
        /// <param name="queryInfo">分页关键字查询信息</param>
        /// <returns>分页车间信息</returns>
        [ApiService("获取车间列表")]
        [return: ApiReturn("分页车间信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWorkShopInfos([ApiParameter("分页关键字查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var workshops = GetResourceWorkShops(pagingInfo, queryInfo.Keyword);
            var infos = new List<BaseDataInfo>();
            workshops.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = workshops.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取SCADA车间信息列表
        /// </summary>
        /// <returns>车间信息列表</returns>
        [ApiService("获取SCADA车间信息列表")]
        [return: ApiReturn("车间列表 List<ScadaEnterpriseInfo>", SampleValueProvider = typeof(ScadaEnterpriseInfoValueProvider))]
        [AllowAnonymous]
        public virtual List<ScadaEnterpriseInfo> GetScadaWorkShopInfoList()
        {
            List<ScadaEnterpriseInfo> workShopInfoList = new List<ScadaEnterpriseInfo>();
            var workshops = GetScadaWorkShops();
            workshops.ForEach(p =>
            {
                var workShopInfo = new ScadaEnterpriseInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    InvOrgId = p.GetProperty(InvOrgIdExtension.INV_ORG_IDProperty)
                };

                workShopInfoList.Add(workShopInfo);
            });

            return workShopInfoList;
        }

        /// <summary>
        /// 获取SCADA车间
        /// </summary>
        /// <returns>车间列表</returns>
        private EntityList<Enterprise> GetScadaWorkShops()
        {
            var query = Query<Enterprise>().Join<ScadaWipResource>((x, y) => x.Id == y.WorkShopId);
            return query.Distinct().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工所属部门信息
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>员工部门信息(不带员工信息)</returns>
        [ApiService("根据员工ID获取部门")]
        [return: ApiReturn("员工部门信息 EmployeeDepartmentInfo")]
        public virtual EmployeeDepartmentInfo GetDepartmentByEmployeeId([ApiParameter("员工ID")] double employeeId)
        {
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeById(employeeId);
            if (employee != null && employee.WorkGroupId.HasValue && employee.WorkGroup.DepartmentId.HasValue)
            {
                var department = employee.WorkGroup.Department;

                return new EmployeeDepartmentInfo()
                {
                    DepartmentId = department.Id,
                    DepartmentCode = department.Code,
                    DepartmentName = department.Name
                };
            }

            return new EmployeeDepartmentInfo()
            {
                EmployeeId = employeeId,
                EmployeeCode = employee?.Code,
                EmployeeName = employee?.Name
            };
        }

        #region 离线下载企业模型
        /// <summary>
        /// 离线下载企业模型
        /// </summary>
        /// <returns></returns>
        [ApiService("离线下载企业模型")]
        [return: ApiReturn("离线下载企业模型")]
        public virtual EntityList<Enterprise> GetOffLineEnterprise()
        {
            var result = RF.GetAll<Enterprise>(null, new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }
        #endregion
    }

    /// <summary>
    /// SCDA车间信息值提供器
    /// </summary>
    class ScadaEnterpriseInfoValueProvider : IApiSampleValueProvider
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns>值</returns>
        public object GetValue()
        {
            return new ScadaEnterpriseInfo();
        }
    }
}