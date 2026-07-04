using SIE.MES.RoutingSettings;
using System;

namespace SIE.Web.MES.RoutingSettings
{
    /// <summary>
    /// 产线工艺路线视图配置
    /// </summary>
    internal class LineRoutingViewConfig : WebViewConfig<ResourceRouting>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands().UseImportCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.Resource).UsePagingLookUpEditor();
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).DefaultValue(DateTime.Now.ToString("yyyy/MM/dd")).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.Resource).UsePagingLookUpEditor();
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrderType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.Resource).UsePagingLookUpEditor();
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Week; }).Show(ShowInWhere.All);
            View.Property(p => p.EndDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Week; }).Show(ShowInWhere.All);
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

        /// <summary>
        /// 配置导入
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.OrderType);
            View.PropertyRef(p => p.Resource.Code).ImportIndexer().HasLabel("资源编码");
            View.PropertyRef(p => p.Routing.Name).ImportIndexer().HasLabel("工艺路线");
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }
    }
}
