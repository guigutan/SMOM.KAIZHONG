using SIE.MES.OnOffDutyA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.OnOffDutyA
{
    /// <summary>
    /// 
    /// </summary>
    public class OnOffDutyARecrodsViewConfig:WPFViewConfig<OnOffDutyRecrodsA>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.EnableEditing();
            View.Property(p => p.EmployeeCode).Show(ShowInWhere.All);
            View.Property(p => p.EmployeeName).Show(ShowInWhere.All);
            View.Property(p => p.EmployeeGroupName).Show(ShowInWhere.All);
            View.Property(p => p.OnDutyTime).HasLabel("上岗补录时间").UseDateTimeEditor().Show(ShowInWhere.All).Readonly(false);
            View.Property(p => p.OffDutyTime).HasLabel("下岗补录时间").UseDateTimeEditor().Show(ShowInWhere.All).Readonly(false);
            View.Property(p => p.UserCode).Show(ShowInWhere.All);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

        }
    }
}
