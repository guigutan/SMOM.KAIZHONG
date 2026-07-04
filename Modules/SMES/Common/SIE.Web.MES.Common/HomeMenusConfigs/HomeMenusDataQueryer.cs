using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Common;
using SIE.Rbac.Menus;
using SIE.Rbac.Roles;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.Common.HomeMenusConfigs
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class HomeMenusDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public List<EntityJson> GetAllMenus(double roleId, double userId, double recordId)
        {
            var role = RT.Service.Resolve<RoleController>().GetRoleById(roleId);
            var roles = new EntityList<Role>();
            if (role == null)
            {
                roles = RT.Service.Resolve<HomeMenusConfigsController>().GetUserRole(userId);
            }
            if (role == null && !roles.Any())
            {
                throw new ValidationException("当前选择行数据的用户未设置角色，请先设置".L10N());
            }
            EntityList<Menu> menus = new EntityList<Menu>();
            if (role != null && role.PermissionAllowList.Any())
            {
                menus = RT.Service.Resolve<HomeMenusConfigsController>().GetMenusByRoles(new List<double>() { role.Id });
            }
            if (userId > 0 && role == null)
            {
                menus = RT.Service.Resolve<HomeMenusConfigsController>().GetMenusByRoles(roles.Select(m => m.Id).ToList());
            }
            var result = new List<EntityJson>();
            var modelKeys = RT.Service.Resolve<HomeMenusConfigsController>().GetHomeMenusConfigDetailmodelKeys(recordId);
            foreach (var item in menus)
            {
                var isChecked = modelKeys.Any() && modelKeys.Contains(item.ModuleKey);
                EntityJson childnode = new EntityJson();
                childnode.SetProperty("Id", item.Id);
                childnode.SetProperty("text", item.Label);
                childnode.SetProperty("TreePId", null);
                childnode.SetProperty("ScopeKey", "");
                childnode.SetProperty("OperationKey", "");
                childnode.SetProperty("ModuleKey", item.ModuleKey);
                childnode.SetProperty("leaf", true);
                childnode.SetProperty("checked", isChecked);
                result.Add(childnode);
            }
            return result;
        }
    }
}
