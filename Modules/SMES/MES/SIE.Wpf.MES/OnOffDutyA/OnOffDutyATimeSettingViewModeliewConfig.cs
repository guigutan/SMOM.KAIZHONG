using SIE.MES.OnOffDutyA;

namespace SIE.Wpf.MES.OnOffDutyA
{
    /// <summary>
    /// 
    /// </summary>
    public class OnOffDutyATimeSettingViewModeliewConfig : WPFViewConfig<OnOffDutyTimeSettingViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OnOffDutyAViewModel));
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
