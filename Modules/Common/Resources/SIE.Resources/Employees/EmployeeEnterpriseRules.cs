using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.Resources.Employees
{

    /// <summary>
    /// 员工工厂规则
    /// </summary>
    [System.ComponentModel.DisplayName("员工与工厂校验规则")]
    [System.ComponentModel.Description("员工不能添加相同工厂")]
    public class EmployeeEnterpriseRules : EntityRule<EmployeeEnterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeEnterpriseRules()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写验证规则
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var employeeResource = entity as EmployeeEnterprise;
            if (RT.Service.Resolve<EmployeeEnterpriseSelectController>().EmployeeHasEnterprise(employeeResource.EmployeeId, employeeResource.EnterpriseId))
            {
                e.BrokenDescription = "员工[{0}]已存在工厂[{1}]".L10nFormat(employeeResource.Employee.Name, employeeResource.Enterprise.Name);
            }
        }
    }
}
