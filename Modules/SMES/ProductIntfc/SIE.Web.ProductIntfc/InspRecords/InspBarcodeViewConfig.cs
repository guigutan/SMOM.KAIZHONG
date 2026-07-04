using SIE.MetaModel.View;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.ProductInsps;

namespace SIE.Web.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检条码视图配置
    /// </summary>
    public class InspBarcodeViewConfig : WebViewConfig<InspBarcode>
    {
        /// <summary>
        /// 成品报检视图组
        /// </summary>
        public static string ProductInspViewGroup { get; } = "ProductInspView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProductInsp));
            View.DeclareExtendViewGroup(ProductInspViewGroup);
            if (ViewGroup == ProductInspViewGroup)
                ConfigProductInspView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigProductInspView()
        {
            View.UseGridSelectionModel();
            View.UseCommands("SIE.Web.ProductIntfc.InspRecords.Commands.ShippingInspCommand", WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Show().Readonly();
                View.Property(p => p.Process).Show().Readonly();
                View.Property(p => p.CollectionDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.InspQty).Show().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseGridSelectionModel();
            View.UseCommands("SIE.Web.ProductIntfc.InspRecords.Commands.ShippingInspCommand", WebCommandNames.ExportXls);
            View.Property(p => p.Barcode).Readonly();
            View.Property(p => p.Process).Readonly();
            View.Property(p => p.CollectionDate).Readonly().ShowInList(width: 150);
            View.Property(p => p.BatchNo).Readonly();
            View.Property(p => p.InspQty).Show().Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            //方法重写
        }
    }
}