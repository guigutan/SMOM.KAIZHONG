using SIE.MES.Storages;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Storages.Commands;

namespace SIE.Wpf.MES.Storages
{
    /// <summary>
    /// 产线库存视图配置
    /// </summary>
    internal class StorageSaftyViewConfig : WPFViewConfig<StorageSafty>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListSave);
            View.ReplaceCommands(typeof(ListAddCommand), typeof(AddStorageSafetyCommand));
            View.ReplaceCommands(typeof(ListEditCommand), typeof(EditStorageAreaExtCommand));
            View.Property(p => p.ItemId);
            View.Property(p => p.ItemName).HasLabel("物料名称");
            View.Property(p => p.MaxQty).UseSpinEditor(e => e.MinValue = 0);
            View.Property(p => p.SafetyQty).UseSpinEditor(e => e.MinValue = 0);
            View.Property(p => p.DeliveryQty).UseSpinEditor(e => e.MinValue = 0);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ItemCode).HasLabel("物料");
            View.Property(p => p.ItemName);
            View.Property(p => p.MaxQty);
            View.Property(p => p.SafetyQty);
            View.Property(p => p.DeliveryQty);
        }
    }
}