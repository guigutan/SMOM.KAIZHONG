using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;

namespace SIE.Web.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图配置
    /// </summary>
    internal class KeyItemUnboundConfigViewConfig : WebViewConfig<KeyItemUnboundConfig>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
        }

        /// <summary>
        /// List View
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.Property(p => p.IsUnbound).UseCheckEditor().ShowInList(60).HasLabel("解绑");
            View.Property(p => p.ItemCode).ShowInList(120).Readonly();
            View.Property(p => p.ItemName).ShowInList(120).Readonly();
            View.Property(p => p.SingleQty).Readonly();
            View.Property(p => p.UnitName).Readonly();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}