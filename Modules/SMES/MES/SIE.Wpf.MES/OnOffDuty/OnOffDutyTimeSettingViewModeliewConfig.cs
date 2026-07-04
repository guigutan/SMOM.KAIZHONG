using SIE.MES.OnOffDuty;

namespace SIE.Wpf.MES.OnOffDuty
{
    /// <summary>
    /// 
    /// </summary>
    public class OnOffDutyTimeSettingViewModeliewConfig : WPFViewConfig<OnOffDutyTimeSettingViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OnOffDutyViewModel));
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(1);
            View.Property(p => p.OnDutyTime).UseDateTimeEditor(p=>p.StringFormat="yyyy-MM-dd hh:mm:ss");
            View.Property(p => p.OffDutyTime).UseDateTimeEditor();
        }
    }
}
