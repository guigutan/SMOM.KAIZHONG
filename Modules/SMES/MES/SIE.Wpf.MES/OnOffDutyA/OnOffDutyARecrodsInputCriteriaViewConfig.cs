using SIE.MES.OnOffDutyA;
using System;

namespace SIE.Wpf.MES.OnOffDutyA
{
    /// <summary>
    /// 
    /// </summary>
    public class OnOffDutyARecrodsInputCriteriaViewConfig : WPFViewConfig<OnOffDutyRecrodsInputCriteria>
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
        protected override void ConfigQueryView()
        {
            View.Property(p => p.StaffCode).Show(ShowInWhere.All).HasLabel("员工号");
            View.Property(p => p.StaffName).Show(ShowInWhere.All).HasLabel("员工名");
            View.Property(p => p.StaffGroupName).Show(ShowInWhere.All);

        }
    }
}
