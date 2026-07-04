using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单查询实体视图配置
    /// </summary>
    internal class WorkOrderCriteriaViewConfig : WebViewConfig<WorkOrderCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            //using (View.OrderProperties())
            //{
            
            View.Property(p => p.PanelWorkOrderNo).Show(ShowInWhere.Detail).HasOrderNo(10);
            View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail).HasOrderNo(20);

            View.Property(p => p.ItemCode).Show(ShowInWhere.Detail).HasOrderNo(30);
            View.Property(p => p.ItemName).Show(ShowInWhere.Detail).HasOrderNo(40);
            View.Property(p=>p.WorkShopCode).Show(ShowInWhere.Detail).HasOrderNo(40);
            //View.Property(p => p.Workshop).UseResourceWorkShopEditor().Cascade(p => p.Resource, null).Show(ShowInWhere.Detail)
            //    .UseListSetting(e => { e.HelpInfo = "更改车间清空资源"; }).HasOrderNo(50);
            View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, pagingInfo, keyword) =>
            {
                var criteria = e as WorkOrderCriteria;
                if (criteria == null)
                {
                    return new EntityList<WipResource>();
                }

                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                if (criteria.WorkshopId.HasValue)
                {
                    return RT.Service.Resolve<WipResourceController>()
                        .GetWipResourcesByWorkShopId(stateList, new List<double?> { criteria.WorkshopId.Value },
                        srcTypeList, pagingInfo, keyword);
                }

                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, srcTypeList, pagingInfo, keyword);

            }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
            .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源"; }).HasOrderNo(60);
            View.Property(p => p.CreateDate).Show().UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
            }).HasOrderNo(81); ;
            View.Property(p => p.UpdateDate).Show().UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
            }).HasOrderNo(82) ;
            View.Property(p => p.ProductShortDescription).Show(ShowInWhere.Detail).HasOrderNo(85);
            View.Property(p => p.State).UseEnumMutilEditor(x => x.EnumType = typeof(SIE.Core.WorkOrders.WorkOrderState)).Show(ShowInWhere.Detail).HasOrderNo(90);
            View.Property(p => p.IsPause).Show(ShowInWhere.Detail).HasOrderNo(100);
            View.Property(p => p.IsClose).Show(ShowInWhere.Detail).HasOrderNo(105);
            View.Property(p => p.KitType).Show(ShowInWhere.Hide).HasOrderNo(110);
            View.Property(p => p.Source).Show(ShowInWhere.Detail).HasOrderNo(120);
            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail).HasOrderNo(130);
            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail).HasOrderNo(140);
            View.Property(p => p.ProcessTechOrderCode).Show(ShowInWhere.Detail).HasOrderNo(150);
            View.Property(p => p.PlanNo).Show(ShowInWhere.Detail).HasOrderNo(160);
            View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
            }).Show(ShowInWhere.Detail).HasOrderNo(170);
            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
            {
                p.Format = "Y/M/d";
                p.DateRangeType = ObjectModel.DateRangeType.Week;
            }).HasOrderNo(180);
            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
            {
                p.Format = "Y/M/d";
                p.DateRangeType = ObjectModel.DateRangeType.Week;
            }).HasOrderNo(190);
            //}
        }
    }
}