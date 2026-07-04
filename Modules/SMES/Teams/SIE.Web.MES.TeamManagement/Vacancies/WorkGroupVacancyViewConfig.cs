using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using SIE.ObjectModel;
using System.Linq;

namespace SIE.Web.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 班组缺编统计
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class WorkGroupVacancyViewConfig : WebViewConfig<WorkGroupVacancy>
    {
        /// <summary>
        /// 班组缺编查看工单视图ViewGroup
        /// </summary>
        public const string WorkGroupView = "WorkGroupView";

        #region 班次时间 ShiftTimes
        /// <summary>
        /// 班次时间
        /// </summary>
        [Label("班次时间")]
        public static readonly Property<string> ShiftTimesProperty = P<WorkGroupVacancy>.RegisterExtensionReadOnly("ShiftTimes", typeof(WorkGroupVacancyViewConfig),
            GetShiftTimes, WorkGroupVacancy.ShiftProperty);

        /// <summary>
        /// 班次时间
        /// </summary>
        /// <param name="me">实体</param>
        /// <returns>str</returns>
        public static string GetShiftTimes(Entity me)
        {
            const string hm = "HH:mm";
            var item = me as WorkGroupVacancy;
            if (item.Shift == null) return string.Empty;
            if (item.Shift.IsOverDay && item.Shift.EndTime < item.Shift.BeginTime)
            {
                return item.Shift.BeginTime.ToString(hm) + "-(次日)" + item.Shift.EndTime.ToString(hm);
            }
            else
            {
                return item.Shift.BeginTime.ToString(hm) + "-" + item.Shift.EndTime.ToString(hm);
            }
        }
        #endregion

        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            // 视图
        }

        /// <summary>
        /// 班组缺编统计
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.MES.TeamManagement.Vacancies.Commands.VacancyExport");
            using (View.OrderProperties())
            {
                View.Property(p => p.VacancyDate).UseDateEditor(p => p.Format = "Y/m/d").Readonly();
                View.Property(p => p.WorkGroupCode).HasLabel("编码").Readonly();
                View.Property(p => p.WorkGroupName).HasLabel("班组").Readonly();
                View.Property(p => p.ActualQty).HasLabel("在编人数").Readonly();
                View.Property(p => p.ClockingInQty).HasLabel("当日出勤").Readonly();
                View.Property(p => p.AbnormalQty).HasLabel("异常人数").Readonly();
                View.Property(ShiftTimesProperty).HasLabel("班次时间").Readonly().ShowInList(150);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(WorkOrderViewModel), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var entity = args.Parent as WorkGroupVacancy;
                    entity = RF.GetById<WorkGroupVacancy>(entity.Id);
                    DateRange dr = new DateRange() { BeginValue = entity.VacancyDate, EndValue = entity.VacancyDate };
                    var empIds = RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(dr, entity.WorkGroupId).Select(p => p.EmployeeId).ToList();
                    var result = RT.Service.Resolve<VacancyController>().GetWoModel(empIds, entity.WipResourceId, dr, entity.Id);
                    if (result == null) result = new EntityList<WorkOrderViewModel>();
                    return result;
                }, viewGroup: WorkGroupView).HasLabel("计划任务");
                View.AttachChildrenProperty(typeof(EmployeeClockIn), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var entity = args.Parent as WorkGroupVacancy;
                    entity = RF.GetById<WorkGroupVacancy>(entity.Id);
                    DateRange dr = new DateRange() { BeginValue = entity.VacancyDate, EndValue = entity.VacancyDate };
                    var rst = RT.Service.Resolve<ClockInController>().GetEmployeeClockInByWorkGroup(entity.WorkGroupId, dr, OnDutyState.Absence, args.PagingInfo);
                    if (rst == null) rst = new EntityList<EmployeeClockIn>();
                    return rst;
                }, viewGroup: WorkGroupView).HasLabel("异常出勤");
            }
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
        }
    }
}
