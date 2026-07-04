using SIE.Kit.APS.FactoryPlanQtys;
using SIE.Resources.Enterprises;
using SIE.Web.Resources;
using System.Linq;

namespace SIE.Web.Kit.APS.FactoryPlanQtys
{
    /// <summary>
    /// 工厂计划数配置视图配置
    /// </summary>
    internal class FactoryPlanQtyViewConfig : WebViewConfig<FactoryPlanQty>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {

        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.WorkCeil).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = false;
                });
            }
        }
    }
}