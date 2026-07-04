using SIE.MES.DashBoard.WorkOrderReachs;

namespace SIE.Wpf.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 达成率明细
    /// </summary>
    public class WoReachDetailViewConfig : WPFViewConfig<WoReachDetailViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.Export);
            View.Property(p => p.No).Show();
            View.Property(p => p.Product.Code).Show().HasLabel("产品编码");
            View.Property(p => p.WorkShop).Show();
            View.Property(p => p.Resource).Show();
            View.Property(p => p.PlanQty).Show();
            View.Property(p => p.FinishQty).Show();
            View.Property(p => p.State).Show();
            View.Property(p => p.ReachState).Show();
            View.Property(p => p.PlanBeginDate).Show();
            View.Property(p => p.PlanEndDate).Show();
            View.Property(p => p.ActuStartDate).Show();
            View.Property(p => p.ActuFinishDate).Show();
        }
    }
}
