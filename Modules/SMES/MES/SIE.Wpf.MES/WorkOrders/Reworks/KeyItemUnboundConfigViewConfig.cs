using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图配置
    /// </summary>
    internal class KeyItemUnboundConfigViewConfig : WPFViewConfig<KeyItemUnboundConfig>
    {
        /// <summary>
        /// List View
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.UseDefaultBehaviors().AddBehavior(typeof(KeyItemUnboundViewBehavior));
            View.RemoveCommands();
            View.UseCommands(typeof(KeyItemAllSelectCommand), WPFCommandNames.ListEdit);
            View.Property(p => p.IsUnbound).UseListSetting(e => e.ListGridWidth = 50).UseCheckEditor().HasLabel("解绑");
            View.Property(p => p.Item).Readonly().HasLabel("物料编码");
            View.Property(p => p.SingleQty).Readonly();
            View.Property(p => p.Unit).Readonly();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}