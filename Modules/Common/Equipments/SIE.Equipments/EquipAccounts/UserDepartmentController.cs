using SIE.Domain;
using SIE.Resources.Enterprises;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 使用部门控制器
    /// </summary>
    public class UserDepartmentController : DomainController
    {
        /// <summary>
        /// 获取用户有权限的业务部门
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetUseDepartments(PagingInfo pagingInfo, string keyword)
        {
            var ctl = RT.Service.Resolve<EnterpriseController>();
            return ctl.GetDepartmentsWithParent(pagingInfo, keyword);
        }
    }
}
