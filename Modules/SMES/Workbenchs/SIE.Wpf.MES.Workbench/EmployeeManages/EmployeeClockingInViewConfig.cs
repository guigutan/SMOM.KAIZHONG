using SIE.MES.Workbench.EmployeeManages;

namespace SIE.Wpf.MES.Workbench.EmployeeManages
{
    internal class EmployeeClockingInViewConfig : WPFViewConfig<EmployeeClockingIn>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.Property(p => p.Employee);
            View.Property(p => p.ClockingInDate).UseDateEditor();
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Employee);
            View.Property(p => p.ClockingInDate).UseDateEditor();
        }
    }
}
