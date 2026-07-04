using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Rbac.Roles;
using System;


namespace SIE.Warehouses
{
    /// <summary>
    /// 角色保存后加入仓库员工权限
    /// </summary>
    [System.ComponentModel.DisplayName("角色是所有仓库权限保存后加入仓库员工权限")]
    [System.ComponentModel.Description("角色是所有仓库权限保存后加入仓库员工权限")]
    public class RoleSubmmit : OnSubmitting<Role>
    {
        /// <summary>
        /// 保存前事件用于更新数据的时候跟数据库对比是否更新了仓库所有权限字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(Role entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Update && entity.GetIsAllWarehouse() == true)
            {
                var role = RF.GetById<RoleExtEntity>(entity.Id);
                if (role.IsAllWarehouse != true)
                {
                    RT.Service.Resolve<WarehouseController>().InsertWarehouseRoleUser(entity.Id);
                }
            }
        }
    }

    /// <summary>
    /// 角色保存后加入仓库员工权限
    /// </summary>
    [System.ComponentModel.DisplayName("角色是所有仓库权限保存后加入仓库员工权限")]
    [System.ComponentModel.Description("角色是所有仓库权限保存后加入仓库员工权限")]
    public class RoleSubmmited : OnSubmitted<Role>
    {
        /// <summary>
        /// 新增的角色要在提交后才绑定仓库权限
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(Role entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert && entity.GetIsAllWarehouse() == true)
            {
                RT.Service.Resolve<WarehouseController>().InsertWarehouseRoleUser(entity.Id);
            }
        }
    }

    /// <summary>
    /// 角色保存后加入仓库员工权限
    /// </summary>
    [System.ComponentModel.DisplayName("角色是所有仓库权限保存后加入仓库员工权限")]
    [System.ComponentModel.Description("角色是所有仓库权限保存后加入仓库员工权限")]
    public class UserInRoleSubmmit : OnSubmitted<UserInRole>
    {
        protected override void Invoke(UserInRole entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                var role = RF.GetById<RoleExtEntity>(entity.RoleId);
                if (role.IsAllWarehouse == true)
                {
                    RT.Service.Resolve<WarehouseController>().InsertWarehouseRoleUser(entity.RoleId, entity.UserId);
                }
            }
        }
    }
}
