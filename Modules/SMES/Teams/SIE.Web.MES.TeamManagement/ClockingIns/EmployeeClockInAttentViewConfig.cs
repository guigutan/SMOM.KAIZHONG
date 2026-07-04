using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.OnLoans;
using SIE.MetaModel.View;
using SIE.ObjectModel;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 人员出勤统计
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EmployeeClockInAttentViewConfig : WebViewConfig<EmployeeClockInAttent>
    {
        /// <summary>
        ///  出勤工时统计
        /// </summary>
        public const string AttentViewGroup = "AttentViewGroup";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AttentViewGroup);
            if (ViewGroup == AttentViewGroup)
            {
                AttentTimeView();
            }
        }

        /// <summary>
        /// 人员工时统计报表
        /// </summary>
        private void AttentTimeView()
        {
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.MES.TeamManagement.ClockingIns.Commands.EmployeeAttentEditCommand");

            View.UseCommands("SIE.Web.MES.TeamManagement.ClockingIns.Commands.EmployeeAttentSaveCommand", "SIE.Web.MES.TeamManagement.ClockingIns.Commands.EmployeeAttentExport");
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.ClockInDate).UseDateEditor(p => p.Format = "Y/m/d").ShowInList().Readonly();
                View.Property(p => p.EmployeeCode).ShowInList().HasLabel("工号").Readonly();
                View.Property(p => p.EmployeeType).UseEnumEditor().ShowInList().Readonly();
                View.Property(p => p.EmployeeName).ShowInList().HasLabel("姓名").Readonly();
                View.Property(p => p.EmployeeSex).UseEnumEditor().ShowInList().HasLabel("性别").Readonly();
                View.Property(p => p.WorkGroupName).ShowInList().Readonly();
                View.Property(p => p.ShiftTimes).ShowInList().HasLabel("班次时间").Readonly();
                View.Property(p => p.OnDutyDate).UseDateTimeEditor().ShowInList(150).Readonly(p => p.OnDutyState == OnDutyState.Rest)
                        .UseListSetting(e => { e.HelpInfo = "出勤状态等于休息不可编辑"; });
                View.Property(p => p.OffDutyDate).UseDateTimeEditor().ShowInList(150).Readonly(p => p.OnDutyState == OnDutyState.Rest)
                        .UseListSetting(e => { e.HelpInfo = "出勤状态等于休息不可编辑"; });
                View.Property(p => p.OnDutyState).ShowInList().UseEnumEditor(p => p.ColumnXType = "SetClockInStateStyle").Readonly();
                View.Property(p => p.AttentHour).ShowInList().HasLabel("工时(小时)").Readonly();
                View.Property(p => p.IsLoan).ShowInList().UseEnumEditor().Readonly();
                View.Property(p => p.UpdateByName).ShowInList().Readonly();
                View.Property(p => p.UpdateDate).ShowInList(150).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ClockInDetail).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(WorkGroupOnLoan), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var entity = args.Parent as EmployeeClockInAttent;
                    entity = RF.GetById<EmployeeClockInAttent>(entity.Id);
                    var rst = RT.Service.Resolve<OnLoanController>().GetWorkGroupOnLoanByEmp(entity.EmployeeId, entity.ClockInDate, args.PagingInfo);
                    return rst;
                }, viewGroup: AttentViewGroup).HasLabel("借调工时");
            }
        }
    }
}
