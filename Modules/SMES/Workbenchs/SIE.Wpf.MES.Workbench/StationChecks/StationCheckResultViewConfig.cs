using SIE.MES.Workbench.StationChecks;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 
    /// </summary>
    internal class StationCheckResultViewConfig : WPFViewConfig<StationCheckResult>
    {
        protected override void ConfigListView()
        {
            base.ConfigListView();
            View.Property(p => p.Station);
            View.Property(p => p.CheckType).UseEnumEditor();
            View.Property(p => p.CheckItemId);
            View.Property(p => p.CheckDate).UseDateEditor();
            View.Property(p => p.IsCheck);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Station);
            View.Property(p => p.CheckType).UseEnumEditor();
            View.Property(p => p.CheckDate).UseDateEditor();
        }
    }
}