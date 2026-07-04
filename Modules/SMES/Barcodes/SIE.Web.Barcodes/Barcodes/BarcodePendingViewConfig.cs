using SIE.Barcodes;
using SIE.MetaModel.View;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码挂起视图配置类
    /// </summary>
    internal class BarcodePendingViewConfig : WebViewConfig<BarcodePending>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("条码挂起");
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BarcodePending));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.UseCommands("SIE.Web.Barcodes.PendingCommand", "SIE.Web.Barcodes.BarcodeResumeCommand", WebCommandNames.ExportXls);
                View.Property(p => p.WONo).HasLabel("工单号").ShowInList(150);
                View.Property(p => p.Sn).ShowInList(150);
                View.Property(p => p.PrintedState).UseEnumEditor().ShowInList();
                View.Property(p => p.IsScraped).Readonly(true).ShowInList();
                View.Property(p => p.IsPending).Readonly(true).ShowInList();
                View.Property(p => p.PrinterName).HasLabel("打印人").ShowInList();
                View.Property(p => p.PrintDate).ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}