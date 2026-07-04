using SIE.WorkBenchCommon.Workbench.TargetWarn;

namespace SIE.Wpf.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定查询视图
    /// </summary>
    internal class TargetWarnSettingCriteriaViewConfig : WPFViewConfig<TargetWarnSettingCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.AddBehavior(typeof(TargetWarnSettingQueryChangeQuota));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("指标分类").Show(ShowInWhere.All).UseQuotaCategoryEditor();
                View.Property(p => p.Name).HasLabel("指标名称").Show(ShowInWhere.All).UseQuotaNameEditor(e => { e.UpperLevelProperty = TargetWarnSettingCriteria.CodeProperty; e.ReloadDataOnPopping = true; });
            }
        }
    }
}
