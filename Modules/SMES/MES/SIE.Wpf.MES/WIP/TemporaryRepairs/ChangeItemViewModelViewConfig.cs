using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.WIP.TemporaryRepairs.Commands;
using SIE.Wpf.MES.WIPTemporaryRepairs.Commands;

namespace SIE.Wpf.MES.WIP.TemporaryRepairs
{
    /// <summary>
    /// 换料视图配置
    /// </summary>
    internal class ChangeItemViewModelViewConfig : WPFViewConfig<ChangeItemViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(TemporaryRepairViewModel));
            View.InlineEdit();
            View.ClearCommands();
            View.UseDefaultBehaviors();
            View.UseCommands(WPFCommandNames.ListEdit, typeof(ChangeItemDeleteCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.ChangeSn).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.ChangeQty).UseChangeQtyEditor().Show(ShowInWhere.List);
                View.Property(p => p.LoadItemBarcodeInfo.Qty).Show(ShowInWhere.List).Readonly().HasLabel("可用数量");
            }
        }
    }
}
