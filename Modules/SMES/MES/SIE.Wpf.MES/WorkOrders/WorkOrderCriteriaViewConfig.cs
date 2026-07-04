using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using SIE.Wpf.Resources;
using System.Collections.Generic;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单查询实体视图配置
    /// </summary>
    internal class WorkOrderCriteriaViewConfig : WPFViewConfig<WorkOrderCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);

                View.Property(p => p.ItemCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ItemName).Show(ShowInWhere.Detail);
                View.Property(p => p.Workshop).UseResourceWorkShopEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
                {
                    var criteria = e as WorkOrderCriteria;
                    if (criteria == null || criteria.Workshop == null)
                        return new EntityList<WipResource>();

                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkshopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; });
                View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p => { p.DateTimePart = ObjectModel.DateTimePart.Date; p.DateRangeType = ObjectModel.DateRangeType.Week; });
                View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p => { p.DateTimePart = ObjectModel.DateTimePart.Date; p.DateRangeType = ObjectModel.DateRangeType.Week; });
                View.Property(p => p.State).Show(ShowInWhere.Detail);
                View.Property(p => p.KitType).Show(ShowInWhere.Hide);
                View.Property(p => p.Source).Show(ShowInWhere.Detail);
                View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.ProcessTechOrderCode).Show(ShowInWhere.Detail);
            }
        }
    }
}