using SIE.Domain;
using SIE.MES.TaskManagement.Reports;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 生产任务报表查询实体视图配置
    /// </summary>
    internal class ProductionDispatchReportCriteriaViewConfig : WebViewConfig<ProductionDispatchReportCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkShop).UseResourceWorkShopEditor().Cascade(p => p.Resource, null).Show(ShowInWhere.Detail)
                    .UseListSetting(e => { e.HelpInfo = "更改车间清空资源"; });
                View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
                {
                    var criteria = e as ProductionDispatchReportCriteria;
                    if (criteria == null || criteria.WorkShop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且来源类型为企业模型的生产资源"; });
                View.Property(p => p.ProductId).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderState).UseEnumEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.No).Show(ShowInWhere.Detail).HasLabel("任务单");
                View.Property(p => p.TaskStatus).UseEnumEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.WorkGroupId).Show(ShowInWhere.Detail);
                View.Property(p => p.EmployeeId).Show(ShowInWhere.Detail);
                View.Property(p => p.BeginTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/M/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Custom;
                });
            }
        }
    }
}