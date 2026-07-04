using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.Fixtures.Demands.ViewModels
{
    /// <summary>
    /// 配置工治具治具需求清单ViewModel视图
    /// </summary>
    internal class FixtureDemandViewModelViewConfig : WebViewConfig<FixtureDemandViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(FixtureDemand));
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Demands.Scripts.DemandBehavior");
            View.ClearCommands();
            View.UseChildrenGroupAsHorizontal();
            View.HasDetailColumnsCount(4);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.WorkOrderNo).Readonly();
            View.Property(p => p.ProductCode).Readonly();
            View.Property(p => p.WarehouseId).HasLabel("仓库");
            View.AttachChildrenProperty(typeof(FixtureDemandDetail), (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var entity = args.Parent as FixtureDemandViewModel;
                var details = RT.Service.Resolve<ElecFixtureController>().GetFixtureDemandDetails(entity.Id, args.PagingInfo);
                if (details == null) details = new EntityList<FixtureDemandDetail>();
                return details;
            }, FixtureDemandDetailViewConfig.ReadonlyDemandView, childLayoutType: ChildLayoutType.Card).Show(ChildShowInWhere.All).HasLabel("需求明细");
            View.AttachChildrenProperty(typeof(UnloadStockViewModel), e => new EntityList<UnloadStockViewModel>(), UnloadStockViewModelViewConfig.ListView, childLayoutType: ChildLayoutType.Card).Show(ChildShowInWhere.All).HasLabel("库存情况");

            View.AttachChildrenProperty(typeof(FixtureUnloadViewModel), (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var entity = args.Parent as FixtureDemandViewModel;
                var unloads = RT.Service.Resolve<ElecFixtureController>().GetFixtureUnloadViewModels(entity.Id, args.PagingInfo);
                if (unloads == null) unloads = new EntityList<FixtureUnloadViewModel>();
                return unloads;
            }, FixtureUnloadViewModelViewConfig.ListView).Show(ChildShowInWhere.All).HasLabel("出库明细".L10N());
        }
    }
}
