using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.Equipments.EquipAccounts;
using SIE.Rbac.Users;
using SIE.Resources.Enterprises;
using System;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 使用部门控制器
    /// </summary>
    public class UserDepartmentPermissionController : UserDepartmentController
    {
        /// <summary>
        /// 获取用户有权限的业务部门
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public override EntityList<Enterprise> GetUseDepartments(PagingInfo pagingInfo, string keyword)
        {
            //判断有当前用户是否已经按部门维护了权限
            var deviceUseDepartments = DB.Query<DeviceUseDepartment>()
                .Join<DevicePur>((x, y) => x.DevicePurId == y.Id)
                .LeftJoin<DevicePur, UserInUserGroup>("uig", (a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((x, y, z) => (y.UserId == RT.Identity.UserId || z.UserId == RT.Identity.UserId))
                .ToList(null, new EagerLoadOptions().LoadWith(DeviceUseDepartment.EnterpriseProperty));

            if (!deviceUseDepartments.Any())
            {
                var ctl = RT.Service.Resolve<EnterpriseController>();
                return ctl.GetDepartmentsWithParent(pagingInfo, keyword);
            }
            else
            {
                //启用用户部门权限，则只列出用记有权限的部门
                EntityList<Enterprise> enterprises = new EntityList<Enterprise>();
                var list = deviceUseDepartments.Select(x => x.Enterprise);

                enterprises.SetTotalCount(deviceUseDepartments.Count);
                if (!keyword.IsNullOrEmpty())
                {
                    list = list.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }

                foreach (var item in list)
                {
                    item.TreePId = null;
                    enterprises.Add(item);
                }

                return enterprises;
            }
        }
    }
}
