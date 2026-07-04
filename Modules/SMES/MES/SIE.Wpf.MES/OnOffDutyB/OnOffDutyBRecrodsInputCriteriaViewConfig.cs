using SIE.MES.OnOffDutyB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.OnOffDutyB
{
    /// <summary>
    /// 
    /// </summary>
    public class OnOffDutyBRecrodsInputCriteriaViewConfig : WPFViewConfig<OnOffDutyBRecrodsInputCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OnOffDutyBViewModel));
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
