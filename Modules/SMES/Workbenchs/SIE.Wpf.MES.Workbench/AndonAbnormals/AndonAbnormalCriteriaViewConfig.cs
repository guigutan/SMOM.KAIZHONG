using SIE.MES.Workbench.AndonAbnormals;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常查询视图类
    /// </summary>
    internal class AndonAbnormalCriteriaViewConfig : WPFViewConfig<AndonAbnormalCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProductLine).Show(ShowInWhere.All);
                View.Property(p => p.AlertType).Show(ShowInWhere.All);
                View.Property(p => p.ExceptionType).Show(ShowInWhere.All);
                View.Property(p => p.ProcessStatus).Show(ShowInWhere.All);
            }
        }
    }
}
