using SIE.Barcodes.Panels;

namespace SIE.Web.Barcodes.Panels
{
    /// <summary>
    /// 拼板码打印查询界面
    /// </summary>
    internal class PanelWorkOrderCriteriaViewConfig : WebViewConfig<PanelWorkOrderCriteria>
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