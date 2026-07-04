using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MetaModel.View;
using SIE.ObjectModel;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EmployeeClockInViewConfig : WebViewConfig<EmployeeClockIn>
    {
        /// <summary>
        ///  员工出勤视图
        /// </summary>
        public const string CusListView = "CusListView";

        /// <summary>
        /// 班组缺编查看工单视图ViewGroup
        /// </summary>
        public const string WorkGroupView = "WorkGroupView";


        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CusListView, WorkGroupView);
            if (ViewGroup == CusListView)
            {
                ConfigCusListView();
            }
            else if (ViewGroup == WorkGroupView)
            {
                ConfigWorkGroupView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        void ConfigCusListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.ClockInDate).UseDateEditor(p => p.Format = "Y/m/d").Readonly().ShowInList();
                View.Property(p => p.EmployeeCode).ShowInList().Readonly();
                View.Property(p => p.EmployeeName).ShowInList().Readonly();
                View.Property(p => p.EmployeeSex).UseEnumEditor().HasLabel("性别").ShowInList().Readonly();
                View.Property(p => p.WorkGroupName).ShowInList().Readonly();
                View.Property(p => p.ShiftTimes).ShowInList().Readonly();
                View.Property(p => p.OnDutyDate).ShowInList(150).Readonly();
                View.Property(p => p.OffDutyDate).ShowInList(150).Readonly();
                View.Property(p => p.OnDutyState).UseEnumEditor(p => p.ColumnXType = "SetClockInStateStyle").ShowInList().Readonly();
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }

            View.ChildrenProperty(p => p.ClockInDetail).HasLabel("打卡记录");
        }

        /// <summary>
        /// 班组缺编视图
        /// </summary>
        void ConfigWorkGroupView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).HasLabel("工号").ShowInList().Readonly();
                View.Property(p => p.EmployeeName).ShowInList().Readonly();
                View.Property(p => p.EmployeeSex).UseEnumEditor().HasLabel("性别").ShowInList().Readonly();
                View.Property(p => p.OnDutyDate).ShowInList(150).Readonly();
                View.Property(p => p.OffDutyDate).ShowInList(150).Readonly();
                View.Property(p => p.OnDutyState).ShowInList().Readonly();
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }

            View.ChildrenProperty(p => p.ClockInDetail).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}
