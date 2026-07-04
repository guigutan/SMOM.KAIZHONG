using SIE.MES.OnOffDuty;
using System;

namespace SIE.Wpf.MES.OnOffDuty
{
    /// <summary>
    /// 
    /// </summary>
    public class OnOffDutyRecrodsInputCriteriaViewConfig : WPFViewConfig<OnOffDutyRecrodsInputCriteria>
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
        protected override void ConfigQueryView()
        {
            View.Property(p => p.StaffCode).Show(ShowInWhere.All).HasLabel("员工号");
            View.Property(p => p.StaffName).Show(ShowInWhere.All).HasLabel("员工名");
            View.Property(p => p.StaffGroupName).Show(ShowInWhere.All);

        }
    }
}
