using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.Fixtures.Querys.ViewModels;
using SIE.Items;
using SIE.Resources.WipResources;
using SIE.Web.Equipments.Extensions;
using SIE.Web.Items._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Querys.ViewModels
{
    /// <summary>
    /// 工治具查询查询体视图配置
    /// </summary>
    internal class FixtureQueryVMCriteriaViewConfig : WebViewConfig<FixtureQueryViewModelCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Querys.Scripts.FixtureCriteriaBehavior");
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Cascade(p => p.FixtureModel, null).Cascade(p => p.FixtureEncode, null).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkShop).UseResourceWorkShopEditor().Cascade(p => p.ResourceId, null).Cascade(p => p.WorkOrderId, null).Cascade(p => p.Product, null).Cascade(p => p.WorkOrderProductName, null).Cascade(p => p.Deck, null).UseListSetting(e => { e.HelpInfo = "更改车间清空产线"; }).HasLabel("车间").Show(ShowInWhere.Detail);
                View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as FixtureQueryViewModelCriteria;
                    if (criteria == null || criteria.WorkShop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor((c, e) =>
                {
                    c.ReloadDataOnPopping = true;
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ResourceCode), nameof(e.Resource.Code));
                    c.DicLinkField = keyValues;

                }).Cascade(p => p.WorkOrderId, null).Cascade(p => p.ProductId, null).Cascade(p => p.WorkOrderProductName, null).Cascade(p => p.Deck, null)
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源"; }).HasLabel("产线").Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as FixtureQueryViewModelCriteria;
                    if (criteria == null)
                        return new EntityList<WorkOrder>();
                    return RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(criteria.WorkShopId, criteria.ResourceId, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).Cascade(p => p.ProductId, null).Cascade(p => p.WorkOrderProductName, null).Cascade(p => p.Deck, null).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.ProductId), nameof(r.WorkOrder.ProductId));
                    keyValues.Add("ProductId_Display", "ProductId_Display");
                    keyValues.Add(nameof(r.WorkOrderProductName), nameof(r.WorkOrder.WorkOrderProductName));
                    keyValues.Add(nameof(r.WorkOrderNo), nameof(r.WorkOrder.No));
                    m.DicLinkField = keyValues;
                }).HasLabel("工单号").Show(ShowInWhere.Detail);
                View.Property(p => p.ProductId).UseProductCombinationEditor().UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.WorkOrderProductName), nameof(r.Product.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("产品编码").Show(ShowInWhere.Detail).Readonly(p => p.WorkOrderId != null);
                View.Property(p => p.WorkOrderProductName).Readonly().Show(ShowInWhere.Detail);
                View.Property(p => p.Deck).UseEnumEditor().Show(ShowInWhere.Detail).Readonly(p => p.WorkOrderId != null);
                View.Property(p => p.RepairBeforeState).UseEnumEditor().DefaultValue(10).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureModel).UseDataSource((e, c, r) =>
                {
                    var criteria = e as FixtureQueryViewModelCriteria;
                    if (criteria == null)
                        return new EntityList<FixtureModel>();
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureModels(criteria.FixtureType, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).Cascade(p => p.FixtureEncode, null)
                .UseListSetting(e => { e.HelpInfo = "显示工治具型号"; }).HasLabel("工治具型号").Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureEncode).UseDataSource((e, c, r) =>
                {
                    var criteria = e as FixtureQueryViewModelCriteria;
                    if (criteria == null)
                        return new EntityList<FixtureEncode>();
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(criteria.FixtureType, criteria.FixtureModelId, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
                .UseListSetting(e => { e.HelpInfo = "显示工治具编码"; }).HasLabel("工治具编码").Show(ShowInWhere.Detail);
            }
        }
    }
}
