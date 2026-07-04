using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Wpf.Resources.Employees.Commands;

namespace SIE.Wpf.Resources.Employees
{
    /// <summary>
    /// 员工视图配置
    /// </summary>
    public class EmployeeViewConfig : WPFViewConfig<Employee>
    {
        /// <summary>
        /// 班组员工ViewGroup
        /// </summary>
        public const string WorkGroupEmployeeView = "WorkGroupEmployeeView";

        /// <summary>
        /// 下拉查询视图
        /// </summary>
        public const string LookUpQueryView = "LookUpQueryView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkGroupEmployeeView);

            //// 班组的员工视图View
            if (this.ViewGroup == WorkGroupEmployeeView)
            {
                WorkGroupEpyConfigView();
            }

            if (ViewGroup == LookUpQueryView)
            {
                LookUpQueryConfigView();
            }
        }

        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(EmployeeAddCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListSave, WPFCommandNames.ListDelete, typeof(LinkUserCommand), typeof(UnLinkUserCommand), typeof(EmployeeGroupCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Sex);
                View.Property(p => p.EmployeeGroup);
                View.Property(p => p.WorkGroup);
                View.Property(p => p.HireDate);
                View.Property(p => p.EmployeeStatus);
                View.Property(p => p.Phone);
                View.Property(p => p.Email);
                View.Property(p => p.Remark);
                View.Property(p => p.User);
                View.ChildrenProperty(p => p.ResourceList).IsVisible = true;
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands(WPFCommandNames.FormSave);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
                View.Property(p => p.Photo).UseImageEditor().UseFormSetting(p => p.RowSpan = 10);
                View.Property(p => p.Name);
                View.Property(p => p.Sex);
                View.Property(p => p.EmployeeGroup);
                View.Property(p => p.WorkGroup);
                View.Property(p => p.HireDate);
                View.Property(p => p.EmployeeStatus);
                View.Property(p => p.EmployeeType).UseEnumEditor();
                View.Property(p => p.Phone);
                View.Property(p => p.Email);
                View.Property(p => p.Remark);
                View.Property(p => p.User);
                View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.HireDate);
                View.Property(p => p.User);
                View.Property(p => p.WorkGroup);
                View.Property(p => p.EmployeeStatus);
                View.Property(p => p.Sex);
            }
        }

        /// <summary>
        ///  配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Sex);
        }

        /// <summary>
        /// 班组的员工视图
        /// </summary>
        protected void WorkGroupEpyConfigView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(ChangeGroupCommand), typeof(ChargehandCommand), typeof(MonitorCommand), typeof(ForemanCommand), typeof(ClearTypeCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.EmployeeGroup).Show();
                View.Property(p => p.EmployeeType).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Sex).Show();
                View.Property(p => p.WorkGroup).Show();
                View.Property(p => p.HireDate).Show();
                View.Property(p => p.Phone).Show();
                View.Property(p => p.Email).Show();
                View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
            }
        }

        /// <summary>
        /// 弹出框查询实体
        /// </summary>
        protected void LookUpQueryConfigView()
        {
            View.ClearCommands();
            View.Property(p => p.Code).HasLabel("编码").Show();
            View.Property(p => p.Name).HasLabel("名称").Show();
            View.Property(p => p.Sex).HasLabel("性别").Show();
            View.Property(p => p.EmployeeGroup).HasLabel("员工组").Show();
            View.Property(p => p.WorkGroup).HasLabel("班组").Show();
            View.Property(p => p.HireDate).HasLabel("入职时间").Show();
            View.Property(p => p.EmployeeStatus).HasLabel("员工状态").Show();
            View.Property(p => p.Phone).HasLabel("电话号码").Show();
            View.Property(p => p.Email).HasLabel("电子邮件").Show();
            View.Property(p => p.Remark).HasLabel("备注").Show();
            View.Property(p => p.User).HasLabel("关联用户").Show();
            View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
        }
    }
}
