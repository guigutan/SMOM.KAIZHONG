using SIE.Domain;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.Employees
{

    /// <summary>
    /// 生产资源控制器
    /// </summary>
    public class EmployeeEnterpriseSelectController : DomainController
    {
        /// <summary>
        /// 根据查询条件 获取生产资源
        /// </summary>
        /// <param name="criteria">查询对象</param>        
        /// <returns>返回生产资源</returns>
        public virtual EntityList GetEmployeeEnterprise(EmployeeEnterpriseSelectCriteria criteria)
        {
            var query = Query<Enterprise>()
                .Where(p => p.Level.Type == Enterprises.EnterpriseType.Plant && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg));
            if (!criteria.Code.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }

            if (!criteria.Name.IsNullOrEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }

            var dataList = query.ToList(criteria.PagingInfo, null);
            dataList.ForEach(p => p.TreePId = -1);
            return dataList;
        }


        /// <summary>
        /// 员工是否存在工厂
        /// </summary>
        /// <param name="employeeId">用户ID</param>
        /// <param name="enterpriseId">工厂ID</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual bool EmployeeHasEnterprise(double employeeId, double enterpriseId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            if (enterpriseId <= 0)
            {
                throw new ArgumentNullException(nameof(enterpriseId));
            }

            var q = Query<EmployeeEnterprise>();
            q.Where(p => p.EnterpriseId == enterpriseId && p.EmployeeId == employeeId);
            return q.Count() > 0;
        }

        /// <summary>
        /// 获取登录用户有权限的工厂Id集合
        /// </summary>
        /// <returns>返回登录用户有权限的工厂Id集合</returns>
        public virtual List<double> GetAuthorityFactoryId()
        {
            return Query<EmployeeEnterprise>().Where(y => y.EmployeeId == RT.IdentityId).Select(p => p.EnterpriseId).Distinct().ToList<double>().ToList();
        }
    }
}