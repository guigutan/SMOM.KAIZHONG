using SIE.MetaModel.View;
using SIE.ProductIntfc.ProductStorages;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 待入库条码视图配置
    /// </summary>
    public class ToStorageBarcodeViewConfig : WebViewConfig<ToStorageBarcode>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseGridSelectionModel();
            View.UseCommands("SIE.Web.ProductIntfc.ProductStorages.Commands.ToStorageCommand", WebCommandNames.ExportXls);
            View.UseChildrenAsHorizontal(true);
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Readonly().ShowInList(150);
                View.Property(p => p.Qty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Level).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PackageRuleName).Readonly().ShowInList(150).HasLabel("包装规则");
                View.Property(p => p.BatchBarcode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ToStorageBarcodeDetailList).Show(ChildShowInWhere.All);
            }
        }
    }
}
