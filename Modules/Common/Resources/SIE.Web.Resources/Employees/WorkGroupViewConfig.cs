using SIE.MetaModel.View;
using SIE.Resources.Employees;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 班组视图配置
    /// </summary>
    public class WorkGroupViewConfig : WebViewConfig<WorkGroup>
    {
        /// <summary>
        /// 员工选择班组视图
        /// </summary>
        public const string WorkGroupSelectView = "WorkGroupSelectView";
        /// <summary>
        /// 排班选择班组视图
        /// </summary>
        public const string ShiftScheduleSelectView = "ShiftScheduleSelectView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkGroupSelectView);
            if (ViewGroup == WorkGroupSelectView)
            {
                WorkGroupSelectConfigView();
            }
            else if (ViewGroup == ShiftScheduleSelectView)
            {
                WorkGroupSelectionView();
            }
        }

        /// <summary>
        /// 默认表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Resources.Employees.Commands.WorkGroupCopyCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.DepartmentId).UseWipWorkShopEditor().UseListSetting(e => { e.HelpInfo = "显示生产资源车间集合"; });
            View.Property(p => p.DemandQty).UseSpinEditor(p => { p.MinValue = 1; });
            View.Property(p => p.ActualQty).Readonly();
            View.AttachChildrenProperty(typeof(Employee), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var workGroup = args.Parent as WorkGroup;
                return RT.Service.Resolve<EmployeeController>().GetEmployeeByWorkOrderGroupId(workGroup.Id, args.SortInfo, args.PagingInfo);
            }, EmployeeViewConfig.WorkGroupEmployeeView, true).HasLabel("员工");
        }

        /// <summary>
        /// 默认表格视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.DemandQty);
            View.Property(p => p.ActualQty).Readonly();
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 转班组弹框的视图
        /// </summary>
        protected void WorkGroupSelectConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
        /// <summary>
        /// 排班选择视图配置
        /// </summary>
        private void WorkGroupSelectionView()
        {
            View.UseGridSelectionModel();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.DepartmentId).UseWipWorkShopEditor().Show(ShowInWhere.All)
                    .UseListSetting(e => { e.HelpInfo = "显示生产资源车间集合"; });
                View.Property(p => p.DemandQty).UseSpinEditor(p => { p.MinValue = 1; }).Show(ShowInWhere.All);
                View.Property(p => p.ActualQty).Readonly().Show(ShowInWhere.All);
            }
        }
    }
}
