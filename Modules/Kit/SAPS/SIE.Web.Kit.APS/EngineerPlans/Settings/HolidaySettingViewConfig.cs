using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Resources.Enterprises;
using SIE.Web.Resources;
using System.Linq;

namespace SIE.Web.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 工程节假日维护 视图配置
    /// </summary>
    internal class HolidaySettingViewConfig : WebViewConfig<HolidaySetting>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor();
                View.Property(p => p.StartDate).UseDateEditor().Show(ShowInWhere.All);
                View.Property(p => p.EndDate).UseDateEditor().Show(ShowInWhere.All);
                View.Property(p => p.Remerk).Show(ShowInWhere.All);
            }
        }

        protected override void ConfigQueryView()
        {
           
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor();
                View.Property(p => p.StartDate).UseDateEditor().Show(ShowInWhere.All);
                View.Property(p => p.EndDate).UseDateEditor().Show(ShowInWhere.All);
                View.Property(p => p.Remerk).Show(ShowInWhere.All);
            }
        }
    }
}
