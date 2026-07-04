using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks;
using SIE.Domain;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{

    /// <summary>
    /// 异常任务查询实体视图配置
    /// </summary>
    public class AbnormalMonitorTaskCriteriaViewConfig : WebViewConfig<AbnormalMonitorTaskCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.AbnormalName).Show();
                View.Property(p => p.AbnormalDefineId).Show();
                View.Property(p => p.TaskState).Show();
                View.Property(p => p.TaskType).Show();
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var result = RT.Service.Resolve<WorkShopController>().GetEnterprises(EnterpriseType.Shop, pagingInfo, keyword);
                    if (result == null) return new EntityList<WorkShop>();
                    return result;
                }).UsePagingLookUpEditor().Show();
                View.Property(p => p.LineId).Show();
                View.Property(p => p.ProblemDescription).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; }).Show();
            }
        }
    }
}
