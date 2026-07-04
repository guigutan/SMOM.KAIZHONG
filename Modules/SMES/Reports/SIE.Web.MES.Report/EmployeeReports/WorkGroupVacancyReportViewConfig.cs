using SIE.Domain;
using SIE.MES.Report.EmployeeReports;
using SIE.MES.Report.EmployeeReports.ClockingIns;
using SIE.MES.Report.EmployeeReports.Vacancies;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using SIE.ObjectModel;
using System;
using System.Linq;

namespace SIE.Web.MES.Report.EmployeeReports
{
    /// <summary>
    /// 班组缺编统计
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class WorkGroupVacancyReportViewConfig : WebViewConfig<WorkGroupVacancyReport>
    {
        /// <summary>
        /// 班组缺编查看工单视图ViewGroup
        /// </summary>
        public const string WorkGroupView = "WorkGroupView";

        /// <summary>
        /// 班组缺编统计报表视图
        /// </summary>
        public const string WorkGroupReportView = "WorkGroupReportView";

        #region 班次时间 ShiftTimes
        /// <summary>
        /// 班次时间
        /// </summary>
        [Label("班次时间")]
        public static readonly Property<string> ShiftTimesProperty = P<WorkGroupVacancyReport>.RegisterExtensionReadOnly("ShiftTimes", typeof(WorkGroupVacancyReportViewConfig), GetShiftTimes, WorkGroupVacancyReport.ShiftProperty);

        /// <summary>
        /// 班次时间
        /// </summary>
        /// <param name="me">实体</param>
        /// <returns>str</returns>
        public static string GetShiftTimes(Entity me)
        {
            const string hm = "HH:mm";
            var item = me as WorkGroupVacancyReport;
            if (item.Shift == null) return string.Empty;
            if (item.Shift.IsOverDay && item.Shift.EndTime < item.Shift.BeginTime)
                return item.Shift.BeginTime.ToString(hm) + "-("+"次日".L10N()+")" + item.Shift.EndTime.ToString(hm);
            else
                return item.Shift.BeginTime.ToString(hm) + "-" + item.Shift.EndTime.ToString(hm);
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkGroupReportView);
            if (ViewGroup == WorkGroupReportView)
                WorkGroupReportConfigView();
        }

        /// <summary>
        /// 班组缺编统计
        /// </summary>
        private void WorkGroupReportConfigView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.MES.TeamManagement.Vacancies.Commands.VacancyExport");
            using (View.OrderProperties())
            {
                View.Property(p => p.VacancyDate).UseDateEditor(p => p.Format = "Y/m/d").Readonly().Show();
                View.Property(p => p.WorkGroupCode).HasLabel("编码").Readonly().Show();
                View.Property(p => p.WorkGroupName).HasLabel("班组").Readonly().Show();
                View.Property(p => p.ActualQty).HasLabel("在编人数").Readonly().Show();
                View.Property(p => p.ClockingInQty).HasLabel("当日出勤").Readonly().Show();
                View.Property(p => p.AbnormalQty).HasLabel("异常人数").Readonly().Show();
                View.Property(ShiftTimesProperty).HasLabel("班次时间").Readonly().ShowInList(150);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(WorkOrderViewModel), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var entity = args.Parent as WorkGroupVacancyReport;
                    entity = RF.GetById<WorkGroupVacancyReport>(entity.Id);
                    DateRange dr = new DateRange() { BeginValue = entity.VacancyDate, EndValue = entity.VacancyDate };
                    var empIds = RT.Service.Resolve<EmployeeReportController>().GetEmpClockInReports(dr, entity.WorkGroupId).Select(p => p.EmployeeId).ToList();
                    var result = RT.Service.Resolve<EmployeeReportController>().GetWoModel(empIds, entity.WipResourceId, dr, entity.Id);
                    if (result == null) result = new EntityList<WorkOrderViewModel>();
                    return result;
                }, viewGroup: WorkGroupView).HasLabel("计划任务");
                View.AttachChildrenProperty(typeof(EmployeeClockInReport), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var entity = args.Parent as WorkGroupVacancyReport;
                    entity = RF.GetById<WorkGroupVacancyReport>(entity.Id);
                    DateRange dr = new DateRange() { BeginValue = entity.VacancyDate, EndValue = entity.VacancyDate };
                    var rst = RT.Service.Resolve<EmployeeReportController>().GetEmpClockReportByWorkGroup(entity.WorkGroupId, dr, OnDutyState.Absence, args.PagingInfo);
                    if (rst == null) rst = new EntityList<EmployeeClockInReport>();
                    return rst;
                }, viewGroup: WorkGroupView).HasLabel("异常出勤");
            }
        }
    }
}
