using SIE.MetaModel.View;
using SIE.ProductIntfc.InspLogs;

namespace SIE.Web.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 报检条码日志明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class FirstInspDetailViewConfig : WebViewConfig<InspBarcodeLog>
    {
        /// <summary>
        /// 报检单明细视图
        /// </summary>
        public const string FirstInspDetailView = "FirstInspDetailView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(FirstInspDetailView);
            if (ViewGroup == FirstInspDetailView)
                ConfigFirstInspDetailView();
        }

        /// <summary>
        /// 配置首检报检明细视图
        /// </summary>
        private void ConfigFirstInspDetailView()
        {
            View.AssignAuthorize(typeof(SIE.ProductIntfc.FirstInsps.FirstInsp));
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.ExportXls);
                View.Property(p => p.Barcode).Readonly().ShowInList();
                View.Property(p => p.Process).Readonly().ShowInList();
                View.Property(p => p.CollectionDate).Readonly().ShowInList(width: 180);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
