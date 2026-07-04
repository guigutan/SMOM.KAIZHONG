using SIE.MES.TeamManagement.ClockingIns;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤配置视图
    /// </summary>
    public class EmployeeClockInSetViewConfig : WebViewConfig<EmployeeClockInSetConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.UseDetail(dialogHeight: 600, dialogWidth: 600);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("配置项信息", detailColumnCount: 2))
                {
                    View.Property(p => p.OnDutyTimeType).UseEnumEditor().ShowInDetail();
                    View.Property(p => p.OffDutyTimeTypeType).UseEnumEditor().ShowInDetail();
                }

                using (View.DeclareGroup("允许时间范围", detailColumnCount: 2))
                {
                    View.Property(p => p.ShiftBeginBefore).ShowInDetail();
                    View.Property(p => p.ShiftEndBefore).ShowInDetail();
                    View.Property(p => p.ShiftBeginAfter).ShowInDetail();
                    View.Property(p => p.ShiftEndAfter).ShowInDetail();
                }
            }
        }
    }
}
