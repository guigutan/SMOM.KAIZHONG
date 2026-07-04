using SIE.MES.PackingPrints;


namespace SIE.Wpf.MES.PackingPrints
{
    /// <summary>
    /// 拼板码打印查询界面
    /// </summary>
    internal class PackingWorkOrderCriteriaViewConfig : WPFViewConfig<PackingWorkOrderCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.PlanBeginDate).UseDateRangeEditor().Show();
            }
        }
    }
}
