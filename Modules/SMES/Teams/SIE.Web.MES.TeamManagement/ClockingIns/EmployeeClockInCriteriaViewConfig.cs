using SIE.MES.TeamManagement.ClockingIns;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 物料查询视图配置
    /// </summary>
    public class EmployeeClockInCriteriaViewConfig : WebViewConfig<EmployeeClockInCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).ShowInDetail();
                View.Property(p => p.EmployeeName).ShowInDetail();
                View.Property(p => p.WorkGroup).ShowInDetail();
                View.Property(p => p.EmployeeDate).Show(ShowInWhere.Hide);
                View.Property(p => p.OnDutyState).ShowInDetail();
                View.Property(p => p.ShiftRange).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Today;
                }).ShowInDetail();
            }
        }
    }
}
