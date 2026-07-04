using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工管理查询实体视图配置
    /// </summary>
    internal class ReportDispatchTaskCriteriaViewConfig : WebViewConfig<ReportDispatchTaskCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.MES.TaskManagement.Report.Behaviors.DispatchTaskCriteriaBehaviors");
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.Detail).HasLabel("任务单");
                View.Property(p => p.TaskStatus).UseEnumEditor("CriteriaEntity").Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.Product).UseDataSource((e, p, s) =>
                {
                    var criteria = e as ReportDispatchTaskCriteria;
                    if (criteria == null)
                        return new EntityList<Item>();
                    return RT.Service.Resolve<ItemController>().GetItemDatas(p, s);
                }).UsePagingLookUpEditor().Show(ShowInWhere.Detail).HasLabel("产品编码");
                View.Property(p => p.ReportMode).UseEnumEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.ResourceId).Show(ShowInWhere.Detail);
                View.Property(p => p.PlanBeginTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                });
                View.Property(p => p.PlanEndTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                });
                View.Property(p => p.IsShowDispatchTask).UseCheckEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.IsUnscheduledInProgress).UseCheckEditor().Show(ShowInWhere.Detail); 
            }
        }
    }
}