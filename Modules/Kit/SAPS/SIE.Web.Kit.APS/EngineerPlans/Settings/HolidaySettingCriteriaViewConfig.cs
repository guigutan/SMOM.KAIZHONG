using SIE.Kit.APS.EngineerPlans.Settings;
using SIE.Resources.Enterprises;
using SIE.Web.Resources;
using System.Linq;

namespace SIE.Web.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 工程节假日维护查询视图
    /// </summary>
    public class HolidaySettingCriteriaViewConfig : WebViewConfig<HolidaySettingCriteria>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.StartDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
                View.Property(p => p.EndDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
                View.Property(p => p.Remerk).Show(ShowInWhere.All);
            }
        }
    }
}
