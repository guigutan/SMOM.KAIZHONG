using SIE.Domain;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 直通率设置添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class AddSettingCommand : ListAddCommand
    {
        /// <summary>
        /// 实体创建后方法
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var current = entity as DataEntity;
            current.UpdateDate = DateTime.Now;
            current.UpdateBy = RT.IdentityId;
        }
    }
}