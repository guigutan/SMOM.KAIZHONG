using SIE.WorkBenchCommon.Workbench.KPI;

namespace SIE.Web.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 绩效目标视图配置
    /// </summary>
    internal class KpiModelViewConfig : WebViewConfig<KpiModel>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.Goal);
            View.Property(p => p.Actual);
            View.Property(p => p.Format);
            View.Property(p => p.KpiOperators);
            View.Property(p => p.KpiGrade);
            View.Property(p => p.ModuleCategory);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.Goal);
            View.Property(p => p.Actual);
            View.Property(p => p.Format);
            View.Property(p => p.KpiOperators);
            View.Property(p => p.KpiGrade);
            View.Property(p => p.ModuleCategory);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.ModuleCategory);
        }
    }
}
