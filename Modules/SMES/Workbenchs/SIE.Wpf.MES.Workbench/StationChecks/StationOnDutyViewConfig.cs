using SIE.MES.Workbench.StationChecks;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    internal class StationOnDutyViewConfig : WPFViewConfig<StationOnDuty>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.Property(p => p.Station);
            View.Property(p => p.OnDuty);
            View.Property(p => p.ActualOnDuty);
            View.Property(p => p.OnDutyDate).UseDateEditor().HasLabel("日期");
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Station);
            View.Property(p => p.OnDuty);
            View.Property(p => p.ActualOnDuty);
            View.Property(p => p.OnDutyDate).UseDateEditor().HasLabel("日期");
        }
    }
}
