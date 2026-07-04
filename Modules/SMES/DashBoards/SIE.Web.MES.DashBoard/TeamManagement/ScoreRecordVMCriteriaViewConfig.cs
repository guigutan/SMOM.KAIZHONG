using SIE.Domain;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using System;

namespace SIE.Web.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 物料查询视图配置
    /// </summary>
    public class ScoreRecordVMCriteriaViewConfig : WebViewConfig<ScoreRecordVMCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.DashBoard.TeamManagement.Commands.ScoreRecordQuery");
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkGroup).UseWorkGroupEditor().Cascade(p => p.Employee, null).ShowInDetail()
                    .UseListSetting(e => { e.HelpInfo = "更改班组清空员工"; });
                View.Property(p => p.Employee).UseDataSource((o, p, k) =>
                {
                    var entity = o as ScoreRecordVMCriteria;
                    if (entity == null || entity.WorkGroup == null)
                        return new EntityList<Employee>();
                    return RT.Service.Resolve<EmployeeController>().GetEmployeeByWorkGroupId(entity.WorkGroupId, p, k);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示当前班组下的员工"; });
                View.Property(p => p.OccurDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                    p.DateFormat = "Y/m/d";
                    p.AllowBlank = false;
                }).ShowInDetail();
                View.Property(p => p.DateType).UseMultiFilterEnumEditor(p => { p.Filters = new string[] { "MonthDay" }; p.AllowBlank = false; }).DefaultValue(1).ShowInDetail();
            }
        }
    }
}
