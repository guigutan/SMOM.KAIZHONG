using SIE.EMS.Report.EquipmentMixReport;
using SIE.Web.EMS.Report.EquipmentMixReport.Commands;

namespace SIE.Web.EMS.Report.EquipmentMixReport
{

    /// <summary>
    /// 视图配置
    /// </summary>
    public class EquipmentMixReportMonViewModelViewConfig : WebViewConfig<EquipmentMixReportMonViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EquipmentMixReportMonViewModel));
            View.UseCommand(typeof(ExportMixCommand).FullName);
        }
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.UseClientOrder();
            View.Property(p => p.FirstColumn).ShowInList(150);

            View.Property(p => p.January).ShowInList(70);
            View.Property(p => p.February).ShowInList(70);
            View.Property(p => p.March).ShowInList(70);
            View.Property(p => p.April).ShowInList(70);
            View.Property(p => p.May).ShowInList(70);
            View.Property(p => p.June).ShowInList(70);
            View.Property(p => p.July).ShowInList(70);
            View.Property(p => p.August).ShowInList(70);
            View.Property(p => p.September).ShowInList(70);
            View.Property(p => p.October).ShowInList(70);
            View.Property(p => p.November).ShowInList(70);
            View.Property(p => p.December).ShowInList(70);
        }
    }
}
