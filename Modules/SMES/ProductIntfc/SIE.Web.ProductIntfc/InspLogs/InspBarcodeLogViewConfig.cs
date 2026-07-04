using SIE.MetaModel.View;
using SIE.ProductIntfc.InspLogs;

namespace SIE.Web.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检条码日志明细视图配置
    /// </summary>
    public class InspBarcodeLogViewConfig : WebViewConfig<InspBarcodeLog>
    {
        /// <summary>
        /// 报检单明细视图
        /// </summary>
        public const string LogDetailView = "LogDetailView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == LogDetailView)
                ConfigLogDetailView();
        }

        /// <summary>
        /// 配置报检单明细视图
        /// </summary>
        private void ConfigLogDetailView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).ShowInList(width: 150).ShowInList();
                View.Property(p => p.BatchNo).ShowInList();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.Barcode).Readonly();
            View.Property(p => p.BatchNo).Readonly().HasLabel("批次号");
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