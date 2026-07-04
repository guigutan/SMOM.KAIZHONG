using SIE.Domain;
using SIE.Equipments.DeviceControls;

namespace SIE.Web.Equipments.DeviceControls
{
    /// <summary>
    /// 设备控制来源清单视图配置
    /// </summary>
    class SourceControlViewConfig : WebViewConfig<SourceControl>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands();
            View.Property(p => p.Source).UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; })
                .Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.Description);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Source);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Source);
            View.Property(p => p.Description);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }
    }
}
