using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Wpf.Tech.Processs.Commands;

namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// 工序对应包装视图配置
    /// </summary>
    internal class ProcessPackingUnitViewConfig : WPFViewConfig<ProcessPackingUnit>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置默认视图
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(LookupPackingUnitCommand), typeof(ListDeleteCommand));
            View.Property(p => p.PackageUnit);
            View.Property(p => p.PackageUnit.Name).HasLabel("名称");
            View.Property(p => p.Description).HasLabel("描述");
            View.Property(p => p.IsMasterUnit).HasLabel("主单位").UseCheckEditor();
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.UseCommands(typeof(LookupPackingUnitCommand), typeof(ListDeleteCommand));
            View.Property(p => p.PackageUnit);
            View.Property(p => p.PackageUnit.Name).HasLabel("名称");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PackageUnitCode).HasLabel("编码");
            View.Property(p => p.PackageUnitName).HasLabel("名称");
        }
    }
}