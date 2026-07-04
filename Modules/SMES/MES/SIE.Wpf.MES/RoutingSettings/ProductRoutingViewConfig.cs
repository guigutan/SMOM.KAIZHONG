using SIE.Common;
using SIE.Domain;
using SIE.MES.RoutingSettings;
using SIE.Wpf.Items;
using System.Linq;

namespace SIE.Wpf.MES.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线视图配置
    /// </summary>
    internal class ProductRoutingViewConfig : WPFViewConfig<ProductRouting>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.ProductId).UseProductCombinationEditor();
            View.Property(p => p.ProductName).HasLabel("产品名称");
            View.Property(p => p.RoutingId).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
            View.AttachChildrenProperty(typeof(RoutingVersionViewModel), obj =>
            {
                var childPaging = obj as ChildPagingDataArgs;
                var productRouting = obj.Parent as ProductRouting;
                if (productRouting == null)
                    return new EntityList<RoutingVersionViewModel>();
                else
                    return productRouting.Routing?.VersionList?.Select(p => new RoutingVersionViewModel()
                    {
                        VersionName = p.Name,
                        IsDefault = p.IsDefault,
                    })?.AsEntityList();
            }, ViewConfig.ListView, true).HasLabel("版本");
            View.AttachDetailChildrenProperty(typeof(DefaultRoutingViewModel), obj =>
            {
                DefaultRoutingViewModel routingVml = new DefaultRoutingViewModel();
                var childPaging = obj as ChildPagingDataArgs;
                var productRouting = obj.Parent as ProductRouting;
                if (productRouting != null)
                {
                    var product = productRouting.Product;
                    var routing = productRouting.Routing;
                    routingVml.Name = routing?.Name;
                    routingVml.Description = routing?.Description;
                    routingVml.DefaultVersion = routing?.DefaultVersion;
                }

                return routingVml;
            }).HasLabel("工艺路线");
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.ProductId).UsePagingLookUpEditor();
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
            View.Property(p => p.Product).UsePagingLookUpEditor();
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
            View.Property(p => p.ProductName);
            View.Property(p => p.RoutingName);
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }
    }
}
