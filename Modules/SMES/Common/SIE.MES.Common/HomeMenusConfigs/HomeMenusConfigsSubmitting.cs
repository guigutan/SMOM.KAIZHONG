using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.MES.Common.HomeMenusConfigs
{
    /// <summary>
    /// 保存事件
    /// </summary>
    [System.ComponentModel.DisplayName("首页菜单配置保存事件")]
    [System.ComponentModel.Description("首页菜单配置保存事件")]
    public class HomeMenusConfigsSubmitting : OnSubmitting<HomeMenusConfig>
    {
        /// <summary>
        /// 客户保存事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(HomeMenusConfig entity, EntitySubmittingEventArgs e)
        {
            if (e == null || entity == null)
            {
                return;
            }
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update){

                if (!entity.RoleId.HasValue && !entity.UserId.HasValue)
                {
                    throw new ValidationException("角色和用户不能同时为空".L10N());
                }
                if (entity.RoleId.HasValue && entity.UserId.HasValue)
                {
                    throw new ValidationException("角色和用户只能填写其中一个".L10N());
                }
                if (RT.Service.Resolve<HomeMenusConfigsController>().IsExsited(entity.UserId, entity.RoleId))
                {
                    throw new ValidationException("角色或用户存在重复数据，请修改!".L10N());
                }
            }

        }
    }
}
