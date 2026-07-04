using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工控制器  API
    /// </summary>
    public partial class EmployeeController : DomainController
    {
        /// <summary>
        /// 获取关联部门的员工部门列表
        /// </summary> 
        /// <returns>员工部门列表</returns>
        [ApiService("获取关联部门的员工部门列表")]
        [return: ApiReturn("分页员工部门列表 PagingEmployeeDepartmentInfo")]
        public virtual PagingEmployeeDepartmentInfo GetEmployeeDepartments([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var employees = GetExistDepartmentEmployees(keyword, pagingInfo);
            List<EmployeeDepartmentInfo> infos = new List<EmployeeDepartmentInfo>();
            employees.ForEach(e =>
            {
                var department = e.WorkGroup.Department;
                infos.Add(new EmployeeDepartmentInfo()
                {
                    EmployeeId = e.Id,
                    EmployeeCode = e.Code,
                    EmployeeName = e.Name,
                    DepartmentId = department.Id,
                    DepartmentCode = department.Code,
                    DepartmentName = department.Name
                });
            });
            PagingEmployeeDepartmentInfo result = new PagingEmployeeDepartmentInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = employees.TotalCount
            };
            result.DepartmentInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取班组列表
        /// </summary> 
        /// <returns>班组列表</returns>
        [ApiService("获取班组列表")]
        [return: ApiReturn("班组列表 List<BaseDataInfo>")]
        public virtual List<BaseDataInfo> GetWorkGroups([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1
            };
            Expression<Func<WorkGroup, bool>> exp = null;
            if (!keyword.IsNullOrEmpty())
                exp = p => p.Code.Contains(keyword) || p.Name.Contains(keyword);
            var workGroups = RT.Service.Resolve<EmployeeController>().GetWorkGroups(exp, pagingInfo);
            var infos = new List<BaseDataInfo>();
            workGroups.ForEach(e =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取员工列表
        /// </summary> 
        /// <returns>员工列表</returns>
        [ApiService("获取员工列表")]
        [return: ApiReturn("分页员工列表信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetEmployees([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            Expression<Func<Employee, bool>> exp = null;
            if (!keyword.IsNullOrEmpty())
                exp = p => p.Code.Contains("%"+keyword+"%") || p.Name.Contains("%" + keyword + "%");
            var employees = RT.Service.Resolve<EmployeeController>().GetEmployees(exp, pagingInfo);
            var infos = new List<BaseDataInfo>();
            employees.ForEach(e =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = employees.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取员工ID
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>员工ID</returns>
        [ApiService("获取员工ID")]
        [return: ApiReturn("获取员工ID. 参数类型: userID")]
        public virtual double GetEmpID(double userID)
        {
            var q = Query<Employee>().Where(p => p.UserId == userID).ToList();
            if (q?.Count() == 0)
                throw new ValidationException("没有维护员工ID".L10N());
            return q.FirstOrDefault().Id;
        }

        /// <summary>
        /// 获取当前登录用户的班组信息
        /// </summary>
        /// <returns>班组</returns>
        [ApiService("获取登录用户的班组信息")]
        [return: ApiReturn("班组信息.  参数类型: WorkGroup")]
        public virtual WorkGroup GetLoginUserWorkGroup()
        {
            WorkGroup curWorkGroup;

            var curEmployee = RT.Service.Resolve<EmployeeController>().GetEmployeeByUserId(RT.IdentityId);
            curWorkGroup = curEmployee?.WorkGroup;
            if (curWorkGroup == null)
                throw new ValidationException("员工: {0} 未找到班组信息!".L10nFormat(RT.Identity.Name));
            else
                return curWorkGroup;
        }
    }

    /// <summary>
    /// 分页员工部门信息
    /// </summary>
    [Serializable]
    public class PagingEmployeeDepartmentInfo
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 页数据数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 员工部门信息列表
        /// </summary>
        public List<EmployeeDepartmentInfo> DepartmentInfos { get; } = new List<EmployeeDepartmentInfo>();
    }

    /// <summary>
    /// 员工部门信息
    /// </summary>
    [Serializable]
    public class EmployeeDepartmentInfo
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public double DepartmentId { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
    }
}