using SIE.Domain;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工提交后处理
    /// </summary>
    internal class EmployeeOnSubmmited : OnSubmitted<Employee>
    {
        /// <summary>
        /// 更新员工后，检查清空员工缓存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(Employee entity, EntitySubmittedEventArgs e)
        {
            if (entity == null)
                return;
            RT.Service.Resolve<EmployeeController>().CheckAndClearEmployeeNameDic(entity.Id);
        }
    }
}
