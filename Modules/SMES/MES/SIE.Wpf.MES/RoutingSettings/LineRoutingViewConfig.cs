using SIE.MES.RoutingSettings;
using SIE.Wpf.Resources;

namespace SIE.Wpf.MES.RoutingSettings
{
    /// <summary>
    /// 产线工艺路线视图配置
    /// </summary>
    internal class LineRoutingViewConfig : WPFViewConfig<ResourceRouting>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.ResourceId).UseEnterpriseResourceEditor();
            View.Property(p => p.RoutingId).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.ResourceId).UseEnterpriseResourceEditor();
            View.Property(p => p.RoutingId).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.Resource).UseEnterpriseResourceEditor();
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Week;
                e.DateTimePart = ObjectModel.DateTimePart.Date;
            });
            View.Property(p => p.EndDate).UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Week;
                e.DateTimePart = ObjectModel.DateTimePart.Date;
            });
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.ResourceName);
            View.Property(p => p.RoutingName);
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }
    }
}
