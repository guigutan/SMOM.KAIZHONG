using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工资源规则
    /// </summary>
    [System.ComponentModel.DisplayName("员工与资源校验规则")]
    [System.ComponentModel.Description("员工不能添加相同资源")]
    public class EmployeeResourceRules : EntityRule<EmployeeResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeResourceRules()
        {
            Scope = EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写验证规则
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var employeeResource = entity as EmployeeResource;
            if (RT.Service.Resolve<EmployeeController>().EmployeeHasResource(employeeResource.EmployeeId, employeeResource.ResourceId))
            {
                e.BrokenDescription = "员工[{0}]已存在资源[{1}]".L10nFormat(employeeResource.Employee.Name, employeeResource.Resource.Name);
            }
        }
    }
}