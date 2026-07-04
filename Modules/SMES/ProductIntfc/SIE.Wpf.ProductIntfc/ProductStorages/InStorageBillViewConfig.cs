using SIE.ProductIntfc.ProductStorages;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class InStorageBillViewConfig : WPFViewConfig<InStorageBill>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands(false).UseCommands(WPFCommandNames.Export);
            View.UseChildrenAsHorizontal(true);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).Show(ShowInWhere.All);
                View.Property(p => p.AsnNo).Show(ShowInWhere.All).HasLabel("ASN单号");
                View.Property(p => p.ReceiveState).Show(ShowInWhere.All).HasLabel("接收情况");
                View.Property(p => p.ReceiveDate).Show(ShowInWhere.All);
                View.Property(p => p.Warehouse).Show(ShowInWhere.All).HasLabel("收货仓库");
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InStorageBarcodeDetailList).Show(ChildShowInWhere.All);
            }
        }
    }
}
