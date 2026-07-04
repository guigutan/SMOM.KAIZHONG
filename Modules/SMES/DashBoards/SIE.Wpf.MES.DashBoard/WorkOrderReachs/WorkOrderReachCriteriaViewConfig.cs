using SIE.Domain;
using SIE.MES.DashBoard.WorkOrderReachs;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Resources;
using System.Collections.Generic;

namespace SIE.Wpf.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 产品报表查询实体视图配置
    /// </summary>
    internal class WorkOrderReachCriteriaViewConfig : WPFViewConfig<WorkOrderReachCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkShop).UseResourceWorkShopEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
                {

                    var criteria = e as WorkOrderReachCriteria;
                    if (criteria == null || criteria.WorkShop == null)
                        return new EntityList<WipResource>();

                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor(e => e.ReloadDataOnPopping = true);
                View.Property(p => p.Shift).UseDataSource((e, p, s) =>
                {
                    EntityList<Shift> shiftList = new EntityList<Shift>();
                    var criteria = e as WorkOrderReachCriteria;
                    if (criteria == null || criteria.ResourceId == null || criteria.ResourceId == 0)
                        return shiftList;
                    var date = RF.Find<Shift>().GetDbTime();
                    var calender = RT.Service.Resolve<WipResourceController>().GetWipResourceShift((double)criteria.ResourceId, date.Date);
                    if (calender == null || calender.ShiftType == null)
                        return shiftList;
                    return calender.ShiftType.ShiftList;
                }).Show(ShowInWhere.Detail);
                View.Property(p => p.Model).Show(ShowInWhere.Detail);
                View.Property(p => p.PlanDate).HasLabel("计划结束").UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Month; p.DateTimePart = ObjectModel.DateTimePart.Date; }).Show(ShowInWhere.Detail);
            }
        }
    }
}
