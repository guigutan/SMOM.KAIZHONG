namespace SIE.Wpf.MES.Workbench.MonthSchedules
{
    public class ProductingReadiesViewConfig : WPFViewConfig<SIE.MES.Workbench.ProductingReadies.ProductingReady>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();

            View.Property(p => p.WorkOrder);
            View.Property(p => p.ItemState);
            View.Property(p => p.ToolState);
            View.Property(p => p.EmployeeState);
            View.Property(p => p.EsopState);
            View.Property(p => p.QualityState);



        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrder);

        }
    }
}
