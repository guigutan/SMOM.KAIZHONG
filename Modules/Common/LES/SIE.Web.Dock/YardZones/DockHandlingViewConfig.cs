using SIE.Dock.YardZones;
using SIE.MetaModel.View;
using SIE.Web.Core;

namespace SIE.Web.Dock.YardZones
{
    /// <summary>
    /// 月台装卸能力视图配置
    /// </summary>
    internal class DockHandlingViewConfig : WebViewConfig<DockHandling>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.BeginTime).UseSelectTimeEditor();
                View.Property(p => p.EndTime).UseSelectTimeEditor();
                View.Property(p => p.ShipAppoNum).ShowInList().UseListSetting(p => { p.HelpInfo = "大于0则没有限制"; });
                View.Property(p => p.ReceiveAppoNum);
                View.Property(p => p.Remark).ShowInList() ;
            }
        }
    }
}