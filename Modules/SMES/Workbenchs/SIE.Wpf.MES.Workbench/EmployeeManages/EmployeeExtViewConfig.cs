using DevExpress.Xpf.Grid;
using SIE.Resources.Employees;

namespace SIE.Wpf.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeExtViewConfig : WPFViewConfig<Employee>
    {
        public const string OnLoanView = "OnLoanView";
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
            View.DeclareExtendViewGroup(OnLoanView);
            if (ViewGroup == OnLoanView)
                ConfigOnLoanView();
        }

        private void ConfigOnLoanView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList();
                View.Property(p => p.Name).ShowInList();
                // TODO: 类型屏蔽了
                ////View.Property(p => p.Type).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmployeeViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void OnAttach()
        {
            var grid = View.Control as GridControl;
            var table = grid.View as TableView;
            table.ShowCheckBoxSelectorColumn = true;
            grid.SelectionMode = MultiSelectMode.MultipleRow;
        }
    }
}