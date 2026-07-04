using SIE.Inventory.Task;
using SIE.MetaModel.View;

namespace SIE.Web.Inventory.Task
{
    /// <summary>
    /// 实际操作人视图配置
    /// </summary>
    internal class ActualOperatorViewConfig : WebViewConfig<ActualOperator>
    {
        protected override void ConfigView()
        {
            View.DisableEditing();
            base.ConfigView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.EmployeeCode).HasLabel("工号");
            View.Property(p => p.EmployeeName).HasLabel("姓名");
            View.Property(p => p.EmployeeGroupName).HasLabel("员工组");
            View.Property(p => p.WorkGroupName).HasLabel("班组");
            View.Property(p => p.IsMaster).Readonly();
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            base.ConfigDetailsView();
            View.Property(p => p.EmployeeCode).HasLabel("工号");
            View.Property(p => p.EmployeeName).HasLabel("姓名");
            View.Property(p => p.EmployeeGroupName).HasLabel("员工组");
            View.Property(p => p.WorkGroupName).HasLabel("班组");
            View.Property(p => p.IsMaster).Readonly();
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
            View.Property(p => p.EmployeeCode).HasLabel("工号");
            View.Property(p => p.EmployeeName).HasLabel("姓名");
            View.Property(p => p.EmployeeGroupName).HasLabel("员工组");
            View.Property(p => p.WorkGroupName).HasLabel("班组");
            View.Property(p => p.IsMaster).Readonly();
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EmployeeCode).HasLabel("工号");
            View.Property(p => p.EmployeeName).HasLabel("姓名");
            View.Property(p => p.EmployeeGroupName).HasLabel("员工组");
            View.Property(p => p.WorkGroupName).HasLabel("班组");
            View.Property(p => p.IsMaster);
        }
    }
}
