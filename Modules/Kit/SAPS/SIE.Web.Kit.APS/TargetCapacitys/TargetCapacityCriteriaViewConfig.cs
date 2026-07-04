using SIE.Kit.APS.TargetCapacitys;
using SIE.Web.Resources;

namespace SIE.Web.Kit.APS.TargetCapacitys
{
    /// <summary>
    /// 目标产能查询视图
    /// </summary>
    public class TargetCapacityCriteriaViewConfig : WebViewConfig<TargetCapacityCriteria>
    {
        /// <summary>
        /// 列表逻辑视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EnterpriseId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.Year).Show(ShowInWhere.All);
            }
        }
    }
}
