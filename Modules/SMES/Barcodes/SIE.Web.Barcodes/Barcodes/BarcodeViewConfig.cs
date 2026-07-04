using SIE.Barcodes;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.Barcodes.Barcodes.Commands;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BarcodeViewConfig : WebViewConfig<Barcode>
    {
        /// <summary>
        /// 条码报废
        /// </summary>
        public const string ScrapView = "BarcodeScrap";

        /// <summary>
        /// 条码补打
        /// </summary>
        public const string ReprintView = "BarcodeReprint";

        /// <summary>
        /// 工单打印
        /// </summary>
        public const string WoPrintView = "BarcodePrintView";

        /// <summary>
        /// 条码报废
        /// </summary>
        public const string PendingView = "BarcodePending";

        #region 条码范围 SnRange
        /// <summary>
        /// 条码范围
        /// </summary>
        public static readonly Property<string> SnRangeProperty = P<Barcode>.RegisterExtensionReadOnly("SnRange", typeof(BarcodeViewConfig),
            GetSnRangeProperty, Barcode.RangeIdProperty);

        /// <summary>
        /// 获取条码范围
        /// </summary>
        /// <param name="me">条码</param>
        /// <returns>条码范围</returns>
        public static string GetSnRangeProperty(Barcode me)
        {
            return $"{me.StartSn}-{me.EndSn}";
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("条码").HasDelegate(Barcode.IdProperty);
            View.DeclareExtendViewGroup(WoPrintView, ScrapView, WoPrintView, PendingView);

            if (ViewGroup == WoPrintView)
            {
                ConfigWoPrintView(); //条码打印中的条码明细 
            }
        }

        /// <summary>
        /// 配置 条码打印条码明细视图
        /// </summary>
        void ConfigWoPrintView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands(typeof(BarcodeBelongCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.WONo).HasLabel("工单号").ShowInList(150).FixColumn(true).Readonly();
                View.Property(p => p.Sn).ShowInList(150).FixColumn(true);
                View.Property(SnRangeProperty).HasLabel("条码范围").ShowInList(width: 300).FixColumn(true);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).ShowInList(150);
                View.Property(p => p.PrintTimes);
                View.Property(p => p.PrinterName).HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsPending).Readonly().Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 条码报废，条码补打 视图配置方法
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseGridSelectionModel();
            View.AssignAuthorize(typeof(Barcode));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.UseCommands("SIE.Web.Barcodes.ScarpCommand", "SIE.Web.Barcodes.BarcodeScrapExportXlsCommand");
                View.Property(p => p.WONo).HasLabel("工单号").Readonly(true).ShowInList(150);
                View.Property(p => p.Sn).Readonly(true).ShowInList(150);
                View.Property(p => p.PrintedState).Readonly(true).UseEnumEditor().ShowInList();
                View.Property(p => p.IsScraped).Readonly(true).ShowInList();
                View.Property(p => p.IsPending).Readonly(true).ShowInList();
                View.Property(p => p.PrinterName).Readonly(true).HasLabel("打印人").ShowInList();
                View.Property(p => p.PrintDate).Readonly(true).ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

    }
}
