using SIE.Inventory.Task;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Task.Commands;

namespace SIE.Web.Inventory.Task
{
    /// <summary>
    /// 任务操作人视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class OperatorViewConfig : WebViewConfig<Operator>
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
            View.UseCommands(typeof(AddOperatorCommand).FullName, typeof(EditOperatorCommand).FullName, typeof(DeleteOperatorCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
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
    }
}
