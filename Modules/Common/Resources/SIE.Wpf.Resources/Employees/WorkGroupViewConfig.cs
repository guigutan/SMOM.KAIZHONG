using SIE.Resources.Employees;

namespace SIE.Wpf.Resources.Employees
{
    /// <summary>
    /// 班组视图配置
    /// </summary>
    internal class WorkGroupViewConfig : WPFViewConfig<WorkGroup>
	{
        /// <summary>
        /// 员工选择班组视图
        /// </summary>
        internal static readonly string WorkGroupSelectView = "WorkGroupSelectView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
		{
            View.DeclareExtendViewGroup(WorkGroupSelectView);
            if (ViewGroup  == WorkGroupSelectView)
            {
                WorkGroupSelectConfigView();
            }
		}

        /// <summary>
        /// 默认表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.AttachChildrenProperty(typeof(Employee), (e) =>
                {
                    var args = e as ChildPagingDataArgs;
                    var workGroup = args.Parent as WorkGroup;
                    return RT.Service.Resolve<EmployeeController>().GetEmployeeByWorkGroupId(workGroup.Id);
                }, EmployeeViewConfig.WorkGroupEmployeeView, true).HasLabel("员工");
            }
        }

        /// <summary>
        /// 默认表格视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
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
    }

    ///// <summary>
    ///// 转班组选择班组视图
    ///// </summary>
    //internal class WorkGroupSelectViewConfig : WPFViewConfig<WorkGroup>
    //{
    //    protected override bool IsDefalutView
    //    {
    //        get { return false; }
    //    }

    //    protected override void ConfigView()
    //    {

    //        using (View.OrderProperties())
    //        {
    //            View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
    //            View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
    //        }
    //    }
    //}
}
