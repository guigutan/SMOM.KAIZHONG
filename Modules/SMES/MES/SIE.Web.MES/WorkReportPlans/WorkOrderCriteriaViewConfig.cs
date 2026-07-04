using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MES.WorkReportPlans;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System.Collections.Generic;

namespace SIE.Web.MES.WorkReportPlans
{
    /// <summary>
    /// 报工配置方案查询实体视图配置
    /// </summary>
    internal class WorkReportPlanCriteriaViewConfig : WebViewConfig<WorkReportPlanCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.PlanCode).Show(ShowInWhere.Detail).HasOrderNo(10);
            View.Property(p => p.PlanName).Show(ShowInWhere.Detail).HasOrderNo(20);
            View.Property(p => p.Process).Show(ShowInWhere.Detail).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).HasOrderNo(30);
            View.Property(p => p.EnableStatus).Show(ShowInWhere.Detail).HasOrderNo(40);
            
        }
    }
}