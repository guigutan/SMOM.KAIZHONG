using SIE.Domain;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 角色保存后加入仓库员工权限（产品公用的员工关联用户，使用的是common的user）
    /// </summary>
    [System.ComponentModel.DisplayName("用户提交事件加入仓库员工权限")]
    [System.ComponentModel.Description("用户提交事件加入仓库员工权限")]
    public class CommonUserSubmmitting : OnSubmitting<SIE.Common.Users.User>
    {
        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Invoke(SIE.Common.Users.User entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Update && entity.EmployeeId > 0)
            {
                var dbUser = RF.GetById<SIE.Common.Users.User>(entity.Id);
                if (dbUser.EmployeeId != entity.EmployeeId)
                {
                    RT.Service.Resolve<WarehouseController>().InserWhEmp(entity.Id, entity.EmployeeId.Value);
                }
            }
        }
    }        
}
