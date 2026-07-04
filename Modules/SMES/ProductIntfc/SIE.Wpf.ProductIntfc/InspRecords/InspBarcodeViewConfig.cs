using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.ProductInsps;
using SIE.Wpf.ProductIntfc.InspRecords.Commands;

namespace SIE.Wpf.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检条码视图配置
    /// </summary>
    internal class InspBarcodeViewConfig : WPFViewConfig<InspBarcode>
    {
        /// <summary>
        /// 成品报检子视图视图
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
            View.AddBehavior(typeof(InspBarcodeViewBehavior));
            View.UseCommands(typeof(ShippingInspCommand), WPFCommandNames.Export);
            View.Property(p => p.Barcode);
            View.Property(p => p.Process);
            View.Property(p => p.CollectionDate).UseListSetting(e => e.ListGridWidth = 150);
            View.Property(p => p.BatchNo);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //方法重写
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