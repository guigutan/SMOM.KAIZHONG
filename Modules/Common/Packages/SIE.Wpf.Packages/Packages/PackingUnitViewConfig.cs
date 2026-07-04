using SIE.Packages.Packages;
using SIE.Wpf.Packages.Commands;

namespace SIE.Wpf.Packages
{
    /// <summary>
    /// 包装单位
    /// </summary>
    internal class PackingUnitViewConfig : WPFViewConfig<PackingUnit>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave, WPFCommandNames.Export, typeof(AddMasterUnitCommand));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.IsMasterUnit).Readonly();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
    }
}
