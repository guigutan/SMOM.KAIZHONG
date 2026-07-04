using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Common.HomeMenusConfigs;
using SIE.Rbac.Menus;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Common
{
    /// <summary>
    /// 首页菜单控制器
    /// </summary>
    public class HomeMenusConfigsController : DomainController
    {
        /// <summary>
        /// 是否已存在
        /// </summary>
        /// <param name="useId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual bool IsExsited(double? useId, double? roleId)
        {
           return  Query<HomeMenusConfig>().WhereIf(useId.HasValue, m => m.UserId == useId).
                WhereIf(roleId.HasValue, m => m.RoleId == roleId).FirstOrDefault()!=null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<HomeMenusConfig> Fetch(HomeMenusConfigCriteria criteria)
        {
            var query = Query<HomeMenusConfig>();
            if (criteria.RoleName.IsNotEmpty())
            {
                query.Where(m => m.Role.Code == criteria.RoleName || m.Role.Name == criteria.RoleName);
            }
            if (criteria.UseName.IsNotEmpty())
            {
                query.Where(m => m.User.Code == criteria.UseName);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="modelKeys"></param>
        /// <returns></returns>
        public virtual EntityList<Menu> GetMenus(List<string> modelKeys)
        {

            return modelKeys.SplitContains((items) =>
             Query<Menu>().Where(p => items.Contains(p.ModuleKey) && p.Platform == SIE.Common.Platform.Platform.Wpf)
             .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

       /// <summary>
       /// 根据角色Ids获取有权限菜单
       /// </summary>
       /// <param name="roleIds"></param>
       /// <returns></returns>
       
        public virtual EntityList<Menu> GetMenusByRoles(List<double> roleIds)
        {
             return  Query<Menu>().Exists<PermissionAllow>((x, y) => y.Join<Role>((n, m) => n.RoleId == m.Id)
          .Where<Menu, PermissionAllow, Role>((t, m, o, r) => m.Platform == SIE.Common.Platform.Platform.Wpf
           && t.Platform == m.Platform
           && roleIds.Contains(r.Id)
           && t.ModuleKey == m.ModuleKey
           && t.ModuleKey == t.ScopeKey
           && t.OperationKey=="系统权限 - 查看"
          )).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="homeMenusConfigDetails"></param>
        public virtual void SetHomeMenusConfigDetail(EntityList<HomeMenusConfigDetail> homeMenusConfigDetails)
        {
            RF.BatchInsert(homeMenusConfigDetails);
        }

        /// <summary>
        /// 获取菜单明细modelKey
        /// </summary>
        /// <param name="homeMenusConfigId"></param>
        /// <returns></returns>
        public virtual List<string> GetHomeMenusConfigDetailmodelKeys(double homeMenusConfigId)
        {
            var res = Query<HomeMenusConfigDetail>().Where(m => m.HomeMenusConfigId == homeMenusConfigId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (res.Any())
            {
                return res.Select(m => m.MoudleKey).ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// 根据用户获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual EntityList<Role> GetUserRole(double userId)
        {
            return Query<Role>().Join<UserInRole>((x, y) => x.Id == y.RoleId && y.UserId == userId)
                 .ToList(null,new EagerLoadOptions().LoadWith(Role.PermissionAllowListProperty));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="selectModelKeys"></param>
        /// <returns></returns>
        public virtual object SaveMenPostData(MenPostData postData, List<string> selectModelKeys)
        {

            using (var tran = DB.TransactionScope(MesCommonEntityDataProvider.ConnectionStringName))
            {
                DB.Delete<HomeMenusConfigDetail>().Where(m => m.HomeMenusConfigId == postData.RecordId).Execute();
                if (!selectModelKeys.Any())
                {
                    tran.Complete();
                    return true;
                }

                var menus = RT.Service.Resolve<HomeMenusConfigsController>().GetMenus(selectModelKeys);
                var curData = RF.GetById<HomeMenusConfig>(postData.RecordId);
                if (curData == null)
                {
                    throw new ValidationException("当前选择行数据异常，请刷新重试".L10N());
                }
                EntityList<HomeMenusConfigDetail> saveHomeMenusConfigDetail = new EntityList<HomeMenusConfigDetail>();
                foreach (var item in menus)
                {
                    HomeMenusConfigDetail homeMenusConfigDetail = new HomeMenusConfigDetail();
                    homeMenusConfigDetail.MenuId = item.Id;
                    homeMenusConfigDetail.HomeMenusConfigId = postData.RecordId;
                    homeMenusConfigDetail.GenerateId();
                    homeMenusConfigDetail.PersistenceStatus = PersistenceStatus.New;
                    saveHomeMenusConfigDetail.Add(item);
                    curData.HomeMenusConfigDetailList.Add(homeMenusConfigDetail);
                }
                RF.Save(curData);
                tran.Complete();
                return true;
            }
        }


        /// <summary>
        /// 获取当前用户或角色的所有菜单
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Menu> GetCurUserMenus()
        {
            //用户权限
            var roles = GetUserRole(RT.Identity.UserId);

            var resultMenus = new EntityList<Menu>();
            foreach (var role in roles)
            {
                var configMenus = Query<HomeMenusConfigDetail>().Join<HomeMenusConfig>((x, y) => x.HomeMenusConfigId == y.Id && (y.UserId == RT.Identity.UserId || y.RoleId == role.Id))
                   .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (role.PermissionAllowList.Any())
                {
                    foreach (var config in configMenus)
                    {
                        if (role.PermissionAllowList.Select(m => m.ModuleKey).Contains(config.MoudleKey))
                        {
                            var containsMenu = resultMenus.FirstOrDefault(m => m.Id == config.MenuId);
                            if (containsMenu == null)
                            {
                                resultMenus.Add(config.Menu);
                            }
                        }
                    }
                }
            }

            return resultMenus;

        }

    }
    /// <summary>
    /// 提交的数据
    /// </summary>
   [Serializable]
    public class MenPostData
    {
        /// <summary>
        /// 
        /// </summary>
        public double RecordId { get; set; }

        /// <summary>
        /// 勾选权限集
        /// </summary>
        public List<AppMenuPermission> CheckedCmds { get; set; }

        /// <summary>
        /// 未勾选权限集
        /// </summary>
        public List<AppMenuPermission> NoCheckedCmds { get; set; }
    }
}
