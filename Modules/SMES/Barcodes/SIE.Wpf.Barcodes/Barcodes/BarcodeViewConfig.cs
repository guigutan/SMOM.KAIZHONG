using SIE.Barcodes;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Barcodes.Commonds;
using SIE.Wpf.Common.ViewBehaviors;

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 条码视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class BarcodeViewConfig : WPFViewConfig<Barcode>
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
            if (me == null || me.Range == null)
            {
                return string.Empty;
            }

            return me.Range.StartSn + "-" + me.Range.EndSn;
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("条码").HasDelegate(Barcode.IdProperty);
            View.DeclareExtendViewGroup(ScrapView, ReprintView, WoPrintView);
            if (ViewGroup == ScrapView)
            {
                ConfigScrapView();
            }
            else if (ViewGroup == ReprintView)
            {
                ConfigDefaultView();
            }
            else if (ViewGroup == WoPrintView)
            {
                ConfigWoPrintView(); //条码打印中的条码明细 
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 条码报废 视图配置方法
        /// </summary>
        void ConfigScrapView()
        {
            View.UseCommands(WPFCommandNames.Export, typeof(ScrapBarcodeCommand));
            View.AddBehavior(typeof(MultipleRowViewBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.WONo).HasLabel("工单号").ShowInList();
                View.Property(p => p.Sn).ShowInList();
                View.Property(p => p.PrintedState).UseEnumEditor().ShowInList();
                View.Property(p => p.IsScraped).ShowInList();
                View.Property(p => p.IsPending).ShowInList();
                View.Property(p => p.PrinterName).HasLabel("打印人").ShowInList();
                View.Property(p => p.PrintDate).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 条码补打 视图配置方法
        /// </summary>
        void ConfigDefaultView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(ReprintCommand), typeof(OutBoxReprintCommand));
                View.AddBehavior(typeof(MultipleRowViewBehavior));
                View.Property(p => p.WONo).HasLabel("工单号").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.Sn).Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(SnRangeProperty).HasLabel("条码范围").UseListSetting(e => e.ListGridWidth = 200).Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.Qty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).Show(ShowInWhere.All);
                View.Property(p => p.PrintTimes).Show(ShowInWhere.All);
                View.Property(p => p.PrinterName).HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Show(ShowInWhere.All);
                View.Property(p => p.IsPending).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置 条码打印条码明细视图
        /// </summary>
        void ConfigWoPrintView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WONo).HasLabel("工单号").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left).Readonly();
                View.Property(p => p.Sn).Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(SnRangeProperty).HasLabel("条码范围").UseListSetting(e => e.ListGridWidth = 250).Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.Qty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).Show(ShowInWhere.All);
                View.Property(p => p.PrintTimes).Show(ShowInWhere.All);
                View.Property(p => p.PrinterName).HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Show(ShowInWhere.All);
                View.Property(p => p.IsPending).Show(ShowInWhere.All);
            }
        }
    }
}