using SIE.Barcodes;
using SIE.Domain;
using SIE.MetaModel.View;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码打印视图配置类
    /// </summary>
    public class BarcodeReprintViewConfig : WebViewConfig<BarcodeReprint>
    {
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
                return string.Empty;
            return me.Range.StartSn + "-" + me.Range.EndSn;
        }
        #endregion

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("条码补打");
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BarcodeReprint));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.UseCommands("SIE.Web.Barcodes.ReprintCommand", "SIE.Web.Barcodes.OutBoxReprintCommand",WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection);
                View.Property(p => p.WONo).Readonly().HasLabel("工单号").ShowInList(150).FixColumn();
                View.Property(p => p.Sn).Readonly().ShowInList(150).FixColumn();
                View.Property(SnRangeProperty).HasLabel("条码范围").ShowInList(300).FixColumn();
                View.Property(p => p.Qty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).Readonly().UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).Readonly().ShowInList(150);
                View.Property(p => p.PrintTimes).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PrinterName).Readonly().HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).Readonly().UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsPending).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).ShowInList();
                View.Property(p => p.CreateDate).ShowInList();
                View.Property(p => p.UpdateByName).ShowInList();
                View.Property(p => p.UpdateDate).ShowInList();
            }
        }
    }
}