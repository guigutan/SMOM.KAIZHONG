using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 班组有关联员工时不允许删除验证
    /// </summary>
    [System.ComponentModel.DisplayName("班组验证规则")]
    [System.ComponentModel.Description("班组有关联员工时不允许删除验证")]
    public class UnDeleteWorkGroupHasEmployee : EntityRule<WorkGroup>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteWorkGroupHasEmployee()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 班组有关联员工时不允许删除验证
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var workgroup = entity as WorkGroup;
            if (RT.Service.Resolve<EmployeeController>().WorkGroupHasEmployee(workgroup.Id))
            {
                e.BrokenDescription = "班组下面存在员工，不允许删除".L10nFormat(workgroup.Name);
            }
        }
    }
}
