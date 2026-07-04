using SIE.Equipments.DeviceControls;

namespace SIE.Web.Equipments.DeviceControls
{
    /// <summary>
    /// 设备控制记录视图配置
    /// </summary>
    class DeviceControlViewConfig : WebViewConfig<DeviceControl>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.EquipAccountId);
            View.Property(p => p.Source);
            View.Property(p => p.IsStop);
            View.Property(p => p.OpearDateTime);
            View.Property(p => p.IsEffective);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

    }
}
