using SIE.Web.WorkBenchCommon._Extensions_;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.WorkBenchCommon.Workbench.TargetWarn;

namespace SIE.Web.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定查询视图
    /// </summary>
    internal class TargetWarnSettingCriteriaViewConfig : WebViewConfig<TargetWarnSettingCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.AddBehavior("SIE.Web.WorkBenchCommon.Workbench.TargetWarn.Behaviors.QuotaWarnSettingCriteriaBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("指标分类").Show(ShowInWhere.All).UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingCodeDic(); });
                View.Property(p => p.Name).HasLabel("指标名称").Show(ShowInWhere.All).UseKpiDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingNameDic(string.Empty); });
            }
        }
    }
}
