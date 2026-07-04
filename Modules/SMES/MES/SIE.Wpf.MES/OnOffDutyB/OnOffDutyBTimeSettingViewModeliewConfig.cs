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
    public class OnOffDutyBTimeSettingViewModeliewConfig:WPFViewConfig<OnOffDutyBTimeSettingViewModel>
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
        protected override void ConfigDetailsView()
        {
            View.UseDetail(1);
            View.Property(p => p.OnDutyTime).UseDateTimeEditor(p => p.StringFormat = "yyyy-MM-dd hh:mm:ss");
            View.Property(p => p.OffDutyTime).UseDateTimeEditor();
        }
    }
}
