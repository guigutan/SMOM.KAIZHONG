using SIE.Domain;
using SIE.EMS.Report.WorkOrderExcuteReports;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Core;
using SIE.Web.Resources;
using System;
using System.Linq;

namespace SIE.Web.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表查询视图
    /// </summary>
    public class WorkOrderExcuteReportCriteriaViewConfig : WebViewConfig<WorkOrderExcuteReportViewModelCriteria>
    {
        /// <summary>
        /// 配置查询列表
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.EMS.Report.WorkOrderExcuteReports.Commands.WorkOrderExcuteReportQuery");
            View.RemoveCommands(WebCommandNames.ClearQuery);
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.Department, null).Show();
                View.Property(p => p.DepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var curr = source as WorkOrderExcuteReportViewModelCriteria;
                    var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword, curr.FactoryId);
                    if (departments == null || departments.Count <= 0)
                    {
                        return new EntityList<Enterprise>();
                    }
                    departments.ForEach(p => p.TreePId = null);
                    return departments;
                }).UsePagingLookUpEditor();
                View.Property(p => p.RepairType).Show();
                View.Property(p => p.BeginMonth).UseYearMonthEditor().DefaultValue(DateTime.Now).HasLabel("开始月份").Show();
                View.Property(p => p.EndMonth).UseYearMonthEditor().DefaultValue(DateTime.Now).HasLabel("结束月份").Show();
            }
        }
    }
}
