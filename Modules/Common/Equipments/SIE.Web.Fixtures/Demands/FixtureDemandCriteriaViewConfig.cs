using SIE.Domain;
using SIE.Fixtures.FixtureDemands;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Web.Equipments.Extensions;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Demands
{
    /// <summary>
    /// 工治具治具需求清单查询实体视图配置
    /// </summary>
    internal class FixtureDemandCriteriaViewConfig : WebViewConfig<FixtureDemandCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkShop).UseResourceWorkShopEditor().Cascade(p => p.Resource, null).UseListSetting(e => { e.HelpInfo = "更改车间清空产线"; }).HasLabel("车间").Show(ShowInWhere.Detail);
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as FixtureDemandCriteria;
                    if (criteria == null || criteria.WorkShop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源"; }).HasLabel("产线").Show(ShowInWhere.Detail);
                View.Property(p => p.ProductCode).HasLabel("产品").Show(ShowInWhere.Detail);
                View.Property(p => p.Employee).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                }).HasLabel("创建人").Show(ShowInWhere.Detail);
                View.Property(p => p.DemandState).Show(ShowInWhere.Detail);
                View.Property(p => p.ReceiveState).Show(ShowInWhere.Detail);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
