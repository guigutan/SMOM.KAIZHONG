using SIE.ProductIntfc.ProductStorages;
using SIE.Wpf.ProductIntfc.InspRecords;
using SIE.Wpf.ProductIntfc.ProductStorages.Commands;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库条码视图配置
    /// </summary>
    public class ToStorageBarcodeViewConfig : WPFViewConfig<ToStorageBarcode>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(InspBarcodeViewBehavior));
            View.UseCommands(typeof(ToStorageCommand), WPFCommandNames.Export);
            View.UseChildrenAsHorizontal(true);
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Show(ShowInWhere.All);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.Level).Show(ShowInWhere.All);
                View.Property(p => p.PackageRuleName).Show(ShowInWhere.All).HasLabel("包装规则");
                View.Property(p => p.BatchBarcode).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ToStorageBarcodeDetailList).Show(ChildShowInWhere.All);
            }
        }
    }
}
