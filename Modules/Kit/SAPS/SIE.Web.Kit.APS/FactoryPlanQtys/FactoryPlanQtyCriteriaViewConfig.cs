using SIE.Kit.APS.FactoryPlanQtys;
using SIE.Resources.Enterprises;
using SIE.Web.Resources;
using System.Linq;

namespace SIE.Web.Kit.APS.FactoryPlanQtys
{
    /// <summary>
    /// 工厂计划配置数查询实体
    /// </summary>
    public class FactoryPlanQtyCriteriaViewConfig : WebViewConfig<FactoryPlanQtyCriteria>
    {
        /// <summary>
        /// 主体
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.All);
            }
        }
    }
}
